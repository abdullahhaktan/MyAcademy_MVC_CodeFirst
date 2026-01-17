namespace MyAcademy_MVC_CodeFirst.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class migPolicySaleTableUpdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PolicySales", "CustomerId", c => c.Int(nullable: false));
            CreateIndex("dbo.PolicySales", "CustomerId");
            AddForeignKey("dbo.PolicySales", "CustomerId", "dbo.Customers", "Id", cascadeDelete: false);
        }

        public override void Down()
        {
            DropForeignKey("dbo.PolicySales", "CustomerId", "dbo.Customers");
            DropIndex("dbo.PolicySales", new[] { "CustomerId" });
            DropColumn("dbo.PolicySales", "CustomerId");
        }
    }
}
