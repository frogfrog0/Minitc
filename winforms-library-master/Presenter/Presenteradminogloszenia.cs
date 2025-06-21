using LibraryApp.Model;
using LibraryApp.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Presenter
{
    internal class Presenteradminogloszenia
    {
        View.Adminpanelogloszenia _adminpanelogloszenia;
        Model.Adminmodelogloszenia _adminmodelogloszenia;
        View.Adminpanelksiazki _adminpanelksiazki;
        View.Adminpaneluzytkownicy _adminpaneluzytkownicy;
        BookForm _bookForm;
        public Presenteradminogloszenia(View.Adminpanelogloszenia adminpanelogloszenia, Model.Adminmodelogloszenia adminmodelogloszenia, View.Adminpanelksiazki adminpanelksiazki, View.Adminpaneluzytkownicy adminpaneluzytkownicy, BookForm bookForm)
        {
            _adminmodelogloszenia = adminmodelogloszenia;
            _adminpanelogloszenia = adminpanelogloszenia;
            _adminpanelksiazki = adminpanelksiazki;
            _adminpaneluzytkownicy = adminpaneluzytkownicy;
            _bookForm = bookForm;
            _adminpanelogloszenia.dodajogl += dodajoglo;
            _adminpanelogloszenia.ustawogl += ustawoglo;
            _adminpanelogloszenia.panelks += ustawpanelks;
            _adminpanelogloszenia.paneluz += ustawpaneluz;
            _adminpanelogloszenia.edytujogl += edytujogl;
            _adminpanelogloszenia.usunogl += usunoglo;
            _adminpanelogloszenia.powrotglowna += powrotglowna;
            _bookForm = bookForm;
        }
        private void dodajoglo(string tytul, string opis)
        {
            _adminpanelogloszenia.dodawanieogl = _adminmodelogloszenia.dodajogloszenie(tytul, opis);
        }
        private void ustawoglo(string ogloszenie)
        {
            _adminpanelogloszenia.tytul = _adminmodelogloszenia.gettytul(ogloszenie);
            _adminpanelogloszenia.opis = _adminmodelogloszenia.getopis(ogloszenie);
        }
        private void edytujogl(string tytul, string opis)
        {
            _adminpanelogloszenia.zmienogl = _adminmodelogloszenia.dodajogloszenie(tytul, opis);
        }
        private void ustawpanelks()
        {
            _adminpanelogloszenia.Hide();
            _adminpanelksiazki.Show();
            Adminmodel adminmodel = new Adminmodel();
            Presenteradmin presenteradmin = new Presenteradmin(_adminpanelksiazki, adminmodel, _adminpaneluzytkownicy, _adminpanelogloszenia, _bookForm);
        }
        private void ustawpaneluz()
        {
            _adminpanelogloszenia.Hide();
            _adminpaneluzytkownicy.Show();
            Adminmodeluzytkownicy adminmodel = new Adminmodeluzytkownicy();
            Presenteradminuzytkownicy presenteradmin = new Presenteradminuzytkownicy(_adminpaneluzytkownicy, adminmodel, _adminpanelksiazki, _adminpanelogloszenia, _bookForm);
        }
        private void usunoglo(string ogloszenie)
        {
            _adminpanelogloszenia.usunogloszenie = _adminmodelogloszenia.usunogloszenie(ogloszenie);
        }
        private void powrotglowna()
        {
            _adminpanelogloszenia.Hide();
            _bookForm.tablica.Show();
        }
    }
}
