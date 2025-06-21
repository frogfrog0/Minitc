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
    public partial class Adminpaneluzytkownicy : UserControl
    {
        public event Action paneluz;
        public event Action<string> ustawuzytkownika;
        public event Action<string> dajban;
        public event Action panelogl;
        public event Action powrotglowna;
        public Adminpaneluzytkownicy()
        {
            InitializeComponent();
        }
        public string uzytkownicy
        {
            get
            {
                return listBox1.Text;
            }
            set
            {
                listBox1.SelectedItem = value;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            paneluz.Invoke();
        }

        private void Ban_CheckedChanged(object sender, EventArgs e)
        {
            dajban.Invoke(listBox1.SelectedItem.ToString());
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ustawuzytkownika.Invoke(listBox1.SelectedItem.ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panelogl.Invoke();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            powrotglowna.Invoke();
        }
    }
}
