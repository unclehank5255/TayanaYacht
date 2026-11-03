namespace TayanaYacht.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveColumnPublishAtNews : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.News", "IsPublished");
        }
        
        public override void Down()
        {
            AddColumn("dbo.News", "IsPublished", c => c.Boolean(nullable: false));
        }
    }
}
