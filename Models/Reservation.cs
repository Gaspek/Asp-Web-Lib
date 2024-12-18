﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Asp_Web_Lib.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }
        // Klucz obcy do Copy
        [Required]
        public int CopyId { get; set; }
        [ForeignKey("CopyId")]
        public virtual Copy Copy { get; set; }
        // Klucz obcy do ApplicationUser
        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        [Required]
        public DateTime ReservationDate { get; set; }
        [Required]
        [StringLength(20)]
        public string Status { get; set; } // np. "Aktywna", "Anulowana", "Zrealizowana"
    }
}