using LibraryApp.View;

namespace LibraryApp
{
    public partial class Form1 : Form
    {
        public UserControl[] userControls;
        public Form1()
        {
            InitializeComponent();
            userControls = new UserControl[] { adminpanelksiazki1, logowanie1, rejestracja1, paneluzytkownika1, userControl12, adminpanelogloszenia1};
            showcontent(0);
        }
        public void showcontent(int poz)
        {
            for (int i = 0; i < userControls.Length; i++)
            {
                userControls[i].Hide();
            }
            userControls[poz].Show();
            if (poz == 0)
            {
                var view = adminpanelksiazki1;
                var model = new Model.Adminmodel();
                var presenter = new Presenter.Presenteradmin(view, model, userControl12, adminpanelogloszenia1);
            }
            else if (poz == 1)
            {
                var view = logowanie1;
                var model = new Model.Logowaniemodel();
                var presenter = new Presenter.Presenterlogowanie(view, model, paneluzytkownika1);
            }
            else if (poz == 2)
            {
                var view = rejestracja1;
                var model = new Model.Rejestracjamodel();
                var presenter = new Presenter.Presenterrejestracja(view, model, paneluzytkownika1);
            }
            else
            {
                var view = paneluzytkownika1;
            }
        }
    }
}
