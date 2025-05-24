namespace Admin_2._0_milestone_6
{
    partial class LoginForm
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
            this.btn_connect = new System.Windows.Forms.Button();
            this.lbl_ip = new System.Windows.Forms.Label();
            this.txt_ip = new System.Windows.Forms.TextBox();
            this.txt_port = new System.Windows.Forms.TextBox();
            this.lbl_title = new System.Windows.Forms.Label();
            this.lbl_port = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_connect
            // 
            this.btn_connect.Enabled = false;
            this.btn_connect.Location = new System.Drawing.Point(84, 206);
            this.btn_connect.Name = "btn_connect";
            this.btn_connect.Size = new System.Drawing.Size(114, 23);
            this.btn_connect.TabIndex = 0;
            this.btn_connect.Text = "Connect To Server";
            this.btn_connect.UseVisualStyleBackColor = true;
            this.btn_connect.Click += new System.EventHandler(this.btn_connect_Click);
            // 
            // lbl_ip
            // 
            this.lbl_ip.AutoSize = true;
            this.lbl_ip.Font = new System.Drawing.Font("Yu Gothic UI Semibold", 10F);
            this.lbl_ip.Location = new System.Drawing.Point(28, 74);
            this.lbl_ip.Name = "lbl_ip";
            this.lbl_ip.Size = new System.Drawing.Size(77, 19);
            this.lbl_ip.TabIndex = 1;
            this.lbl_ip.Text = "Server\'s IP:";
            // 
            // txt_ip
            // 
            this.txt_ip.Location = new System.Drawing.Point(125, 73);
            this.txt_ip.Name = "txt_ip";
            this.txt_ip.Size = new System.Drawing.Size(116, 20);
            this.txt_ip.TabIndex = 2;
            this.txt_ip.TextChanged += new System.EventHandler(this.txt_TextChanged);
            // 
            // txt_port
            // 
            this.txt_port.Location = new System.Drawing.Point(125, 136);
            this.txt_port.Name = "txt_port";
            this.txt_port.Size = new System.Drawing.Size(116, 20);
            this.txt_port.TabIndex = 4;
            this.txt_port.TextChanged += new System.EventHandler(this.txt_TextChanged);
            // 
            // lbl_title
            // 
            this.lbl_title.AutoSize = true;
            this.lbl_title.Font = new System.Drawing.Font("Yu Gothic UI Semibold", 15F, ((System.Drawing.FontStyle)(((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic) 
                | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_title.Location = new System.Drawing.Point(65, 18);
            this.lbl_title.Name = "lbl_title";
            this.lbl_title.Size = new System.Drawing.Size(150, 28);
            this.lbl_title.TabIndex = 6;
            this.lbl_title.Text = "Server Connect";
            // 
            // lbl_port
            // 
            this.lbl_port.AutoSize = true;
            this.lbl_port.Font = new System.Drawing.Font("Yu Gothic UI Semibold", 10F);
            this.lbl_port.Location = new System.Drawing.Point(28, 136);
            this.lbl_port.Name = "lbl_port";
            this.lbl_port.Size = new System.Drawing.Size(91, 19);
            this.lbl_port.TabIndex = 7;
            this.lbl_port.Text = "Server\'s Port:";
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.lbl_port);
            this.Controls.Add(this.lbl_title);
            this.Controls.Add(this.txt_port);
            this.Controls.Add(this.txt_ip);
            this.Controls.Add(this.lbl_ip);
            this.Controls.Add(this.btn_connect);
            this.Name = "LoginForm";
            this.Text = "Connect To Server";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_connect;
        private System.Windows.Forms.Label lbl_ip;
        private System.Windows.Forms.TextBox txt_ip;
        private System.Windows.Forms.TextBox txt_port;
        private System.Windows.Forms.Label lbl_title;
        private System.Windows.Forms.Label lbl_port;
    }
}