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
        public event Action<string> ustawksiazke;
        public event Action<string, string, string, DateTimePicker, string, string, string, int> edytujksiazke;
        public event Action<int> usunksiazke;
        public event Action paneluzytkownikow;
        public event Action panelogloszen;
        public event Action powrotglowna;
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
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != null && comboBox2.SelectedItem != null && comboBox3.SelectedItem != null && label8.Text != null && label12.Text != null)
            {
                dodajksiazke.Invoke(textBox1.Text, comboBox2.SelectedItem.ToString(), comboBox3.SelectedItem.ToString(), dateTimePicker1, textBox7.Text, label8.Text, label12.Text, ((int)numericUpDown1.Value));
            }
            else
            {
                MessageBox.Show("nie uzupełniono wszyskich danych");
            }
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
                ustawksiazke.Invoke(listBox1.SelectedItem.ToString());
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
                edytujksiazke.Invoke(textBox1.Text, comboBox2.SelectedItem.ToString(), comboBox3.SelectedItem.ToString(), dateTimePicker1, textBox7.Text, label8.Text, label12.Text, ((int)numericUpDown1.Value));
            else
            {
                MessageBox.Show("nie wybrano elementu");
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
                usunksiazke.Invoke(listBox1.SelectedIndex);
            else
            {
                MessageBox.Show("nie wybrano elementu");
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
    }
}