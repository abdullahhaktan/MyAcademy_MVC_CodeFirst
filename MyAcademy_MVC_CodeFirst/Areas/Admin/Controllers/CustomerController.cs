using MyAcademy_MVC_CodeFirst.Data.Context;
using MyAcademy_MVC_CodeFirst.Data.Entities;
using MyAcademy_MVC_CodeFirst.DTOs.CustomerDtos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyAcademy_MVC_CodeFirst.Areas.Admin.Controllers
{
    [RouteArea("Admin")]
    public class CustomerController : Controller
    {
        private readonly AppDbContext context = new AppDbContext();

        // LIST
        public async Task<ActionResult> Index()
        {
            var values = await context.Customers.Where(c => c.IsActive == true).ToListAsync();

            var customers = MyAcademy_MVC_CodeFirst.MvcApplication
                .mapperInstance
                .Map<List<ResultCustomerDto>>(values);

            return View(customers);
        }

        // CREATE
        [HttpGet]
        public ActionResult CreateCustomer()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateCustomer(CreateCustomerDto createCustomerDto)
        {
            createCustomerDto.RegisteredAt = DateTime.Now;
            createCustomerDto.IsActive = true;
            var customer = MyAcademy_MVC_CodeFirst.MvcApplication
                .mapperInstance
                .Map<Customer>(createCustomerDto);


            context.Customers.Add(customer);
            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // UPDATE (GET)
        [HttpGet]
        public async Task<ActionResult> UpdateCustomer(int id)
        {
            var value = await context.Customers.FindAsync(id);

            var customer = MyAcademy_MVC_CodeFirst.MvcApplication
                .mapperInstance
                .Map<GetCustomerByIdDto>(value);

            return View(customer);
        }

        // UPDATE (POST)
        [HttpPost]
        public async Task<ActionResult> UpdateCustomer(UpdateCustomerDto updateCustomerDto)
        {
            var customer = await context.Customers.FindAsync(updateCustomerDto.Id);

            MyAcademy_MVC_CodeFirst.MvcApplication
                .mapperInstance
                .Map(updateCustomerDto, customer);

            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // DELETE
        public async Task<ActionResult> DeleteCustomer(int id)
        {
            var customer = await context.Customers.Include(c => c.PolicySales)
                .FirstOrDefaultAsync(c => c.Id == id);

            customer.IsActive = false;

            foreach (var policySale in customer.PolicySales)
            {
                policySale.IsActive = true;
            }

            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
