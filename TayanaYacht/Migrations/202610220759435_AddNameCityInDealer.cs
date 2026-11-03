namespace TayanaYacht.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNameCityInDealer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Dealers", "Name", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.Dealers", "City", c => c.String(nullable: false, maxLength: 60));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Dealers", "City");
            DropColumn("dbo.Dealers", "Name");
        }
    }
}
