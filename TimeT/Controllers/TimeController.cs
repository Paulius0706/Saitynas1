using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using TimeT.Auth;
using TimeT.Auth.Model;
using TimeT.Data;
using TimeT.Data.Dtos.Registration;
using TimeT.Data.Dtos.Service;
using TimeT.Data.Dtos.Time;
using TimeT.Data.Entities;
using TimeT.Data.Repository;
using TimeT.Helpers;

namespace TimeT.Controllers
{
    [ApiController]
    [Route("api/services/{serviceId}/times")]
    public class TimeController : ControllerBase
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IServicesRepository _servicesRepository;
        private readonly ITimeRepository _timeRepository;
        private readonly IMapper _mapper;
        public TimeController(IServicesRepository servicesRepository,ITimeRepository timeRepository, IMapper mapper, IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
            _servicesRepository = servicesRepository;
            _timeRepository = timeRepository;
            _mapper = mapper;
        }

        //times?pageNumber=1&pageSize=5
        [HttpGet(Name ="GetTimes")]
        public async Task<ActionResult<PagedList<TimeDto>>> GetAllPaging(int serviceId,[FromQuery] TimeSearchParam timeSearchParam)
        {
            var service = await _servicesRepository.Get(serviceId);
            if (service == null) return NotFound();

            timeSearchParam.serviceId = serviceId;
            var times = await _timeRepository.GetAll(timeSearchParam);

            var previousPageLink = times.HasPrevious ?
            CreateTimeResourceUri(timeSearchParam,
                ResourceUriType.PreviousPage) : null;

            var nextPageLink = times.HasNext ?
                CreateTimeResourceUri(timeSearchParam,
                    ResourceUriType.NextPage) : null;

            var paginationMetadata = new
            {
                totalCount = times.TotalCount,
                pageSize = times.PageSize,
                currentPage = times.CurrentPage,
                totalPages = times.TotalPages,
                previousPageLink,
                nextPageLink
            };

            Response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationMetadata));

            return Ok(times.Select(o => _mapper.Map<TimeDto>(o)));
        }

        [HttpGet("{timeId}",Name ="GetTime")]
        public async Task<ActionResult<TimeDto>> Get(int serviceId, int timeId)
        {
            var service = await _servicesRepository.Get(serviceId);
            if (service == null) return NotFound();

            var time = await _timeRepository.Get(timeId);
            if (time == null) return NotFound();
            if (time.serviceId != serviceId) return NotFound();
            

            return Ok(_mapper.Map<TimeDto>(time));
        }

        [HttpPost(Name ="CreateTime")]
        [Authorize(Roles = UserRoles.ServiceUser)]
        public async Task<ActionResult<TimeDto>> Post(int serviceId, TimeCreateDto timeCreateDto)
        {
            var service = await _servicesRepository.Get(serviceId);
            if (service == null) return NotFound();

            var time = _mapper.Map<Time>(timeCreateDto);
            time.serviceId = serviceId;

            // Authorization
            if (!CostumeAuth.Atuh(PolicyNames.ResourceOwner, User.FindFirstValue(JwtRegisteredClaimNames.Sub), service.userId))
            { return Forbid(); }

            await _timeRepository.Create(time);
            return Created($"api/services/{serviceId}/times/{time.id}", _mapper.Map<TimeDto>(time));
        }

        [HttpPut("{timeId}",Name ="UpdateTime")]
        [Authorize(Roles = UserRoles.ServiceUser)]
        public async Task<ActionResult<TimeDto>> Put(int serviceId, int timeId, TimeUpdateDto timeUpdateDto)
        {
            var service = await _servicesRepository.Get(serviceId);
            if (service == null) return NotFound();

            var time = await _timeRepository.Get(timeId);
            if (time == null) return NotFound();
            if (time.serviceId != serviceId) return NotFound();

            // Authorization
            if (!CostumeAuth.Atuh(PolicyNames.ResourceOwner, User.FindFirstValue(JwtRegisteredClaimNames.Sub), service.userId))
            { return Forbid(); }

            _mapper.Map(timeUpdateDto, time);
            await _timeRepository.Update(time);
            return Ok(_mapper.Map<TimeDto>(time));
        }

        [HttpDelete("{timeId}",Name ="DeleteTime")]
        [Authorize(Roles = UserRoles.ServiceUser)]
        public async Task<ActionResult<TimeDto>> Delete(int serviceId, int timeId)
        {
            var service = await _servicesRepository.Get(serviceId);
            if (service == null) return NotFound();

            var time = await _timeRepository.Get(timeId);
            if (time == null) return NotFound();
            if (time.serviceId != serviceId) return NotFound();


            // Authorization
            if (!CostumeAuth.Atuh(PolicyNames.ResourceOwner, User.FindFirstValue(JwtRegisteredClaimNames.Sub), service.userId))
            { return Forbid(); }

            await _timeRepository.Delete(time);
            return NoContent();
        }

        private string? CreateTimeResourceUri(TimeSearchParam timeSearchParam, ResourceUriType type)
        {
            return type switch
            {
                ResourceUriType.PreviousPage => Url.Link("GetTimes",
                    new
                    {
                        pageNumber = timeSearchParam.pageNumber - 1,
                        pageSize = timeSearchParam.pageSize,
                    }),
                ResourceUriType.NextPage => Url.Link("GetTimes",
                    new
                    {
                        pageNumber = timeSearchParam.pageNumber + 1,
                        pageSize = timeSearchParam.pageSize,
                    }),
                _ => Url.Link("GetTimes",
                    new
                    {
                        pageNumber = timeSearchParam.pageNumber,
                        pageSize = timeSearchParam.pageSize,
                    })
            };
        }
    }
}
