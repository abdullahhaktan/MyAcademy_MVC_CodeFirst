namespace MyAcademy_MVC_CodeFirst.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class migUpdateTestimonial1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Testimonials", "FullName", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.Testimonials", "FullName");
        }
    }
}
