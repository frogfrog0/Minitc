namespace LibraryApp.View
{
    partial class Adminpaneluzytkownicy
    {
        /// <summary> 
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod wygenerowany przez Projektanta składników

        /// <summary> 
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować 
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            listBox1 = new ListBox();
            Ban = new CheckBox();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 27F);
            label1.Location = new Point(273, 31);
            label1.Name = "label1";
            label1.Size = new Size(563, 61);
            label1.TabIndex = 0;
            label1.Text = "Admin panel - użytkownicy";
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.Location = new Point(30, 195);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(480, 304);
            listBox1.TabIndex = 1;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // Ban
            // 
            Ban.AutoSize = true;
            Ban.Location = new Point(619, 195);
            Ban.Name = "Ban";
            Ban.Size = new Size(56, 24);
            Ban.TabIndex = 2;
            Ban.Text = "ban";
            Ban.UseVisualStyleBackColor = true;
            Ban.CheckedChanged += Ban_CheckedChanged;
            // 
            // button1
            // 
            button1.Location = new Point(30, 133);
            button1.Name = "button1";
            button1.Size = new Size(94, 29);
            button1.TabIndex = 3;
            button1.Text = "książki";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(156, 133);
            button2.Name = "button2";
            button2.Size = new Size(94, 29);
            button2.TabIndex = 4;
            button2.Text = "ogłoszenia";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(870, 82);
            button3.Name = "button3";
            button3.Size = new Size(193, 29);
            button3.TabIndex = 5;
            button3.Text = "Powrót do głównej strony";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // Adminpaneluzytkownicy
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(Ban);
            Controls.Add(listBox1);
            Controls.Add(label1);
            Name = "Adminpaneluzytkownicy";
            Size = new Size(1080, 543);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private ListBox listBox1;
        private CheckBox Ban;
        private Button button1;
        private Button button2;
        private Button button3;
    }
}
