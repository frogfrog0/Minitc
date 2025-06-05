using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace projekt_2_na_punkty
{
    public partial class PanelTC : UserControl
    {
        // Required public properties per assignment
        public string CurrentPath { get;  set; } = "";
        public string[] AvailableDrives { get;  set; } = new string[0];
        public string[] DirectoryContents { get;  set; } = new string[0];

        // Events for MVP communication (matching friend's pattern)
        public event Action<string> DriveChanged;
        public event Action<string> PathChanged;
        public event Action<string> ItemSelected;
        public event Action PanelActivated;

        public PanelTC()
        {
            InitializeComponent();
        }

        // Property for drive selection (matching friend's placeholder property)
        public string[] DriveItems
        {
            set
            {
                comboBox.BeginUpdate();
                comboBox.Items.Clear();
                for (int i = 0; i < value.Length; i++)
                    comboBox.Items.Add(value[i]);
                comboBox.EndUpdate();
                AvailableDrives = value;
            }
            get
            {
                return comboBox.Items.Cast<string>().ToArray();
            }
        }

        // Property for tree content (matching friend's tree property)
        public string[] TreeItems
        {
            set
            {
                treeView.BeginUpdate();
                treeView.Nodes.Clear();
                for (int i = 0; i < value.Length; i++)
                    treeView.Nodes.Add(value[i]);
                treeView.EndUpdate();
                DirectoryContents = value;
            }
            get
            {
                return treeView.Nodes.Cast<string>().ToArray();
            }
        }

        // Property for path display (matching friend's path property)
        public string PathDisplay
        {
            set
            {
                pathLabel.Text = value;
                CurrentPath = value;
            }
            get
            {
                return pathLabel.Text;
            }
        }

        // Method to get selected file for copying (extracted from friend's logic)
        public string GetSelectedFile()
        {
            if (treeView.SelectedNode?.Text != null)
            {
                string selectedItem = treeView.SelectedNode.Text;

                // Return file name if it's a file (not directory, not "..")
                if (!selectedItem.StartsWith("<") && selectedItem != "..")
                {
                    return selectedItem;
                }
            }
            return null;
        }

        // Event handlers (extracted from friend's Form1)
        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DriveChanged?.Invoke(comboBox.SelectedItem?.ToString());
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            PathChanged?.Invoke(treeView.SelectedNode?.Text);
        }

        private void treeView_Enter(object sender, EventArgs e)
        {
            PanelActivated?.Invoke();
        }

        private void PanelTC_Enter(object sender, EventArgs e)
        {
            PanelActivated?.Invoke();
        }
    }
}