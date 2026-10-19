namespace TayanaYacht.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewsIsPublished : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.News", "IsPublished", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.News", "IsPublished");
        }
    }
}
