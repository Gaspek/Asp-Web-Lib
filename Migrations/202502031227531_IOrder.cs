namespace Asp_Web_Lib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IOrder : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Loans", "LoanDate", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.Loans", "DueDate", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.Loans", "ReturnDate", c => c.DateTimeOffset(precision: 7));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Loans", "ReturnDate", c => c.DateTime());
            AlterColumn("dbo.Loans", "DueDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Loans", "LoanDate", c => c.DateTime(nullable: false));
        }
    }
}
