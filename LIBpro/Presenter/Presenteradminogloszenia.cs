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
    internal class Presenteradminogloszenia
    {
        View.Adminpanelogloszenia _adminpanelogloszenia;
        Model.Adminmodelogloszenia _adminmodelogloszenia;
        View.Adminpanelksiazki _adminpanelksiazki;
        View.Adminpaneluzytkownicy _adminpaneluzytkownicy;
        BookForm _bookForm;

        private AnnouncementInfo? _selectedAnnouncement;
        private bool _isEditing = false;
        public Presenteradminogloszenia(View.Adminpanelogloszenia adminpanelogloszenia, Model.Adminmodelogloszenia adminmodelogloszenia, View.Adminpanelksiazki adminpanelksiazki, View.Adminpaneluzytkownicy adminpaneluzytkownicy, BookForm bookForm)
        {
            _adminmodelogloszenia = adminmodelogloszenia;
            _adminpanelogloszenia = adminpanelogloszenia;
            _adminpanelksiazki = adminpanelksiazki;
            _adminpaneluzytkownicy = adminpaneluzytkownicy;
            _adminpanelogloszenia.dodajogl += dodajoglo;
            _adminpanelogloszenia.ustawogl += ustawoglo;
            _adminpanelogloszenia.panelks += ustawpanelks;
            _adminpanelogloszenia.paneluz += ustawpaneluz;
            _adminpanelogloszenia.edytujogl += edytujogl;
            _adminpanelogloszenia.usunogl += usunoglo;
            _adminpanelogloszenia.powrotglowna += powrotglowna;
            _bookForm = bookForm;

            LoadAnnouncements();
        }
        private void LoadAnnouncements()
        {
            var announcements = _adminmodelogloszenia.GetAllAnnouncements();
            _adminpanelogloszenia.LoadAnnouncementList(announcements);
        }

        private void dodajoglo(string tytul, string opis)
        {
            if (_isEditing && _selectedAnnouncement != null)
            {
                // Update existing announcement
                var result = _adminmodelogloszenia.UpdateAnnouncement(_selectedAnnouncement.Id, tytul, opis);

                if (result.Success)
                {
                    MessageBox.Show(result.Message, "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadAnnouncements(); // Refresh list
                    ClearForm();
                }
                else
                {
                    MessageBox.Show(result.ErrorMessage, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // Add new announcement
                var result = _adminmodelogloszenia.AddAnnouncement(tytul, opis);

                if (result.Success)
                {
                    MessageBox.Show(result.Message, "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadAnnouncements(); // Refresh list
                    ClearForm();
                }
                else
                {
                    MessageBox.Show(result.ErrorMessage, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ustawoglo(AnnouncementInfo announcement)
        {
            _selectedAnnouncement = announcement;
            _adminpanelogloszenia.tytul = announcement.Tytul;
            _adminpanelogloszenia.opis = announcement.Opis;
            _adminpanelogloszenia.SetEditMode(true);
            _isEditing = true;
        }

        private void edytujogl(string tytul, string opis)
        {
            // This is now handled in dodajoglo method
            dodajoglo(tytul, opis);
        }

        private void usunoglo(AnnouncementInfo announcement)
        {
            var confirmResult = MessageBox.Show(
                $"Czy na pewno chcesz usunąć ogłoszenie '{announcement.Tytul}'?",
                "Potwierdzenie",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                var result = _adminmodelogloszenia.DeleteAnnouncement(announcement.Id);

                if (result.Success)
                {
                    MessageBox.Show(result.Message, "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadAnnouncements(); // Refresh list
                    ClearForm();
                }
                else
                {
                    MessageBox.Show(result.ErrorMessage, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ClearForm()
        {
            _adminpanelogloszenia.tytul = "";
            _adminpanelogloszenia.opis = "";
            _adminpanelogloszenia.SetEditMode(false);
            _selectedAnnouncement = null;
            _isEditing = false;
        }
        public void powrotglowna()
        {
            _adminpaneluzytkownicy.Hide();
            _bookForm.Show();
            if (UserSession.IsLoggedIn)
            {
                _bookForm.przycisk1.Text = "panel użytkownika";
                _bookForm.przycisk2.Hide();
            }
        }
        public void ustawpaneluz()
        {
            _adminpanelogloszenia.Hide();
            _adminpaneluzytkownicy.Show();
            Adminmodeluzytkownicy adminmodeluzytkownicy = new Adminmodeluzytkownicy();
            Presenteradminuzytkownicy presenteradminuzytkownicy = new Presenteradminuzytkownicy(_adminpaneluzytkownicy, adminmodeluzytkownicy, _adminpanelksiazki, _adminpanelogloszenia, _bookForm);
        }
        public void ustawpanelks()
        {
            _adminpanelogloszenia.Hide();
            _adminpanelksiazki.Show();
            Adminmodel adminmodel = new Adminmodel();
            Presenteradmin presenteradmin = new Presenteradmin(_adminpanelksiazki, adminmodel, _adminpaneluzytkownicy, _adminpanelogloszenia, _bookForm);
        }
    }
}
