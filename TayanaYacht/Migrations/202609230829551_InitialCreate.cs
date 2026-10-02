namespace TayanaYacht.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ContactMessages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Email = c.String(nullable: false, maxLength: 50),
                        Phone = c.String(nullable: false, maxLength: 30),
                        Country = c.String(nullable: false, maxLength: 60),
                        YachtId = c.Int(nullable: false),
                        YachtNameSnapshot = c.String(nullable: false, maxLength: 100),
                        Comment = c.String(nullable: false),
                        AcceptedPrivacy = c.Boolean(nullable: false),
                        PrivacyVersionId = c.Int(nullable: false),
                        SubmittedAt = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PrivacyVersions", t => t.PrivacyVersionId)
                .ForeignKey("dbo.Yachts", t => t.YachtId)
                .Index(t => t.YachtId)
                .Index(t => t.PrivacyVersionId);
            
            CreateTable(
                "dbo.PrivacyVersions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Version = c.String(nullable: false, maxLength: 50),
                        ContentHtml = c.String(nullable: false),
                        CreatedAt = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        ContactBannerPath = c.String(nullable: false, maxLength: 255),
                        MapImagePath = c.String(nullable: false, maxLength: 255),
                        ContentHtml = c.String(nullable: false),
                        PrivacyVersionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PrivacyVersions", t => t.PrivacyVersionId)
                .Index(t => t.PrivacyVersionId);
            
            CreateTable(
                "dbo.Yachts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SeriesName = c.String(nullable: false, maxLength: 50),
                        ModelName = c.String(nullable: false, maxLength: 50),
                        StatusText = c.String(maxLength: 50),
                        YachtBannerPath = c.String(nullable: false, maxLength: 255),
                        OverviewHtml = c.String(nullable: false),
                        PrincipleDimensionHtml = c.String(nullable: false),
                        SpecificationHtml = c.String(nullable: false),
                        DisplayOrder = c.Int(nullable: false),
                        DeletedAt = c.DateTime(),
                        IsLatest = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.YachtFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        YachtId = c.Int(nullable: false),
                        FileOriginalName = c.String(nullable: false, maxLength: 50),
                        StoredFileName = c.String(nullable: false, maxLength: 50),
                        FilePath = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Yachts", t => t.YachtId, cascadeDelete: true)
                .Index(t => t.YachtId);
            
            CreateTable(
                "dbo.YachtPictures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        YachtId = c.Int(nullable: false),
                        PictureType = c.Int(nullable: false),
                        ImagePath = c.String(nullable: false, maxLength: 255),
                        DisplayOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Yachts", t => t.YachtId, cascadeDelete: true)
                .Index(t => t.YachtId);
            
            CreateTable(
                "dbo.DealerPages",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        DealerBannerPath = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Dealers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Country = c.String(nullable: false, maxLength: 60),
                        DealerCoverPath = c.String(nullable: false, maxLength: 255),
                        ContentHtml = c.String(nullable: false),
                        DisplayOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.News",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 25),
                        Summary = c.String(nullable: false, maxLength: 50),
                        NewsCoverPath = c.String(nullable: false, maxLength: 255),
                        ContentHtml = c.String(nullable: false),
                        PublishDate = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                        DeletedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NewsDetailImages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NewsId = c.Int(nullable: false),
                        DetailImagePath = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.News", t => t.NewsId, cascadeDelete: true)
                .Index(t => t.NewsId);
            
            CreateTable(
                "dbo.NewsFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NewsId = c.Int(nullable: false),
                        OriginalFileName = c.String(nullable: false, maxLength: 255),
                        StoredFileName = c.String(nullable: false, maxLength: 255),
                        FilePath = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.News", t => t.NewsId, cascadeDelete: true)
                .Index(t => t.NewsId);
            
            CreateTable(
                "dbo.NewsPages",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        NewsBannerPath = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            Sql("ALTER TABLE dbo.NewsPages ADD CONSTRAINT CK_NewsPages_Id CHECK (Id = 1)");
            Sql("ALTER TABLE dbo.DealerPages ADD CONSTRAINT CK_DealerPages_Id CHECK (Id = 1)");
            Sql("ALTER TABLE dbo.Contacts ADD CONSTRAINT CK_Contacts_Id CHECK (Id = 1)");
        }
        
        public override void Down()
        {
            Sql("ALTER TABLE dbo.NewsPages DROP CONSTRAINT CK_NewsPages_Id");
            Sql("ALTER TABLE dbo.DealerPages DROP CONSTRAINT CK_DealerPages_Id");
            Sql("ALTER TABLE dbo.Contacts DROP CONSTRAINT CK_Contacts_Id");

            DropForeignKey("dbo.NewsFiles", "NewsId", "dbo.News");
            DropForeignKey("dbo.NewsDetailImages", "NewsId", "dbo.News");
            DropForeignKey("dbo.ContactMessages", "YachtId", "dbo.Yachts");
            DropForeignKey("dbo.YachtPictures", "YachtId", "dbo.Yachts");
            DropForeignKey("dbo.YachtFiles", "YachtId", "dbo.Yachts");
            DropForeignKey("dbo.ContactMessages", "PrivacyVersionId", "dbo.PrivacyVersions");
            DropForeignKey("dbo.Contacts", "PrivacyVersionId", "dbo.PrivacyVersions");
            DropIndex("dbo.NewsFiles", new[] { "NewsId" });
            DropIndex("dbo.NewsDetailImages", new[] { "NewsId" });
            DropIndex("dbo.YachtPictures", new[] { "YachtId" });
            DropIndex("dbo.YachtFiles", new[] { "YachtId" });
            DropIndex("dbo.Contacts", new[] { "PrivacyVersionId" });
            DropIndex("dbo.ContactMessages", new[] { "PrivacyVersionId" });
            DropIndex("dbo.ContactMessages", new[] { "YachtId" });
            DropTable("dbo.NewsPages");
            DropTable("dbo.NewsFiles");
            DropTable("dbo.NewsDetailImages");
            DropTable("dbo.News");
            DropTable("dbo.Dealers");
            DropTable("dbo.DealerPages");
            DropTable("dbo.YachtPictures");
            DropTable("dbo.YachtFiles");
            DropTable("dbo.Yachts");
            DropTable("dbo.Contacts");
            DropTable("dbo.PrivacyVersions");
            DropTable("dbo.ContactMessages");
        }
    }
}
