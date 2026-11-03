namespace TayanaYacht.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovePrivacyVersionFromContact : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Contacts", "PrivacyVersionId", "dbo.PrivacyVersions");
            DropIndex("dbo.Contacts", new[] { "PrivacyVersionId" });
            DropColumn("dbo.Contacts", "PrivacyVersionId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Contacts", "PrivacyVersionId", c => c.Int(nullable: false));
            CreateIndex("dbo.Contacts", "PrivacyVersionId");
            AddForeignKey("dbo.Contacts", "PrivacyVersionId", "dbo.PrivacyVersions", "Id");
        }
    }
}
