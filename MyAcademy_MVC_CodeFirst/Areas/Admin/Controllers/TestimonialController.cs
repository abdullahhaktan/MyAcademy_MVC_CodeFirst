using MyAcademy_MVC_CodeFirst.Data.Context;
using MyAcademy_MVC_CodeFirst.Data.Entities;
using MyAcademy_MVC_CodeFirst.DTOs.TestimonialDtos;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyAcademy_MVC_CodeFirst.Areas.Admin.Controllers
{
    [RouteArea("Admin")]
    public class TestimonialController : Controller
    {
        private readonly AppDbContext context = new AppDbContext();

        // LIST
        public async Task<ActionResult> Index()
        {
            var values = await context.Testimonials.ToListAsync();
            var testimonials = MvcApplication.mapperInstance
                .Map<List<ResultTestimonialDto>>(values);

            return View(testimonials);
        }

        // CREATE
        [HttpGet]
        public ActionResult CreateTestimonial()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateTestimonial(CreateTestimonialDto createTestimonialDto)
        {
            var testimonial = MvcApplication.mapperInstance
                .Map<Testimonial>(createTestimonialDto);

            context.Testimonials.Add(testimonial);
            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // UPDATE (GET)
        [HttpGet]
        public async Task<ActionResult> UpdateTestimonial(int id)
        {
            var value = await context.Testimonials.FindAsync(id);

            var testimonial = MvcApplication.mapperInstance
                .Map<GetTestimonialByIdDto>(value);

            return View(testimonial);
        }

        // UPDATE (POST)
        [HttpPost]
        public async Task<ActionResult> UpdateTestimonial(UpdateTestimonialDto updateTestimonialDto)
        {
            var testimonial = await context.Testimonials.FindAsync(updateTestimonialDto.Id);

            MvcApplication.mapperInstance
                .Map(updateTestimonialDto, testimonial);

            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // DELETE
        public async Task<ActionResult> DeleteTestimonial(int id)
        {
            var value = await context.Testimonials.FindAsync(id);

            context.Testimonials.Remove(value);
            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
