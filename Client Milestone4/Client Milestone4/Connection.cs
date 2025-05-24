using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_Milestone4
{
    class Connection
    {
        public string Protocol;
        public string Local_address_ip;
        public string Local_address_port;
        public string Foreign_address_ip;
        public string Foreign_address_port;
        public string state;
        public string PID;
        public string image_name;


        public Connection(string Protocol, string Local_address_ip, string Local_address_port, string Foreign_address_ip, string Foreign_address_port, string state, string PID, string image_name)
        {
            this.Protocol = Protocol;
            this.Local_address_ip = Local_address_ip;
            this.Local_address_port = Local_address_port;
            this.Foreign_address_ip = Foreign_address_ip;
            this.Foreign_address_port = Foreign_address_port;
            this.state = state;
            this.PID = PID;
            this.image_name = image_name;
        }

        public override string ToString()
        {
            return "Protocol: " + this.Protocol + " local address ip: " + this.Local_address_ip +
                " local address port: " + this.Local_address_port + " foreign address ip: " +
                this.Foreign_address_ip + " foreign address port: " + this.Foreign_address_port +
                " PID: " + this.PID + " Image name: " + this.image_name;
        }
    }
}
