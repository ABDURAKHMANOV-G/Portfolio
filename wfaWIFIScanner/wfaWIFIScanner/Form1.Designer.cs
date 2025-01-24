namespace wfaWIFIScanner
{
    partial class Form1
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
            components = new System.ComponentModel.Container();
            Network_DataGridView = new DataGridView();
            sSIDDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            signalQualityDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            wifiNetworkBindingSource = new BindingSource(components);
            GraphicUpdateTime = new Label();
            BestNetworkLabel = new Label();
            Scan = new Button();
            ShowData = new Button();
            ShowGraphic = new Button();
            SaveData = new Button();
            TimerLabel = new Label();
            GraphicUpdateSpeed = new TextBox();
            wifiModelBindingSource = new BindingSource(components);
            ((System.ComponentModel.ISupportInitialize)Network_DataGridView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)wifiNetworkBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)wifiModelBindingSource).BeginInit();
            SuspendLayout();
            // 
            // Network_DataGridView
            // 
            Network_DataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            Network_DataGridView.AutoGenerateColumns = false;
            Network_DataGridView.BackgroundColor = SystemColors.Info;
            Network_DataGridView.BorderStyle = BorderStyle.None;
            Network_DataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            Network_DataGridView.Columns.AddRange(new DataGridViewColumn[] { sSIDDataGridViewTextBoxColumn, signalQualityDataGridViewTextBoxColumn });
            Network_DataGridView.DataSource = wifiNetworkBindingSource;
            Network_DataGridView.Location = new Point(12, 71);
            Network_DataGridView.Name = "Network_DataGridView";
            Network_DataGridView.RowHeadersWidth = 51;
            Network_DataGridView.Size = new Size(753, 379);
            Network_DataGridView.TabIndex = 0;
            Network_DataGridView.MouseClick += Network_DataGridView_MouseClick;
            Network_DataGridView.MouseDoubleClick += Network_DataGridView_MouseDoubleClick;
            // 
            // sSIDDataGridViewTextBoxColumn
            // 
            sSIDDataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            sSIDDataGridViewTextBoxColumn.DataPropertyName = "SSID";
            sSIDDataGridViewTextBoxColumn.HeaderText = "SSID";
            sSIDDataGridViewTextBoxColumn.MinimumWidth = 6;
            sSIDDataGridViewTextBoxColumn.Name = "sSIDDataGridViewTextBoxColumn";
            sSIDDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // signalQualityDataGridViewTextBoxColumn
            // 
            signalQualityDataGridViewTextBoxColumn.DataPropertyName = "SignalQuality";
            signalQualityDataGridViewTextBoxColumn.HeaderText = "SignalQuality";
            signalQualityDataGridViewTextBoxColumn.MinimumWidth = 6;
            signalQualityDataGridViewTextBoxColumn.Name = "signalQualityDataGridViewTextBoxColumn";
            signalQualityDataGridViewTextBoxColumn.ReadOnly = true;
            signalQualityDataGridViewTextBoxColumn.Width = 125;
            // 
            // wifiNetworkBindingSource
            // 
            wifiNetworkBindingSource.DataSource = typeof(MyLibrary.WifiNetwork);
            // 
            // GraphicUpdateTime
            // 
            GraphicUpdateTime.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            GraphicUpdateTime.AutoSize = true;
            GraphicUpdateTime.Location = new Point(71, 9);
            GraphicUpdateTime.Name = "GraphicUpdateTime";
            GraphicUpdateTime.Size = new Size(214, 20);
            GraphicUpdateTime.TabIndex = 1;
            GraphicUpdateTime.Text = "Частота обновления графика";
            // 
            // BestNetworkLabel
            // 
            BestNetworkLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BestNetworkLabel.AutoSize = true;
            BestNetworkLabel.Location = new Point(588, 26);
            BestNetworkLabel.Name = "BestNetworkLabel";
            BestNetworkLabel.Size = new Size(95, 20);
            BestNetworkLabel.TabIndex = 2;
            BestNetworkLabel.Text = "Лучшая сеть";
            // 
            // Scan
            // 
            Scan.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            Scan.BackColor = Color.SpringGreen;
            Scan.Location = new Point(9, 469);
            Scan.Margin = new Padding(0);
            Scan.Name = "Scan";
            Scan.RightToLeft = RightToLeft.No;
            Scan.Size = new Size(191, 49);
            Scan.TabIndex = 3;
            Scan.Text = "Начать сканирование";
            Scan.UseVisualStyleBackColor = false;
            Scan.Click += Scan_Click;
            // 
            // ShowData
            // 
            ShowData.Anchor = AnchorStyles.Bottom;
            ShowData.AutoSize = true;
            ShowData.BackColor = Color.SpringGreen;
            ShowData.Location = new Point(208, 469);
            ShowData.Margin = new Padding(10);
            ShowData.Name = "ShowData";
            ShowData.Size = new Size(186, 49);
            ShowData.TabIndex = 4;
            ShowData.Text = "Показать данные";
            ShowData.UseVisualStyleBackColor = false;
            ShowData.Click += ShowData_Click;
            // 
            // ShowGraphic
            // 
            ShowGraphic.Anchor = AnchorStyles.Bottom;
            ShowGraphic.AutoSize = true;
            ShowGraphic.BackColor = Color.SpringGreen;
            ShowGraphic.Location = new Point(401, 469);
            ShowGraphic.Name = "ShowGraphic";
            ShowGraphic.Size = new Size(158, 49);
            ShowGraphic.TabIndex = 5;
            ShowGraphic.Text = "Показать график";
            ShowGraphic.UseVisualStyleBackColor = false;
            ShowGraphic.Click += ShowGraphic_Click;
            // 
            // SaveData
            // 
            SaveData.AllowDrop = true;
            SaveData.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            SaveData.AutoSize = true;
            SaveData.BackColor = Color.SpringGreen;
            SaveData.Location = new Point(564, 469);
            SaveData.Name = "SaveData";
            SaveData.Size = new Size(203, 49);
            SaveData.TabIndex = 6;
            SaveData.Text = "Сохранить данные";
            SaveData.UseCompatibleTextRendering = true;
            SaveData.UseVisualStyleBackColor = false;
            SaveData.Click += SaveData_Click;
            // 
            // TimerLabel
            // 
            TimerLabel.Anchor = AnchorStyles.Bottom;
            TimerLabel.AutoSize = true;
            TimerLabel.Location = new Point(294, 528);
            TimerLabel.Name = "TimerLabel";
            TimerLabel.Size = new Size(138, 20);
            TimerLabel.TabIndex = 7;
            TimerLabel.Text = "Осталось секунд: 0";
            TimerLabel.Click += TimerLabel_Click;
            // 
            // GraphicUpdateSpeed
            // 
            GraphicUpdateSpeed.BorderStyle = BorderStyle.FixedSingle;
            GraphicUpdateSpeed.Location = new Point(124, 38);
            GraphicUpdateSpeed.Name = "GraphicUpdateSpeed";
            GraphicUpdateSpeed.Size = new Size(96, 27);
            GraphicUpdateSpeed.TabIndex = 8;
            // 
            // wifiModelBindingSource
            // 
            wifiModelBindingSource.DataSource = typeof(MyLibrary.WifiModel);
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = SystemColors.Info;
            ClientSize = new Size(777, 563);
            Controls.Add(GraphicUpdateSpeed);
            Controls.Add(TimerLabel);
            Controls.Add(SaveData);
            Controls.Add(ShowGraphic);
            Controls.Add(ShowData);
            Controls.Add(Scan);
            Controls.Add(BestNetworkLabel);
            Controls.Add(GraphicUpdateTime);
            Controls.Add(Network_DataGridView);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)Network_DataGridView).EndInit();
            ((System.ComponentModel.ISupportInitialize)wifiNetworkBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)wifiModelBindingSource).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView Network_DataGridView;
        private Label GraphicUpdateTime;
        private Label BestNetworkLabel;
        private Button Scan;
        private Button ShowData;
        private Button ShowGraphic;
        private Button SaveData;
        private Label TimerLabel;
        private TextBox GraphicUpdateSpeed;
        private BindingSource wifiNetworkBindingSource;
        private BindingSource wifiModelBindingSource;
        private DataGridViewTextBoxColumn sSIDDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn signalQualityDataGridViewTextBoxColumn;
    }
}
