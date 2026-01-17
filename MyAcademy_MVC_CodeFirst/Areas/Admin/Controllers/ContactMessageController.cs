using MyAcademy_MVC_CodeFirst.Data.Context;
using MyAcademy_MVC_CodeFirst.DTOs.ContactMessageDtos;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyAcademy_MVC_CodeFirst.Areas.Admin.Controllers
{
    [RouteArea("Admin")]
    public class ContactMessageController : Controller
    {
        private readonly AppDbContext context = new AppDbContext();

        // LIST
        public async Task<ActionResult> Index()
        {
            var values = await context.ContactMessages.ToListAsync();

            var messages = MyAcademy_MVC_CodeFirst.MvcApplication
                .mapperInstance
                .Map<List<ResultContactMessageDto>>(values);

            return View(messages);
        }
    }
}
