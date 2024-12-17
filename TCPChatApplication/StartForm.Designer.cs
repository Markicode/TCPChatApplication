namespace TCPChatApplication
{
    partial class StartForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ClientButton = new Button();
            ServerButton = new Button();
            SuspendLayout();
            // 
            // ClientButton
            // 
            ClientButton.Location = new Point(265, 65);
            ClientButton.Name = "ClientButton";
            ClientButton.Size = new Size(75, 23);
            ClientButton.TabIndex = 0;
            ClientButton.Text = "Client";
            ClientButton.UseVisualStyleBackColor = true;
            ClientButton.Click += this.ClientButton_Click;
            // 
            // ServerButton
            // 
            ServerButton.Location = new Point(74, 65);
            ServerButton.Name = "ServerButton";
            ServerButton.Size = new Size(75, 23);
            ServerButton.TabIndex = 1;
            ServerButton.Text = "Server";
            ServerButton.UseVisualStyleBackColor = true;
            ServerButton.Click += this.ServerButton_Click;
            // 
            // StartForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(438, 155);
            Controls.Add(ServerButton);
            Controls.Add(ClientButton);
            Name = "StartForm";
            Text = "TCP Chat Application";
            ResumeLayout(false);
        }

        #endregion

        private Button ClientButton;
        private Button ServerButton;
    }
}
