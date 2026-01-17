namespace MyAcademy_MVC_CodeFirst.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class migCustomerLogTableAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerLogs",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Email = c.String(),
                    Action = c.String(),
                    IpAddress = c.String(),
                    LogType = c.String(),
                    Description = c.String(),
                    CreatedDate = c.DateTime(),
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropTable("dbo.CustomerLogs");
        }
    }
}
