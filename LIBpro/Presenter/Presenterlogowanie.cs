using LibraryApp.Model;
using LibraryApp.View;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibraryApp.Presenter
{
    internal class Presenterlogowanie
    {
        private View.Logowanie _logowanie;
        private Logowaniemodel _logowaniemodel;
        private UserControl _nastepnyinterface;
        private View.BookForm _bookform;
        public Presenterlogowanie(View.Logowanie logowanie, Logowaniemodel logowaniemodel, UserControl nastepnyinterface, BookForm bookform)
        {
            _bookform = bookform;
            _logowanie = logowanie;
            _logowaniemodel = logowaniemodel;
            _nastepnyinterface = nastepnyinterface;
            _logowanie.logowanie += login;
            _logowanie.powrotglowna += powrot;
            _bookform = bookform;
        }
        private void login(string email, string haslo)
        {
            var result = _logowaniemodel.Login(email, haslo);

            if (result.Success)
            {
                _logowanie.Hide();
                if (!UserSession.IsAdmin)
                {
                _nastepnyinterface.Show();
                var model = new Model.Uzytkownikmodel();
                var presenter = new Presenter.Presenteruzytkownik((Paneluzytkownika)_nastepnyinterface, model, _bookform);
                }
                else {
                    _bookform.PanelAdmina.Show();
                    var model = new Model.Adminmodel();
                    var presenter = new Presenter.Presenteradmin((Adminpanelksiazki)_bookform.PanelAdmina, model, (Adminpaneluzytkownicy)_bookform.Paneladminauzytkownicy, (Adminpanelogloszenia)_bookform.Paneladminaogloszenia, _bookform);
                }
            }
            else
            {
                MessageBox.Show(result.ErrorMessage, "Błąd logowania", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void powrot()
        {
            _logowanie.Hide();
            _bookform.tablica.Show();
        }
    }
}
