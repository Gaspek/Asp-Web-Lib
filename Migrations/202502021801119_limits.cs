namespace Asp_Web_Lib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class limits : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Limits",
                c => new
                    {
                        IdLimits = c.Int(nullable: false, identity: true),
                        MaxBorrowedBooks = c.Int(nullable: false),
                        MaxWaitingBooks = c.Int(nullable: false),
                        MaxExtensionNumber = c.Int(nullable: false),
                        ExtensionDays = c.Int(nullable: false),
                        LoanAmount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdLimits);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Limits");
        }
    }
}
