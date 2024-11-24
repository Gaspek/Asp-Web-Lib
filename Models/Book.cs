using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Helpers;

namespace Asp_Web_Lib.Models
{
    public class Book
    {
        [Key]   
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset PublicationYear { get; set; }
        public string CoverImage { get; set; }
        [ForeignKey("Id")]
        public virtual Publisher Publisher { get; set; }
        public virtual ICollection<Author> Authors { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Copy> Copies { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }

    }
}