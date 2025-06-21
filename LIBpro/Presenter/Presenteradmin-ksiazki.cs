using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryApp.Model;
using LibraryApp.View;
using System.Windows.Forms;


namespace LibraryApp.Presenter
{
    internal class Presenteradmin
    {
        private View.Adminpanelksiazki _adminpanel;
        private Adminmodel _adminmodel;
        private Adminpaneluzytkownicy _adminuz;
        private Adminpanelogloszenia _adminpanelogloszenia;
        private BookForm _bookForm;
        private List<BookInfo> _allBooks = new List<BookInfo>();
        private BookInfo? _selectedBook;
        private bool _isEditMode = false;
        public Presenteradmin(View.Adminpanelksiazki adminpanel, Adminmodel adminmodel, Adminpaneluzytkownicy adminuz, Adminpanelogloszenia adminogl, BookForm bookForm)
        {
            _adminpanel = adminpanel;
            _adminmodel = adminmodel;
            _adminuz = adminuz;
            _adminpanelogloszenia = adminogl;
            _bookForm = bookForm;
            _adminpanel.listagatunkow = _adminmodel.listagat();
            _adminpanel.rodzaj = _adminmodel.rodz();
            _adminpanel.rodzajokladki = _adminmodel.rodzokl();
            _adminpanel.dodajgatunek += dodaj_gatunek;
            _adminpanel.dodajautora += dodaj_autora;
            _adminpanel.wyczyscgatunki += wyczyscgatunki;
            _adminpanel.wyczyscautorow += wyczyscautorow;
            _adminpanel.dodajksiazke += dodaj_ksiazke;
            _adminpanel.ustawksiazke += ustaw_ksiazke;
            _adminpanel.edytujksiazke += edytuj_ksiazke;
            _adminpanel.usunksiazke += usun_ksiazke;
            _adminpanel.paneluzytkownikow += zmien_paneluz;
            _adminpanel.panelogloszen += zmien_panelogl;
            _adminpanel.powrotglowna += powrotglowna;

            LoadBooks();
        }

        private void LoadBooks()
        {
            _allBooks = _adminmodel.GetAllBooksForAdmin();
            _adminpanel.LoadBookList(_allBooks);
        }

        private void ClearForm()
        {
            _adminpanel.tytul = "";
            _adminpanel.gatunek = "";
            _adminpanel.autorzy = "";
            _adminpanel.ilosc = 1;
            _adminpanel.SetEditMode(false);
            _selectedBook = null;
            _isEditMode = false;
            wyczyscautorow();
            wyczyscgatunki();
        }

        private void wyczyscgatunki()
        {
            _adminpanel.gatunek = _adminmodel.wyczyscgatunki();
        }
        private void wyczyscautorow()
        {
            _adminpanel.autorzy = _adminmodel.wyczyscautorow();
        }
        private void dodaj_gatunek(string gatunek)
        {
            _adminpanel.gatunek = _adminmodel.dodajgat(gatunek, _adminpanel.gatunek);
        }
        private void dodaj_autora(string imie, string nazwisko)
        {
            _adminpanel.autorzy = _adminmodel.dodajaut(imie, nazwisko, _adminpanel.autorzy);
        }
        private void dodaj_ksiazke(string tytul, string rodzaj, string rodzajokl, DateTimePicker data_wydania, string ISBN, string gatunki, string autorzy, int ile)
        {
            if (_isEditMode && _selectedBook != null)
            {
                var result = _adminmodel.UpdateBook(_selectedBook.Id, tytul, rodzaj, rodzajokl, 0, ISBN, data_wydania.Value.Year);

                if (result.Success)
                {
                    MessageBox.Show(result.Message, "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadBooks();
                    ClearForm();
                }
                else
                {
                    MessageBox.Show(result.ErrorMessage, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                var result = _adminmodel.dodajksiaz(tytul, rodzaj, rodzajokl, data_wydania.Value, ISBN, ile);

                if (result.Success)
                {
                    MessageBox.Show(result.Message, "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadBooks();
                    ClearForm();
                }
                else
                {
                    MessageBox.Show(result.ErrorMessage, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void ustaw_ksiazke(BookInfo book)
        {
            _selectedBook = book;
            _adminpanel.tytul = book.Tytul;
            _adminpanel.wybranyrodzaj = book.Rodzaj.ToString();
            _adminpanel.wybranyrodzajokladki = book.RodzajOkladki.ToString();
            _adminpanel.gatunek = book.Gatunki;
            _adminpanel.autorzy = book.Autorzy;
            _adminpanel.ISBN = book.ISBN ?? "";
            _adminpanel.datawyd = new DateTime(book.RokWydania ?? DateTime.Now.Year, 1, 1);
            _adminpanel.SetEditMode(true);
            _isEditMode = true;
        }
        private void edytuj_ksiazke(string tytul, string rodzaj, string rodzajokl, DateTimePicker data_wydania, string ISBN, string gatunki, string autorzy, int ile)
        {
            _adminpanel.listaksiazekzmien = _adminmodel.dodajksiaz(tytul, rodzaj, rodzajokl, data_wydania, ISBN, gatunki, autorzy, ile);
        }
        private void usun_ksiazke(BookInfo book)
        {
            var confirmResult = MessageBox.Show(
                $"Czy na pewno chcesz usunąć książkę '{book.Tytul}'?",
                "Potwierdzenie",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                var result = _adminmodel.usunksiaz(book.Id);

                if (result.Success)
                {
                    MessageBox.Show(result.Message, "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadBooks();
                    ClearForm();
                }
                else
                {
                    MessageBox.Show(result.ErrorMessage, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void zmien_paneluz()
        {
            _adminpanel.Hide();
            _adminuz.Show();
            Adminmodeluzytkownicy adminmodeluzytkownicy = new Adminmodeluzytkownicy();
            Presenteradminuzytkownicy presenteradminuzytkownicy = new Presenteradminuzytkownicy(_adminuz,adminmodeluzytkownicy, _adminpanel, _adminpanelogloszenia, _bookForm);
        }
        private void zmien_panelogl()
        {
            _adminpanel.Hide();
            _adminpanelogloszenia.Show();
            Adminmodelogloszenia adminmodelogloszenia = new Adminmodelogloszenia();
            Presenteradminogloszenia presenteradminogloszenia = new Presenteradminogloszenia(_adminpanelogloszenia, adminmodelogloszenia, _adminpanel, _adminuz, _bookForm);
        }
        private void powrotglowna()
        {
            _adminpanel.Hide();
            _bookForm.tablica.Show();
        }
    }
}
