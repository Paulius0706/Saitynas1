using AutoMapper;
using TimeT.Data.Dtos.Service;
using TimeT.Data.Dtos.Time;
using TimeT.Data.Dtos.Registration;
using TimeT.Data.Entities;
using TimeT.Auth.Model.AuthDtos;

namespace TimeT.Data
{
    public class RestProfile : Profile
    {
        public RestProfile()
        {
            CreateMap<Service, ServiceDto>();
            CreateMap<Service, FullServiceDto>();
            CreateMap<ServiceCreateDto, Service>();
            CreateMap<ServiceUpdateDto, Service>();

            CreateMap<Time, TimeDto>();
            CreateMap<TimeCreateDto, Time>();
            CreateMap<TimeUpdateDto, Time>();

            CreateMap<Registration, RegistrationDto>();
            CreateMap<RegistrationCreateDto, Registration>();
            CreateMap<RegistrationUpdateDto, Registration>();

        }
    }
}
