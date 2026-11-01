namespace TayanaYacht.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDealerUnpublishedAt : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Dealers", "UnpublishedAt", c => c.DateTime());
            Sql("UPDATE dbo.Dealers SET UnpublishedAt = GETDATE() WHERE DealerCoverPath IS NULL OR DealerCoverPath = ''");
        }

        public override void Down()
        {
            DropColumn("dbo.Dealers", "UnpublishedAt");
        }
    }
}
