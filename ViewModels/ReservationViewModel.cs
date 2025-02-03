using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Asp_Web_Lib.Models;

namespace Asp_Web_Lib.ViewModels
{
    public class ReservationViewModel
    {
        public List<ReservationItemViewModel> Items { get; set; } = new List<ReservationItemViewModel>();
        public int TotalItems => Items.Count;
    }
    public class ReservationItemViewModel
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string CoverImage { get; set; }
        public int ReservationId { get; set; }
        public Status.CopyStatus ReservationStatus { get; set; }
        public DateTimeOffset? AcceptanceDate { get; set; }
    }
    
}