namespace iManual.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class subcategoryuserdatetype : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Articles", new[] { "User_Id" });
            DropColumn("dbo.Articles", "UserId");
            RenameColumn(table: "dbo.Articles", name: "User_Id", newName: "UserId");
            AlterColumn("dbo.Articles", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Articles", "UserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Articles", new[] { "UserId" });
            AlterColumn("dbo.Articles", "UserId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Articles", name: "UserId", newName: "User_Id");
            AddColumn("dbo.Articles", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Articles", "User_Id");
        }
    }
}
