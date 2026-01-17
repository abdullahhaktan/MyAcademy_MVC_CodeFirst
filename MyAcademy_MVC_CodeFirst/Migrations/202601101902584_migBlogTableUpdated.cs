namespace MyAcademy_MVC_CodeFirst.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class migBlogTableUpdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Blogs", "Subject", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.Blogs", "Subject");
        }
    }
}
