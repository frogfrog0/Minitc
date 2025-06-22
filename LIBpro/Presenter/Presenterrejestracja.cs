using LibraryApp.Model;
using LibraryApp.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace LibraryApp.Presenter
{
    internal class Presenterrejestracja
    {
        private View.Rejestracja _rejestracja;
        private Rejestracjamodel _rejestracjamodel;
        private UserControl _nastepnyinteface;
        private BookForm _bookform;
        public Presenterrejestracja(View.Rejestracja rejestracja, Rejestracjamodel rejestracjamodel, UserControl nastepnyinterface, BookForm bookForm)
        {
            _rejestracja = rejestracja;
            _bookform = bookForm;
            _rejestracjamodel = rejestracjamodel;
            _nastepnyinteface = nastepnyinterface;
            _rejestracja.rejestracja += register;
            _rejestracja.powrot += powrotglowna;
        }
        public void register(string imie, string nazwisko, string nazwauz, string haslo, string email, string numtel, DateTimePicker datur, string woj, string miasto, string ul, string numdom, string nummiesz, string kod1, string kod2)
        {
            var result = _rejestracjamodel.Register(imie, nazwisko, nazwauz, haslo, email, numtel, datur.Value, woj, miasto, ul, numdom, nummiesz, kod1, kod2);

            if (result.Success)
            {
                _rejestracja.Hide();
                _nastepnyinteface.Show();
                var model = new Model.Uzytkownikmodel();
                var presenter = new Presenter.Presenteruzytkownik((Paneluzytkownika)_nastepnyinteface, model, _bookform);
            }
            else
            {
                MessageBox.Show(result.ErrorMessage, "Błąd rejestracji", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void powrotglowna()
        {
            _rejestracja.Hide();
            _bookform.tablica.Show();
        }
    }
}
