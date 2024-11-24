using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Asp_Web_Lib.Filters;
using Asp_Web_Lib.Models;
using Microsoft.AspNet.Identity.Owin;

namespace Asp_Web_Lib.Controllers
{
    [Authorize(Roles = "Admin")]
    [Culture]
    public class AdminController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Admin
        public async Task<ActionResult> Index()
        {
            var users = context.Users.ToList();
            var model = new List<UserViewModel>();

            foreach (var user in users)
            {
                var userRoles = await UserManager.GetRolesAsync(user.Id);
                var userModel = new UserViewModel
                {
                    UserId = user.Id,
                    Email = user.Email,
                    Roles = userRoles
                };
                model.Add(userModel);
            }

            return View(model);
        }

        // GET: Admin/ManageRoles?userId=...
        public ActionResult ManageRoles(string userId)
        {
            var user = context.Users.Find(userId);

            if (user == null)
            {
                return HttpNotFound();
            }

            // Pobierz listę identyfikatorów ról użytkownika
            var userRoleIds = user.Roles.Select(r => r.RoleId).ToList();

            // Załaduj wszystkie role do pamięci
            var roles = context.Roles.ToList();

            var model = new ManageUserRolesViewModel
            {
                UserId = user.Id,
                Email = user.Email,
                RolesList = roles.Select(r => new SelectListItem
                {
                    Selected = userRoleIds.Contains(r.Id),
                    Text = r.Name,
                    Value = r.Name
                }).ToList()
            };

            return View(model);
        }


        // POST: Admin/ManageRoles
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ManageRoles(ManageUserRolesViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = context.Users.Find(model.UserId);
                var roles = await UserManager.GetRolesAsync(user.Id);

                // Usuń wszystkie role użytkownika
                await UserManager.RemoveFromRolesAsync(user.Id, roles.ToArray());

                // Dodaj wybrane role
                if (model.SelectedRoles != null)
                {
                    await UserManager.AddToRolesAsync(user.Id, model.SelectedRoles);
                }

                return RedirectToAction("Index");
            }

            // Jeśli model jest nieprawidłowy, ponownie wyświetl formularz
            return View(model);
        }
    }
}
