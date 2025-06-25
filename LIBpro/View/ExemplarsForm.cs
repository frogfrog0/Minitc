using LibraryApp;
using LibraryApp.Model;
using LibraryManagement.Models.Data;
using LibraryManagement.Models.Entities.JunctionTables;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.View
{
    public partial class ExemplarsForm : Form
    {
        private DataGridView dgvExemplars;
        private DataGridView dgvBooks;
        private BindingList<Exemplar> exemplars;
        private LibraryContext _context;
        private BorrowingService borrowingService = new BorrowingService();
        public event Action<Exemplar> porzycz;
        public event Action<Exemplar> zwroc;
        public event Action<Exemplar, int> przedluz;

        public ExemplarsForm(string bookTitle, List<Exemplar> exemplarList, DataGridView dgv)
        {
            this.Text = $"Egzemplarze - {bookTitle}";
            this.Size = new Size(700, 400);
            InitializeComponent();

            exemplars = new BindingList<Exemplar>(exemplarList);
            dgvBooks = dgv;
            dgvExemplars.DataSource = exemplars;
            AddActionButtons();
            var connectionString = "Data Source=Library.db";
            var optionsBuilder = new DbContextOptionsBuilder<LibraryContext>();
            optionsBuilder.UseSqlite(connectionString);
            _context = new LibraryContext(optionsBuilder.Options);
        }

        private void AddActionButtons()
        {
            var borrowButtonCol = new DataGridViewButtonColumn
            {
                Name = "Borrow",
                HeaderText = "Wypożycz",
                Text = "Wypożycz",
                UseColumnTextForButtonValue = true
            };

            var returnButtonCol = new DataGridViewButtonColumn
            {
                Name = "Return",
                HeaderText = "Zwróć",
                Text = "Zwróć",
                UseColumnTextForButtonValue = true
            };

            var extendButtonCol = new DataGridViewButtonColumn
            {
                Name = "Extend",
                HeaderText = "Przedłuż",
                Text = "Przedłuż",
                UseColumnTextForButtonValue = true
            };

            dgvExemplars.Columns.Add(borrowButtonCol);
            dgvExemplars.Columns.Add(returnButtonCol);
            dgvExemplars.Columns.Add(extendButtonCol);
        }

        private void dgvExemplars_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            var exemplar = dgvExemplars.Rows[e.RowIndex].DataBoundItem as Exemplar;
            if (exemplar == null) return;

            string colName = dgvExemplars.Columns[e.ColumnIndex].Name;

            if (colName == "Borrow" && exemplar.IsAvailable)
            {
                exemplar.BorrowingDate = DateTime.Today;
                exemplar.ReturnDate = DateTime.Today.AddDays(14);
                porzycz.Invoke(exemplar);
                //borrowingService.BorrowBook(exemplar.BookId, (int)UserSession.UserId);
            }
            else if (colName == "Return" && !exemplar.IsAvailable)
            {
                exemplar.BorrowingDate = null;
                exemplar.ReturnDate = null;
                zwroc.Invoke(exemplar);
                //borrowingService.ReturnBook(exemplar.BookId, (int)UserSession.UserId);
            }
            else if (colName == "Extend" && !exemplar.IsAvailable)
            {
                exemplar.ReturnDate = exemplar.ReturnDate?.AddDays(14);
                przedluz.Invoke(exemplar, 14);
                //borrowingService.ExtendBorrowing(exemplar.BookId, (int)UserSession.UserId, 14);
            }

            dgvExemplars.Refresh();
            dgvBooks.Refresh();
        }

        private void dgvExemplars_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var exemplar = dgvExemplars.Rows[e.RowIndex].DataBoundItem as Exemplar;
            if (exemplar == null) return;

            if (exemplar.HasReturnDatePassed)
            {
                dgvExemplars.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightCoral;
            }
            else
            {
                dgvExemplars.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
            }
        }
    }
}
