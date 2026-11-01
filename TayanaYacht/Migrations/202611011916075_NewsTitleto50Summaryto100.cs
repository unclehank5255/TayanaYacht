namespace TayanaYacht.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewsTitleto50Summaryto100 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.News", "Title", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.News", "Summary", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.News", "Summary", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.News", "Title", c => c.String(nullable: false, maxLength: 25));
        }
    }
}
