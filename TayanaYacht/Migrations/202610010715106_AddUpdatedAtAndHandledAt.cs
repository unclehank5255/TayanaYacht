namespace TayanaYacht.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUpdatedAtAndHandledAt : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContactMessages", "HandledAt", c => c.DateTime());
            AddColumn("dbo.News", "UpdatedAt", c => c.DateTime(nullable: false, defaultValueSql: "GETDATE()"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.News", "UpdatedAt");
            DropColumn("dbo.ContactMessages", "HandledAt");
        }
    }
}
