namespace Admin_2._0_milestone_6
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.LstView = new System.Windows.Forms.ListView();
            this.ImgLst = new System.Windows.Forms.ImageList(this.components);
            this.Timer = new System.Windows.Forms.Timer(this.components);
            this.lstbox_log = new System.Windows.Forms.ListBox();
            this.lbl_log = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LstView
            // 
            this.LstView.Dock = System.Windows.Forms.DockStyle.Left;
            this.LstView.LargeImageList = this.ImgLst;
            this.LstView.Location = new System.Drawing.Point(0, 0);
            this.LstView.Name = "LstView";
            this.LstView.Size = new System.Drawing.Size(489, 504);
            this.LstView.TabIndex = 0;
            this.LstView.UseCompatibleStateImageBehavior = false;
            this.LstView.SelectedIndexChanged += new System.EventHandler(this.LstView_SelectedIndexChanged);
            // 
            // ImgLst
            // 
            this.ImgLst.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImgLst.ImageStream")));
            this.ImgLst.TransparentColor = System.Drawing.Color.Transparent;
            this.ImgLst.Images.SetKeyName(0, "copmuter icon new.jpg");
            this.ImgLst.Images.SetKeyName(1, "comp2 picture.png");
            this.ImgLst.Images.SetKeyName(2, "computer_x_icon.png");
            // 
            // Timer
            // 
            this.Timer.Enabled = true;
            this.Timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // lstbox_log
            // 
            this.lstbox_log.Font = new System.Drawing.Font("Yu Gothic UI Semibold", 9F);
            this.lstbox_log.FormattingEnabled = true;
            this.lstbox_log.ItemHeight = 15;
            this.lstbox_log.Location = new System.Drawing.Point(512, 46);
            this.lstbox_log.Name = "lstbox_log";
            this.lstbox_log.ScrollAlwaysVisible = true;
            this.lstbox_log.Size = new System.Drawing.Size(240, 439);
            this.lstbox_log.TabIndex = 1;
            // 
            // lbl_log
            // 
            this.lbl_log.AutoSize = true;
            this.lbl_log.Font = new System.Drawing.Font("Yu Gothic UI Semibold", 13F);
            this.lbl_log.Location = new System.Drawing.Point(567, 13);
            this.lbl_log.Name = "lbl_log";
            this.lbl_log.Size = new System.Drawing.Size(143, 25);
            this.lbl_log.TabIndex = 2;
            this.lbl_log.Text = "Notificaton Log";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(764, 504);
            this.Controls.Add(this.lbl_log);
            this.Controls.Add(this.lstbox_log);
            this.Controls.Add(this.LstView);
            this.Name = "Form1";
            this.Text = "Network View";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ListView LstView;
        public System.Windows.Forms.Timer Timer;
        private System.Windows.Forms.ImageList ImgLst;
        public System.Windows.Forms.ListBox lstbox_log;
        private System.Windows.Forms.Label lbl_log;
    }
}

