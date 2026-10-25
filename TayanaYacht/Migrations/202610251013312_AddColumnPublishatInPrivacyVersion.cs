namespace TayanaYacht.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnPublishatInPrivacyVersion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PrivacyVersions", "PublishedAt", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PrivacyVersions", "PublishedAt");
        }
    }
}
