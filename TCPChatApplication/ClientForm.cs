using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;

namespace TCPChatApplication
{
    public partial class ClientForm : Form
    {
        private TcpClient? client;
        private string? chatName;
        CancellationTokenSource connectionCTS;
        CancellationToken connectionCancelToken;

        public ClientForm()
        {
            InitializeComponent();
            connectionCTS = new CancellationTokenSource();
            connectionCancelToken = connectionCTS.Token;

        }

        private void ClientForm_Load(object sender, EventArgs e)
        {

        }

        // (Dis)connect button handler.
        private async void ConnectButton_Click(object sender, EventArgs e)
        {

            if (ConnectButton.Text == "Connect")
            {
                if (connectionCTS != null)
                {
                    // Refresh cancellation token upon making a new connection.
                    connectionCTS.Dispose();
                    connectionCTS = new CancellationTokenSource();
                    connectionCancelToken = connectionCTS.Token;
                }

                await this.Connect(connectionCancelToken);
            }
            if (ConnectButton.Text == "Disconnect")
            {
                if (connectionCTS != null)
                {
                    connectionCTS.Cancel();
                }
            }
        }


        private async void SendButton_Click(object sender, EventArgs e)
        {
            string textToSend = MessageTextBox.Text;
            await this.Send(textToSend);
        }

        private Task Connect(CancellationToken connCancelToken)
        {
            Task connectTask = Task.Run(async () =>
            {


                try
                {
                    // Create TCPClient at specified ip/port and assign chatname. Upon succes, change connect button to disconnect button.
                    string ipAddress = HostTextBox.Text;
                    int port = Convert.ToInt32(PortTextBox.Text);
                    this.client = new TcpClient(ipAddress, port);
                    this.chatName = ChatnameTextBox.Text;
                    ConnectButton.Invoke(() => { ConnectButton.Text = "Disconnect"; });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Task connect error");
                }

                NetworkStream ns = client.GetStream();
                string answer = "";

                if (this.chatName != null && client != null)
                {
                    // Send chatname to server
                    byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(this.chatName);
                    ns.Write(bytesToSend, 0, bytesToSend.Length);

                    for (int i = 0; i < 10000; i++)
                    {
                        if (ns.DataAvailable)
                        {
                            byte[] receivedBytes = new byte[1024];
                            int byte_count = ns.Read(receivedBytes, 0, receivedBytes.Length);
                            answer = Encoding.ASCII.GetString(receivedBytes, 0, byte_count);
                            break;
                        }
                        else
                        {
                            answer = "timeout";
                        }
                    }

                    if (answer == "taken")
                    {
                        await this.Disconnect();
                        ChatTextBox.Invoke(() => ChatTextBox.Text += "Username already taken. Disconnected! \r\n");
                    }
                    if (answer == "free")
                    {
                        ChatTextBox.Invoke(() => ChatTextBox.Text += "Username appointed. Connected! \r\n");
                    }
                    if (answer == "timeout")
                    {
                        ChatTextBox.Invoke(() => ChatTextBox.Text += "Server timeout. Not connected! \r\n");
                    }

                    /*while ((byte_count = ns.Read(receivedBytes, 0, receivedBytes.Length)) > 0)
                    {
                        string name = Encoding.ASCII.GetString(receivedBytes, 0, byte_count);
                    }*/
                }

                else
                {
                    await this.Disconnect();
                }


                while (true)
                {
                    try
                    {

                        if (!connCancelToken.IsCancellationRequested)
                        {
                            if (ns.DataAvailable)
                            {
                                try
                                {
                                    await ReceiveData(client, connCancelToken);
                                }
                                catch (Exception e)
                                {
                                    MessageBox.Show(e.ToString());
                                }
                            }
                        }
                        else
                        {
                            connCancelToken.ThrowIfCancellationRequested();
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ex is OperationCanceledException)
                        {
                            MessageBox.Show($"Disconnected by user");
                            ConnectButton.Invoke(() => { ConnectButton.Text = "Connect"; });
                            break;
                        }
                        else
                        {
                            MessageBox.Show($"Other exception");
                            ConnectButton.Invoke(() => { ConnectButton.Text = "Connect"; });
                            break;
                        }
                    }
                }

                // TODO: proper cleanup and control functionality
                client.Client.Shutdown(SocketShutdown.Send);
                ns.Close();
                client.Close();
                ChatTextBox.Invoke(() => ChatTextBox.Text += "Disconnected \r\n");

            });
            return connectTask;
        }

        private Task Send(string message)
        {
            Task sendTask = Task.Run(() =>
            {
                try
                {
                    // Open stream and convert message to bytes
                    // Add nr.1 at the beginning to let the server know its a string message.
                    NetworkStream nwStream = client.GetStream();
                    byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes("1~" + this.chatName + "~" + message);

                    // Send the message
                    nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                    MessageTextBox.Invoke(() => { MessageTextBox.Text = ""; });

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            });
            return sendTask;
        }

        private Task SendObject(object o)
        {
            Task sendObjectTask = Task.Run(() =>
            {
                try
                {
                    // Convert object to json string, open stream and send json string as byte array.
                    // Add nr. 2 at the beginning of the string to signal the server that it is receiving an object.
                    string jsonString = JsonSerializer.Serialize(o);
                    NetworkStream nwStream = client.GetStream();
                    byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes("2~" + this.chatName + "~" + jsonString);

                    // Send the message
                    nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                    MessageTextBox.Invoke(() => { MessageTextBox.Text = ""; });
                    ChatTextBox.Invoke(() => { ChatTextBox.Text += "You sent an object \r\n"; });

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            });
            return sendObjectTask;
        }

        private Task ReceiveData(TcpClient client, CancellationToken connCancelToken)
        {
            Task receiveDataTask = Task.Run(() =>
            {
                try
                {
                    NetworkStream ns = client.GetStream();
                    byte[] receivedBytes = new byte[1024];
                    int byte_count;

                    while (!connCancelToken.IsCancellationRequested)
                    {
                        if (ns.DataAvailable)
                        {
                            byte_count = ns.Read(receivedBytes, 0, receivedBytes.Length);
                            string data = Encoding.ASCII.GetString(receivedBytes, 0, byte_count);
                            string[] segments = data.Split('~');

                            if (segments[0] == "1")
                            {
                                ChatTextBox.Invoke(() => ChatTextBox.Text += segments[1] + ": " + segments[2] + "\r\n");
                            }
                            if (segments[0] == "2")
                            {
                                ChatTextBox.Invoke(() => ChatTextBox.Text += segments[1] + ": " + segments[2] + "\r\n");
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    if (ex is OperationCanceledException)
                    {
                        MessageBox.Show($"Operation canceled");
                    }
                    else
                    {
                        MessageBox.Show("task recievedata error");
                    }

                }
                ChatTextBox.Invoke(() => ChatTextBox.Text += "7");

            });
            return receiveDataTask;
        }

        private Task Disconnect()
        {
            Task disconnectTask = Task.Run(() =>
            {
                connectionCTS.Cancel();
            });
            return disconnectTask;
        }

        private void ClientForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private async void SendObjectButton_Click(object sender, EventArgs e)
        {
            TestObject testObject = new TestObject()
            {
                firstName = "Mark",
                lastName = "Schwering",
                age = 38
            };
            await this.SendObject(testObject);
        }
    }
}
