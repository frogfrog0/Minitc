using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Model
{
    public class BookRepository
    {
        private List<Book> books;

        public List<Book> GetAll() => books;

        public void Add(Book book) => books.Add(book);

        public List<Book> Filter(string title, string genre, string isbn, string author, string releaseYear, string rodzaj, string rodzajokladki)
        {
            var newBooks = books.Where(b =>
                (string.IsNullOrEmpty(title) || b.Title.Contains(title, StringComparison.OrdinalIgnoreCase)) && (string.IsNullOrEmpty(genre) || b.Genre.Contains(genre, StringComparison.OrdinalIgnoreCase)) && (string.IsNullOrEmpty(isbn) || b.ISBN.Contains(isbn)) && (string.IsNullOrEmpty(author) || b.Author.Contains(author, StringComparison.OrdinalIgnoreCase)) && (string.IsNullOrEmpty(releaseYear) || b.ReleaseYear.Contains(releaseYear, StringComparison.OrdinalIgnoreCase)) && (string.IsNullOrEmpty(rodzaj) || b.rodzaj.Contains(rodzaj, StringComparison.OrdinalIgnoreCase)) && (string.IsNullOrEmpty(rodzajokladki) || b.rodzajokladki.Contains(rodzajokladki, StringComparison.OrdinalIgnoreCase))
            ).ToList();
            return newBooks;
        }

        public BookRepository()
        {
            books = new List<Book>();

            // tutaj są dodawane książki z bazy danych
            
            books.Add(new Book("Harry Potter i Kamień Filozoficzny", "Fantazy", "83-7278-000-5", "J. K. Rowling", "1997", "książka", "miękka"));
            books.Add(new Book("Harry Potter i Komnata Tajemnic", "Fantazy", "83-7278-012-9", "J. K. Rowling", "1998", "album", "miękka"));
            books.Add(new Book("Harry Potter i Więzień Askabanu", "Fantazy", "78-83-8265-448-6", "J. K. Rowling", "1999", "książka", "miękka"));
            books.Add(new Book("Harry Potter i Czara Ognia", "Fantazy", "978-83-8265-456-1", "J. K. Rowling", "2000", "książka", "miękka"));
            books.Add(new Book("Harry Potter i Zakon Feniksa", "Fantazy", "83-7278-000-5", "J. K. Rowling", "2003", "książka", "miękka"));
            books.Add(new Book("Harry Potter i Książe Półkrwi", "Fantazy", "83-7278-000-5", "J. K. Rowling", "2005", "książka", "twarda"));
            books.Add(new Book("Harry Potter i Insygnia Śmierci", "Fantazy", "978-83-7278-281-6", "J. K. Rowling", "2007", "książka", "miękka"));

            foreach (Book item in books)
            {
                item.AddExemplar(new Exemplar { Name = "Egz. 1", BorrowingDate = DateTime.Today, ReturnDate = DateTime.Today.AddDays(14) });
                item.AddExemplar(new Exemplar { Name = "Egz. 2", BorrowingDate = DateTime.Today.AddDays(-15), ReturnDate = DateTime.Today.AddDays(-1) });
                item.AddExemplar(new Exemplar { Name = "Egz. 3" });
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
    }
}
