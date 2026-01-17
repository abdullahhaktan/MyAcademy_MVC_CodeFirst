namespace MyAcademy_MVC_CodeFirst.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class relatedColumnsAddedToPolicy : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Policies", "StartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Policies", "EndDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Policies", "PremiumAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Policies", "Description", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.Policies", "Description");
            DropColumn("dbo.Policies", "PremiumAmount");
            DropColumn("dbo.Policies", "EndDate");
            DropColumn("dbo.Policies", "StartDate");
        }
    }
}
