namespace MyAcademy_MVC_CodeFirst.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CustomerTableUpdated : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Policies", "Customer_Id", "dbo.Customers");
            DropIndex("dbo.Policies", new[] { "Customer_Id" });
            DropColumn("dbo.Policies", "Customer_Id");
        }

        public override void Down()
        {
            AddColumn("dbo.Policies", "Customer_Id", c => c.Int());
            CreateIndex("dbo.Policies", "Customer_Id");
            AddForeignKey("dbo.Policies", "Customer_Id", "dbo.Customers", "Id");
        }
    }
}
