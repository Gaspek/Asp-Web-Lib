using System.Collections.Generic;

namespace Asp_Web_Lib.ViewModels
{
    public class CartItemViewModel
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string CoverImage { get; set; }
        public int AvailableCopies { get; set; }
        public bool IsAvailable => AvailableCopies > 0;
        public bool CanJoinQueue => !IsAvailable;
    }

    public class CartViewModel
    {
        public List<CartItemViewModel> Items { get; set; } = new List<CartItemViewModel>();
        public int TotalItems => Items.Count;
    }
}