namespace TayanaYacht.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpadatePublishDateNullAble : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.News", "PublishDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.News", "PublishDate", c => c.DateTime(nullable: false));
        }
    }
}
