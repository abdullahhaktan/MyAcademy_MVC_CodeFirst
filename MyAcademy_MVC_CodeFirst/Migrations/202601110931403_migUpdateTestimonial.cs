namespace MyAcademy_MVC_CodeFirst.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class migUpdateTestimonial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Testimonials", "ImageUrl", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.Testimonials", "ImageUrl");
        }
    }
}
