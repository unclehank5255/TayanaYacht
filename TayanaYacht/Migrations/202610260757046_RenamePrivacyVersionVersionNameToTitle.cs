namespace TayanaYacht.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenamePrivacyVersionVersionNameToTitle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PrivacyVersions", "Title", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.PrivacyVersions", "Version");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PrivacyVersions", "Version", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.PrivacyVersions", "Title");
        }
    }
}
