using Asp_Web_Lib.Models;
using System;
using System.Data.Entity;

namespace Asp_Web_Lib.Services
{
    public class BookService
    {
        private readonly ApplicationDbContext _db;

        public BookService(ApplicationDbContext context)
        {
            _db = context;
        }

        // Zdarzenie wywoływane po zwrocie kopii książki
        public event EventHandler<BookEventArgs> BookReleased;

        protected virtual void OnBookReleased(Book book, Copy returnedCopy)
        {
            BookReleased?.Invoke(this, new BookEventArgs(book, returnedCopy));
        }

        // Metoda wywoływana przy zwrocie kopii książki
        public void ReturnCopy(int copyId)
        {
            // Oznacz kopię jako dostępna
            var copy = _db.Copies.Find(copyId);
            copy.Status = Status.CopyStatus.Available;
            _db.Entry(copy).State = EntityState.Modified;
            _db.SaveChanges();

            // Wywołaj zdarzenie przekazując książkę i zwróconą kopię
            OnBookReleased(copy.Book, copy);
        }
    }
}