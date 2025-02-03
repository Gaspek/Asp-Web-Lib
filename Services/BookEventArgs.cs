using Asp_Web_Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asp_Web_Lib.Services
{
    public class BookEventArgs : EventArgs
    {
        public Book Book { get; private set; }
        public Copy ReturnedCopy { get; private set; }

        public BookEventArgs(Book book, Copy returnedCopy)
        {
            Book = book;
            ReturnedCopy = returnedCopy;
        }
    }
}