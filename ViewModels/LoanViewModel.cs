using Asp_Web_Lib.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Asp_Web_Lib.ViewModels
{
    public class LoanViewModel
    {
        public List<LoanItemViewModel> Items = new List<LoanItemViewModel>();
        public int TotalItems => Items.Count;
    }

    public class LoanItemViewModel
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string CoverImage { get; set; }
        public int LoanId { get; set; }
        public Status.CopyStatus LoanStatus { get; set; }
        [Required]
        public DateTimeOffset LoanDate { get; set; }
        [Required]
        public DateTimeOffset DueDate { get; set; }
        public DateTimeOffset? ReturnDate { get; set; }
    }
}