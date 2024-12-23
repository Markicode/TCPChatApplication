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
        //SimpleTcpServer server;
        TcpListener listener;
        CancellationTokenSource listenCancellationTokenSource;
        CancellationToken listenCancellationToken;

        public ServerForm()
        {
            InitializeComponent();
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


        private Task Listen(IPAddress ipAddress, int Port)
        {
            Task listenTask = Task.Run(() =>
            {
                // Listen at specified ip/port.
                listener = new TcpListener(ipAddress, Port);
                StatusTextBox.Invoke(() => StatusTextBox.Text += "listening...\r\n");
                listener.Start();

                // listen loop as long as task is not canceled.
                while (!listenCancellationToken.IsCancellationRequested)
                {

                    // connect client.
                    TcpClient client = listener.AcceptTcpClient();

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
                }
                listener.Stop();
                StatusTextBox.Invoke(() => StatusTextBox.Text += "Stopped listening...\r\n");
            });
            return listenTask;
        }

        private void ServerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
