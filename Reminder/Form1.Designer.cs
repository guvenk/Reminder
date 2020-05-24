namespace Reminder
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
            System.ComponentModel.ComponentResourceManager resources = GetFormResources();
            this.SuspendLayout();
            SetupContentText();
            SetupTimer();
            SetupForm(resources);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.ComponentModel.ComponentResourceManager GetFormResources()
        {
            this.components = new System.ComponentModel.Container();
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.txt = new System.Windows.Forms.TextBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
            return resources;
        }

        private void SetupForm(System.ComponentModel.ComponentResourceManager resources)
        {
            // 
            // Form1
            // 
            FormStep1(resources);
            FormStep2();
        }
        private void FormStep1(System.ComponentModel.ComponentResourceManager resources)
        {
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(232, 81);
            this.Controls.Add(this.txt);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        }

        private void FormStep2()
        {
            this.Name = "Form1";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reminder";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Load += new System.EventHandler(this.Form1_Load);
        }


        private void SetupTimer()
        {
            // 
            // timer
            // 
            this.timer.Interval = 120000;
            this.timer.Tick += new System.EventHandler(this.Timer_Tick);
        }

        private void SetupContentText()
        {
            // 
            // txt
            // 
            this.txt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt.Location = new System.Drawing.Point(12, 12);
            this.txt.Multiline = true;
            this.txt.Name = "txt";
            this.txt.Size = new System.Drawing.Size(208, 57);
            this.txt.TabIndex = 0;
            this.txt.Text = "Reminder is working...";
        }

        #endregion

        private System.Windows.Forms.TextBox txt;
        private System.Windows.Forms.Timer timer;
    }
}
