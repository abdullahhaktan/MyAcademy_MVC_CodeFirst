using MyAcademy_MVC_CodeFirst.Data.Context;
using MyAcademy_MVC_CodeFirst.Data.Entities;
using MyAcademy_MVC_CodeFirst.DTOs.FaqDtos;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyAcademy_MVC_CodeFirst.Areas.Admin.Controllers
{
    [RouteArea("Admin")]
    public class FaqController : Controller
    {
        private readonly AppDbContext context = new AppDbContext();

        // LIST
        public async Task<ActionResult> Index()
        {
            var values = await context.Faqs.ToListAsync();

            var faqs = MyAcademy_MVC_CodeFirst.MvcApplication
                .mapperInstance
                .Map<List<ResultFaqDto>>(values);

            return View(faqs);
        }

        // CREATE
        [HttpGet]
        public ActionResult CreateFaq()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateFaq(CreateFaqDto createFaqDto)
        {
            var faq = MyAcademy_MVC_CodeFirst.MvcApplication
                .mapperInstance
                .Map<Faq>(createFaqDto);

            context.Faqs.Add(faq);
            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // UPDATE (GET)
        [HttpGet]
        public async Task<ActionResult> UpdateFaq(int id)
        {
            var value = await context.Faqs.FindAsync(id);

            var faq = MyAcademy_MVC_CodeFirst.MvcApplication
                .mapperInstance
                .Map<GetFaqByIdDto>(value);

            return View(faq);
        }

        // UPDATE (POST)
        [HttpPost]
        public async Task<ActionResult> UpdateFaq(UpdateFaqDto updateFaqDto)
        {
            var faq = await context.Faqs.FindAsync(updateFaqDto.Id);

            MyAcademy_MVC_CodeFirst.MvcApplication
                .mapperInstance
                .Map(updateFaqDto, faq);

            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // DELETE
        public async Task<ActionResult> DeleteFaq(int id)
        {
            var value = await context.Faqs.FindAsync(id);

            context.Faqs.Remove(value);
            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
