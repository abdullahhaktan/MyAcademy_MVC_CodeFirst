using AutoMapper;
using MyAcademy_MVC_CodeFirst.Data.Entities;
using MyAcademy_MVC_CodeFirst.DTOs.BlogDtos;

namespace MyAcademy_MVC_CodeFirst.Mappings
{
    public class BlogMappings : Profile
    {
        public BlogMappings()
        {
            CreateMap<Blog, CreateBlogDto>().ReverseMap();
            CreateMap<Blog, ResultBlogDto>().ReverseMap();
            CreateMap<Blog, UpdateBlogDto>().ReverseMap();
            CreateMap<Blog, GetBlogByIdDto>().ReverseMap();
        }
    }
}