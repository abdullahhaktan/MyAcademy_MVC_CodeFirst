namespace MyAcademy_MVC_CodeFirst.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class migIDK : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ContactMessages", "IsReplied", c => c.Boolean());
        }

        public override void Down()
        {
            AlterColumn("dbo.ContactMessages", "IsReplied", c => c.Boolean(nullable: false));
        }
    }
}
