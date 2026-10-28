namespace TayanaYacht.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateContentHtmlNotRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.News", "ContentHtml", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.News", "ContentHtml", c => c.String(nullable: false));
        }
    }
}
