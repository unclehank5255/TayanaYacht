namespace TayanaYacht.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameFileOriginNametoFileDisplayName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.YachtFiles", "FileDisplayName", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.YachtFiles", "FileOriginalName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.YachtFiles", "FileOriginalName", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.YachtFiles", "FileDisplayName");
        }
    }
}
