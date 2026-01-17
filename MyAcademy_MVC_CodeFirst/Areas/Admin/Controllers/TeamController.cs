using MyAcademy_MVC_CodeFirst.Data.Context;
using MyAcademy_MVC_CodeFirst.Data.Entities;
using MyAcademy_MVC_CodeFirst.DTOs.TeamDtos;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyAcademy_MVC_CodeFirst.Areas.Admin.Controllers
{
    [RouteArea("Admin")]
    public class TeamController : Controller
    {
        private readonly AppDbContext context = new AppDbContext();

        // LIST
        public async Task<ActionResult> Index()
        {
            var values = await context.Teams.ToListAsync();
            var teams = MvcApplication.mapperInstance
                .Map<List<ResultTeamDto>>(values);

            return View(teams);
        }

        // CREATE
        [HttpGet]
        public ActionResult CreateTeam()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateTeam(CreateTeamDto createTeamDto)
        {
            var team = MvcApplication.mapperInstance
                .Map<Team>(createTeamDto);

            context.Teams.Add(team);
            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // UPDATE (GET)
        [HttpGet]
        public async Task<ActionResult> UpdateTeam(int id)
        {
            var value = await context.Teams.FindAsync(id);

            var team = MvcApplication.mapperInstance
                .Map<GetTeamByIdDto>(value);

            return View(team);
        }

        // UPDATE (POST)
        [HttpPost]
        public async Task<ActionResult> UpdateTeam(UpdateTeamDto updateTeamDto)
        {
            var team = await context.Teams.FindAsync(updateTeamDto.Id);

            MvcApplication.mapperInstance
                .Map(updateTeamDto, team);

            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // DELETE
        public async Task<ActionResult> DeleteTeam(int id)
        {
            var value = await context.Teams.FindAsync(id);

            context.Teams.Remove(value);
            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
