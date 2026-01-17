using MyAcademy_MVC_CodeFirst.Data.Context;
using MyAcademy_MVC_CodeFirst.DTOs.AboutDtos;
using MyAcademy_MVC_CodeFirst.DTOs.BlogDtos;
using MyAcademy_MVC_CodeFirst.DTOs.FaqDtos;
using MyAcademy_MVC_CodeFirst.DTOs.FeatureDtos;
using MyAcademy_MVC_CodeFirst.DTOs.PolicyDtos;
using MyAcademy_MVC_CodeFirst.DTOs.TeamDtos;
using MyAcademy_MVC_CodeFirst.DTOs.TestimonialDtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace YourProject.Controllers
{
    [AllowAnonymous]
    public class DefaultController : Controller
    {
        AppDbContext context = new AppDbContext();
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult HeadPartial()
        {
            return PartialView();
        }

        public PartialViewResult SpinnerPartial()
        {
            return PartialView();
        }

        public PartialViewResult TopbarPartial()
        {
            return PartialView();
        }

        public async Task<PartialViewResult> NavbarPartial()
        {
            return PartialView();
        }

        public PartialViewResult SearchModalPartial()
        {
            return PartialView();
        }

        public PartialViewResult CarouselPartial()
        {
            return PartialView();
        }

        public PartialViewResult FeaturePartial()
        {
            var values = context.Features.ToList();
            var features = MyAcademy_MVC_CodeFirst.MvcApplication.mapperInstance.Map<List<ResultFeatureDto>>(values);
            return PartialView(features);
        }

        public PartialViewResult AboutPartial()
        {
            var value = context.Abouts.FirstOrDefault();
            var about = MyAcademy_MVC_CodeFirst.MvcApplication.mapperInstance.Map<ResultAboutDto>(value);
            return PartialView(about);
        }

        public PartialViewResult ServicePartial()
        {
            var values = context.Policies.ToList();
            var policies = MyAcademy_MVC_CodeFirst.MvcApplication.mapperInstance.Map<List<ResultPolicyDto>>(values);
            return PartialView(policies);
        }

        public PartialViewResult FaqPartial()
        {
            var values = context.Faqs.ToList();
            var faqs = MyAcademy_MVC_CodeFirst.MvcApplication.mapperInstance.Map<List<ResultFaqDto>>(values);
            return PartialView(faqs);
        }

        public PartialViewResult BlogPartial()
        {
            var values = context.Blogs.ToList();
            var blogs = MyAcademy_MVC_CodeFirst.MvcApplication.mapperInstance.Map<List<ResultBlogDto>>(values);
            return PartialView(blogs);
        }

        public PartialViewResult TeamPartial()
        {
            var values = context.Teams.ToList();
            var teams = MyAcademy_MVC_CodeFirst.MvcApplication.mapperInstance.Map<List<ResultTeamDto>>(values);
            return PartialView(teams);
        }

        public PartialViewResult TestimonialPartial()
        {
            var values = context.Testimonials.ToList();
            var testimonials = MyAcademy_MVC_CodeFirst.MvcApplication.mapperInstance.Map<List<ResultTestimonialDto>>(values);
            return PartialView(testimonials);
        }

        public PartialViewResult FooterPartial()
        {
            return PartialView();
        }

        public PartialViewResult CopyrightPartial()
        {
            return PartialView();
        }

        public PartialViewResult BackToTopPartial()
        {
            return PartialView();
        }

        public PartialViewResult ScriptPartial()
        {
            return PartialView();
        }
    }
}
