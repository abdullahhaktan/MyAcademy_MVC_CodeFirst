using MyAcademy_MVC_CodeFirst.Data.Context;
using MyAcademy_MVC_CodeFirst.DTOs.AboutDtos;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyAcademy_MVC_CodeFirst.Areas.Admin.Controllers
{
    [RouteArea("Admin")]
    public class AboutController : Controller
    {
        private readonly AppDbContext context = new AppDbContext();

        // GET
        public async Task<ActionResult> Index()
        {
            var value = await context.Abouts.FirstOrDefaultAsync();

            var about = MvcApplication.mapperInstance
                .Map<GetAboutByIdDto>(value);

            return View(about);
        }

        // POST
        [HttpPost]
        public async Task<ActionResult> Index(UpdateAboutDto updateAboutDto)
        {
            var value = await context.Abouts.FindAsync(updateAboutDto.Id);

            // DTO → Tracked Entity
            MvcApplication.mapperInstance
                .Map(updateAboutDto, value);

            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
