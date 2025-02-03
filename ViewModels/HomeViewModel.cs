using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using Asp_Web_Lib.Models;
using Publisher = System.Security.Policy.Publisher;

namespace Asp_Web_Lib.ViewModels
{
    public class HomeViewModel
    {
        public List<BookViewModel> Books { get; set; }
        public List<BookViewModel> NewBooks { get; set; }

    }

    public class BookViewModel
    {
        [Required]
        public int BookId { get; set; }
        [Required]
        [StringLength(20)]
        public string Title { get; set; }
        [StringLength(200)]
        public string Description { get; set; }
        public string CoverImage { get; set; }
        public string Authors { get; set; }
        public string Category { get; set; }
        public string ISBN { get; set; }
        public uint Rating { get; set; }
        public string PublicationYear { get; set; }
        public string Publisher { get; set; }
        public List<string> Tags { get; set; }
    }
}