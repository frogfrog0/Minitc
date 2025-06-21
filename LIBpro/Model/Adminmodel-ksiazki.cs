using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Models.Data;
using LibraryManagement.Models.Entities;
using LibraryManagement.Models.Entities.JunctionTables;
using LibraryManagement.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Model
{
    internal class Adminmodel
    {
        private readonly LibraryContext _context;
        private List<Autor> _tempAuthors = new List<Autor>();
        private List<Gatunek> _tempGenres = new List<Gatunek>();

        public Adminmodel()
        {
            var connectionString = "Data Source=Library.db";
            var optionsBuilder = new DbContextOptionsBuilder<LibraryContext>();
            optionsBuilder.UseSqlite(connectionString);
            _context = new LibraryContext(optionsBuilder.Options);
        }

        // Get all available genres from database
        public string[] listagat()
        {
            try
            {
                var genres = _context.Gatunki
                    .OrderBy(g => g.Gatunki)
                    .Select(g => g.Gatunki)
                    .ToArray();
                return genres;
            }
            catch (Exception ex)
            {
                return new string[0];
            }
        }

        // Get all books for admin management
        public List<BookInfo> GetAllBooksForAdmin()
        {
            try
            {
                var books = _context.Ksiazki
                    .Include(k => k.KsiazkaAutorzy)
                        .ThenInclude(ka => ka.Autor)
                    .Include(k => k.KsiazkaGatunki)
                        .ThenInclude(kg => kg.Gatunek)
                    .Select(k => new BookInfo
                    {
                        Id = k.Id,
                        Tytul = k.Tytul,
                        Cena = k.Cena,
                        Rodzaj = k.Rodzaj,
                        RodzajOkladki = k.RodzajOkladki,
                        Uszkodzenia = k.Uszkodzenia,
                        Status = k.Status,
                        Autorzy = string.Join(", ", k.KsiazkaAutorzy.Select(ka => $"{ka.Autor.Imie} {ka.Autor.Nazwisko}")),
                        Gatunki = string.Join(", ", k.KsiazkaGatunki.Select(kg => kg.Gatunek.Gatunki))
                        ISBN = k.ISBN,
                        RokWydania = k.RokWydania
                    })
                    .OrderBy(k => k.Tytul)
                    .ToList();

                return books;
            }
            catch (Exception ex)
            {
                return new List<BookInfo>();
            }
        }

        // Add genre to temporary list (for UI building)
        public string dodajgat(string gatunek, string resztagat)
        {
            try
            {
                var existingGenre = _context.Gatunki.FirstOrDefault(g => g.Gatunki == gatunek);
                if (existingGenre == null)
                {
                    return resztagat; // Genre doesn't exist in database
                }

                // Add to temp list if not already there
                if (!_tempGenres.Any(g => g.Gatunki == gatunek))
                {
                    _tempGenres.Add(existingGenre);
                }

                // Return formatted string for UI
                var genreNames = _tempGenres.Select(g => g.Gatunki).ToList();
                return string.Join(", ", genreNames);
            }
            catch (Exception ex)
            {
                return resztagat;
            }
        }

        // Add author to temporary list (for UI building)
        public string dodajaut(string imie, string nazwisko, string resztaaut)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(imie) || string.IsNullOrWhiteSpace(nazwisko))
                {
                    return resztaaut;
                }

                // Check if author exists, if not create new one
                var existingAuthor = _context.Autorzy
                    .FirstOrDefault(a => a.Imie.ToLower() == imie.ToLower() &&
                                   a.Nazwisko.ToLower() == nazwisko.ToLower());

                if (existingAuthor == null)
                {
                    // Create new author
                    existingAuthor = new Autor
                    {
                        Imie = imie.Trim(),
                        Nazwisko = nazwisko.Trim()
                    };
                    _context.Autorzy.Add(existingAuthor);
                    _context.SaveChanges();
                }

                // Add to temp list if not already there
                if (!_tempAuthors.Any(a => a.Id == existingAuthor.Id))
                {
                    _tempAuthors.Add(existingAuthor);
                }

                // Return formatted string for UI
                var authorNames = _tempAuthors.Select(a => $"{a.Imie} {a.Nazwisko}").ToList();
                return string.Join(", ", authorNames);
            }
            catch (Exception ex)
            {
                return resztaaut;
            }
        }

        // Get book types
        public string[] rodz()
        {
            return new string[] { "Książka", "Album", "Komiks" };
        }

        // Get cover types
        public string[] rodzokl()
        {
            return new string[] { "Twarda", "Miękka" };
        }

        // Clear temporary authors list
        public string wyczyscautorow()
        {
            _tempAuthors.Clear();
            return "";
        }

        // Clear temporary genres list
        public string wyczyscgatunki()
        {
            _tempGenres.Clear();
            return "";
        }

        // Add new book to database
        public BookOperationResult dodajksiaz(string tytul, string rodzaj, string rodzajokl, DateTime dataWydania, string isbn, int ilosc, decimal cena = 0)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tytul) || _tempAuthors.Count == 0 || _tempGenres.Count == 0)
                {
                    return new BookOperationResult
                    {
                        Success = false,
                        ErrorMessage = "Tytuł, autorzy i gatunki są wymagane"
                    };
                }

                var books = new List<Ksiazka>();
                var rokWydania = dataWydania.Year;

                // Create specified number of book copies
                for (int i = 0; i < ilosc; i++)
                {
                    var ksiazka = new Ksiazka
                    {
                        Tytul = tytul.Trim(),
                        Cena = cena,
                        Rodzaj = ParseRodzajKsiazki(rodzaj),
                        RodzajOkladki = ParseRodzajOkladki(rodzajokl),
                        Uszkodzenia = Uszkodzenia.Brak,
                        Status = StatusKsiazki.Dostępna,
                        ISBN = string.IsNullOrWhiteSpace(isbn) ? null : isbn.Trim(),
                        RokWydania = rokWydania
                    };

                    _context.Ksiazki.Add(ksiazka);
                    books.Add(ksiazka);
                }

                _context.SaveChanges();

                // Create relationships for each book copy
                foreach (var ksiazka in books)
                {
                    // Add author relationships
                    foreach (var autor in _tempAuthors)
                    {
                        var ksiazkaAutor = new KsiazkaAutor
                        {
                            KsiazkaId = ksiazka.Id,
                            AutorId = autor.Id
                        };
                        _context.KsiazkiAutorzy.Add(ksiazkaAutor);
                    }

                    // Add genre relationships
                    foreach (var gatunek in _tempGenres)
                    {
                        var ksiazkaGatunek = new KsiazkaGatunek
                        {
                            KsiazkaId = ksiazka.Id,
                            GatunekId = gatunek.Id
                        };
                        _context.KsiazkiGatunki.Add(ksiazkaGatunek);
                    }
                }

                _context.SaveChanges();

                // Clear temporary lists
                wyczyscautorow();
                wyczyscgatunki();

                return new BookOperationResult
                {
                    Success = true,
                    Message = $"Dodano {ilosc} egzemplarzy książki '{tytul}'"
                };
            }
            catch (Exception ex)
            {
                return new BookOperationResult
                {
                    Success = false,
                    ErrorMessage = "Błąd podczas dodawania książki: " + ex.Message
                };
            }
        }

        // Update existing book
        public BookOperationResult UpdateBook(int bookId, string tytul, string rodzaj, string rodzajokl, decimal cena, string isbn, int? rokWydania)
        {
            try
            {
                var ksiazka = _context.Ksiazki.Find(bookId);
                if (ksiazka == null)
                {
                    return new BookOperationResult { Success = false, ErrorMessage = "Nie znaleziono książki" };
                }

                ksiazka.Tytul = tytul.Trim();
                ksiazka.Rodzaj = ParseRodzajKsiazki(rodzaj);
                ksiazka.RodzajOkladki = ParseRodzajOkladki(rodzajokl);
                ksiazka.Cena = cena;
                ksiazka.ISBN = string.IsNullOrWhiteSpace(isbn) ? null : isbn.Trim();
                ksiazka.RokWydania = rokWydania;

                _context.SaveChanges();

                return new BookOperationResult
                {
                    Success = true,
                    Message = "Książka została zaktualizowana"
                };
            }
            catch (Exception ex)
            {
                return new BookOperationResult
                {
                    Success = false,
                    ErrorMessage = "Błąd podczas aktualizacji książki: " + ex.Message
                };
            }
        }

        // Delete book
        public BookOperationResult usunksiaz(int bookId)
        {
            try
            {
                var ksiazka = _context.Ksiazki
                    .Include(k => k.KsiazkaAutorzy)
                    .Include(k => k.KsiazkaGatunki)
                    .Include(k => k.Wypozyczenia)
                    .Include(k => k.Ulubione)
                    .FirstOrDefault(k => k.Id == bookId);

                if (ksiazka == null)
                {
                    return new BookOperationResult { Success = false, ErrorMessage = "Nie znaleziono książki" };
                }

                // Check if book is currently borrowed
                if (ksiazka.Wypozyczenia.Any())
                {
                    return new BookOperationResult
                    {
                        Success = false,
                        ErrorMessage = "Nie można usunąć wypożyczonej książki"
                    };
                }

                // Remove relationships (EF will handle this with cascade delete)
                _context.Ksiazki.Remove(ksiazka);
                _context.SaveChanges();

                return new BookOperationResult
                {
                    Success = true,
                    Message = "Książka została usunięta"
                };
            }
            catch (Exception ex)
            {
                return new BookOperationResult
                {
                    Success = false,
                    ErrorMessage = "Błąd podczas usuwania książki: " + ex.Message
                };
            }
        }

        // Get book by ID for editing
        public BookInfo? GetBookById(int bookId)
        {
            try
            {
                var book = _context.Ksiazki
                    .Include(k => k.KsiazkaAutorzy)
                        .ThenInclude(ka => ka.Autor)
                    .Include(k => k.KsiazkaGatunki)
                        .ThenInclude(kg => kg.Gatunek)
                    .Where(k => k.Id == bookId)
                    .Select(k => new BookInfo
                    {
                        Id = k.Id,
                        Tytul = k.Tytul,
                        Cena = k.Cena,
                        Rodzaj = k.Rodzaj,
                        RodzajOkladki = k.RodzajOkladki,
                        Uszkodzenia = k.Uszkodzenia,
                        Status = k.Status,
                        Autorzy = string.Join(", ", k.KsiazkaAutorzy.Select(ka => $"{ka.Autor.Imie} {ka.Autor.Nazwisko}")),
                        Gatunki = string.Join(", ", k.KsiazkaGatunki.Select(kg => kg.Gatunek.Gatunki))
                        ISBN = k.ISBN,
                        RokWydania = k.RokWydania
                    })
                    .FirstOrDefault();

                return book;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // Helper methods
        private RodzajKsiazki ParseRodzajKsiazki(string rodzaj)
        {
            return rodzaj?.ToLower() switch
            {
                "książka" => RodzajKsiazki.Książka,
                "komiks" => RodzajKsiazki.Komiks,
                "album" => RodzajKsiazki.Album,
                _ => RodzajKsiazki.Książka
            };
        }

        private RodzajOkladki ParseRodzajOkladki(string rodzajOkladki)
        {
            return rodzajOkladki?.ToLower() switch
            {
                "twarda" => RodzajOkladki.Twarda,
                "miękka" => RodzajOkladki.Miękka,
                _ => RodzajOkladki.Miękka
            };
        }

        // Legacy methods for compatibility - simplified since we use structured data now
        public string tytul(string tekst) => "";
        public string rodzaj(string tekst) => "";
        public string rodzajokladki(string tekst) => "";
        public DateTime getdata_wyd(string tekst) => DateTime.Now;
        public string getISBN(string tekst) => "";
        public string gatunki(string tekst) => "";
        public string autorzy(string tekst) => "";
        public int getilosc(string tekst) => 1;

        public void Dispose()
        {
            _context?.Dispose();
        }
    }

    public class BookInfo
    {
        public int Id { get; set; }
        public string Tytul { get; set; } = string.Empty;
        public decimal Cena { get; set; }
        public RodzajKsiazki Rodzaj { get; set; }
        public RodzajOkladki RodzajOkladki { get; set; }
        public Uszkodzenia Uszkodzenia { get; set; }
        public StatusKsiazki Status { get; set; }
        public string Autorzy { get; set; } = string.Empty;
        public string Gatunki { get; set; } = string.Empty;
        public string? ISBN { get; set; }
        public int? RokWydania { get; set; }

        public override string ToString()
        {
            var year = RokWydania?.ToString() ?? "?";
            var isbn = ISBN ?? "Brak ISBN";
            return $"{Tytul} - {Autorzy} ({year}) - ISBN: {isbn} - {Cena:C}";
        }
    }

    public class BookOperationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public BookInfo? Book { get; set; }
    }
}