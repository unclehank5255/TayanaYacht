namespace TayanaYacht.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPublishDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.News", "PublishDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.News", "PublishDate");
        }
    }
}
