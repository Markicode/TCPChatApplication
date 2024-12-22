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
                // Listen at specified IP and port number.
                listener = new TcpListener(ipAddress, Port);
                //StatusTextBox.Text += "Listening...\n";
                StatusTextBox.Invoke(() => StatusTextBox.Text += "listening...\r\n");
                listener.Start();

                while (!listenCancellationToken.IsCancellationRequested)
                {
                    Thread.Sleep(1000);
                    //StatusTextBox.Text += "Still listening... \n";
                    StatusTextBox.Invoke(() => StatusTextBox.Text += "Still listening...\r\n");
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
