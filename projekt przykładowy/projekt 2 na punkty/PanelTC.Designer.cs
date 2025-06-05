namespace projekt_2_na_punkty
{
    partial class PanelTC
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pathTextLabel = new Label();
            pathLabel = new Label();
            driveTextLabel = new Label();
            comboBox = new ComboBox();
            treeView = new TreeView();
            SuspendLayout();
            // 
            // pathTextLabel
            // 
            pathTextLabel.AutoSize = true;
            pathTextLabel.Location = new Point(20, 11);
            pathTextLabel.Name = "pathTextLabel";
            pathTextLabel.Size = new Size(40, 20);
            pathTextLabel.TabIndex = 2;
            pathTextLabel.Text = "Path:";
            // 
            // pathLabel
            // 
            pathLabel.Location = new Point(66, 11);
            pathLabel.Name = "pathLabel";
            pathLabel.Size = new Size(305, 27);
            pathLabel.TabIndex = 6;
            // 
            // driveTextLabel
            // 
            driveTextLabel.AutoSize = true;
            driveTextLabel.Location = new Point(218, 49);
            driveTextLabel.Name = "driveTextLabel";
            driveTextLabel.Size = new Size(47, 20);
            driveTextLabel.TabIndex = 3;
            driveTextLabel.Text = "Drive:";
            // 
            // comboBox
            // 
            comboBox.FormattingEnabled = true;
            comboBox.Location = new Point(271, 46);
            comboBox.Name = "comboBox";
            comboBox.Size = new Size(117, 28);
            comboBox.TabIndex = 5;
            comboBox.SelectedIndexChanged += comboBox_SelectedIndexChanged;
            // 
            // treeView
            // 
            treeView.Location = new Point(0, 90);
            treeView.Name = "treeView";
            treeView.Size = new Size(320, 299);
            treeView.TabIndex = 0;
            treeView.AfterSelect += treeView_AfterSelect;
            treeView.Enter += treeView_Enter;
            // 
            // PanelTC
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(pathLabel);
            Controls.Add(comboBox);
            Controls.Add(driveTextLabel);
            Controls.Add(pathTextLabel);
            Controls.Add(treeView);
            Name = "PanelTC";
            Size = new Size(391, 389);
            Enter += PanelTC_Enter;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TreeView treeView;
        private Label driveTextLabel;
        private Label pathTextLabel;
        private ComboBox comboBox;
        private Label pathLabel;
    }
}