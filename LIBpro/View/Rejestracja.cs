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
    public partial class Rejestracja : UserControl
    {
        public event Action<string, string, string, string, string, string, DateTimePicker, string, string, string, string, string, string, string> rejestracja;
        public event Action powrot;
        public Rejestracja()
        {
            InitializeComponent();
        }
        public void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != null && textBox2.Text != null && textBox3.Text != null && textBox4.Text != null && textBox5.Text != null && textBox6.Text != null && textBox7.Text != null && textBox8.Text != null && textBox9.Text != null && textBox10.Text != null && textBox12.Text != null && textBox13.Text != null)
            {
                rejestracja.Invoke(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text, dateTimePicker1, textBox7.Text, textBox8.Text, textBox9.Text, textBox10.Text, textBox11.Text, textBox12.Text, textBox13.Text);
            }
            else
            {
                MessageBox.Show("nie podano wszyskich informacji (tylko numer mieszkania nie jest wymagany)");
            }
        }
        private void textBox12_keypress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }
        private void textBox13_keypress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            powrot.Invoke();
        }
    }
}
