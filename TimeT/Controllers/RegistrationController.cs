using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TimeT.Data.Dtos.Time;
using TimeT.Data.Dtos.Registration;
using TimeT.Data.Entities;
using TimeT.Data.Repository;
using TimeT.Helpers;
using TimeT.Data;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using TimeT.Auth.Model;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using TimeT.Auth;

namespace TimeT.Controllers
{
    [ApiController]
    [Route("api/services/{serviceId}/times/{timeId}/registrations")]
    public class RegistrationController : ControllerBase
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IServicesRepository _servicesRepository;
        private readonly ITimeRepository _timeRepository;
        private readonly IRegistrationRepository _registrationRepository;
        private readonly IMapper _mapper;
        public RegistrationController(IServicesRepository servicesRepository, ITimeRepository timeRepository, IRegistrationRepository registrationRepository, IMapper mapper, IAuthorizationService authorizationService)
        {
            _servicesRepository = servicesRepository;
            _timeRepository = timeRepository;
            _registrationRepository = registrationRepository;
            _mapper = mapper;
            _authorizationService = authorizationService;
        }

        [HttpGet(Name ="GetRegistrations")]
        public async Task<ActionResult<PagedList<RegistrationDto>>> GetAllPaging(int serviceId, int timeId, [FromQuery] RegistrationSearchPrams registrationSearchPrams)
        {
            var service = await _servicesRepository.Get(serviceId);
            if (service == null) return BadRequest();

            var time = await _timeRepository.Get(timeId);
            if (time == null) return BadRequest();
            if (time.serviceId != serviceId) return NotFound();

            registrationSearchPrams.timeId = timeId;
            var registrations = await _registrationRepository.GetAll(registrationSearchPrams);

            var previousPageLink = registrations.HasPrevious ?
            CreateRegistrationResourceUri(registrationSearchPrams,
                ResourceUriType.PreviousPage) : null;

            var nextPageLink = registrations.HasNext ?
                CreateRegistrationResourceUri(registrationSearchPrams,
                    ResourceUriType.NextPage) : null;

            var paginationMetadata = new
            {
                totalCount = registrations.TotalCount,
                pageSize = registrations.PageSize,
                currentPage = registrations.CurrentPage,
                totalPages = registrations.TotalPages,
                previousPageLink,
                nextPageLink
            };

            Response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationMetadata));

            return Ok(registrations.Select(o => _mapper.Map<RegistrationDto>(o)));
        }

        [HttpGet("{registrationId}",Name ="GetRegistration")]
        public async Task<ActionResult<RegistrationDto>> Get(int serviceId, int timeId, int registrationId)
        {

            var service = await _servicesRepository.Get(serviceId);
            if (service == null) return NotFound();

            var time = await _timeRepository.Get(timeId);
            if (time == null) return NotFound();
            if (time.serviceId != serviceId) return NotFound();

            var registration = await _registrationRepository.Get(registrationId);
            if (registration == null) return NotFound();
            if (registration.TimeId != timeId) return NotFound();

            return Ok(_mapper.Map<RegistrationDto>(registration));
        }

        [HttpPost(Name ="CreateRegistration")]
        [Authorize(Roles = UserRoles.ClientUser + "," + UserRoles.ServiceUser)]
        public async Task<ActionResult<RegistrationDto>> Post(int serviceId,int timeId, RegistrationCreateDto registrationCreateDto)
        {
            var service = await _servicesRepository.Get(serviceId);
            if (service == null) return NotFound();

            var time = await _timeRepository.Get(timeId);
            if (time == null) return NotFound();
            if (time.serviceId != serviceId) return NotFound();

            var registration= _mapper.Map<Registration>(registrationCreateDto);
            registration.userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            registration.TimeId = timeId;

            await _registrationRepository.Create(registration);
            return Created($"api/services/{serviceId}/times/{timeId}/registrations/{registration.id}", _mapper.Map<RegistrationDto>(registration));
        }

        [HttpPut("{registrationId}",Name ="UpdateRegistration")]
        [Authorize(Roles = UserRoles.ClientUser + "," + UserRoles.ServiceUser)]
        public async Task<ActionResult<RegistrationDto>> Put(int serviceId, int timeId, int registrationId, RegistrationUpdateDto registrationUpdateDto)
        {
            var service = await _servicesRepository.Get(serviceId);
            if (service == null) return NotFound();

            var time = await _timeRepository.Get(timeId);
            if (time == null) return NotFound();
            if (time.serviceId != serviceId) return NotFound();

            var registration = await _registrationRepository.Get(registrationId);
            if (registration == null) return NotFound();
            if (registration.TimeId != timeId) return NotFound();

            // Authorization
            if(!CostumeAuth.Atuh(PolicyNames.ResourceOwner, User.FindFirstValue(JwtRegisteredClaimNames.Sub),registration.userId, service.userId))
            { return Forbid(); }
            
            _mapper.Map(registrationUpdateDto, registration);
            await _registrationRepository.Update(registration);
            return Ok(_mapper.Map<RegistrationDto>(registration));
        }

        [HttpDelete("{registrationId}", Name ="DeleteRegistration")]
        [Authorize(Roles = UserRoles.ServiceUser)]
        public async Task<ActionResult<TimeDto>> Delete(int serviceId, int timeId, int registrationId)
        {
            var service = await _servicesRepository.Get(serviceId);
            if (service == null) return NotFound();

            var time = await _timeRepository.Get(timeId);
            if (time == null) return NotFound();
            if (time.serviceId != serviceId) return NotFound();

            var registration = await _registrationRepository.Get(registrationId);
            if (registration == null) return NotFound();
            if (registration.TimeId != timeId) return NotFound();

            // Authorization
            if (!CostumeAuth.Atuh(PolicyNames.ResourceOwner, User.FindFirstValue(JwtRegisteredClaimNames.Sub), registration.userId, service.userId))
            { return Forbid(); }

            await _registrationRepository.Delete(registration);
            return NoContent();
        }

        private string? CreateRegistrationResourceUri(RegistrationSearchPrams registrationSearchPrams, ResourceUriType type)
        {
            return type switch
            {
                ResourceUriType.PreviousPage => Url.Link("GetRegistrations",
                    new
                    {
                        pageNumber = registrationSearchPrams.pageNumber - 1,
                        pageSize = registrationSearchPrams.pageSize,
                    }),
                ResourceUriType.NextPage => Url.Link("GetRegistrations",
                    new
                    {
                        pageNumber = registrationSearchPrams.pageNumber + 1,
                        pageSize = registrationSearchPrams.pageSize,
                    }),
                _ => Url.Link("GetRegistrations",
                    new
                    {
                        pageNumber = registrationSearchPrams.pageNumber,
                        pageSize = registrationSearchPrams.pageSize,
                    })
            };
        }
    }
}
