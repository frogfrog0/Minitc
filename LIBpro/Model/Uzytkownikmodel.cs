using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Models.Data;
using LibraryManagement.Models.Entities;
using LibraryManagement.Models.Entities.JunctionTables;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Model
{
    internal class Uzytkownikmodel
    {
        private readonly LibraryContext _context;

        public Uzytkownikmodel()
        {
            var connectionString = "Data Source=Library.db";
            var optionsBuilder = new DbContextOptionsBuilder<LibraryContext>();
            optionsBuilder.UseSqlite(connectionString);
            _context = new LibraryContext(optionsBuilder.Options);
        }

        // Update user data in database
        public UserUpdateResult zmianadan(string email, string numtel, string woj, string miasto, string ul, string numdom, string nummiesz, string kod1, string kod2)
        {
            try
            {
                if (!UserSession.IsLoggedIn)
                {
                    return new UserUpdateResult { Success = false, ErrorMessage = "Użytkownik nie jest zalogowany" };
                }

                var userId = UserSession.UserId!.Value;
                var user = _context.Uzytkownicy
                    .Include(u => u.Adres)
                    .FirstOrDefault(u => u.Id == userId);

                if (user == null)
                {
                    return new UserUpdateResult { Success = false, ErrorMessage = "Nie znaleziono użytkownika" };
                }

                // Update user data
                user.Email = email;
                user.NumTelefonu = numtel;

                // Update or create address
                if (user.Adres == null)
                {
                    user.Adres = new Adres { UzytkownikId = userId };
                    _context.Adresy.Add(user.Adres);
                }

                user.Adres.Wojewodztwo = woj;
                user.Adres.Miasto = miasto;
                user.Adres.Ulica = ul;
                user.Adres.NrDomu = numdom;
                user.Adres.NrMieszkania = nummiesz;
                user.Adres.KodPocztowy = $"{kod1}-{kod2}";

                _context.SaveChanges();

                // Update session with fresh data
                UserSession.UpdateCurrentUser(user);

                return new UserUpdateResult { Success = true };
            }
            catch (Exception ex)
            {
                return new UserUpdateResult { Success = false, ErrorMessage = "Błąd podczas aktualizacji: " + ex.Message };
            }
        }

        // Get user's borrowed books
        public List<BorrowedBookInfo> GetBorrowedBooks()
        {
            try
            {
                if (!UserSession.IsLoggedIn)
                    return new List<BorrowedBookInfo>();

                var userId = UserSession.UserId.Value;
                var borrowedBooks = _context.Wypozyczone
                    .Where(w => w.UzytkownikId == userId)
                    .Include(w => w.Ksiazka)
                        .ThenInclude(k => k.KsiazkaAutorzy)
                            .ThenInclude(ka => ka.Autor)
                    .Select(w => new BorrowedBookInfo
                    {
                        BookTitle = w.Ksiazka.Tytul,
                        Authors = string.Join(", ", w.Ksiazka.KsiazkaAutorzy.Select(ka => $"{ka.Autor.Imie} {ka.Autor.Nazwisko}")),
                        BorrowDate = w.DataWypozyczenia,
                        ReturnDate = w.TerminOddania,
                        IsOverdue = w.TerminOddania < DateTime.Now
                    })
                    .ToList();
                return borrowedBooks;
            }
            catch (Exception ex)
            {
                return new List<BorrowedBookInfo>();
            }
        }

        // Get user's favorite books
        public List<FavoriteBookInfo> GetFavoriteBooks()
        {
            try
            {
                if (!UserSession.IsLoggedIn)
                    return new List<FavoriteBookInfo>();

                var userId = UserSession.UserId!.Value;
                var favoriteBooks = _context.Ulubione
                    .Where(u => u.UzytkownikId == userId)
                    .Include(u => u.Ksiazka)
                        .ThenInclude(k => k.KsiazkaAutorzy)
                            .ThenInclude(ka => ka.Autor)
                    .Include(u => u.Ksiazka)
                        .ThenInclude(k => k.KsiazkaGatunki)
                            .ThenInclude(kg => kg.Gatunek)
                    .Select(u => new FavoriteBookInfo
                    {
                        BookTitle = u.Ksiazka.Tytul,
                        Authors = string.Join(", ", u.Ksiazka.KsiazkaAutorzy.Select(ka => $"{ka.Autor.Imie} {ka.Autor.Nazwisko}")),
                        Genres = string.Join(", ", u.Ksiazka.KsiazkaGatunki.Select(kg => kg.Gatunek.Gatunki))
                    })
                    .ToList();

                return favoriteBooks;
            }
            catch (Exception ex)
            {
                return new List<FavoriteBookInfo>();
            }
        }

        // Get current user data methods
        public string getemail()
        {
            return UserSession.CurrentUser?.Email ?? "";
        }

        public string getnumtel()
        {
            return UserSession.CurrentUser?.NumTelefonu ?? "";
        }

        public string getnumdom()
        {
            return UserSession.CurrentUser?.Adres?.NrDomu ?? "";
        }

        public string getkod1()
        {
            var kodPocztowy = UserSession.CurrentUser?.Adres?.KodPocztowy ?? "";
            return kodPocztowy.Split('-').FirstOrDefault() ?? "";
        }

        public string getkod2()
        {
            var kodPocztowy = UserSession.CurrentUser?.Adres?.KodPocztowy ?? "";
            var parts = kodPocztowy.Split('-');
            return parts.Length > 1 ? parts[1] : "";
        }

        public string getwoj()
        {
            return UserSession.CurrentUser?.Adres?.Wojewodztwo ?? "";
        }

        public string getmiasto()
        {
            return UserSession.CurrentUser?.Adres?.Miasto ?? "";
        }

        public string getnummiesz()
        {
            return UserSession.CurrentUser?.Adres?.NrMieszkania ?? "";
        }

        public string getul()
        {
            return UserSession.CurrentUser?.Adres?.Ulica ?? "";
        }

        public string GetUserFullName()
        {
            return UserSession.UserFullName ?? "";
        }

        public decimal GetUserSaldo()
        {
            return UserSession.CurrentUser?.Saldo ?? 0;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }

    // Result classes for better error handling
    public class UserUpdateResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }

    public class BorrowedBookInfo
    {
        public string BookTitle { get; set; } = string.Empty;
        public string Authors { get; set; } = string.Empty;
        public DateTime BorrowDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public bool IsOverdue { get; set; }

        public override string ToString()
        {
            var status = IsOverdue ? " (PRZETERMINOWANE)" : "";
            return $"{BookTitle} - {Authors} (zwrot: {ReturnDate:dd.MM.yyyy}){status}";
        }
    }

    public class FavoriteBookInfo
    {
        public string BookTitle { get; set; } = string.Empty;
        public string Authors { get; set; } = string.Empty;
        public string Genres { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"{BookTitle} - {Authors} ({Genres})";
        }
    }
}