namespace Asp_Web_Lib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Queue : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QueueEntries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BookId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        AddedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.BookId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QueueEntries", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.QueueEntries", "BookId", "dbo.Books");
            DropIndex("dbo.QueueEntries", new[] { "UserId" });
            DropIndex("dbo.QueueEntries", new[] { "BookId" });
            DropTable("dbo.QueueEntries");
        }
    }
}
