namespace TayanaYacht.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedContactMessaColumnAndLegt : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ContactMessages", "Email", c => c.String(nullable: false, maxLength: 254));
            AlterColumn("dbo.ContactMessages", "YachtNameSnapshot", c => c.String(nullable: false, maxLength: 2000));
            AlterColumn("dbo.ContactMessages", "Comment", c => c.String(nullable: false, maxLength: 100));
            DropColumn("dbo.ContactMessages", "AcceptedPrivacy");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ContactMessages", "AcceptedPrivacy", c => c.Boolean(nullable: false));
            AlterColumn("dbo.ContactMessages", "Comment", c => c.String(nullable: false));
            AlterColumn("dbo.ContactMessages", "YachtNameSnapshot", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.ContactMessages", "Email", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
