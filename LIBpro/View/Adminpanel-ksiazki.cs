using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using LibraryApp.Model;

namespace LibraryApp.View
{
    public partial class Adminpanelksiazki : UserControl
    {
        public Adminpanelksiazki()
        {
            InitializeComponent();
        }
        public event Action<string> dodajgatunek;
        public event Action wyczyscgatunki;
        public event Action<string, string> dodajautora;
        public event Action wyczyscautorow;
        public event Action<string, string, string, DateTimePicker, string, string, string, int> dodajksiazke;
        public event Action<string, string, string, DateTimePicker, string, string, string, int> edytujksiazke;
        public event Action paneluzytkownikow;
        public event Action panelogloszen;
        public event Action powrotglowna;
        public event Action<BookInfo> ustawksiazke;
        public event Action<BookInfo> usunksiazke;
        public event Action wylogowanie;
        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                comboBox1.BackColor = Color.White;
                dodajgatunek?.Invoke(comboBox1.SelectedItem.ToString());
            }
            else
            {
                comboBox1.BackColor = Color.Red;
            }
        }
        public string tytul
        {
            set
            {
                textBox1.Text = value;
            }
            get
            {
                return textBox1.Text;
            }
        }
        public string gatunek
        {
            set
            {
                label8.Text = value;
            }
            get
            {
                return label8.Text;
            }
        }
        public string autorzy
        {
            set
            {
                label12.Text = value;
            }
            get
            {
                return label12.Text;
            }
        }
        public string[] listagatunkow
        {
            set
            {
                if (comboBox1.Items.Count == 0)
                {
                    comboBox1.BeginUpdate();
                    for (int i = 0; i < value.Length; i++)
                        comboBox1.Items.Add(value[i]);
                    comboBox1.EndUpdate();
                }
            }
            get
            {
                return comboBox1.Items.Cast<string>().ToArray();
            }
        }
        public string[] rodzaj
        {
            set
            {
                if (comboBox2.Items.Count == 0)
                {
                    comboBox2.BeginUpdate();
                    for (int i = 0; i < value.Length; i++)
                        comboBox2.Items.Add(value[i]);
                    comboBox2.EndUpdate();
                }
            }
            get
            {
                return comboBox2.Items.Cast<string>().ToArray();
            }
        }
        public DateTime datawyd
        {
            set
            {
                dateTimePicker1.Value = value;
            }
            get
            {
                return dateTimePicker1.Value;
            }
        }
        public string ISBN
        {
            set
            {
                textBox2.Text = value;
            }
            get
            {
                return textBox2.Text;
            }
        }
        public string[] rodzajokladki
        {
            set
            {
                if (comboBox3.Items.Count == 0)
                {
                    comboBox3.BeginUpdate();
                    for (int i = 0; i < value.Length; i++)
                        comboBox3.Items.Add(value[i]);
                    comboBox3.EndUpdate();
                }
            }
            get
            {
                return comboBox3.Items.Cast<string>().ToArray();
            }
        }
        public string wybranyrodzaj
        {
            set
            {
                comboBox2.SelectedIndex = -1;
                for (int i = 0; i < comboBox2.Items.Count; i++)
                {
                    if (value == comboBox2.Items[i].ToString())
                    {
                        comboBox2.SelectedIndex = i;
                    }
                }
            }
            get
            {
                return comboBox2.SelectedItem.ToString();
            }
        }
        public string wybranyrodzajokladki
        {
            set
            {
                for (int i = 0; i < comboBox3.Items.Count; i++)
                {
                    if (value == comboBox3.Items[i].ToString())
                    {
                        comboBox3.SelectedIndex = i;
                    }
                }
            }
            get
            {
                return comboBox3.SelectedItem.ToString();
            }
        }
        public string listaksiazekdod
        {
            set
            {
                listBox1.BeginUpdate();
                listBox1.Items.Add(value);
                listBox1.EndUpdate();
            }
        }
        public string listaksiazekzmien
        {
            set
            {
                listBox1.Items[listBox1.SelectedIndex] = value;
            }
        }
        public int listaksiazekusun
        {
            set
            {
                listBox1.Items.Remove(listBox1.Items[value]);
            }
        }
        public int ilosc
        {
            set
            {
                numericUpDown1.Value = value;
            }
            get
            {
                return (int)numericUpDown1.Value;
            }
        }
        public void button4_Click(object sender, EventArgs e)
        {
            dodajautora.Invoke(textBox5.Text, textBox7.Text);
        }
        public void button5_Click(object sender, EventArgs e)
        {
            wyczyscautorow.Invoke();
        }
        public void button6_Click(object sender, EventArgs e)
        {
            wyczyscgatunki.Invoke();
        }
        public void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && comboBox2.SelectedItem != null && comboBox3.SelectedItem != null && label8.Text != null && label12.Text != null)
            {
                dodajksiazke.Invoke(textBox1.Text, comboBox2.SelectedItem.ToString(), comboBox3.SelectedItem.ToString(), dateTimePicker1, textBox2.Text, label8.Text, label12.Text, ((int)numericUpDown1.Value));
            }
            else
            {
                MessageBox.Show("nie uzupełniono wszyskich danych");
            }
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem is BookInfo selectedBook)
            {
                ustawksiazke?.Invoke(selectedBook);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (button7.Text == "Anuluj")
            {
                SetEditMode(false);
            }
            else
            {
                var selectedBook = GetSelectedBook();
                if (selectedBook != null)
                {
                    ustawksiazke?.Invoke(selectedBook);
                }
                else
                {
                    MessageBox.Show("Wybierz książkę do edycji", "Uwaga", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        private void button2_Click_1(object sender, EventArgs e)
        {
            var selectedBook = GetSelectedBook();
            if (selectedBook != null)
            {
                usunksiazke?.Invoke(selectedBook);
            }
            else
            {
                MessageBox.Show("Wybierz książkę do usunięcia", "Uwaga", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            paneluzytkownikow.Invoke();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            panelogloszen.Invoke();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            powrotglowna.Invoke();
        }

        public void LoadBookList(List<BookInfo> books)
        {
            listBox1.Items.Clear();
            foreach (var book in books)
            {
                listBox1.Items.Add(book);
            }
        }

        public void SetEditMode(bool isEditing)
        {
            if (isEditing)
            {
                button1.Text = "Aktualizuj";
                button7.Text = "Anuluj";
            }
            else
            {
                button1.Text = "Dodaj książkę";
                button7.Text = "Edytuj";
            }
        }

        public BookInfo? GetSelectedBook()
        {
            return listBox1.SelectedItem as BookInfo;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            wylogowanie.Invoke();
        }
    }
}