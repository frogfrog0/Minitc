namespace projekt_2_na_punkty
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            leftPanel = new PanelTC();
            rightPanel = new PanelTC();
            button1 = new Button();
            SuspendLayout();
            // 
            // leftPanel
            // 
            leftPanel.AvailableDrives = new string[0];
            leftPanel.BorderStyle = BorderStyle.FixedSingle;
            leftPanel.CurrentPath = "";
            leftPanel.DirectoryContents = new string[0];
            leftPanel.Location = new Point(-8, -4);
            leftPanel.Name = "leftPanel";
            leftPanel.Size = new Size(391, 464);
            leftPanel.TabIndex = 0;
            // 
            // rightPanel
            // 
            rightPanel.AvailableDrives = new string[0];
            rightPanel.BorderStyle = BorderStyle.FixedSingle;
            rightPanel.CurrentPath = "";
            rightPanel.DirectoryContents = new string[0];
            rightPanel.Location = new Point(405, -3);
            rightPanel.Name = "rightPanel";
            rightPanel.Size = new Size(391, 464);
            rightPanel.TabIndex = 5;
            // 
            // button1
            // 
            button1.Location = new Point(338, 407);
            button1.Name = "button1";
            button1.Size = new Size(94, 29);
            button1.TabIndex = 6;
            button1.Text = "Copy>>";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(button1);
            Controls.Add(rightPanel);
            Controls.Add(leftPanel);
            Name = "Form1";
            Text = "MiniTC";
            ResumeLayout(false);
        }

        #endregion

        private PanelTC leftPanel;
        private PanelTC rightPanel;
        private Button button1;
    }
}