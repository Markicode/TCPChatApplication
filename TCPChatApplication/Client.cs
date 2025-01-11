using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCPChatApplication
{
    public class Client
    {
        public TcpClient tcpClient {  get; set; }
        public NetworkStream networkStream {  get; set; }
        public string clientName {  get; set; }
        public bool writingToSever {  get; set; }
        public bool readingFromServer { get; set; }

        public Client(TcpClient tcpClient, string clientName) 
        { 
            this.clientName = clientName;
            this.tcpClient = tcpClient;
            this.networkStream = tcpClient.GetStream();
            this.writingToSever = false;   
            this.readingFromServer = false;
        }
    }
}
