using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Asp_Web_Lib.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        // Klucz obcy do ApplicationUser
        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public bool IsRead { get; set; }
    }
}