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
            // Wczytujemy Book i Copy
            var bookFromDb = _db.Books
                .Include(b => b.QueueEntries)
                .FirstOrDefault(b => b.Id == e.Book.Id);

            if (bookFromDb == null) return;

            var copyFromDb = _db.Copies
                .FirstOrDefault(c => c.Id == e.ReturnedCopy.Id);

            if (copyFromDb == null) return;

            // Jeśli książka posiada kolejkę...
            if (bookFromDb.QueueEntries.Any())
            {
                // 1. Znajdź pierwszego użytkownika z kolejki
                var firstEntry = bookFromDb.QueueEntries
                    .OrderBy(q => q.AddedAt)
                    .First();

                // 2. Znajdź rezerwację tego użytkownika w statusie "InQueue"
                var existingReservation = _db.Reservations
                    .Where(r => r.UserId == firstEntry.UserId
                             && r.BookId == bookFromDb.Id
                             && r.Status == Status.CopyStatus.InQueue)
                    .OrderByDescending(r => r.Id)
                    .FirstOrDefault();

                // 3. Jeśli znaleziono rezerwację w kolejce, aktualizujemy jej status
                if (existingReservation != null)
                {
                    existingReservation.Status = Status.CopyStatus.Reserved;
                    existingReservation.CopyId = copyFromDb.Id;
                    existingReservation.ReservationDate = DateTimeOffset.Now;

                    _db.SaveChanges();
                }
                else
                {
                    var newReservation = new Reservation
                    {
                        CopyId = copyFromDb.Id,
                        BookId = bookFromDb.Id,
                        UserId = firstEntry.UserId,
                        Status = Status.CopyStatus.Reserved,
                        ReservationDate = DateTimeOffset.Now
                    };
                    _db.Reservations.Add(newReservation);
                    _db.SaveChanges();
                }

                // 4. Usuwamy wpis z kolejki z bazy, żeby nie ustawiać BookId = null
                _db.QueueEntries.Remove(firstEntry);
                _db.SaveChanges();

                // 5. Ustawiamy status egzemplarza na Reserved
                copyFromDb.Status = Status.CopyStatus.Reserved;
                _db.SaveChanges();
            }
        }

    }
}
