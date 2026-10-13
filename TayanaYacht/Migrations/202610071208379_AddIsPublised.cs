namespace TayanaYacht.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsPublised : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Yachts", "IsPublished", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Yachts", "IsPublished");
        }
    }
}
