using Microsoft.AspNetCore.Mvc;
using TimeT.Data.Repository;
using TimeT.Data.Entities;
using TimeT.Data.Dtos.Service;
using System.Linq;
using AutoMapper;
using TimeT.Data.Dtos.Registration;
using TimeT.Helpers;
using TimeT.Data;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using TimeT.Auth.Model;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using TimeT.Auth;

namespace TimeT.Controllers
{
    [ApiController]
    [Route("api/services")]
    public class ServiceController : ControllerBase
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IServicesRepository _servicesRepository;
        private readonly IMapper _mapper;
        public ServiceController(IServicesRepository servicesRepository, IMapper mapper, IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
            _servicesRepository = servicesRepository;
            _mapper = mapper;
        }

        //services?pageNumber=1&pageSize=5
        [HttpGet(Name ="GetServices")]
        public async Task<ActionResult<PagedList<ServiceDto>>> GetAllPaging([FromQuery] ServiceSearchParam serviceSearchParam)
        {
            var services = await _servicesRepository.GetAll(serviceSearchParam);

            var previousPageLink = services.HasPrevious ?
            CreateServiceResourceUri(serviceSearchParam,
                ResourceUriType.PreviousPage) : null;

            var nextPageLink = services.HasNext ?
                CreateServiceResourceUri(serviceSearchParam,
                    ResourceUriType.NextPage) : null;

            var paginationMetadata = new
            {
                totalCount = services.TotalCount,
                pageSize = services.PageSize,
                currentPage = services.CurrentPage,
                totalPages = services.TotalPages,
                previousPageLink,
                nextPageLink
            };

            Response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationMetadata));

            return Ok(services.Select(o => _mapper.Map<ServiceDto>(o)));
        }

        [HttpGet("{serviceId}",Name = "GetService")]
        public async Task<ActionResult<ServiceDto>> Get(int serviceId)
        {
            var service = await _servicesRepository.Get(serviceId);
            // 404
            if (service == null) return NotFound();
            
            return Ok(_mapper.Map<ServiceDto>(service));
        }

        [HttpPost(Name ="CreateService")]
        [Authorize(Roles = UserRoles.ServiceUser)]
        public async Task<ActionResult<ServiceDto>> Post(ServiceCreateDto serviceCreateDto)
        {
            var service = _mapper.Map<Service>(serviceCreateDto);
            service.userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            await _servicesRepository.Create(service);
            return Created($"api/services/{service.id}",_mapper.Map<ServiceDto>(service));
        }

        [HttpPut("{serviceId}",Name ="UpdateService")]
        [Authorize(Roles = UserRoles.ServiceUser)]
        public async Task<ActionResult<ServiceDto>> Put(int serviceId, ServiceUpdateDto serviceUpdateDto)
        {
            var service = await _servicesRepository.Get(serviceId);
            if (service == null) return NotFound();

            // Authorization
            if(!CostumeAuth.Atuh(PolicyNames.ResourceOwner, User.FindFirstValue(JwtRegisteredClaimNames.Sub), service.userId) ) 
            { return Forbid(); }


            _mapper.Map(serviceUpdateDto, service);
            await _servicesRepository.Update(service);
            return Ok(_mapper.Map<ServiceDto>(service));
        }

        [HttpDelete("{serviceId}", Name = "DeleteService")]
        [Authorize(Roles = UserRoles.ServiceUser)]
        public async Task<ActionResult<ServiceDto>> Delete(int serviceId)
        {
            var service = await _servicesRepository.Get(serviceId);
            if (service == null) return NotFound();

            // Authorization
            if (!CostumeAuth.Atuh(PolicyNames.ResourceOwner, User.FindFirstValue(JwtRegisteredClaimNames.Sub), service.userId))
            { return Forbid(); }

            await _servicesRepository.Delete(service);
            return NoContent();
        }


        private string? CreateServiceResourceUri(ServiceSearchParam serviceSearchParam,ResourceUriType type)
        {
            return type switch
            {
                ResourceUriType.PreviousPage => Url.Link("GetServices",
                    new
                    {
                        pageNumber = serviceSearchParam.pageNumber - 1,
                        pageSize = serviceSearchParam.pageSize,
                    }),
                ResourceUriType.NextPage => Url.Link("GetServices",
                    new
                    {
                        pageNumber = serviceSearchParam.pageNumber + 1,
                        pageSize = serviceSearchParam.pageSize,
                    }),
                _ => Url.Link("GetServices",
                    new
                    {
                        pageNumber = serviceSearchParam.pageNumber,
                        pageSize = serviceSearchParam.pageSize,
                    })
            };
        }
    }
}
