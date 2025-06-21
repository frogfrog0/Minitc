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
    public partial class Adminpanelogloszenia : UserControl
    {
        public event Action<string, string> dodajogl;
        public event Action<string> ustawogl;
        public event Action paneluz;
        public event Action panelks;
        public event Action<string, string> edytujogl;
        public event Action<string> usunogl;
        public event Action powrotglowna;
        public event Action<AnnouncementInfo> ustawogl;
        public event Action<AnnouncementInfo> usunogl;
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
            if (listBox1.SelectedItem is AnnouncementInfo selectedAnnouncement)
            {
                ustawogl?.Invoke(selectedAnnouncement);
            }
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
            var selectedAnnouncement = GetSelectedAnnouncement();
            if (selectedAnnouncement != null)
            {
                if (button4.Text == "Anuluj")
                {
                    tytul = "";
                    opis = "";
                    SetEditMode(false);
                }
                else
                {
                    edytujogl?.Invoke(textBox1.Text, textBox2.Text);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var selectedAnnouncement = GetSelectedAnnouncement();
            if (selectedAnnouncement != null)
            {
                usunogl?.Invoke(selectedAnnouncement);
            }
            else
            {
                MessageBox.Show("Wybierz ogłoszenie do usunięcia", "Uwaga", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            powrotglowna.Invoke();
        }
        public void LoadAnnouncementList(List<AnnouncementInfo> announcements)
        {
            listBox1.Items.Clear();
            foreach (var announcement in announcements)
            {
                listBox1.Items.Add(announcement);
            }
        }

        public void SetEditMode(bool isEditing)
        {
            if (isEditing)
            {
                button1.Text = "Aktualizuj";
                button4.Text = "Anuluj";
            }
            else
            {
                button1.Text = "Dodaj";
                button4.Text = "Edytuj";
            }
        }

        public AnnouncementInfo? GetSelectedAnnouncement()
        {
            return listBox1.SelectedItem as AnnouncementInfo;
        }
    }
}
