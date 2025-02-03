using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Asp_Web_Lib.Models
{
    public interface IOrder
    {
        int Id { get; set; }
        int? CopyId { get; set; }
        Copy Copy { get; set; }
        string UserId { get; set; }
        ApplicationUser User { get; set; }
    }
}