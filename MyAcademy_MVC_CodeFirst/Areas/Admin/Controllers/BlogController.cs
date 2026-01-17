using MyAcademy_MVC_CodeFirst.Data.Context;
using MyAcademy_MVC_CodeFirst.Data.Entities;
using MyAcademy_MVC_CodeFirst.DTOs.BlogDtos;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyAcademy_MVC_CodeFirst.Areas.Admin.Controllers
{
    [RouteArea("Admin")]
    public class BlogController : Controller
    {
        private readonly AppDbContext context = new AppDbContext();

        // LIST
        public async Task<ActionResult> Index()
        {
            var values = await context.Blogs.ToListAsync();

            var blogs = MvcApplication.mapperInstance
                .Map<List<ResultBlogDto>>(values);

            return View(blogs);
        }

        // CREATE (GET)
        [HttpGet]
        public ActionResult CreateBlog()
        {
            return View();
        }

        // CREATE (POST)
        [HttpPost]
        public async Task<ActionResult> CreateBlog(CreateBlogDto createBlogDto)
        {
            var blog = MvcApplication.mapperInstance
                .Map<Blog>(createBlogDto);

            context.Blogs.Add(blog);
            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // UPDATE (GET)
        [HttpGet]
        public async Task<ActionResult> UpdateBlog(int id)
        {
            var value = await context.Blogs.FindAsync(id);

            var blog = MvcApplication.mapperInstance
                .Map<GetBlogByIdDto>(value);

            return View(blog);
        }

        // UPDATE (POST)
        [HttpPost]
        public async Task<ActionResult> UpdateBlog(UpdateBlogDto updateBlogDto)
        {
            var value = await context.Blogs.FindAsync(updateBlogDto.Id);

            // DTO → Tracked Entity
            MvcApplication.mapperInstance
                .Map(updateBlogDto, value);

            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // DELETE
        public async Task<ActionResult> DeleteBlog(int id)
        {
            var value = await context.Blogs.FindAsync(id);

            context.Blogs.Remove(value);
            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
