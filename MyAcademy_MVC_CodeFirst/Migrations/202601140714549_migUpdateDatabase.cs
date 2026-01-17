namespace MyAcademy_MVC_CodeFirst.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class migUpdateDatabase : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AdminLogs", "LogType", c => c.String());
            AddColumn("dbo.AdminLogs", "Description", c => c.String());
            AddColumn("dbo.AdminLogs", "CreatedDate", c => c.DateTime());
            DropColumn("dbo.AdminLogs", "TimeStamp");
        }

        public override void Down()
        {
            AddColumn("dbo.AdminLogs", "TimeStamp", c => c.DateTime(nullable: false));
            DropColumn("dbo.AdminLogs", "CreatedDate");
            DropColumn("dbo.AdminLogs", "Description");
            DropColumn("dbo.AdminLogs", "LogType");
        }
    }
}
