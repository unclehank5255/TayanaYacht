namespace TayanaYacht.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveNewsBannerPathRequire : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.News", "NewsCoverPath", c => c.String(maxLength: 255));
            AlterColumn("dbo.NewsPages", "NewsBannerPath", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.NewsPages", "NewsBannerPath", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.News", "NewsCoverPath", c => c.String(nullable: false, maxLength: 255));
        }
    }
}
