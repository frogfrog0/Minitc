using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibraryApp.Model;

namespace LibraryApp.View
{
    public partial class Paneluzytkownika : UserControl
    {
        public event Action<string, string, string, string, string, string, string, string, string> zmianadanych;
        public event Action powrot;
        public event Action wylogowanie;
        public Paneluzytkownika()
        {
            InitializeComponent();
        }
        public string numtel
        {
            set { textBox2.Text = value; }
            get { return textBox2.Text; }
        }
        public string email
        {
            set { textBox1.Text = value; }
            get { return textBox1.Text; }
        }
        public string woj
        {
            set { textBox3.Text = value; }
            get { return textBox3.Text; }
        }
        public string miasto
        {
            set { textBox4.Text = value; }
            get { return textBox4.Text; }
        }
        public string ul
        {
            set { textBox5.Text = value; }
            get { return textBox5.Text; }
        }
        public string numdom
        {
            set { textBox6.Text = value; }
            get { return textBox6.Text; }
        }
        public string nummiesz
        {
            set { textBox7.Text = value; }
            get { return textBox7.Text; }
        }
        public string kod1
        {
            set { textBox8.Text = value; }
            get { return textBox8.Text; }
        }
        public string kod2
        {
            set { textBox9.Text = value; }
            get { return textBox9.Text; }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            zmianadanych.Invoke(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text, textBox7.Text, textBox8.Text, textBox9.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            powrot.Invoke();
        }

        public void SetUserInfo(string fullName, decimal saldo)
        {
            label2.Text = fullName.Split(' ').FirstOrDefault() ?? ""; // First name
            label3.Text = fullName.Split(' ').Skip(1).FirstOrDefault() ?? ""; // Last name
            label5.Text = $"{saldo:C}"; // Saldo with currency formatting
        }

        public void SetBorrowedBooks(List<BorrowedBookInfo> borrowedBooks)
        {
            listBox2.Items.Clear();
            foreach (var book in borrowedBooks)
            {
                listBox2.Items.Add(book.ToString());
            }
        }

        public void SetFavoriteBooks(List<FavoriteBookInfo> favoriteBooks)
        {
            listBox1.Items.Clear();
            foreach (var book in favoriteBooks)
            {
                listBox1.Items.Add(book.ToString());
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            wylogowanie.Invoke();
        }
    }
}
