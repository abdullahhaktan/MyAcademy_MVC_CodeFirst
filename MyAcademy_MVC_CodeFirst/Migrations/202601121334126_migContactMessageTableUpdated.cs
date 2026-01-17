namespace MyAcademy_MVC_CodeFirst.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class migContactMessageTableUpdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContactMessages", "SenderFullName", c => c.String());
            AddColumn("dbo.ContactMessages", "MessageSubject", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.ContactMessages", "MessageSubject");
            DropColumn("dbo.ContactMessages", "SenderFullName");
        }
    }
}
