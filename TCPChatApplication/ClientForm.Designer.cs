namespace TCPChatApplication
{
    partial class ClientForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            HostLabel = new Label();
            HostTextBox = new TextBox();
            PortLabel = new Label();
            PortTextBox = new TextBox();
            ConnectButton = new Button();
            MessageTextBox = new TextBox();
            SendButton = new Button();
            ChatTextBox = new TextBox();
            ChatnameTextBox = new TextBox();
            ChatnameLabel = new Label();
            SuspendLayout();
            // 
            // HostLabel
            // 
            HostLabel.AutoSize = true;
            HostLabel.Location = new Point(12, 9);
            HostLabel.Name = "HostLabel";
            HostLabel.Size = new Size(35, 15);
            HostLabel.TabIndex = 0;
            HostLabel.Text = "Host:";
            // 
            // HostTextBox
            // 
            HostTextBox.Location = new Point(53, 6);
            HostTextBox.Name = "HostTextBox";
            HostTextBox.Size = new Size(116, 23);
            HostTextBox.TabIndex = 1;
            HostTextBox.Text = "127.0.0.1";
            // 
            // PortLabel
            // 
            PortLabel.AutoSize = true;
            PortLabel.Location = new Point(186, 9);
            PortLabel.Name = "PortLabel";
            PortLabel.Size = new Size(32, 15);
            PortLabel.TabIndex = 2;
            PortLabel.Text = "Port:";
            // 
            // PortTextBox
            // 
            PortTextBox.Location = new Point(224, 6);
            PortTextBox.Name = "PortTextBox";
            PortTextBox.Size = new Size(64, 23);
            PortTextBox.TabIndex = 3;
            PortTextBox.Text = "8168";
            // 
            // ConnectButton
            // 
            ConnectButton.Location = new Point(308, 5);
            ConnectButton.Name = "ConnectButton";
            ConnectButton.Size = new Size(75, 23);
            ConnectButton.TabIndex = 4;
            ConnectButton.Text = "Connect";
            ConnectButton.UseVisualStyleBackColor = true;
            ConnectButton.Click += ConnectButton_Click;
            // 
            // MessageTextBox
            // 
            MessageTextBox.Location = new Point(53, 44);
            MessageTextBox.Multiline = true;
            MessageTextBox.Name = "MessageTextBox";
            MessageTextBox.Size = new Size(330, 64);
            MessageTextBox.TabIndex = 5;
            // 
            // SendButton
            // 
            SendButton.Location = new Point(308, 126);
            SendButton.Name = "SendButton";
            SendButton.Size = new Size(75, 23);
            SendButton.TabIndex = 6;
            SendButton.Text = "Send";
            SendButton.UseVisualStyleBackColor = true;
            SendButton.Click += SendButton_Click;
            // 
            // ChatTextBox
            // 
            ChatTextBox.Location = new Point(53, 166);
            ChatTextBox.Multiline = true;
            ChatTextBox.Name = "ChatTextBox";
            ChatTextBox.ReadOnly = true;
            ChatTextBox.ScrollBars = ScrollBars.Vertical;
            ChatTextBox.Size = new Size(330, 242);
            ChatTextBox.TabIndex = 7;
            // 
            // ChatnameTextBox
            // 
            ChatnameTextBox.Location = new Point(124, 127);
            ChatnameTextBox.Name = "ChatnameTextBox";
            ChatnameTextBox.Size = new Size(164, 23);
            ChatnameTextBox.TabIndex = 8;
            ChatnameTextBox.Text = "Client";
            // 
            // ChatnameLabel
            // 
            ChatnameLabel.AutoSize = true;
            ChatnameLabel.Location = new Point(53, 130);
            ChatnameLabel.Name = "ChatnameLabel";
            ChatnameLabel.Size = new Size(65, 15);
            ChatnameLabel.TabIndex = 9;
            ChatnameLabel.Text = "Chatname:";
            // 
            // ClientForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(435, 432);
            Controls.Add(ChatnameLabel);
            Controls.Add(ChatnameTextBox);
            Controls.Add(ChatTextBox);
            Controls.Add(SendButton);
            Controls.Add(MessageTextBox);
            Controls.Add(ConnectButton);
            Controls.Add(PortTextBox);
            Controls.Add(PortLabel);
            Controls.Add(HostTextBox);
            Controls.Add(HostLabel);
            Name = "ClientForm";
            Text = "Client";
            FormClosed += ClientForm_FormClosed;
            Load += ClientForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label HostLabel;
        private TextBox HostTextBox;
        private Label PortLabel;
        private TextBox PortTextBox;
        private Button ConnectButton;
        private TextBox MessageTextBox;
        private Button SendButton;
        private TextBox ChatTextBox;
        private TextBox ChatnameTextBox;
        private Label ChatnameLabel;
    }
}