namespace iManual.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class artical_category1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "UserId", c => c.Int(nullable: false));
            AddColumn("dbo.Articles", "User_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Articles", "User_Id");
            AddForeignKey("dbo.Articles", "User_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Articles", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Articles", new[] { "User_Id" });
            DropColumn("dbo.Articles", "User_Id");
            DropColumn("dbo.Articles", "UserId");
        }
    }
}
