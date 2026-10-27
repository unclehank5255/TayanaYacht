namespace TayanaYacht.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChancelYachtOrviewPricinpleDimensionSpectifiactionHtmlRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Yachts", "OverviewHtml", c => c.String());
            AlterColumn("dbo.Yachts", "PrincipleDimensionHtml", c => c.String());
            AlterColumn("dbo.Yachts", "SpecificationHtml", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Yachts", "SpecificationHtml", c => c.String(nullable: false));
            AlterColumn("dbo.Yachts", "PrincipleDimensionHtml", c => c.String(nullable: false));
            AlterColumn("dbo.Yachts", "OverviewHtml", c => c.String(nullable: false));
        }
    }
}
