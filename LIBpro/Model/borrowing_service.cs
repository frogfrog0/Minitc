using LibraryManagement.Models.Data;
using LibraryManagement.Models.Entities;
using LibraryManagement.Models.Entities.JunctionTables;
using LibraryManagement.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Model
{
    public class BorrowingService
    {
        private readonly LibraryContext _context;

        public BorrowingService()
        {
            var connectionString = "Data Source=Library.db";
            var optionsBuilder = new DbContextOptionsBuilder<LibraryContext>();
            optionsBuilder.UseSqlite(connectionString);
            _context = new LibraryContext(optionsBuilder.Options);
        }

        public BorrowingResult BorrowBook(int ksiazkaId, int userId)
        {
            try
            {
                // Sprawdź czy książka istnieje i jest dostępna
                var ksiazka = _context.Ksiazki
                    .Include(k => k.Wypozyczenia)
                    .FirstOrDefault(k => k.Id == ksiazkaId);

                if (ksiazka == null)
                {
                    return new BorrowingResult { Success = false, ErrorMessage = "Książka nie została znaleziona" };
                }

                if (ksiazka.Status != StatusKsiazki.Dostępna)
                {
                    return new BorrowingResult { Success = false, ErrorMessage = "Książka nie jest dostępna" };
                }

                // Sprawdź czy książka nie jest już wypożyczona
                if (ksiazka.Wypozyczenia.Any())
                {
                    return new BorrowingResult { Success = false, ErrorMessage = "Książka jest już wypożyczona" };
                }

                // Sprawdź czy użytkownik istnieje i nie jest zbanowany
                var user = _context.Uzytkownicy.Find(userId);
                if (user == null)
                {
                    return new BorrowingResult { Success = false, ErrorMessage = "Użytkownik nie został znaleziony" };
                }

                if (user.Ban)
                {
                    return new BorrowingResult { Success = false, ErrorMessage = "Konto użytkownika jest zablokowane" };
                }

                // Utwórz wypożyczenie
                var wypozyczenie = new Wypozyczone
                {
                    KsiazkaId = ksiazkaId,
                    UzytkownikId = userId,
                    DataWypozyczenia = DateTime.Now,
                    TerminOddania = DateTime.Now.AddDays(14)
                };

                _context.Wypozyczone.Add(wypozyczenie);

                // Zmień status książki
                ksiazka.Status = StatusKsiazki.Wypożyczona;

                _context.SaveChanges();

                return new BorrowingResult
                {
                    Success = true,
                    Message = "Książka została wypożyczona",
                    BorrowingDate = wypozyczenie.DataWypozyczenia,
                    ReturnDate = wypozyczenie.TerminOddania
                };
            }
            catch (Exception ex)
            {
                return new BorrowingResult { Success = false, ErrorMessage = "Błąd podczas wypożyczania: " + ex.Message };
            }
        }

        public BorrowingResult ReturnBook(int ksiazkaId, int userId)
        {
            try
            {
                var wypozyczenie = _context.Wypozyczone
                    .Include(w => w.Ksiazka)
                    .FirstOrDefault(w => w.KsiazkaId == ksiazkaId && w.UzytkownikId == userId);

                if (wypozyczenie == null)
                {
                    return new BorrowingResult { Success = false, ErrorMessage = "Nie znaleziono wypożyczenia" };
                }

                // Usuń wypożyczenie
                _context.Wypozyczone.Remove(wypozyczenie);

                // Zmień status książki
                wypozyczenie.Ksiazka.Status = StatusKsiazki.Dostępna;

                _context.SaveChanges();

                return new BorrowingResult
                {
                    Success = true,
                    Message = "Książka została zwrócona"
                };
            }
            catch (Exception ex)
            {
                return new BorrowingResult { Success = false, ErrorMessage = "Błąd podczas zwracania: " + ex.Message };
            }
        }

        public BorrowingResult ExtendBorrowing(int ksiazkaId, int userId, int daysToExtend = 14)
        {
            try
            {
                var wypozyczenie = _context.Wypozyczone
                    .FirstOrDefault(w => w.KsiazkaId == ksiazkaId && w.UzytkownikId == userId);

                if (wypozyczenie == null)
                {
                    return new BorrowingResult { Success = false, ErrorMessage = "Nie znaleziono wypożyczenia" };
                }

                wypozyczenie.TerminOddania = wypozyczenie.TerminOddania.AddDays(daysToExtend);
                _context.SaveChanges();

                return new BorrowingResult
                {
                    Success = true,
                    Message = "Termin zwrotu został przedłużony",
                    ReturnDate = wypozyczenie.TerminOddania
                };
            }
            catch (Exception ex)
            {
                return new BorrowingResult { Success = false, ErrorMessage = "Błąd podczas przedłużania: " + ex.Message };
            }
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }

    public class BorrowingResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public DateTime? BorrowingDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}