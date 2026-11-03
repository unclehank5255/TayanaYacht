namespace TayanaYacht.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameColumnNameContact : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contacts", "MapEmbedUrl", c => c.String(nullable: false, maxLength: 2000));
            AlterColumn("dbo.ContactMessages", "SubmittedAt", c => c.DateTime(nullable: false, defaultValueSql: "GETDATE()"));
            AlterColumn("dbo.PrivacyVersions", "CreatedAt", c => c.DateTime(nullable: false, defaultValueSql: "GETDATE()"));
            AlterColumn("dbo.Contacts", "ContactBannerPath", c => c.String(maxLength: 255));
            DropColumn("dbo.Contacts", "MapImagePath");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Contacts", "MapImagePath", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Contacts", "ContactBannerPath", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.PrivacyVersions", "CreatedAt", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ContactMessages", "SubmittedAt", c => c.DateTime(nullable: false));
            DropColumn("dbo.Contacts", "MapEmbedUrl");
        }
    }
}
