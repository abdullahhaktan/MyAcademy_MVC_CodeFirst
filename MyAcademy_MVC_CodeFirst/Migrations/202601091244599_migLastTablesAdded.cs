namespace MyAcademy_MVC_CodeFirst.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class migLastTablesAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Abouts",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Title = c.String(),
                    Description = c.String(),
                    Description1 = c.String(),
                    item = c.String(),
                    item1 = c.String(),
                    item2 = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Blogs",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Title = c.String(),
                    Description = c.String(),
                    ImageUrl = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Faqs",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Question = c.String(),
                    Answer = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Features",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Title = c.String(),
                    Icon = c.String(),
                    Description = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Services",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Title = c.String(),
                    ImageUrl = c.String(),
                    Description = c.String(),
                    Description1 = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Teams",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    FullName = c.String(),
                    Job = c.String(),
                    ImageUrl = c.String(),
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropTable("dbo.Teams");
            DropTable("dbo.Services");
            DropTable("dbo.Features");
            DropTable("dbo.Faqs");
            DropTable("dbo.Blogs");
            DropTable("dbo.Abouts");
        }
    }
}
