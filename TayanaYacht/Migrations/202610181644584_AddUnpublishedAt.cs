namespace TayanaYacht.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUnpublishedAt : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.News", "UnpublishedAt", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.News", "UnpublishedAt");
        }
    }
}
