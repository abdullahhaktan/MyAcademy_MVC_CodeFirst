using MyAcademy_MVC_CodeFirst.Data.Entities;
using System.Data.Entity;

namespace MyAcademy_MVC_CodeFirst.Data.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<About> Abouts { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Faq> Faqs { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Policy> Policies { get; set; }
        public DbSet<PolicySale> PolicySales { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<AdminLog> AdminLogs { get; set; }
        public DbSet<CustomerLog> CustomerLogs { get; set; }
    }
}