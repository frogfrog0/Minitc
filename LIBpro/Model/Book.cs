using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Model
{
    public class Book
    {
        public string Title { get; set; }
        public string Genre { get; set; }
        public string ISBN { get; set; }
        public string Author { get; set; }
        public string ReleaseYear { get; set; }
        public string rodzaj { get; set; }
        public string rodzajokladki { get; set; }
        public List<Exemplar> Exemplars { get; set; }
        public string ExemplarsStatus => $"{Exemplars?.Count(e => e.IsAvailable) ?? 0}/{Exemplars?.Count ?? 0}";

        public Book(string title, string genre, string isbn, string author, string releaseYear, string rodzaj, string rodzajokladki) { 
            this.Title = title;
            this.Genre = genre;
            this.ISBN = isbn;
            this.Author = author;
            this.ReleaseYear = releaseYear;
            this.rodzaj = rodzaj;
            this.rodzajokladki=rodzajokladki;
            Exemplars = new List<Exemplar>();
        }

        public void AddExemplar(Exemplar exemplar)
        {
            this.Exemplars.Add(exemplar);
        }

        public Book() { }
    }
}
