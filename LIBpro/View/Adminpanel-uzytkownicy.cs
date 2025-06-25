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
    public partial class Adminpaneluzytkownicy : UserControl
    {
        public event Action paneluz;
        public event Action<string> ustawuzytkownika;
        public event Action<string> dajban;
        public event Action panelogl;
        public event Action powrotglowna;
        public event Action wylogowanie;
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
            var selectedUser = GetSelectedUser();
            if (selectedUser != null)
            {
                dajban?.Invoke(selectedUser.ToString());
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem is UserInfo selectedUser)
            {
                SetSelectedUser(selectedUser);
                ustawuzytkownika?.Invoke(selectedUser.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panelogl.Invoke();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            powrotglowna.Invoke();
        }
        public void LoadUserList(List<UserInfo> users)
        {
            listBox1.Items.Clear();
            foreach (var user in users)
            {
                listBox1.Items.Add(user);
            }
        }

        public void SetSelectedUser(UserInfo user)
        {
            // Update ban checkbox to reflect user's current status
            Ban.Checked = user.IsBanned;
            Ban.Text = $"ban - {user.FullName}";
            Ban.Tag = user; // Store user info for later use
        }

        public UserInfo? GetSelectedUser()
        {
            return Ban.Tag as UserInfo;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            wylogowanie.Invoke();
        }
    }
}
