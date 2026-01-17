namespace MyAcademy_MVC_CodeFirst.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class migTestimonialTableAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Testimonials",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Job = c.String(),
                    StarCount = c.Int(nullable: false),
                    Description = c.String(),
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropTable("dbo.Testimonials");
        }
    }
}
