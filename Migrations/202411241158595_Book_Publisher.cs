namespace Asp_Web_Lib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Book_Publisher : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Books", "Id", "dbo.Publishers");
            DropForeignKey("dbo.BookAuthors", "BookId", "dbo.Books");
            DropForeignKey("dbo.BookCategories", "BookId", "dbo.Books");
            DropForeignKey("dbo.Copies", "BookId", "dbo.Books");
            DropForeignKey("dbo.Reviews", "BookId", "dbo.Books");
            DropIndex("dbo.Books", new[] { "Id" });
            DropPrimaryKey("dbo.Books");
            AddColumn("dbo.Books", "PublisherId", c => c.Int(nullable: false));
            AlterColumn("dbo.Books", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Books", "Id");
            CreateIndex("dbo.Books", "PublisherId");
            AddForeignKey("dbo.Books", "PublisherId", "dbo.Publishers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.BookAuthors", "BookId", "dbo.Books", "Id", cascadeDelete: true);
            AddForeignKey("dbo.BookCategories", "BookId", "dbo.Books", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Copies", "BookId", "dbo.Books", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Reviews", "BookId", "dbo.Books", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reviews", "BookId", "dbo.Books");
            DropForeignKey("dbo.Copies", "BookId", "dbo.Books");
            DropForeignKey("dbo.BookCategories", "BookId", "dbo.Books");
            DropForeignKey("dbo.BookAuthors", "BookId", "dbo.Books");
            DropForeignKey("dbo.Books", "PublisherId", "dbo.Publishers");
            DropIndex("dbo.Books", new[] { "PublisherId" });
            DropPrimaryKey("dbo.Books");
            AlterColumn("dbo.Books", "Id", c => c.Int(nullable: false));
            DropColumn("dbo.Books", "PublisherId");
            AddPrimaryKey("dbo.Books", "Id");
            CreateIndex("dbo.Books", "Id");
            AddForeignKey("dbo.Reviews", "BookId", "dbo.Books", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Copies", "BookId", "dbo.Books", "Id", cascadeDelete: true);
            AddForeignKey("dbo.BookCategories", "BookId", "dbo.Books", "Id", cascadeDelete: true);
            AddForeignKey("dbo.BookAuthors", "BookId", "dbo.Books", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Books", "Id", "dbo.Publishers", "Id");
        }
    }
}
