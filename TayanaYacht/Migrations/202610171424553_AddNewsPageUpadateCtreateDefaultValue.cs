namespace TayanaYacht.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewsPageUpadateCtreateDefaultValue : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.News", "PublishDate", c => c.DateTime(nullable: false, defaultValueSql: "GETDATE()"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.News", "PublishDate", c => c.DateTime(nullable: false));
        }
    }
}
