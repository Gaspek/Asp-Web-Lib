using System;
using System.Collections;
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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        public string Description { get; set; }
        [StringLength(13)]
        public string ISBN { get; set; }
        public DateTimeOffset PublicationYear { get; set; }
        public string CoverImage { get; set; }
        public int PublisherId { get; set; }
        [ForeignKey("PublisherId")]
        public virtual Publisher Publisher { get; set; }
        public virtual ICollection<Author> Authors { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        public virtual ICollection<Copy> Copies { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<QueueEntry> QueueEntries { get; set; }
    }
}