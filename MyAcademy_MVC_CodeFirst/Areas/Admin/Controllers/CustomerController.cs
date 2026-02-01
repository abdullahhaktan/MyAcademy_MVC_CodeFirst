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
        // Database context for customer operations
        private readonly AppDbContext context = new AppDbContext();

        // LIST - Display all active customers
        public async Task<ActionResult> Index()
        {
            // Get only active customers from database
            var values = await context.Customers.Where(c => c.IsActive == true).ToListAsync();

            // Map Customer entities to ResultCustomerDto objects using AutoMapper
            var customers = MyAcademy_MVC_CodeFirst.MvcApplication
                .mapperInstance
                .Map<List<ResultCustomerDto>>(values);

            return View(customers);
        }

        // CREATE - Display customer creation form
        [HttpGet]
        public ActionResult CreateCustomer()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateCustomer(CreateCustomerDto createCustomerDto)
        {
            // Set default values for new customer
            createCustomerDto.RegisteredAt = DateTime.Now;
            createCustomerDto.IsActive = true;

            // Map DTO to Customer entity
            var customer = MyAcademy_MVC_CodeFirst.MvcApplication
                .mapperInstance
                .Map<Customer>(createCustomerDto);

            // Add customer to database and save changes
            context.Customers.Add(customer);
            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // UPDATE - Display customer edit form (GET)
        [HttpGet]
        public async Task<ActionResult> UpdateCustomer(int id)
        {
            // Find customer by ID
            var value = await context.Customers.FindAsync(id);

            // Map Customer entity to GetCustomerByIdDto for editing
            var customer = MyAcademy_MVC_CodeFirst.MvcApplication
                .mapperInstance
                .Map<GetCustomerByIdDto>(value);

            return View(customer);
        }

        // UPDATE - Process customer update (POST)
        [HttpPost]
        public async Task<ActionResult> UpdateCustomer(UpdateCustomerDto updateCustomerDto)
        {
            // Find existing customer
            var customer = await context.Customers.FindAsync(updateCustomerDto.Id);

            // Map updated values from DTO to existing entity
            MyAcademy_MVC_CodeFirst.MvcApplication
                .mapperInstance
                .Map(updateCustomerDto, customer);

            // Save changes to database
            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // DELETE - Soft delete customer (mark as inactive)
        public async Task<ActionResult> DeleteCustomer(int id)
        {
            // Find customer with related policy sales (eager loading)
            var customer = await context.Customers.Include(c => c.PolicySales)
                .FirstOrDefaultAsync(c => c.Id == id);

            // Soft delete: Mark customer as inactive
            customer.IsActive = false;

            // Keep related policy sales active
            foreach (var policySale in customer.PolicySales)
            {
                policySale.IsActive = true; // Maintain policy sales even if customer is deactivated
            }

            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}