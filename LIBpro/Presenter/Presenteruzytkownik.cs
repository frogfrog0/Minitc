﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryApp.Model;
using LibraryApp.View;
using Microsoft.Win32;
using System.Windows.Forms;
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
            _paneluzytkownika.powrot += powrot1;

            // Load user data
            LoadUserData();
            LoadUserBooks();
        }
        private void zmiendane(string email, string numtel, string woj, string miasto, string ul, string numdom, string nummiesz, string kod1, string kod2)
        {
            var result = _uzytkownikmodel.zmianadan(email, numtel, woj, miasto, ul, numdom, nummiesz, kod1, kod2);

            if (result.Success)
            {
                MessageBox.Show("Dane zostały zaktualizowane", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(result.ErrorMessage, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void powrot1()
        {
            _paneluzytkownika.Hide();
            _bookform.tablica.Show();
        }
        private void LoadUserData()
        {
            _paneluzytkownika.email = _uzytkownikmodel.getemail();
            _paneluzytkownika.numtel = _uzytkownikmodel.getnumtel();
            _paneluzytkownika.woj = _uzytkownikmodel.getwoj();
            _paneluzytkownika.miasto = _uzytkownikmodel.getmiasto();
            _paneluzytkownika.ul = _uzytkownikmodel.getul();
            _paneluzytkownika.numdom = _uzytkownikmodel.getnumdom();
            _paneluzytkownika.nummiesz = _uzytkownikmodel.getnummiesz();
            _paneluzytkownika.kod1 = _uzytkownikmodel.getkod1();
            _paneluzytkownika.kod2 = _uzytkownikmodel.getkod2();

            // Set user name and balance (will need to add properties to view)
            _paneluzytkownika.SetUserInfo(_uzytkownikmodel.GetUserFullName(), _uzytkownikmodel.GetUserSaldo());
        }

        private void LoadUserBooks()
        {
            // Load borrowed books
            var borrowedBooks = _uzytkownikmodel.GetBorrowedBooks();
            _paneluzytkownika.SetBorrowedBooks(borrowedBooks);

            // Load favorite books  
            var favoriteBooks = _uzytkownikmodel.GetFavoriteBooks();
            _paneluzytkownika.SetFavoriteBooks(favoriteBooks);
        }
    }
}
