using AutoMapper;
using MyAcademy_MVC_CodeFirst.Data.Entities;
using MyAcademy_MVC_CodeFirst.DTOs.PolicyDtos;

namespace MyAcademy_MVC_CodeFirst.Mappings
{
    public class PolicyMappings : Profile
    {
        public PolicyMappings()
        {
            CreateMap<Policy, ResultPolicyDto>().ReverseMap();
            CreateMap<Policy, CreatePolicyDto>().ReverseMap();

            CreateMap<Policy, UpdatePolicyDto>();

            CreateMap<UpdatePolicyDto, Policy>()
                .ForMember(dest => dest.PolicySales, opt => opt.Ignore());
            CreateMap<Policy, GetPolicyByIdDto>().ReverseMap();
        }
    }
}