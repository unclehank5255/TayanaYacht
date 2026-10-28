namespace TayanaYacht.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DealerContacentHtmlNotRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Dealers", "ContentHtml", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Dealers", "ContentHtml", c => c.String(nullable: false));
        }
    }
}
