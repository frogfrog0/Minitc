using System.Diagnostics;
using LibraryApp.View;
using LibraryApp.Model;
using LibraryApp.Presenter;
using LibraryApp.View;

namespace LibraryApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var form = new BookForm();
            var repo = new BookRepository();
            var panellog = new Logowanie();
            var panelrej = new Rejestracja();
            var paneluz = new Paneluzytkownika();
            Debug.WriteLine(repo.GetAll().Count);
            var presenter = new LibraryPresenter(form, repo);

            Application.Run(form);
        }
    }
}