using MyAcademy_MVC_CodeFirst.Data.Context;
using MyAcademy_MVC_CodeFirst.Data.Entities;
using MyAcademy_MVC_CodeFirst.DTOs.FeatureDtos;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyAcademy_MVC_CodeFirst.Areas.Admin.Controllers
{
    [RouteArea("Admin")]
    public class FeatureController : Controller
    {
        private readonly AppDbContext context = new AppDbContext();

        // LIST
        public async Task<ActionResult> Index()
        {
            var values = await context.Features.ToListAsync();
            var features = MvcApplication.mapperInstance
                .Map<List<ResultFeatureDto>>(values);

            return View(features);
        }

        // CREATE
        [HttpGet]
        public ActionResult CreateFeature()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateFeature(CreateFeatureDto createFeatureDto)
        {
            var feature = MvcApplication.mapperInstance
                .Map<Feature>(createFeatureDto);

            context.Features.Add(feature);
            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // UPDATE (GET)
        [HttpGet]
        public async Task<ActionResult> UpdateFeature(int id)
        {
            var value = await context.Features.FindAsync(id);

            var feature = MvcApplication.mapperInstance
                .Map<GetFeatureByIdDto>(value);

            return View(feature);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateFeature(UpdateFeatureDto updateFeatureDto)
        {
            var feature = await context.Features.FindAsync(updateFeatureDto.Id);

            MvcApplication.mapperInstance
                .Map(updateFeatureDto, feature);

            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        // DELETE
        public async Task<ActionResult> DeleteFeature(int id)
        {
            var value = await context.Features.FindAsync(id);

            context.Features.Remove(value);
            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
