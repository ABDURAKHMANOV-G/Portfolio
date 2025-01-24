namespace wfaWIFIScanner
{
    partial class Connection
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
            Connect = new Button();
            Password_label = new Label();
            SSID_label = new Label();
            SSID_textBox = new TextBox();
            Password_textBox = new TextBox();
            SuspendLayout();
            // 
            // Connect
            // 
            Connect.BackColor = Color.SpringGreen;
            Connect.Cursor = Cursors.Hand;
            Connect.Location = new Point(12, 135);
            Connect.Name = "Connect";
            Connect.Size = new Size(448, 35);
            Connect.TabIndex = 0;
            Connect.Text = "Подключиться";
            Connect.UseVisualStyleBackColor = false;
            Connect.Click += Connect_Click;
            // 
            // Password_label
            // 
            Password_label.AutoSize = true;
            Password_label.Location = new Point(18, 72);
            Password_label.Name = "Password_label";
            Password_label.Size = new Size(73, 20);
            Password_label.TabIndex = 1;
            Password_label.Text = "Password:";
            // 
            // SSID_label
            // 
            SSID_label.AutoSize = true;
            SSID_label.Location = new Point(31, 22);
            SSID_label.Name = "SSID_label";
            SSID_label.Size = new Size(43, 20);
            SSID_label.TabIndex = 2;
            SSID_label.Text = "SSID:";
            // 
            // SSID_textBox
            // 
            SSID_textBox.Location = new Point(104, 19);
            SSID_textBox.Name = "SSID_textBox";
            SSID_textBox.Size = new Size(356, 27);
            SSID_textBox.TabIndex = 3;
            // 
            // Password_textBox
            // 
            Password_textBox.Location = new Point(104, 69);
            Password_textBox.Name = "Password_textBox";
            Password_textBox.Size = new Size(356, 27);
            Password_textBox.TabIndex = 4;
            // 
            // Connection
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(474, 195);
            Controls.Add(Password_textBox);
            Controls.Add(SSID_textBox);
            Controls.Add(SSID_label);
            Controls.Add(Password_label);
            Controls.Add(Connect);
            Name = "Connection";
            Text = "Подключение к сети";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button Connect;
        private Label Password_label;
        private Label SSID_label;
        private TextBox SSID_textBox;
        private TextBox Password_textBox;
    }
}