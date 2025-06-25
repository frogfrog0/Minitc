using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Models.Data;
using LibraryManagement.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Model
{
    public class BookRepository
    {
        private readonly LibraryContext _context;

        public BookRepository()
        {
            var connectionString = "Data Source=Library.db";
            var optionsBuilder = new DbContextOptionsBuilder<LibraryContext>();
            optionsBuilder.UseSqlite(connectionString);
            _context = new LibraryContext(optionsBuilder.Options);
        }

        public List<Book> GetAll()
        {
            try
            {
                // Get all books with their related data
                var ksiazki = _context.Ksiazki
                    .Include(k => k.KsiazkaAutorzy)
                        .ThenInclude(ka => ka.Autor)
                    .Include(k => k.KsiazkaGatunki)
                        .ThenInclude(kg => kg.Gatunek)
                    .Include(k => k.Wypozyczenia)
                    .ToList();

                // Group books by title and convert to Book objects
                var books = ksiazki
                    .GroupBy(k => k.Tytul)
                    .Select(group => ConvertToBook(group.ToList()))
                    .ToList();

                return books;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading books: {ex.Message}");
                return new List<Book>();
            }
        }

        public void Add(Book book)
        {
            try
            {
                // This would need more complex logic to handle authors/genres
                // For now, just add a simple book
                var ksiazka = new Ksiazka
                {
                    Tytul = book.Title,
                    Cena = 0,
                    Rodzaj = ParseRodzajKsiazki(book.rodzaj),
                    RodzajOkladki = ParseRodzajOkladki(book.rodzajokladki),
                    Uszkodzenia = LibraryManagement.Models.Enums.Uszkodzenia.Brak,
                    Status = LibraryManagement.Models.Enums.StatusKsiazki.Dostępna
                };

                _context.Ksiazki.Add(ksiazka);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error adding book: {ex.Message}");
            }
        }

        public List<Book> Filter(string title, string genre, string isbn, string author, string releaseYear, string rodzaj, string rodzajokladki)
        {
            try
            {
                var query = _context.Ksiazki
                    .Include(k => k.KsiazkaAutorzy)
                        .ThenInclude(ka => ka.Autor)
                    .Include(k => k.KsiazkaGatunki)
                        .ThenInclude(kg => kg.Gatunek)
                    .Include(k => k.Wypozyczenia)
                    .AsQueryable();

                // Apply filters
                if (!string.IsNullOrEmpty(title))
                {
                    query = query.Where(k => k.Tytul.Contains(title));
                }

                if (!string.IsNullOrEmpty(genre))
                {
                    query = query.Where(k => k.KsiazkaGatunki.Any(kg => kg.Gatunek.Gatunki.Contains(genre)));
                }

                if (!string.IsNullOrEmpty(author))
                {
                    query = query.Where(k => k.KsiazkaAutorzy.Any(ka =>
                        ka.Autor.Imie.Contains(author) || ka.Autor.Nazwisko.Contains(author)));
                }

                if (!string.IsNullOrEmpty(rodzaj))
                {
                    var rodzajEnum = ParseRodzajKsiazki(rodzaj);
                    query = query.Where(k => k.Rodzaj == rodzajEnum);
                }

                if (!string.IsNullOrEmpty(rodzajokladki))
                {
                    var okladkaEnum = ParseRodzajOkladki(rodzajokladki);
                    query = query.Where(k => k.RodzajOkladki == okladkaEnum);
                }

                if (!string.IsNullOrEmpty(isbn))
                {
                    query = query.Where(k => k.ISBN != null && k.ISBN.Contains(isbn));
                }

                if (!string.IsNullOrEmpty(releaseYear))
                {
                    if (int.TryParse(releaseYear, out int year))
                    {
                        query = query.Where(k => k.RokWydania == year);
                    }
                }

                // Group by title and convert to Book objects
                var filteredBooks = query.ToList()
                    .GroupBy(k => k.Tytul)
                    .Select(group => ConvertToBook(group.ToList()))
                    .ToList();

                return filteredBooks;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error filtering books: {ex.Message}");
                return new List<Book>();
            }
        }

        public List<Book> Sort(List<Book> input, string sortBy)
        {
            return sortBy switch
            {
                "Autor" => input.OrderBy(b => b.Author).ToList(),
                "Nazwa" => input.OrderBy(b => b.Title).ToList(),
                "Data wydania" => input.OrderBy(b => b.ReleaseYear).ToList(),
                _ => input
            };
        }

        private Book ConvertToBook(List<Ksiazka> ksiazki)
        {
            var firstBook = ksiazki.First();

            // Get authors (from first book, assuming all copies have same authors)
            var authors = firstBook.KsiazkaAutorzy
                .Select(ka => $"{ka.Autor.Imie} {ka.Autor.Nazwisko}")
                .ToList();
            var authorString = string.Join(", ", authors);

            // Get genres (from first book, assuming all copies have same genres)
            var genres = firstBook.KsiazkaGatunki
                .Select(kg => kg.Gatunek.Gatunki)
                .ToList();
            var genreString = string.Join(", ", genres);

            var book = new Book(
                title: firstBook.Tytul,
                genre: genreString,
                isbn: firstBook.ISBN ?? "Brak ISBN",
                author: authorString,
                releaseYear: firstBook.RokWydania?.ToString() ?? "Nieznany", // Release year not in current database design
                rodzaj: firstBook.Rodzaj.ToString(),
                rodzajokladki: firstBook.RodzajOkladki.ToString()
            );

            // Convert each Ksiazka to an Exemplar
            foreach (var ksiazka in ksiazki)
            {
                var wypozyczenie = ksiazka.Wypozyczenia.FirstOrDefault();
                var exemplar = new Exemplar
                {
                    BookId = ksiazka.Id, // Dodane ID książki
                    Name = $"Egz. {ksiazka.Id}",
                    BorrowingDate = wypozyczenie?.DataWypozyczenia,
                    ReturnDate = wypozyczenie?.TerminOddania
                };
                book.AddExemplar(exemplar);
            }

            return book;
        }

        private LibraryManagement.Models.Enums.RodzajKsiazki ParseRodzajKsiazki(string rodzaj)
        {
            return rodzaj?.ToLower() switch
            {
                "książka" => LibraryManagement.Models.Enums.RodzajKsiazki.Książka,
                "komiks" => LibraryManagement.Models.Enums.RodzajKsiazki.Komiks,
                "album" => LibraryManagement.Models.Enums.RodzajKsiazki.Album,
                _ => LibraryManagement.Models.Enums.RodzajKsiazki.Książka
            };
        }

        private LibraryManagement.Models.Enums.RodzajOkladki ParseRodzajOkladki(string rodzajOkladki)
        {
            return rodzajOkladki?.ToLower() switch
            {
                "twarda" => LibraryManagement.Models.Enums.RodzajOkladki.Twarda,
                "miękka" => LibraryManagement.Models.Enums.RodzajOkladki.Miękka,
                _ => LibraryManagement.Models.Enums.RodzajOkladki.Miękka
            };
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}