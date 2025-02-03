namespace Asp_Web_Lib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReservationBook : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservations", "BookId", c => c.Int());
            CreateIndex("dbo.Reservations", "BookId");
            AddForeignKey("dbo.Reservations", "BookId", "dbo.Books", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reservations", "BookId", "dbo.Books");
            DropIndex("dbo.Reservations", new[] { "BookId" });
            DropColumn("dbo.Reservations", "BookId");
        }
    }
}
