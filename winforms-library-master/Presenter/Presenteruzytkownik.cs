using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryApp.Model;
using LibraryApp.View;
using Microsoft.Win32;

namespace LibraryApp.Presenter
{
    internal class Presenteruzytkownik
    {
        private View.Paneluzytkownika _paneluzytkownika;
        private Uzytkownikmodel _uzytkownikmodel;
        BookForm _bookform;
        public Presenteruzytkownik(View.Paneluzytkownika paneluzytkownika, Uzytkownikmodel uzytkownikmodel, BookForm bookForm)
        {
            _paneluzytkownika = paneluzytkownika;
            _uzytkownikmodel = uzytkownikmodel;
            _bookform = bookForm;
            _paneluzytkownika.zmianadanych += zmiendane;
            _paneluzytkownika.email = _uzytkownikmodel.getemail();
            _paneluzytkownika.numtel = _uzytkownikmodel.getnumtel();
            _paneluzytkownika.woj = _uzytkownikmodel.getwoj();
            _paneluzytkownika.miasto = _uzytkownikmodel.getmiasto();
            _paneluzytkownika.ul = _uzytkownikmodel.getul();
            _paneluzytkownika.numdom = _uzytkownikmodel.getnumdom();
            _paneluzytkownika.nummiesz = _uzytkownikmodel.getnummiesz();
            _paneluzytkownika.kod1 = _uzytkownikmodel.getkod1();
            _paneluzytkownika.kod2 = _uzytkownikmodel.getkod2();
            _paneluzytkownika.powrot += powrot1;
        }
        private void zmiendane(string email,string numtel, string woj, string miasto, string ul, string numdom, string nummiesz, string kod1, string kod2)
        {
            _uzytkownikmodel.zmianadan(email, numtel, woj, miasto, ul, numdom, nummiesz, kod1, kod2);
        }
        private void powrot1()
        {
            _paneluzytkownika.Hide();
            _bookform.tablica.Show();
        }
    }
}
