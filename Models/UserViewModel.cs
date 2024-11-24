using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Asp_Web_Lib.Models
{
    public class UserViewModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public IList<string> Roles { get; set; }
    }
}