namespace TayanaYacht.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnContactMessage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContactMessages", "CountryId", c => c.Int(nullable: false));
            AddColumn("dbo.ContactMessages", "CountryNameSnapshot", c => c.String(nullable: false, maxLength: 60));
            CreateIndex("dbo.ContactMessages", "CountryId");
            AddForeignKey("dbo.ContactMessages", "CountryId", "dbo.Countries", "Id", cascadeDelete: false);
            DropColumn("dbo.ContactMessages", "Country");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ContactMessages", "Country", c => c.String(nullable: false, maxLength: 60));
            DropForeignKey("dbo.ContactMessages", "CountryId", "dbo.Countries");
            DropIndex("dbo.ContactMessages", new[] { "CountryId" });
            DropColumn("dbo.ContactMessages", "CountryNameSnapshot");
            DropColumn("dbo.ContactMessages", "CountryId");
        }
    }
}
