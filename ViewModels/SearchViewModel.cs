using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asp_Web_Lib.ViewModels
{
    public class SearchViewModel
    {
        public string ISBN { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public int Id { get; set; }
        public string CoverImg { get; set; }
    }
}