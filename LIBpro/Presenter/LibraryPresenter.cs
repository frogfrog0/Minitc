using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using LibraryApp.Model;
using LibraryApp.View;
using LibraryApp.View;
using LibraryApp.Model;
using LibraryApp.Presenter;

namespace LibraryApp.Presenter
{
    public class LibraryPresenter
    {
        private readonly BookForm view;
        private readonly BookRepository repository;

        public LibraryPresenter(BookForm view, BookRepository repository)
        {
            this.view = view;
            this.repository = repository;
            view.FilterChanged += OnFilterChanged;
            view.logowaniepanel += logowaniepanel;
            view.rejestracjapanel += rejestracjapanel;

            LoadInitialData();
            UpdateView();
        }

        private void LoadInitialData()
        {
            //repository.Add(new Book { Title = "Solaris", Genre = "Sci-fi", ISBN = "123", Author = "Lem", ReleaseYear = "2000" });
            //repository.Add(new Book { Title = "Wiedźmin", Genre = "Fantasy", ISBN = "456", Author = "Sapkowski", ReleaseYear = "2001" });
        }

        private void OnFilterChanged() => UpdateView();

        private void UpdateView()
        {
            var filtered = repository.Filter(view.FilterTitle, view.FilterGenre, view.FilterISBN, view.FilterAuthor, view.FilterReleaseYear, view.Filterrodzaj, view.Filterrodzajokladki);
            Debug.WriteLine($"Znaleziono: {filtered.Count} książek");
            var sorted = repository.Sort(filtered, view.SortBy);
            view.SetBookList(sorted);
        }
        private void logowaniepanel()
        {
            view.tablica.Hide();
            view.logowanie.Show();
            Logowaniemodel logowaniemodel = new Logowaniemodel();
            Presenterlogowanie presenterlogowanie = new Presenterlogowanie((Logowanie)view.logowanie, logowaniemodel, (Paneluzytkownika)view.paneluzytkownika, view);
        }
        private void rejestracjapanel()
        {
            view.tablica.Hide();
            view.rejestracja.Show();
            Rejestracjamodel rejestracjamodel = new Rejestracjamodel();
            Presenterrejestracja presenterrejestracja = new Presenterrejestracja((Rejestracja)view.rejestracja, rejestracjamodel, (Paneluzytkownika)view.paneluzytkownika, view);
        }
    }
}
