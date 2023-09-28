namespace RaceFantasyUnInstaller
{
    partial class MainWindow
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Label label2;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.label1 = new System.Windows.Forms.Label();
            this.titlebar = new System.Windows.Forms.Panel();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.displayPanel = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            label2 = new System.Windows.Forms.Label();
            this.titlebar.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.CausesValidation = false;
            label2.Enabled = false;
            label2.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            label2.Location = new System.Drawing.Point(280, 3);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(208, 17);
            label2.TabIndex = 2;
            label2.Text = "Race Fantasy Installation Wizard";
            label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 470);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(229, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Wizards built with <3 by ChocolateAdventurouz";
            // 
            // titlebar
            // 
            this.titlebar.BackColor = System.Drawing.Color.Transparent;
            this.titlebar.Controls.Add(label2);
            this.titlebar.Controls.Add(this.button5);
            this.titlebar.Controls.Add(this.button4);
            this.titlebar.Location = new System.Drawing.Point(-1, 1);
            this.titlebar.Name = "titlebar";
            this.titlebar.Size = new System.Drawing.Size(818, 21);
            this.titlebar.TabIndex = 10;
            // 
            // button5
            // 
            this.button5.FlatAppearance.BorderSize = 0;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.ForeColor = System.Drawing.Color.Transparent;
            this.button5.Image = ((System.Drawing.Image)(resources.GetObject("button5.Image")));
            this.button5.Location = new System.Drawing.Point(749, 0);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(30, 23);
            this.button5.TabIndex = 1;
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.FlatAppearance.BorderSize = 0;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.ForeColor = System.Drawing.Color.Transparent;
            this.button4.Image = ((System.Drawing.Image)(resources.GetObject("button4.Image")));
            this.button4.Location = new System.Drawing.Point(785, 0);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(30, 23);
            this.button4.TabIndex = 0;
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // displayPanel
            // 
            this.displayPanel.BackColor = System.Drawing.Color.Transparent;
            this.displayPanel.Location = new System.Drawing.Point(0, 24);
            this.displayPanel.Name = "displayPanel";
            this.displayPanel.Size = new System.Drawing.Size(818, 436);
            this.displayPanel.TabIndex = 7;
            this.displayPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.displayPanel_Paint);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(0, 0);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 12;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(722, 465);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(641, 465);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 8;
            this.button2.Text = "Yes";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(816, 489);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.titlebar);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.displayPanel);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.Text = "Race Fantasy Uninstall Wizard";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainWindow_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.titlebar.ResumeLayout(false);
            this.titlebar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel titlebar;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        public System.Windows.Forms.Button button3;
        public System.Windows.Forms.Button button1;
        public System.Windows.Forms.Button button2;
        public System.Windows.Forms.Panel displayPanel;
    }
}

