namespace TCPChatApplication
{
    partial class ServerForm
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
            StopButton = new Button();
            StatusTextBox = new TextBox();
            StartButton = new Button();
            PortTextBox = new TextBox();
            PortLabel = new Label();
            HostTextBox = new TextBox();
            HostLabel = new Label();
            SuspendLayout();
            // 
            // StopButton
            // 
            StopButton.Location = new Point(411, 17);
            StopButton.Name = "StopButton";
            StopButton.Size = new Size(75, 23);
            StopButton.TabIndex = 13;
            StopButton.Text = "Stop";
            StopButton.UseVisualStyleBackColor = true;
            StopButton.Click += StopButton_Click;
            // 
            // StatusTextBox
            // 
            StatusTextBox.Location = new Point(65, 56);
            StatusTextBox.Multiline = true;
            StatusTextBox.Name = "StatusTextBox";
            StatusTextBox.ReadOnly = true;
            StatusTextBox.ScrollBars = ScrollBars.Vertical;
            StatusTextBox.Size = new Size(421, 287);
            StatusTextBox.TabIndex = 12;
            // 
            // StartButton
            // 
            StartButton.Location = new Point(320, 17);
            StartButton.Name = "StartButton";
            StartButton.Size = new Size(75, 23);
            StartButton.TabIndex = 11;
            StartButton.Text = "Start";
            StartButton.UseVisualStyleBackColor = true;
            StartButton.Click += StartButton_Click;
            // 
            // PortTextBox
            // 
            PortTextBox.Location = new Point(236, 18);
            PortTextBox.Name = "PortTextBox";
            PortTextBox.Size = new Size(64, 23);
            PortTextBox.TabIndex = 10;
            PortTextBox.Text = "8168";
            // 
            // PortLabel
            // 
            PortLabel.AutoSize = true;
            PortLabel.Location = new Point(198, 21);
            PortLabel.Name = "PortLabel";
            PortLabel.Size = new Size(32, 15);
            PortLabel.TabIndex = 9;
            PortLabel.Text = "Port:";
            // 
            // HostTextBox
            // 
            HostTextBox.Location = new Point(65, 18);
            HostTextBox.Name = "HostTextBox";
            HostTextBox.Size = new Size(116, 23);
            HostTextBox.TabIndex = 8;
            HostTextBox.Text = "127.0.0.1";
            // 
            // HostLabel
            // 
            HostLabel.AutoSize = true;
            HostLabel.Location = new Point(24, 21);
            HostLabel.Name = "HostLabel";
            HostLabel.Size = new Size(35, 15);
            HostLabel.TabIndex = 7;
            HostLabel.Text = "Host:";
            // 
            // ServerForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(541, 373);
            Controls.Add(StopButton);
            Controls.Add(StatusTextBox);
            Controls.Add(StartButton);
            Controls.Add(PortTextBox);
            Controls.Add(PortLabel);
            Controls.Add(HostTextBox);
            Controls.Add(HostLabel);
            Name = "ServerForm";
            Text = "Server";
            FormClosed += ServerForm_FormClosed;
            Load += ServerForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button StopButton;
        private TextBox StatusTextBox;
        private Button StartButton;
        private TextBox PortTextBox;
        private Label PortLabel;
        private TextBox HostTextBox;
        private Label HostLabel;
    }
}