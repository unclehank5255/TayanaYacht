namespace TayanaYacht.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameColumnStoredFilenameAndDispalyFileName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NewsFiles", "DisplayFileName", c => c.String(nullable: false, maxLength: 255));
            DropColumn("dbo.NewsFiles", "OriginalFileName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.NewsFiles", "OriginalFileName", c => c.String(nullable: false, maxLength: 255));
            DropColumn("dbo.NewsFiles", "DisplayFileName");

        }
    }
}
