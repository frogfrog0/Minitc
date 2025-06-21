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
    internal class Presenteradminuzytkownicy
    {
        private View.Adminpaneluzytkownicy _adminpaneluzytkownicy;
        private Adminmodeluzytkownicy _adminmodeluzytkownicy;
        private Adminpanelksiazki _ksiaz;
        private Adminpanelogloszenia _adminpanelogloszenia;
        private BookForm _bookForm;
        public Presenteradminuzytkownicy(View.Adminpaneluzytkownicy adminpaneluzytkownicy, Adminmodeluzytkownicy adminmodeluzytkownicy, Adminpanelksiazki ksiaz, Adminpanelogloszenia adminpanelogloszenia, BookForm bookForm)
        {
            _adminpaneluzytkownicy = adminpaneluzytkownicy;
            _adminmodeluzytkownicy = adminmodeluzytkownicy;
            _ksiaz = ksiaz;
            _bookForm = bookForm;
            _adminpanelogloszenia = adminpanelogloszenia;
            _adminpaneluzytkownicy.paneluz += zmienpanelksiaz;
            _adminpaneluzytkownicy.ustawuzytkownika += ustawuzytkownika;
            _adminpaneluzytkownicy.dajban += dajbana;
            _adminpaneluzytkownicy.panelogl += ustaw_panelogl;
            _adminpaneluzytkownicy.powrotglowna += powrotglowna;

            LoadUsers();
        }
        public void zmienpanelksiaz()
        {
            _adminpaneluzytkownicy.Hide();
            _ksiaz.Show();
            Adminmodel adminksiaz = new Adminmodel();
            Presenteradmin presenteradmin = new Presenteradmin(_ksiaz,adminksiaz, _adminpaneluzytkownicy, _adminpanelogloszenia, _bookForm);
        }
        public void ustawuzytkownika(string uzyt)
        {
            _adminpaneluzytkownicy.uzytkownicy=_adminmodeluzytkownicy.ustawuz(uzyt);
        }
        public void dajbana(string uzyt)
        {
            _adminpaneluzytkownicy.uzytkownicy = _adminmodeluzytkownicy.dajbana(uzyt);
        }
        public void ustaw_panelogl()
        {
            _adminpaneluzytkownicy.Hide();
            _adminpanelogloszenia.Show();
            Adminmodelogloszenia adminmodelogloszenia = new Adminmodelogloszenia();
            Presenteradminogloszenia presenteradminogloszenia = new Presenteradminogloszenia(_adminpanelogloszenia, adminmodelogloszenia, _ksiaz, _adminpaneluzytkownicy, _bookForm);
        }
        public void powrotglowna()
        {
            _adminpaneluzytkownicy.Hide();
            _bookForm.Show();
        }
        private void LoadUsers()
        {
            var users = _adminmodeluzytkownicy.GetAllUsers();
            _adminpaneluzytkownicy.LoadUserList(users);
        }

        private void HandleUserSelection(UserInfo selectedUser)
        {
            // Update UI to show selected user details
            _adminpaneluzytkownicy.SetSelectedUser(selectedUser);
        }

        private void HandleBanToggle(UserInfo user)
        {
            var result = _adminmodeluzytkownicy.ToggleBan(user.Id);

            if (result.Success)
            {
                MessageBox.Show(result.Message, "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadUsers(); // Refresh user list
            }
            else
            {
                MessageBox.Show(result.ErrorMessage, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
