namespace Asp_Web_Lib.Migrations
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Asp_Web_Lib.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<Asp_Web_Lib.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Asp_Web_Lib.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!roleManager.RoleExists("Admin"))
            {
                var role = new IdentityRole("Admin");
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("User"))
            {
                var role = new IdentityRole("User");
                roleManager.Create(role);
            }
        }
    }
}
