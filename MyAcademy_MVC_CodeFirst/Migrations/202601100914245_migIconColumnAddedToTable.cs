namespace MyAcademy_MVC_CodeFirst.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class migIconColumnAddedToTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Services", "Icon", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.Services", "Icon");
        }
    }
}
