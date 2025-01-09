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
        static readonly Dictionary<string, TcpClient> clients = new Dictionary<string, TcpClient>();
        TcpListener listener;

        CancellationTokenSource listenCancellationTokenSource;
        CancellationToken listenCancellationToken;

        public event clientConnectedDelegate clientConnected;
        public delegate void clientConnectedDelegate(string clientName);
        public event clientConnectedDelegate clientDisconnected;
        public delegate void clientDisconnectedDelegate(string clientName);
        public event duplicateClientAttemptedDelegate duplicateClientAttempted;
        public delegate void duplicateClientAttemptedDelegate(string message);


        public ServerForm()
        {
            InitializeComponent();
            clientConnected += HandleClient;
            clientConnected += AddClientToList;
            clientDisconnected += DeleteClientFromList;
            duplicateClientAttempted += AddLogMessage;
        }

        private void ServerForm_Load(object sender, EventArgs e)
        {

        }

        private async void StartButton_Click(object sender, EventArgs e)
        {
            // Import connection settings from form and start listen task.
            try
            {
                IPAddress ipAddress = IPAddress.Parse(HostTextBox.Text);
                int port = Convert.ToInt32(PortTextBox.Text);
                this.listenCancellationTokenSource = new CancellationTokenSource();
                this.listenCancellationToken = listenCancellationTokenSource.Token;
                await this.Listen(ipAddress, port);
            }
            catch (Exception ex)
            {
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
                }
                listener.Stop();
                StatusTextBox.Invoke(() => StatusTextBox.Text += "Stopped listening...\r\n");
            });
            return listenTask;
        }

        private Task AcceptClients()
        {
            Task acceptClientsTask = Task.Run(async () =>
            {
                bool nameTaken = false;

                // If there are no clients pending to connect, continue listening. 
                for (int i = 0; i < 5; i++)
                {
                    if (!listener.Pending())
                    {
                        continue;
                    }

                    // In case a client does want to connect, it is assigned to client variable.
                    // The client will send its name given clientside, and the name will be checked for availability in the clients dictionary.
                    // The server will respond with a taken or free message.
                    TcpClient client = listener.AcceptTcpClient();
                    string clientName = await ReceiveName(client);

                    foreach (var c in clients)
                    {
                        if (c.Key == clientName)
                        {
                            // If the name is taken, the code will end without the client being added to the dictionary
                            this.duplicateClientAttempted(clientName + " attempted to connect. Name already in use, Connection denied.");
                            await this.Send("taken", client);
                            nameTaken = true;
                            break;
                        }

                    }

                    if (!nameTaken)
                    {
                        // if the name is free, the client will be added to the dictionary and the clientconnected event will be invoked.
                        lock (_lock) clients.Add(clientName, client);
                        string chatters = "";
                        lock (_lock)
                        {
                            if (clients.Count > 0)
                            {
                                foreach (string chatter in clients.Keys)
                                {
                                    chatters += chatter + ",";
                                }
                            }
                        }
                        chatters = chatters.Remove(chatters.Length - 1);
                        await this.Send("free~" + chatters, client);
                        this.clientConnected(clientName);
                        StatusTextBox.Invoke(() => StatusTextBox.Text += clientName + "  connected.\r\n");
                    }

                }
            });
            return acceptClientsTask;
        }

        private async void HandleClient(string clientName)
        {
            Task handleClientsTask = Task.Run(async () =>
            {
                string name = clientName;
                TcpClient client;

                lock (_lock) client = clients[name];

                /*try
                {
                    string chatters = "";
                    lock (_lock)
                    {
                        if (clients.Count > 0)
                        {
                            foreach (string chatter in clients.Keys)
                            {
                                chatters += chatter + ",";
                            }
                        }
                    }
                    chatters = chatters.Remove(chatters.Length-1);
                    await this.Send(chatters, client);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Failed to send list of chatters \r\n" + ex.ToString());
                }*/

                try
                {
                    Broadcast("3~" + clientName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to broadcast joining client name \r\n" + ex.ToString());
                }

                while (true)
                {
                    try
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
                        string[] segments = data.Split("~");

                        if (segments[0] == "1")
                        {
                            StatusTextBox.Invoke(() => StatusTextBox.Text += segments[1] + ": " + segments[2] + "\r\n");
                        }
                        if (segments[0] == "2")
                        {
                            StatusTextBox.Invoke(() => StatusTextBox.Text += segments[1] + " sent an object: " + segments[2] + "\r\n");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        break;
                    }
                }

                try
                {
                    Broadcast("4~" + clientName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to broadcast leaving client name \r\n" + ex.ToString());
                }

                lock (_lock) clients.Remove(name);
                client.Client.Shutdown(SocketShutdown.Both);
                client.Close();
                this.clientDisconnected(clientName);
                StatusTextBox.Invoke(() => StatusTextBox.Text += clientName + " disconnected \r\n");
            });
            await handleClientsTask;
        }

        private Task Send(string message, TcpClient client)
        {
            Task sendTask = Task.Run(() =>
            {
                try
                {
                    // Open stream and convert message to bytes
                    NetworkStream nwStream = client.GetStream();
                    byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(message);

                    // Send the message
                    nwStream.Write(bytesToSend, 0, bytesToSend.Length);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            });
            return sendTask;
        }

        private Task<string> ReceiveName(TcpClient client)
        {
            return Task.Run(() =>
            {
                NetworkStream nwStream = client.GetStream();
                byte[] buffer = new byte[1024];
                int byteCount = nwStream.Read(buffer, 0, buffer.Length);
                string name = Encoding.ASCII.GetString(buffer, 0, byteCount);
                return name;
            });
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

        private async void AddLogMessage(string message)
        {
            Task addLogMessageTask = Task.Run(() =>
            {
                StatusTextBox.Invoke(() => StatusTextBox.Text += message + "\r\n");
            });

            await addLogMessageTask;
        }

        private void ServerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void AddClientToList(string clientName)
        {
            ClientsCheckedListBox.Invoke(() => ClientsCheckedListBox.Items.Add(clientName));
        }

        private void DeleteClientFromList(string clientName)
        {
            ClientsCheckedListBox.Invoke(() => ClientsCheckedListBox.Items.Remove(clientName));
        }
    }
}
