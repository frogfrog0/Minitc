using System.Diagnostics;
namespace projekt_2_na_punkty
{
    public partial class Form1 : Form
    {

        public event Action GetDisks;
        public event Action Copybutton;
        public event Action<string> Setdisk;
        public event Action<string> Setpath;
        public event Action<string> Setdisk2;
        public event Action<string> Setpath2;
        public string[] placeholder
        {
            set
            {
                comboBox1.BeginUpdate();
                for (int i = 0; i < value.Length; i++)
                    comboBox1.Items.Add(value[i]);
                comboBox1.EndUpdate();
            }
            get
            {
                return comboBox1.Items.Cast<string>().ToArray();
            }
        }
        public string[] tree
        {
            set
            {
                treeView1.BeginUpdate();
                treeView1.Nodes.Clear();
                for (int i = 0; i < value.Length; i++)
                    treeView1.Nodes.Add(value[i]);
                treeView1.EndUpdate();
            }
            get
            {
                return treeView1.Nodes.Cast<string>().ToArray();
            }
        }
        public string path
        {
            set
            {
                label5.Text = value;
            }
            get
            {
                return label5.Text;
            }
        }
        public string[] treev2
        {
            set
            {
                treeView2.BeginUpdate();
                treeView2.Nodes.Clear();
                for (int i = 0; i < value.Length; i++)
                    treeView2.Nodes.Add(value[i]);
                treeView2.EndUpdate();
            }
            get
            {
                return treeView2.Nodes.Cast<string>().ToArray();
            }
        }
        public string pathv2
        {
            set
            {
                label6.Text = value;
            }
            get
            {
                return label6.Text;
            }
        }
        public string[] placeholderv2
        {
            set
            {
                comboBox2.BeginUpdate();
                for (int i = 0; i < value.Length; i++)
                    comboBox2.Items.Add(value[i]);
                comboBox2.EndUpdate();
            }
            get
            {
                return comboBox2.Items.Cast<string>().ToArray();
            }
        }
        public Form1()
        {
            InitializeComponent();
            GetDisks?.Invoke();
        }

        protected void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Setdisk?.Invoke(comboBox1.SelectedItem.ToString());
        }
        protected void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Setdisk2?.Invoke(comboBox2.SelectedItem.ToString());
        }
        protected void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Setpath?.Invoke(treeView1.SelectedNode.Text);
        }
        protected void treeView2_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Setpath2?.Invoke(treeView2.SelectedNode.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Copybutton?.Invoke();
        }
    }
}
