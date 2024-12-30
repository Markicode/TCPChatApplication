using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace TCPChatApplication
{
    public partial class ServerForm : Form
    {
        static readonly object _lock = new object();
        static readonly Dictionary<int, TcpClient> clients = new Dictionary<int, TcpClient>();
        static readonly Dictionary<int, NetworkStream> nwStreams = new Dictionary<int, NetworkStream>();
        TcpListener listener;
        CancellationTokenSource listenCancellationTokenSource;
        CancellationToken listenCancellationToken;
        int clientCounter;
        public event clientConnectedDelegate ClientConnected;
        public delegate void clientConnectedDelegate(int clientCounter);


        public ServerForm()
        {
            InitializeComponent();
            ClientConnected += HandleClient;
        }

        private void ServerForm_Load(object sender, EventArgs e)
        {

        }

        private async void StartButton_Click(object sender, EventArgs e)
        {
            try
            {
                IPAddress ipAddress = IPAddress.Parse(HostTextBox.Text);
                int port = Convert.ToInt32(PortTextBox.Text);
                clientCounter = 1;
                this.listenCancellationTokenSource = new CancellationTokenSource();
                this.listenCancellationToken = listenCancellationTokenSource.Token;
                await this.Listen(ipAddress, port);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            listenCancellationTokenSource.Cancel();
        }


        private Task Listen(IPAddress ipAddress, int port)
        {
            Task listenTask = Task.Run(async () =>
            {

                // Listen at specified ip/port.
                listener = new TcpListener(ipAddress, port);
                StatusTextBox.Invoke(() => StatusTextBox.Text += "listening...\r\n");
                listener.Start();

                // listen loop as long as task is not canceled.
                while (!listenCancellationToken.IsCancellationRequested)
                {
                    await AcceptClients();
                    // connect client.
                    

                    //HandleClients(clientCounter).Start();


                    /*
                    // Get incoming data through stream
                    NetworkStream nwStream = client.GetStream();
                    byte[] buffer = new byte[client.ReceiveBufferSize];

                    // Read incoming stream
                    int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

                    // Convert data to string
                    string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    StatusTextBox.Invoke(() => StatusTextBox.Text += dataReceived + "\r\n");

                    // Write back to client
                    byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes("--received--");
                    nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                    */
                }
                listener.Stop();
                StatusTextBox.Invoke(() => StatusTextBox.Text += "Stopped listening...\r\n");
            });
            return listenTask;
        }

        private Task AcceptClients()
        {
            Task acceptClientsTask = Task.Run(() =>
            {
                for (int i = 0; i < 5; i++)
                { 
                    if(!listener.Pending())
                    {
                        continue;
                    }
                    TcpClient client = listener.AcceptTcpClient();
                    lock (_lock) clients.Add(clientCounter, client);
                    this.ClientConnected(clientCounter);
                    StatusTextBox.Invoke(() => StatusTextBox.Text += clientCounter.ToString() + "Someone connected.\r\n");
                    clientCounter++;
                    
                }
            });
            return acceptClientsTask;
        }

        private async void HandleClient(int clientId)
        {
            Task handleClientsTask = Task.Run(() =>
            {
                int id = clientId;
                TcpClient client;

                lock (_lock) client = clients[id];

                while (true)
                {
                    NetworkStream nwStream = client.GetStream();
                    byte[] buffer = new byte[1024];
                    int byteCount = nwStream.Read(buffer, 0, buffer.Length);

                    if (byteCount == 0)
                    {
                        break;
                    }

                    string data = Encoding.ASCII.GetString(buffer, 0, byteCount);
                    Broadcast(data);
                    StatusTextBox.Invoke(() => StatusTextBox.Text += data + "\r\n");
                }

                //lock (_lock) clients.Remove(id);
                //client.Client.Shutdown(SocketShutdown.Both);
                //client.Close();
            });
            await handleClientsTask;
        }

        public static void Broadcast(string data)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(data);

            lock (_lock)
            {
                foreach (TcpClient c in clients.Values)
                {
                    NetworkStream stream = c.GetStream();

                    stream.Write(buffer, 0, buffer.Length);
                }
            }
        }

        private void ServerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
