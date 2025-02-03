using Asp_Web_Lib.Models;
using System;
using System.Linq;
using System.Data.Entity;

namespace Asp_Web_Lib.Services
{
    public class QueueObserver
    {
        private readonly BookService _bookService;
        private readonly ApplicationDbContext _db;

        public QueueObserver(BookService bookService, ApplicationDbContext context)
        {
            _bookService = bookService;
            _db = context;
            // Subskrybuj zdarzenie zwolnienia książki
            _bookService.BookReleased += OnBookReleased;
        }

        // Metoda obsługująca zdarzenie BookReleased
        private void OnBookReleased(object sender, BookEventArgs e)
        {
            Book book = e.Book;

            // Jeśli książka posiada kolejkę oczekujących użytkowników...
            if (book.QueueEntries != null && book.QueueEntries.Any())
            {
                // Pobierz pierwszego użytkownika w kolejce (FIFO)
                var firstEntry = book.QueueEntries.OrderBy(q => q.AddedAt).First();

                // Przypisz zwróconą kopię do tego użytkownika:
                // Ustaw status kopii na Reserved
                e.ReturnedCopy.Status = Status.CopyStatus.Reserved;
                _db.Entry(e.ReturnedCopy).State = EntityState.Modified;

                var reservation = new Reservation
                {
                    CopyId = e.ReturnedCopy.Id,
                    BookId = book.Id,
                    UserId = firstEntry.UserId,
                    ReservationDate = DateTimeOffset.Now,
                    Status = Status.CopyStatus.Reserved
                };
                _db.Reservations.Add(reservation);

                // Usuń użytkownika z kolejki
                book.QueueEntries.Remove(firstEntry);
                _db.Entry(book).State = EntityState.Modified;

                _db.SaveChanges();

                // Ewentualne wysłanie powiadomień w przysłości wish us luck
            }
        }
    }
}
