namespace Asp_Web_Lib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsHistory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Loans", "IsHistory", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Loans", "IsHistory");
        }
    }
}
