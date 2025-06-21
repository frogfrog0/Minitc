using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryApp.Model;
using LibraryApp.View;

namespace LibraryApp.Presenter
{
    internal class Presenteradmin
    {
        private View.Adminpanelksiazki _adminpanel;
        private Adminmodel _adminmodel;
        private Adminpaneluzytkownicy _adminuz;
        private Adminpanelogloszenia _adminpanelogloszenia;
        private BookForm _bookForm;
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
            _bookForm = bookForm;
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
        private void dodaj_ksiazke(string tytul, string rodzaj, string rodzajokl,DateTimePicker data_wydania, string ISBN, string gatunki, string autorzy, int ile)
        {
            _adminpanel.listaksiazekdod = _adminmodel.dodajksiaz(tytul,rodzaj,rodzajokl,data_wydania,ISBN, gatunki, autorzy, ile);
            wyczyscautorow();
            wyczyscgatunki();
        }
        private void ustaw_ksiazke(string ksiazka)
        {
            _adminpanel.tytul = _adminmodel.tytul(ksiazka);
            _adminpanel.wybranyrodzaj = _adminmodel.rodzaj(ksiazka);
            _adminpanel.wybranyrodzajokladki = _adminmodel.rodzajokladki(ksiazka);
            _adminpanel.datawyd = _adminmodel.getdata_wyd(ksiazka);
            _adminpanel.ISBN = _adminmodel.getISBN(ksiazka);
            _adminpanel.gatunek = _adminmodel.gatunki(ksiazka);
            _adminpanel.autorzy = _adminmodel.autorzy(ksiazka);
            _adminpanel.ilosc = _adminmodel.getilosc(ksiazka);
            
        }
        private void edytuj_ksiazke(string tytul, string rodzaj, string rodzajokl, DateTimePicker data_wydania, string ISBN, string gatunki, string autorzy, int ile)
        {
            _adminpanel.listaksiazekzmien = _adminmodel.dodajksiaz(tytul, rodzaj, rodzajokl, data_wydania, ISBN, gatunki, autorzy, ile);
        }
        private void usun_ksiazke(int indeks)
        {
            _adminpanel.listaksiazekusun = _adminmodel.usunksiaz(indeks);
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
