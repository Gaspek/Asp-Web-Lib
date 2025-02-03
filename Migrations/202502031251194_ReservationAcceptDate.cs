namespace Asp_Web_Lib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReservationAcceptDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservations", "AcceptanceDate", c => c.DateTimeOffset(precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reservations", "AcceptanceDate");
        }
    }
}
