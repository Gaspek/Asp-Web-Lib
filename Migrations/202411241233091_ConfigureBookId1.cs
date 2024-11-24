namespace Asp_Web_Lib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConfigureBookId1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Books", "Id", c => c.Int(nullable: false, identity: true));
        }

        public override void Down()
        {
        }
    }
}
