using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibraryApp.View
{
    public partial class Adminpanelogloszenia : UserControl
    {
        public event Action<string, string> dodajogl;
        public event Action<string> ustawogl;
        public event Action paneluz;
        public event Action panelks;
        public event Action<string, string> edytujogl;
        public event Action<string> usunogl;
        public event Action powrotglowna;
        public Adminpanelogloszenia()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            dodajogl.Invoke(textBox1.Text, textBox2.Text);
        }
        public string dodawanieogl
        {
            set
            {
                listBox1.Items.Add(value);
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
        public string opis
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
        public string zmienogl
        {
            set
            {
                listBox1.Items[listBox1.SelectedIndex] = value;
            }
            get
            {
                return listBox1.SelectedItem.ToString();
            }
        }
        public string usunogloszenie
        {
            set
            {
                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    if (listBox1.Items[i] == value)
                    {
                        listBox1.Items.RemoveAt(i);
                    }
                }
            }
        }
        public void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
                ustawogl.Invoke(listBox1.SelectedItem.ToString());
        }
        public void button2_Click(object sender, EventArgs e)
        {
            panelks.Invoke();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            paneluz.Invoke();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            edytujogl.Invoke(textBox1.Text, textBox2.Text);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
                usunogl.Invoke(listBox1.SelectedItem.ToString());
        }

        private void button6_Click(object sender, EventArgs e)
        {
            powrotglowna.Invoke();
        }
    }
}
