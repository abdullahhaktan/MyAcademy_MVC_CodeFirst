using AutoMapper;
using MyAcademy_MVC_CodeFirst.Data.Entities;
using MyAcademy_MVC_CodeFirst.DTOs.ContactMessageDtos;

namespace MyAcademy_MVC_CodeFirst.Mappings
{
    public class ContactMessageMappings : Profile
    {
        public ContactMessageMappings()
        {
            CreateMap<ContactMessage, CreateContactMessageDto>().ReverseMap();
            CreateMap<ContactMessage, ResultContactMessageDto>().ReverseMap();
        }
    }
}