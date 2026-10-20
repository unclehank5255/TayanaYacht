namespace TayanaYacht.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCountryAndAddForeignKey : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 60),
                        DisplayOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Dealers", "CountryId", c => c.Int(nullable: false));
            CreateIndex("dbo.Dealers", "CountryId");
            AddForeignKey("dbo.Dealers", "CountryId", "dbo.Countries", "Id", cascadeDelete: false);
            DropColumn("dbo.Dealers", "Country");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Dealers", "Country", c => c.String(nullable: false, maxLength: 60));
            DropForeignKey("dbo.Dealers", "CountryId", "dbo.Countries");
            DropIndex("dbo.Dealers", new[] { "CountryId" });
            DropColumn("dbo.Dealers", "CountryId");
            DropTable("dbo.Countries");
        }
    }
}
