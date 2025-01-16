using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Asp_Web_Lib.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        // Klucz obcy do Book
        [Required]
        public int BookId { get; set; }
        [ForeignKey("BookId")]
        public virtual Book Book { get; set; }
        // Klucz obcy do ApplicationUser
        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; } // Ocena w skali 1-5
        [StringLength(500)]
        public string Comment { get; set; }
        [Required]
        public DateTimeOffset Date { get; set; }
    }
}