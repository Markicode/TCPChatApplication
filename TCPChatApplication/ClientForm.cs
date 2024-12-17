using SimpleTCP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCPChatApplication
{
    public partial class ClientForm : Form
    {
        SimpleTcpClient client;

        public ClientForm()
        {
            InitializeComponent();
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {
            client = new SimpleTcpClient();
            client.StringEncoder = Encoding.UTF8;
            client.DataReceived += Client_DataReceived;
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            ConnectButton.Enabled = false;

            client.Connect(HostTextBox.Text, Convert.ToInt32(PortTextBox.Text));
        }

        private void Client_DataReceived(object sender, SimpleTCP.Message e)
        {

            ChatTextBox.Invoke((MethodInvoker)delegate ()
            {
                ChatTextBox.Text += e.MessageString;
            });
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            client.WriteLineAndGetReply(MessageTextBox.Text, TimeSpan.FromSeconds(3));
        }

        private void ClientForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
