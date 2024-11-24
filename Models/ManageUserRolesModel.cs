using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Asp_Web_Lib.Models
{
    public class ManageUserRolesViewModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public List<SelectListItem> RolesList { get; set; }
        public string[] SelectedRoles { get; set; }
    }
}