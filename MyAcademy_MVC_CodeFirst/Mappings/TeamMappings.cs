using AutoMapper;
using MyAcademy_MVC_CodeFirst.Data.Entities;
using MyAcademy_MVC_CodeFirst.DTOs.TeamDtos;

namespace MyAcademy_MVC_CodeFirst.Mappings
{
    public class TeamMappings : Profile
    {
        public TeamMappings()
        {
            CreateMap<Team, ResultTeamDto>().ReverseMap();
            CreateMap<Team, CreateTeamDto>().ReverseMap();
            CreateMap<Team, UpdateTeamDto>().ReverseMap();
            CreateMap<Team, GetTeamByIdDto>().ReverseMap();
        }
    }
}