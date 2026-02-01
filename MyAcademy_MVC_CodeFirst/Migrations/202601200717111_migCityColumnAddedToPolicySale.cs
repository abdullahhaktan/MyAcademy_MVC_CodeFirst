namespace MyAcademy_MVC_CodeFirst.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class migCityColumnAddedToPolicySale : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PolicySales", "City", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.PolicySales", "City");
        }
    }
}
