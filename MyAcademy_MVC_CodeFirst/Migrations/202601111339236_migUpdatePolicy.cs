namespace MyAcademy_MVC_CodeFirst.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class migUpdatePolicy : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Policies", "Description1", c => c.String());
            AddColumn("dbo.Policies", "ImageUrl", c => c.String());
            AddColumn("dbo.Policies", "Icon", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.Policies", "Icon");
            DropColumn("dbo.Policies", "ImageUrl");
            DropColumn("dbo.Policies", "Description1");
        }
    }
}
