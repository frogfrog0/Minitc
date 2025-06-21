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
    public partial class Logowanie : UserControl
    {
        public event Action<string, string> logowanie;
        public event Action powrotglowna;
        public Logowanie()
        {
            InitializeComponent();
        }
        public void button1_Click(object sender, EventArgs e)
        {
            logowanie.Invoke(textBox1.Text, textBox2.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            powrotglowna.Invoke();
        }
    }
}
