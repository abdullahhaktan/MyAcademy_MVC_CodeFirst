using AutoMapper;
using MyAcademy_MVC_CodeFirst.Data.Entities;
using MyAcademy_MVC_CodeFirst.DTOs.PolicySaleDtos;

namespace MyAcademy_MVC_CodeFirst.Mappings
{
    public class PolicySaleSaleMappings : Profile
    {
        public PolicySaleSaleMappings()
        {
            CreateMap<PolicySale, ResultPolicySaleDto>().ReverseMap();
            CreateMap<PolicySale, CreatePolicySaleDto>().ReverseMap();
            CreateMap<PolicySale, UpdatePolicySaleDto>().ReverseMap();
            CreateMap<PolicySale, GetPolicySaleByIdDto>().ReverseMap();
        }
    }
}