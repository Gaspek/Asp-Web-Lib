using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Asp_Web_Lib.Models
{
     public class Author
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(40)]
        public string FirstName { get; set; }
        [StringLength(40)]
        public string LastName { get; set; }
        public string Biography { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}