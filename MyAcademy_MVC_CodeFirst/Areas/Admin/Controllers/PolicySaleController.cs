using MyAcademy_MVC_CodeFirst.Data.Context;
using MyAcademy_MVC_CodeFirst.Data.Entities;
using MyAcademy_MVC_CodeFirst.DTOs.PolicySaleDtos;
using PagedList;
using PagedList.Mvc;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyAcademy_MVC_CodeFirst.Areas.Admin.Controllers
{
    [RouteArea("Admin")]
    public class PolicySaleController : Controller
    {
        private readonly AppDbContext context = new AppDbContext();

        private async Task GetPoliciesAsync(int? selectedId = null)
        {
            var policies = await context.Policies.ToListAsync();

            ViewBag.policies = (from policy in policies
                                select new SelectListItem
                                {
                                    Text = policy.PolicyNumber,
                                    Value = policy.Id.ToString(),
                                    Selected = selectedId != null && policy.Id == selectedId
                                }).ToList();
        }

        private async Task GetCustomersAsync(int? selectedId = null)
        {
            var customers = await context.Customers.ToListAsync();

            ViewBag.customers = (from customer in customers
                                 select new SelectListItem
                                 {
                                     Text = customer.FullName + " " + customer.Email,
                                     Value = customer.Id.ToString(),
                                     Selected = selectedId != null && customer.Id == selectedId
                                 }).ToList();
        }


        public async Task<ActionResult> Index(int page = 1, int pageSize = 500)
        {
            var query = context.PolicySales
                .AsNoTracking()
                .Where(ps => ps.IsActive)
                .Include(ps => ps.Policy)
                .Include(ps => ps.Customer)
                .OrderByDescending(ps => ps.Id);

            var totalCount = await query.CountAsync();

            var values = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var valuesDto = MvcApplication.mapperInstance
                .Map<List<ResultPolicySaleDto>>(values);

            var policySales = new StaticPagedList<ResultPolicySaleDto>(
                valuesDto, page, pageSize, totalCount
            );

            ViewBag.PaginationOptions = new PagedListRenderOptions
            {
                DisplayLinkToPreviousPage = PagedListDisplayMode.Always,
                DisplayLinkToNextPage = PagedListDisplayMode.Always,
                MaximumPageNumbersToDisplay = 5,
                LinkToPreviousPageFormat = "‹",
                LinkToNextPageFormat = "›"
            };

            return View(policySales);
        }


        [HttpGet]
        public async Task<ActionResult> CreatePolicySale()
        {
            await GetPoliciesAsync();
            await GetCustomersAsync();
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreatePolicySale(CreatePolicySaleDto createPolicySaleDto)
        {
            var policySale = MvcApplication.mapperInstance
                .Map<PolicySale>(createPolicySaleDto);

            policySale.IsActive = true;

            context.PolicySales.Add(policySale);

            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> UpdatePolicySale(int id)
        {
            var value = await context.PolicySales.FindAsync(id);

            await GetPoliciesAsync(value.PolicyId);
            await GetCustomersAsync(value.CustomerId);

            var dto = MvcApplication.mapperInstance
                .Map<GetPolicySaleByIdDto>(value);

            return View(dto);
        }

        [HttpPost]
        public async Task<ActionResult> UpdatePolicySale(UpdatePolicySaleDto updatePolicySaleDto)
        {
            var value = await context.PolicySales.FindAsync(updatePolicySaleDto.Id);

            MvcApplication.mapperInstance
                .Map(updatePolicySaleDto, value);

            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> DeletePolicySale(int id)
        {
            var value = await context.PolicySales.FindAsync(id);

            value.IsActive = false;

            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
