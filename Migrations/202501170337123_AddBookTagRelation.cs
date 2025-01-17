namespace Asp_Web_Lib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBookTagRelation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BookTags",
                c => new
                    {
                        BookId = c.Int(nullable: false),
                        TagId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BookId, t.TagId })
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.TagId, cascadeDelete: true)
                .Index(t => t.BookId)
                .Index(t => t.TagId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BookTags", "TagId", "dbo.Tags");
            DropForeignKey("dbo.BookTags", "BookId", "dbo.Books");
            DropIndex("dbo.BookTags", new[] { "TagId" });
            DropIndex("dbo.BookTags", new[] { "BookId" });
            DropTable("dbo.BookTags");
            DropTable("dbo.Tags");
        }
    }
}
