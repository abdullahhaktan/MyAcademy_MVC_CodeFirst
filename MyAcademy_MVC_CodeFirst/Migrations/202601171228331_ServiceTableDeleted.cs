namespace MyAcademy_MVC_CodeFirst.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class ServiceTableDeleted : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Services");
        }

        public override void Down()
        {
            CreateTable(
                "dbo.Services",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Title = c.String(),
                    ImageUrl = c.String(),
                    Icon = c.String(),
                    Description = c.String(),
                    Description1 = c.String(),
                })
                .PrimaryKey(t => t.Id);

        }
    }
}
