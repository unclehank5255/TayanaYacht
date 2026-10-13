namespace TayanaYacht.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedYachtBnner : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Yachts", "YachtBannerPath", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Yachts", "YachtBannerPath", c => c.String(nullable: false, maxLength: 255));
        }
    }
}
