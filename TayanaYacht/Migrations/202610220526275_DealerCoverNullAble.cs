namespace TayanaYacht.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DealerCoverNullAble : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Dealers", "DealerCoverPath", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Dealers", "DealerCoverPath", c => c.String(nullable: false, maxLength: 255));
        }
    }
}
