namespace LibraryApp
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            logowanie1 = new LibraryApp.View.Logowanie();
            rejestracja1 = new LibraryApp.View.Rejestracja();
            paneluzytkownika1 = new LibraryApp.View.Paneluzytkownika();
            userControl12 = new LibraryApp.View.Adminpaneluzytkownicy();
            adminpanelksiazki1 = new LibraryApp.View.Adminpanelksiazki();
            adminpanelogloszenia1 = new LibraryApp.View.Adminpanelogloszenia();
            SuspendLayout();
            // 
            // logowanie1
            // 
            logowanie1.Location = new Point(-5, -5);
            logowanie1.Name = "logowanie1";
            logowanie1.Size = new Size(978, 634);
            logowanie1.TabIndex = 1;
            // 
            // rejestracja1
            // 
            rejestracja1.Location = new Point(-5, -5);
            rejestracja1.Name = "rejestracja1";
            rejestracja1.Size = new Size(1332, 699);
            rejestracja1.TabIndex = 2;
            // 
            // paneluzytkownika1
            // 
            paneluzytkownika1.email = "";
            paneluzytkownika1.kod1 = "";
            paneluzytkownika1.kod2 = "";
            paneluzytkownika1.Location = new Point(-4, -5);
            paneluzytkownika1.miasto = "";
            paneluzytkownika1.Name = "paneluzytkownika1";
            paneluzytkownika1.numdom = "";
            paneluzytkownika1.nummiesz = "";
            paneluzytkownika1.numtel = "";
            paneluzytkownika1.Size = new Size(1289, 674);
            paneluzytkownika1.TabIndex = 3;
            paneluzytkownika1.ul = "";
            paneluzytkownika1.woj = "";
            // 
            // userControl12
            // 
            userControl12.Location = new Point(-5, -5);
            userControl12.Name = "userControl12";
            userControl12.Size = new Size(1350, 679);
            userControl12.TabIndex = 4;
            userControl12.uzytkownicy = "";
            // 
            // adminpanelksiazki1
            // 
            adminpanelksiazki1.autorzy = "";
            adminpanelksiazki1.datawyd = new DateTime(2025, 6, 19, 20, 30, 34, 616);
            adminpanelksiazki1.gatunek = "";
            adminpanelksiazki1.ilosc = 0;
            adminpanelksiazki1.ISBN = "";
            adminpanelksiazki1.Location = new Point(-1, -9);
            adminpanelksiazki1.Name = "adminpanelksiazki1";
            adminpanelksiazki1.Size = new Size(1356, 669);
            adminpanelksiazki1.TabIndex = 5;
            adminpanelksiazki1.tytul = "";
            // 
            // adminpanelogloszenia1
            // 
            adminpanelogloszenia1.Location = new Point(-5, -24);
            adminpanelogloszenia1.Name = "adminpanelogloszenia1";
            adminpanelogloszenia1.Size = new Size(1720, 684);
            adminpanelogloszenia1.TabIndex = 6;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1343, 519);
            Controls.Add(adminpanelogloszenia1);
            Controls.Add(adminpanelksiazki1);
            Controls.Add(userControl12);
            Controls.Add(paneluzytkownika1);
            Controls.Add(rejestracja1);
            Controls.Add(logowanie1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private View.Logowanie logowanie1;
        private View.Rejestracja rejestracja1;
        private View.Paneluzytkownika paneluzytkownika1;
        private View.Adminpaneluzytkownicy userControl12;
        private View.Adminpanelksiazki adminpanelksiazki1;
        private View.Adminpanelogloszenia adminpanelogloszenia1;
    }
}
