using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCPChatApplication
{
    public partial class ClientForm : Form
    {
        private TcpClient client;

        public ClientForm()
        {
            InitializeComponent();
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {

        }

        private async void ConnectButton_Click(object sender, EventArgs e)
        {
            await this.Connect();
        }



        private async void SendButton_Click(object sender, EventArgs e)
        {
            await this.Send();
        }

        private Task Connect()
        {
            Task connectTask = Task.Run(() =>
            {
                try
                {
                    // Create TCPClient at specified ip/port
                    string ipAddress = HostTextBox.Text;
                    int port = Convert.ToInt32(PortTextBox.Text);
                    client = new TcpClient(ipAddress, port);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

            });
            return connectTask;
        }

        private Task Send()
        {
            Task sendTask = Task.Run(() =>
            {
                try
                {
                    // Open stream and convert message to bytes
                    string textToSend = MessageTextBox.Text;
                    NetworkStream nwStream = client.GetStream();
                    byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);

                    // Send the message
                    ChatTextBox.Text += "You sent: " + textToSend;
                    nwStream.Write(bytesToSend, 0, bytesToSend.Length);

                    // Confirm message received
                    byte[] bytesToRead = new byte[client.ReceiveBufferSize];
                    int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
                    ChatTextBox.Invoke(() => ChatTextBox.Text += Encoding.ASCII.GetString(bytesToRead, 0, bytesRead) + "\r\n");
                    client.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            });
            return sendTask;
        }

        private void ClientForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
