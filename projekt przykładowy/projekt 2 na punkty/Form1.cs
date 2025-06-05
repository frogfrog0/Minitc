using System.Diagnostics;
namespace projekt_2_na_punkty
{
    public partial class Form1 : Form
    {
        private PanelTC activePanel;

        // Events for MVP pattern (keeping friend's pattern)
        public event Action GetDisks;
        public event Action Copybutton;
        public event Action<string> Setdisk;
        public event Action<string> Setpath;
        public event Action<string> Setdisk2;
        public event Action<string> Setpath2;

        // Simplified properties that work with both panels
        public string[] AvailableDrives
        {
            set
            {
                leftPanel.DriveItems = value;
                rightPanel.DriveItems = value;
            }
        }

        public string[] LeftPanelContent
        {
            set => leftPanel.TreeItems = value;
        }

        public string[] RightPanelContent
        {
            set => rightPanel.TreeItems = value;
        }

        public string LeftPanelPath
        {
            set => leftPanel.PathDisplay = value;
            get => leftPanel.PathDisplay;
        }

        public string RightPanelPath
        {
            set => rightPanel.PathDisplay = value;
            get => rightPanel.PathDisplay;
        }

        public Form1()
        {
            InitializeComponent();
            SetupPanels();
            GetDisks?.Invoke();
        }

        private void SetupPanels()
        {
            // Subscribe to left panel events
            leftPanel.DriveChanged += (drive) => {
                SetActivePanel(leftPanel);
                Setdisk?.Invoke(drive);
            };
            leftPanel.PathChanged += (path) => {
                SetActivePanel(leftPanel);
                Setpath?.Invoke(path);
            };
            leftPanel.PanelActivated += () => SetActivePanel(leftPanel);

            // Subscribe to right panel events  
            rightPanel.DriveChanged += (drive) => {
                SetActivePanel(rightPanel);
                Setdisk2?.Invoke(drive);
            };
            rightPanel.PathChanged += (path) => {
                SetActivePanel(rightPanel);
                Setpath2?.Invoke(path);
            };
            rightPanel.PanelActivated += () => SetActivePanel(rightPanel);

            // Set default active panel
            SetActivePanel(leftPanel);
        }

        private void SetActivePanel(PanelTC panel)
        {
            activePanel = panel;
        }

        public string GetSelectedFile()
        {
            return activePanel?.GetSelectedFile();
        }

        public PanelTC GetInactivePanel()
        {
            return activePanel == leftPanel ? rightPanel : leftPanel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Copybutton?.Invoke();
        }
    }
}