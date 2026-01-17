namespace MyAcademy_MVC_CodeFirst.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class migInitial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdminLogs",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    AdminUsername = c.String(),
                    Action = c.String(),
                    IpAddress = c.String(),
                    TimeStamp = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.ContactMessages",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    SenderEmail = c.String(),
                    MessageBody = c.String(),
                    Category = c.String(),
                    DetectedLanguage = c.String(),
                    IsReplied = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Customers",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    FullName = c.String(),
                    City = c.String(),
                    Email = c.String(),
                    RegisteredAt = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Policies",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    PolicyNumber = c.String(),
                    InsuranceType = c.String(),
                    IsActive = c.Boolean(nullable: false),
                    CustomerId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);

            CreateTable(
                "dbo.PolicySales",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    PolicyId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Policies", t => t.PolicyId, cascadeDelete: true)
                .Index(t => t.PolicyId);

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

        public override void Down()
        {
            DropForeignKey("dbo.PolicySales", "PolicyId", "dbo.Policies");
            DropForeignKey("dbo.Policies", "CustomerId", "dbo.Customers");
            DropIndex("dbo.PolicySales", new[] { "PolicyId" });
            DropIndex("dbo.Policies", new[] { "CustomerId" });
            DropTable("dbo.InsurancePackages");
            DropTable("dbo.PolicySales");
            DropTable("dbo.Policies");
            DropTable("dbo.Customers");
            DropTable("dbo.ContactMessages");
            DropTable("dbo.AdminLogs");
        }
    }
}
