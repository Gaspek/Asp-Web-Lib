namespace Asp_Web_Lib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class limitschanges : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Limits", "LoanAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Limits", "LoanAmount", c => c.Int(nullable: false));
        }
    }
}
