using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Admin_2._0_milestone_6
{
    public class ClientSock
    {
        private System.Net.Sockets.TcpClient clientSocket;
        private NetworkStream serverStream;
        private string server_ip;
        private int server_port;

        public ClientSock()
        {
            this.clientSocket = new System.Net.Sockets.TcpClient();
        }

        public void Close()
        {
            this.clientSocket.Close();
        }

        public void Connect(string ip, int port)
        {
            this.server_ip = ip;
            this.server_port = port;
            this.clientSocket.Connect(this.server_ip, this.server_port);
            this.serverStream = this.clientSocket.GetStream();
        }
        public void Send(string message)
        {
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(message);
            this.serverStream.Write(outStream, 0, outStream.Length);
            this.serverStream.Flush();
        }

        public string Receive()
        {

            byte[] inStream = new byte[(int)this.clientSocket.ReceiveBufferSize];
            serverStream.Read(inStream, 0, (int)this.clientSocket.ReceiveBufferSize);
            this.serverStream.Flush();
            //Console.WriteLine(System.Text.Encoding.ASCII.GetString(inStream));
            return System.Text.Encoding.ASCII.GetString(inStream);

        }
    }
}

