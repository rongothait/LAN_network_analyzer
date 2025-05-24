using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Admin_2._0_milestone_6
{
    public partial class Form2 : Form
    {
        private string comp_ip;
        private ClientSock clisock;
        private string computer_info = string.Empty;
        private Form1 f1 = (Form1)Application.OpenForms["Form1"];
        


        public Form2(string ip, ClientSock clisock)
        {
            InitializeComponent();
            this.timer.Enabled = true;
            this.comp_ip = ip;
            this.clisock = clisock;
   
        }

        private void show_notification(string noti_ip, string noti_app)
        {
            MessageBox.Show(string.Format("User {0} entered {1}", noti_ip, noti_app), "Illegal Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void show_msg_disconnected()
        {
            MessageBox.Show("The Client you are now watching is no longer available", "Client Unavailable", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }


        private void timer_Tick(object sender, EventArgs e)
        {
            //options:
                //computer disconnected - the computer that now is watched is not availble --> close form2, show notification, and show in log
                //notification - an illegal app is enterd --> show notification and update log
                //network status and info - expected information --> update LstView in Form1, and update Form2

            clisock.Send(string.Format("info and network status-{0}", comp_ip));
            string tmp = clisock.Receive();

            if (tmp.Contains("computer disconnected"))
            {
                this.timer.Enabled = false;
                this.DialogResult = DialogResult.Cancel;
                Thread t = new Thread(this.show_msg_disconnected);
                t.Start();
            }

            else
            {
                foreach (string msg in tmp.Split('*'))
                {
                    string new_msg = msg.Trim(); ////removes all white spaces from beggining or end of msg

                    if (new_msg.Contains("notification"))
                    {
                        this.f1.msg_notification_recieved(new_msg);
                    }

                    if (new_msg.Contains("info and network status"))
                    {
                        msg_info_network_status_recieved(new_msg);
                    }
                }
            }         
        }
            
        

        private void msg_info_network_status_recieved(string msg)
        {
            this.f1.msg_network_status_recieved(msg.Split('&')[0]);

            string tmp_info = msg.Split('&')[1];
            if (tmp_info != computer_info)
            {
                computer_info = tmp_info;
                string[] information = computer_info.Split('@');
                lbl_cmp_ip.Text = "IP address: " + information[0];
                lbl_mac.Text = "MAC address: " + information[1];
                lbl_OS.Text = "Operating System: " + information[2];
                lbl_illegal_apps.Text = "Illegal Applications: " + information[3];
                lbl_domains.Text = "Current Chrome Domain: " + information[4];
                lbl_http.Text = "HTTP: " + information[5];
                lbl_https.Text = "HTTPS: " + information[6];
                lbl_telnet.Text = "TELNET: " + information[7];
                llb_illegal_domain1.Text = "Illegal Domains: " + information[8];
                lbl_has_client.Text = "Client software Active: " + information[9];

                if (information[9] == "False")
                    picturebox_computer_title.Image = this.imageList1.Images[0];
                
                else
                    picturebox_computer_title.Image = this.imageList1.Images[1];

            }
        }

        private void btn_add_illegal_Click(object sender, EventArgs e)
        {
            clisock.Send(string.Format("illegal list update-{0}-{1}", this.comp_ip, this.txtbox_add_illegal.Text));
            this.txtbox_add_illegal.Text = string.Empty;
        }

        private void txtbox_add_illegal_TextChanged(object sender, EventArgs e)
        {
            if(this.txtbox_add_illegal.Text.Length > 0)
            {
                this.btn_add_illegal.Enabled = true;
            }

            else
            {
                this.btn_add_illegal.Enabled = false;
            }
        }

        private void btn_illegal_domain_Click(object sender, EventArgs e)
        {
            clisock.Send(string.Format("illegal domain update-{0}-{1}", this.comp_ip, this.txtbox_illegal_domain.Text));
            this.txtbox_illegal_domain.Text = string.Empty;
        }

        private void txtbox_illegal_domain_TextChanged(object sender, EventArgs e)
        {
            if (this.txtbox_illegal_domain.Text.Length > 0)
                this.btn_illegal_domain.Enabled = true;
            else
                this.btn_illegal_domain.Enabled = false;
        }
    }
}
