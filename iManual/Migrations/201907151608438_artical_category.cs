namespace iManual.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class artical_category : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArticleContents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        FileName = c.String(),
                        FullPath = c.String(),
                        HrefName = c.String(),
                        Order = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        ArticleId = c.Int(nullable: false),
                        UploadedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Articles", t => t.ArticleId, cascadeDelete: true)
                .Index(t => t.ArticleId);
            
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        Status = c.Int(nullable: false),
                        PublishStatus = c.Int(nullable: false),
                        SubCategoryId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SubCategories", t => t.SubCategoryId, cascadeDelete: true)
                .Index(t => t.SubCategoryId);
            
            CreateTable(
                "dbo.SubCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Status = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        MainCategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MainCategories", t => t.MainCategoryId, cascadeDelete: true)
                .Index(t => t.MainCategoryId);
            
            CreateTable(
                "dbo.MainCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Active = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ArticleContents", "ArticleId", "dbo.Articles");
            DropForeignKey("dbo.Articles", "SubCategoryId", "dbo.SubCategories");
            DropForeignKey("dbo.SubCategories", "MainCategoryId", "dbo.MainCategories");
            DropIndex("dbo.SubCategories", new[] { "MainCategoryId" });
            DropIndex("dbo.Articles", new[] { "SubCategoryId" });
            DropIndex("dbo.ArticleContents", new[] { "ArticleId" });
            DropTable("dbo.MainCategories");
            DropTable("dbo.SubCategories");
            DropTable("dbo.Articles");
            DropTable("dbo.ArticleContents");
        }
    }
}
