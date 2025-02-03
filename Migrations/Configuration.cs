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

            if (!roleManager.RoleExists("Worker"))
            {
                var role = new IdentityRole("Worker");
                roleManager.Create(role);
            }


            if (!roleManager.RoleExists("User"))
            {
                var role = new IdentityRole("User");
                roleManager.Create(role);
            }
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            var adminUser = userManager.FindByEmail("Admin@admin.ad");
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = "Admin@admin.ad",
                    Email = "Admin@admin.ad"
                };
                userManager.Create(adminUser, "mWI734!I");

                // Przypisz rolę "Admin"
                userManager.AddToRole(adminUser.Id, "Admin");
            }

			// dodaje do bazy rekord z limitami który jest później do zedytowania
			if (!context.Limits.Any()) // Sprawdza, czy tabela jest pusta
			{
				context.Limits.Add(new Limits
				{
					IdLimits = 1, // Zawsze ten sam ID
					MaxBorrowedBooks = 3,
					MaxWaitingBooks = 5,
					MaxExtensionNumber = 3,
					ExtensionDays = 30,
					LoanAmount = 0.50m
				});

				context.SaveChanges();
			}
		}
    }
}
