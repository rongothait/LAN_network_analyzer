using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace Admin_2._0_milestone_6
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btn_connect_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 f1 = new Form1(txt_ip.Text, Int32.Parse(txt_port.Text));
            f1.Show();
        }

        private void txt_TextChanged(object sender, EventArgs e)
        {
            if (txt_ip.Text.Length > 0 && txt_port.Text.Length > 0)
                if (ValidateIPv4(txt_ip.Text) && Regex.IsMatch(txt_port.Text, @"^\d+$"))
                    btn_connect.Enabled = true;
                else
                    btn_connect.Enabled = false;
            else
                btn_connect.Enabled = false;
        }


        public bool ValidateIPv4(string ipString)
        {
            if (String.IsNullOrWhiteSpace(ipString))
            {
                return false;
            }

            string[] splitValues = ipString.Split('.');
            if (splitValues.Length != 4)
            {
                return false;
            }

            byte tempForParsing;

            return splitValues.All(r => byte.TryParse(r, out tempForParsing));
        }
    }
}
