namespace MyAcademy_MVC_CodeFirst.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class migPolicyAndPolicySaleTablesUpdated : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Policies", "CustomerId", "dbo.Customers");
            DropIndex("dbo.Policies", new[] { "CustomerId" });
            RenameColumn(table: "dbo.Policies", name: "CustomerId", newName: "Customer_Id");
            AddColumn("dbo.PolicySales", "StartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.PolicySales", "EndDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Policies", "Customer_Id", c => c.Int());
            CreateIndex("dbo.Policies", "Customer_Id");
            AddForeignKey("dbo.Policies", "Customer_Id", "dbo.Customers", "Id");
            DropColumn("dbo.Policies", "StartDate");
            DropColumn("dbo.Policies", "EndDate");
        }

        public override void Down()
        {
            AddColumn("dbo.Policies", "EndDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Policies", "StartDate", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.Policies", "Customer_Id", "dbo.Customers");
            DropIndex("dbo.Policies", new[] { "Customer_Id" });
            AlterColumn("dbo.Policies", "Customer_Id", c => c.Int(nullable: false));
            DropColumn("dbo.PolicySales", "EndDate");
            DropColumn("dbo.PolicySales", "StartDate");
            RenameColumn(table: "dbo.Policies", name: "Customer_Id", newName: "CustomerId");
            CreateIndex("dbo.Policies", "CustomerId");
            AddForeignKey("dbo.Policies", "CustomerId", "dbo.Customers", "Id", cascadeDelete: true);
        }
    }
}
