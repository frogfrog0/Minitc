using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryApp.Model;

namespace LibraryApp.View
{
    public partial class BookForm : Form
    {
        public event Action logowaniepanel;
        public event Action rejestracjapanel;
        UserControl[] userControls;
        public BookForm()
        {
            InitializeComponent();
            userControls = new UserControl[] { logowanie1, rejestracja1, paneluzytkownika1, adminpanelksiazki1, adminpanelogloszenia1, adminpaneluzytkownicy1 };
            cmbSort.Items.AddRange(new string[] { "Autor", "Nazwa", "Data wydania" });
            comboBox1.Items.AddRange(new string[] { "książka", "album", "komiks" });
            comboBox2.Items.AddRange(new string[] { "miękka", "twarda" });
            cmbSort.SelectedIndex = 0;
            for (int i = 0; i < userControls.Length; i++)
            {
                userControls[i].Hide();
            }

            txtTitle.TextChanged += (s, e) => FilterChanged?.Invoke();
            txtGenre.TextChanged += (s, e) => FilterChanged?.Invoke();
            txtISBN.TextChanged += (s, e) => FilterChanged?.Invoke();
            txtAuthor.TextChanged += (s, e) => FilterChanged?.Invoke();
            txtReleaseYear.TextChanged += (s, e) => FilterChanged?.Invoke();
            cmbSort.SelectedIndexChanged += (s, e) => FilterChanged?.Invoke();
            comboBox1.SelectedIndexChanged += (s, e) => FilterChanged?.Invoke();
            comboBox2.SelectedIndexChanged += (s, e) => FilterChanged?.Invoke();
            dgvBooks.CellClick += dgvBooks_CellClick;
        }

        public string FilterTitle => txtTitle.Text;
        public string FilterGenre => txtGenre.Text;
        public string FilterISBN => txtISBN.Text;
        public string FilterAuthor => txtAuthor.Text;
        public string FilterReleaseYear => txtReleaseYear.Text;
        public string Filterrodzaj => comboBox1.SelectedItem?.ToString();
        public string Filterrodzajokladki => comboBox2.SelectedItem?.ToString();
        public string SortBy => cmbSort.SelectedItem?.ToString() ?? "Nazwa";

        public event Action FilterChanged;
        public TableLayoutPanel tablica
        {
            get
            {
                return tableLayoutPanel1;
            }
        }
        public UserControl logowanie
        {
            get
            {
                return logowanie1;
            }
        } 
        public UserControl rejestracja
        {
            get
            {
                return rejestracja1;
            }
        }
        public UserControl paneluzytkownika
        {
            get
            {
                return paneluzytkownika1;
            }
        }
        public void SetBookList(List<Book> books)
        {
            dgvBooks.Columns.Clear();
            dgvBooks.DataSource = null;
            dgvBooks.AutoGenerateColumns = false;

            dgvBooks.DataSource = books;
            dgvBooks.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Title", HeaderText = "Tytuł", Width = 260 });
            dgvBooks.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Genre", HeaderText = "Gatunek" });
            dgvBooks.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ISBN", HeaderText = "ISBN", Width = 150 });
            dgvBooks.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Author", HeaderText = "Autor", Width = 150 });
            dgvBooks.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ReleaseYear", HeaderText = "Rok wydania", Width = 80 });
            dgvBooks.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Rodzaj", HeaderText = "Rodzaj" });
            dgvBooks.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Rodzajokladki", HeaderText = "Rodzaj okładki" });
            dgvBooks.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ExemplarsStatus", HeaderText = "Egzemplarze" });

            var borrowButtonColumn = new DataGridViewButtonColumn
            {
                HeaderText = "Menu",
                Name = "BorrowButton1",
                Text = "WypożyczMenu",
                UseColumnTextForButtonValue = true
            };
            dgvBooks.Columns.Add(borrowButtonColumn);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            FilterChanged?.Invoke(); // wymuś odświeżenie widoku przy starcie
        }

        private void dgvBooks_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvBooks.Columns[e.ColumnIndex].Name == "BorrowButton1")
            {
                // Przykład – pobierz tytuł książki
                Book book = dgvBooks.Rows[e.RowIndex].DataBoundItem as Book;
                Debug.WriteLine(book.Title);

                // Otwórz nowe okno z egzemplarzami
                var exemplars = book.Exemplars; // symulowane dane
                var exemplarsForm = new ExemplarsForm(book.Title, exemplars, dgvBooks);
                exemplarsForm.ShowDialog();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            logowaniepanel.Invoke();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            rejestracjapanel.Invoke();
        }
    }
}
