using AutoMapper;
using MyAcademy_MVC_CodeFirst.Data.Entities;
using MyAcademy_MVC_CodeFirst.DTOs.FaqDtos;

namespace MyAcademy_MVC_CodeFirst.Mappings
{
    public class FaqMappings : Profile
    {
        public FaqMappings()
        {
            CreateMap<Faq, ResultFaqDto>().ReverseMap();
            CreateMap<Faq, CreateFaqDto>().ReverseMap();
            CreateMap<Faq, UpdateFaqDto>().ReverseMap();
            CreateMap<Faq, GetFaqByIdDto>().ReverseMap();
        }
    }
}