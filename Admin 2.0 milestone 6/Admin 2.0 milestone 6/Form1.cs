using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;


namespace Admin_2._0_milestone_6
{
    public partial class Form1 : Form
    {
        private string network_status = ""; //the network status message the server is sending
        private ClientSock clisock = new ClientSock();
        private bool connected = false;

        private string server_ip = string.Empty; //hardcoded for now
        private int server_port = 0;   //hardcoded for now

        public Form1(string IP, int port)
        {
            InitializeComponent();
            this.server_ip = IP;
            this.server_port = port;
            Connect();

        }


        public void Connect()
        {
            try
            {
                clisock.Connect(server_ip, server_port);
                Console.WriteLine("Connected to server...");
                clisock.Send("admin"); //send to admin type of client - admin
                connected = true;
            }
            catch
            {
                Console.WriteLine("Unable to connect to server...");
            }
        }


        private void show_notification(string noti_ip, string noti_app)
        {
            MessageBox.Show(string.Format("user {0} entered {1}", noti_ip, noti_app), "illegal notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }


        private void Timer_Tick(object sender, EventArgs e)
        {
            //options:
                //notification: if an illegal notification is shown --> show message and update log
                //network status: network status is sent from server --> update LstView

            if (connected)
            {
                clisock.Send("network status");
                string tmp = clisock.Receive();

                foreach (string msg in tmp.Split('*'))
                {
                    string new_msg = msg.Trim(); //removes all white spaces from beggining or end of msg
                    if (new_msg.Contains("notification"))
                        msg_notification_recieved(new_msg);

                    if (new_msg.Contains("network status"))
                        msg_network_status_recieved(new_msg);
                }
            }
        }

        public void msg_notification_recieved(string msg)
        {
            string noti_ip = msg.Split('-')[1];
            string noti_app = msg.Split('-')[2];
            this.lstbox_log.Items.Add(string.Format("User {0} enterd {1}", noti_ip, noti_app));
            Thread t = new Thread(() => show_notification(noti_ip, noti_app));
            t.Start();
        }

        public void msg_network_status_recieved(string msg)
        {
            msg = msg.Split('-')[1];
            if (msg != network_status) // if the information has changed since the last netowrk status
            {
                network_status = msg;
                LstView.Items.Clear();
                foreach (string ip in network_status.Split('@'))
                {
                    if (ip.Contains('+'))  // if the computer has client!!
                    {
                        string correct_ip = ip.Split('+')[0];
                        LstView.Items.Add(correct_ip, 1);
                    }

                    else if (ip.Contains("()")) // if the computer is disconnected!
                    {
                        string disconnected_ip = ip.Split('(')[0];
                        LstView.Items.Add(disconnected_ip, 2);
                    }

                    else // if the computer doesnt have a client installed
                        LstView.Items.Add(ip, 0);
                }
            }
    }

        private void LstView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LstView.SelectedIndices.Count <= 0)
            {
                return;
            }

            int index = LstView.SelectedIndices[0];
            if (index >= 0)
            {
                string selected_ip = LstView.Items[index].Text;
                this.Timer.Enabled = false;
                //this.Hide();
                Form2 f2 = new Form2(selected_ip, clisock);
                f2.ShowDialog();
                if (f2.DialogResult == DialogResult.Cancel) //client clicked "back" button or the client that was watched is now disconnected
                {
                    f2.timer.Enabled = false;
                    this.Timer.Enabled = true;
                    //this.Location = f2.Location;
                    this.Show();
                }
                    
            }
        }

        

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //clisock.Close();
            Application.Exit();
        }
    }
}
