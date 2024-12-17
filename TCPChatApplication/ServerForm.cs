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
    public partial class ServerForm : Form
    {
        SimpleTcpServer server;

        public ServerForm()
        {
            InitializeComponent();
        }

        private void ServerForm_Load(object sender, EventArgs e)
        {
            server = new SimpleTcpServer();
            server.Delimiter = 0x13;//enter
            server.StringEncoder = Encoding.UTF8;
            server.DataReceived += Server_DataReceived;
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            StatusTextBox.Text += "Server starting...";
            System.Net.IPAddress ip = System.Net.IPAddress.Parse(HostTextBox.Text);
            server.Start(ip, Convert.ToInt32(PortTextBox.Text));
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            if (server.IsStarted)
                server.Stop();
        }

        private void Server_DataReceived(object sender, SimpleTCP.Message e)
        {
            //Update mesage to txtStatus
            StatusTextBox.Invoke((MethodInvoker)delegate ()
            {
                StatusTextBox.Text += e.MessageString;
                e.ReplyLine(string.Format("You said: {0}", e.MessageString));
            });
        }

        private void ServerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
