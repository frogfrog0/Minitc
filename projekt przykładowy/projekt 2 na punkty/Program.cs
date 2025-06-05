using System.Windows.Forms;

namespace projekt_2_na_punkty
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            var view = new Form1();
            var model = new Model.Totalcommander();
            var presenter = new Presenter.presenter(view, model);
            Application.Run(view);
        }
    }
}