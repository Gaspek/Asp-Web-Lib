using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Asp_Web_Lib.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(20)]
        public string Name { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}