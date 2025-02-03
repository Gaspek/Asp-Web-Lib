namespace Asp_Web_Lib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Null : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Reservations", "CopyId", "dbo.Copies");
            DropIndex("dbo.Reservations", new[] { "CopyId" });
            AlterColumn("dbo.Reservations", "CopyId", c => c.Int());
            CreateIndex("dbo.Reservations", "CopyId");
            AddForeignKey("dbo.Reservations", "CopyId", "dbo.Copies", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reservations", "CopyId", "dbo.Copies");
            DropIndex("dbo.Reservations", new[] { "CopyId" });
            AlterColumn("dbo.Reservations", "CopyId", c => c.Int(nullable: false));
            CreateIndex("dbo.Reservations", "CopyId");
            AddForeignKey("dbo.Reservations", "CopyId", "dbo.Copies", "Id", cascadeDelete: true);
        }
    }
}
