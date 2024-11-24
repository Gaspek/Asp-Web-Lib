using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Asp_Web_Lib.Models
{
    public class Copy
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int BookId { get; set; }
        [ForeignKey("BookId")]
        public virtual Book Book { get; set; }
        [StringLength(50)]
        public string ShelfLocation { get; set; }
        [Required]
        [StringLength(20)]
        public string Status { get; set; } // np. "Dostępny", "Wypożyczony", "Zarezerwowany"
        public virtual ICollection<Loan> Loans { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}