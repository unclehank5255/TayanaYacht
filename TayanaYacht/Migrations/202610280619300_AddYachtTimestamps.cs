namespace TayanaYacht.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddYachtTimestamps : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Yachts", "CreatedAt", c => c.DateTime(nullable: false, defaultValueSql: "GETDATE()"));
            AddColumn("dbo.Yachts", "UpdatedAt", c => c.DateTime(nullable: false, defaultValueSql: "GETDATE()"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Yachts", "UpdatedAt");
            DropColumn("dbo.Yachts", "CreatedAt");
        }
    }
}
