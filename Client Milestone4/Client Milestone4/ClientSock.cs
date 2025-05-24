using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace Client_Milestone4
{
    class ClientSock
    {
        private System.Net.Sockets.TcpClient clientSocket;
        private NetworkStream serverStream;


        public ClientSock(string ip, int port)
        {
            this.clientSocket = new System.Net.Sockets.TcpClient();
            this.clientSocket.Connect(ip, port);
            this.serverStream = this.clientSocket.GetStream();
        }

        public void Send(string message)
        {
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(message);
            this.serverStream.Write(outStream, 0, outStream.Length);
            //Console.WriteLine(string.Format("sent - {0}", message));
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
