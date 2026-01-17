namespace MyAcademy_MVC_CodeFirst.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class migInsurancePackageDeleted : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.InsurancePackages");
        }

        public override void Down()
        {
            CreateTable(
                "dbo.InsurancePackages",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    PackageName = c.String(),
                    Description = c.String(),
                    BasePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.Id);

        }
    }
}
