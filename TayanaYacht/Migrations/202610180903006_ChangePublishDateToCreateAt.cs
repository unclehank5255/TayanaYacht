namespace TayanaYacht.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangePublishDateToCreateAt : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.News", "PublishDate", "CreatedAt");
        }
        
        public override void Down()
        {
            RenameColumn("dbo.News", "CreatedAt", "PublishDate");
        }
    }
}
