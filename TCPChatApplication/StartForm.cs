namespace TCPChatApplication
{
    public partial class StartForm : Form
    {
        public StartForm()
        {
            InitializeComponent();
        }

        private void ServerButton_Click(object sender, EventArgs e)
        {
            ServerForm serverForm = new ServerForm();
            serverForm.Show();
            //this.Hide();
        }

        private void ClientButton_Click(object sender, EventArgs e)
        {
            ClientForm clientForm = new ClientForm();
            clientForm.Show();
            //this.Hide();
        }
    }

    
}
