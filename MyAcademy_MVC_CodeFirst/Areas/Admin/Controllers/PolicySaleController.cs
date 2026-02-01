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
        // Database context for policy sale operations
        private readonly AppDbContext context = new AppDbContext();

        // Helper method to load policies for dropdown selection
        private async Task GetPoliciesAsync(int? selectedId = null)
        {
            var policies = await context.Policies.ToListAsync();

            ViewBag.policies = (from policy in policies
                                select new SelectListItem
                                {
                                    Text = policy.PolicyNumber, // Display text
                                    Value = policy.Id.ToString(), // Underlying value
                                    Selected = selectedId != null && policy.Id == selectedId // Preselect if editing
                                }).ToList();
        }

        // Helper method to load customers for dropdown selection
        private async Task GetCustomersAsync(int? selectedId = null)
        {
            var customers = await context.Customers.ToListAsync();

            ViewBag.customers = (from customer in customers
                                 select new SelectListItem
                                 {
                                     Text = customer.FullName + " " + customer.Email, // Combined display
                                     Value = customer.Id.ToString(),
                                     Selected = selectedId != null && customer.Id == selectedId
                                 }).ToList();
        }

        // Display paginated list of policy sales
        public async Task<ActionResult> Index(int page = 1, int pageSize = 500)
        {
            // Base query with eager loading and filtering
            var query = context.PolicySales
                .AsNoTracking() // Read-only optimization
                .Where(ps => ps.IsActive) // Only active sales
                .Include(ps => ps.Policy) // Load related policy
                .Include(ps => ps.Customer) // Load related customer
                .OrderByDescending(ps => ps.Id); // Most recent first

            // Get total count for pagination
            var totalCount = await query.CountAsync();

            // Get paginated results
            var values = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Map entities to DTOs
            var valuesDto = MvcApplication.mapperInstance
                .Map<List<ResultPolicySaleDto>>(values);

            // Create static paged list for display
            var policySales = new StaticPagedList<ResultPolicySaleDto>(
                valuesDto, page, pageSize, totalCount
            );

            // Configure pagination display options
            ViewBag.PaginationOptions = new PagedListRenderOptions
            {
                DisplayLinkToPreviousPage = PagedListDisplayMode.Always,
                DisplayLinkToNextPage = PagedListDisplayMode.Always,
                MaximumPageNumbersToDisplay = 5, // Show 5 page numbers
                LinkToPreviousPageFormat = "‹", // Custom previous arrow
                LinkToNextPageFormat = "›" // Custom next arrow
            };

            return View(policySales);
        }

        // Display form to create new policy sale
        [HttpGet]
        public async Task<ActionResult> CreatePolicySale()
        {
            // Load dropdown data
            await GetPoliciesAsync();
            await GetCustomersAsync();
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreatePolicySale(CreatePolicySaleDto createPolicySaleDto)
        {
            // Map DTO to entity
            var policySale = MvcApplication.mapperInstance
                .Map<PolicySale>(createPolicySaleDto);

            policySale.IsActive = true; // Set as active

            context.PolicySales.Add(policySale);
            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // Display form to update existing policy sale
        [HttpGet]
        public async Task<ActionResult> UpdatePolicySale(int id)
        {
            var value = await context.PolicySales.FindAsync(id);

            // Load dropdowns with preselected values
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

            // Map updated values to existing entity
            MvcApplication.mapperInstance
                .Map(updatePolicySaleDto, value);

            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // Soft delete policy sale (mark as inactive)
        public async Task<ActionResult> DeletePolicySale(int id)
        {
            var value = await context.PolicySales.FindAsync(id);

            value.IsActive = false; // Soft delete

            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}