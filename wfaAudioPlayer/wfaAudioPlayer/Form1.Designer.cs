namespace wfaAudioPlayer
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
            OpenPlaylist_Btn = new Button();
            OpenTrack_Btn = new Button();
            CreatePlaylist_Btn = new Button();
            AddTrack_Btn = new Button();
            ExportPlaylist_Btn = new Button();
            PlaylistBox = new ListBox();
            Info_Btn = new Button();
            TimerText = new Label();
            Volume_Btn = new Button();
            SpeedBtn = new Button();
            PlayBtn = new Button();
            Next_Btn = new Button();
            Previous_Btn = new Button();
            TBFileName = new Label();
            Player_groupBox = new GroupBox();
            TrackSlider = new TrackBar();
            PauseBtn = new Button();
            Playlist_name = new Label();
            VolumeSlider = new TrackBar();
            PlaylistContextMenuStrip = new ContextMenuStrip(components);
            удалитьТрекToolStripMenuItem = new ToolStripMenuItem();
            переместитьВверхToolStripMenuItem = new ToolStripMenuItem();
            переместитьВнизToolStripMenuItem = new ToolStripMenuItem();
            Player_groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)TrackSlider).BeginInit();
            ((System.ComponentModel.ISupportInitialize)VolumeSlider).BeginInit();
            PlaylistContextMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // OpenPlaylist_Btn
            // 
            OpenPlaylist_Btn.Anchor = AnchorStyles.Left;
            OpenPlaylist_Btn.Location = new Point(25, 150);
            OpenPlaylist_Btn.Name = "OpenPlaylist_Btn";
            OpenPlaylist_Btn.Size = new Size(94, 53);
            OpenPlaylist_Btn.TabIndex = 1;
            OpenPlaylist_Btn.Text = "Открыть Плейлист";
            OpenPlaylist_Btn.UseVisualStyleBackColor = true;
            OpenPlaylist_Btn.Click += OpenPlaylist_Btn_Click;
            // 
            // OpenTrack_Btn
            // 
            OpenTrack_Btn.Anchor = AnchorStyles.Left;
            OpenTrack_Btn.Location = new Point(25, 220);
            OpenTrack_Btn.Name = "OpenTrack_Btn";
            OpenTrack_Btn.Size = new Size(94, 52);
            OpenTrack_Btn.TabIndex = 2;
            OpenTrack_Btn.Text = "Открыть трек";
            OpenTrack_Btn.UseVisualStyleBackColor = true;
            OpenTrack_Btn.Click += OpenTrack_Btn_Click;
            // 
            // CreatePlaylist_Btn
            // 
            CreatePlaylist_Btn.Anchor = AnchorStyles.Right;
            CreatePlaylist_Btn.Location = new Point(758, 139);
            CreatePlaylist_Btn.Name = "CreatePlaylist_Btn";
            CreatePlaylist_Btn.Size = new Size(94, 52);
            CreatePlaylist_Btn.TabIndex = 3;
            CreatePlaylist_Btn.Text = "Создать плейлист";
            CreatePlaylist_Btn.UseVisualStyleBackColor = true;
            CreatePlaylist_Btn.Click += CreatePlaylist_Btn_Click;
            // 
            // AddTrack_Btn
            // 
            AddTrack_Btn.Anchor = AnchorStyles.Right;
            AddTrack_Btn.Location = new Point(758, 211);
            AddTrack_Btn.Name = "AddTrack_Btn";
            AddTrack_Btn.Size = new Size(94, 50);
            AddTrack_Btn.TabIndex = 4;
            AddTrack_Btn.Text = "Добавить трек";
            AddTrack_Btn.UseVisualStyleBackColor = true;
            AddTrack_Btn.Click += AddTrack_Btn_Click;
            // 
            // ExportPlaylist_Btn
            // 
            ExportPlaylist_Btn.Anchor = AnchorStyles.Right;
            ExportPlaylist_Btn.Location = new Point(758, 277);
            ExportPlaylist_Btn.Name = "ExportPlaylist_Btn";
            ExportPlaylist_Btn.Size = new Size(94, 48);
            ExportPlaylist_Btn.TabIndex = 5;
            ExportPlaylist_Btn.Text = "Выгрузить плейлист";
            ExportPlaylist_Btn.UseVisualStyleBackColor = true;
            ExportPlaylist_Btn.Click += ExportPlaylist_Btn_Click;
            // 
            // PlaylistBox
            // 
            PlaylistBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            PlaylistBox.BackColor = Color.SpringGreen;
            PlaylistBox.BorderStyle = BorderStyle.None;
            PlaylistBox.FormattingEnabled = true;
            PlaylistBox.Location = new Point(157, 20);
            PlaylistBox.Name = "PlaylistBox";
            PlaylistBox.Size = new Size(570, 360);
            PlaylistBox.TabIndex = 6;
            PlaylistBox.SelectedIndexChanged += Playlist_listBox_SelectedIndexChanged;
            PlaylistBox.MouseDown += PlaylistBox_MouseDown;
            // 
            // Info_Btn
            // 
            Info_Btn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Info_Btn.Location = new Point(822, 7);
            Info_Btn.Name = "Info_Btn";
            Info_Btn.Size = new Size(25, 29);
            Info_Btn.TabIndex = 7;
            Info_Btn.Text = "?";
            Info_Btn.UseVisualStyleBackColor = true;
            Info_Btn.Click += Info_Btn_Click;
            // 
            // TimerText
            // 
            TimerText.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            TimerText.AutoSize = true;
            TimerText.BackColor = Color.SpringGreen;
            TimerText.Location = new Point(717, 446);
            TimerText.Name = "TimerText";
            TimerText.Size = new Size(36, 20);
            TimerText.TabIndex = 9;
            TimerText.Text = "0:00";
            // 
            // Volume_Btn
            // 
            Volume_Btn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            Volume_Btn.BackColor = Color.SpringGreen;
            Volume_Btn.Location = new Point(730, 37);
            Volume_Btn.Name = "Volume_Btn";
            Volume_Btn.Size = new Size(21, 29);
            Volume_Btn.TabIndex = 10;
            Volume_Btn.Text = "🔈";
            Volume_Btn.UseVisualStyleBackColor = false;
            Volume_Btn.Click += Volume_Btn_Click;
            // 
            // SpeedBtn
            // 
            SpeedBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            SpeedBtn.BackColor = Color.SpringGreen;
            SpeedBtn.Location = new Point(792, 440);
            SpeedBtn.Name = "SpeedBtn";
            SpeedBtn.Size = new Size(38, 29);
            SpeedBtn.TabIndex = 11;
            SpeedBtn.Text = "1х";
            SpeedBtn.UseVisualStyleBackColor = false;
            SpeedBtn.Click += SpeedCtrl_Btn_Click;
            // 
            // PlayBtn
            // 
            PlayBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            PlayBtn.BackColor = Color.SpringGreen;
            PlayBtn.BackgroundImageLayout = ImageLayout.Center;
            PlayBtn.Location = new Point(230, 34);
            PlayBtn.Name = "PlayBtn";
            PlayBtn.Size = new Size(33, 29);
            PlayBtn.TabIndex = 12;
            PlayBtn.Text = "▶";
            PlayBtn.UseVisualStyleBackColor = false;
            PlayBtn.Click += Play_Btn_Click;
            // 
            // Next_Btn
            // 
            Next_Btn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            Next_Btn.BackColor = Color.SpringGreen;
            Next_Btn.Location = new Point(322, 437);
            Next_Btn.Name = "Next_Btn";
            Next_Btn.Size = new Size(33, 29);
            Next_Btn.TabIndex = 13;
            Next_Btn.Text = ">";
            Next_Btn.UseVisualStyleBackColor = false;
            Next_Btn.Click += Next_Btn_Click;
            // 
            // Previous_Btn
            // 
            Previous_Btn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            Previous_Btn.BackColor = Color.SpringGreen;
            Previous_Btn.Location = new Point(172, 34);
            Previous_Btn.Name = "Previous_Btn";
            Previous_Btn.Size = new Size(33, 29);
            Previous_Btn.TabIndex = 14;
            Previous_Btn.Text = "<";
            Previous_Btn.UseVisualStyleBackColor = false;
            Previous_Btn.Click += Previous_Btn_Click;
            // 
            // TBFileName
            // 
            TBFileName.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            TBFileName.AutoSize = true;
            TBFileName.Location = new Point(25, 34);
            TBFileName.MaximumSize = new Size(100, 25);
            TBFileName.Name = "TBFileName";
            TBFileName.Size = new Size(77, 20);
            TBFileName.TabIndex = 15;
            TBFileName.Text = "Название";
            // 
            // Player_groupBox
            // 
            Player_groupBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            Player_groupBox.BackColor = Color.SpringGreen;
            Player_groupBox.Controls.Add(PlayBtn);
            Player_groupBox.Controls.Add(TrackSlider);
            Player_groupBox.Controls.Add(Previous_Btn);
            Player_groupBox.Controls.Add(TBFileName);
            Player_groupBox.Controls.Add(Volume_Btn);
            Player_groupBox.Location = new Point(36, 403);
            Player_groupBox.Name = "Player_groupBox";
            Player_groupBox.Size = new Size(804, 86);
            Player_groupBox.TabIndex = 16;
            Player_groupBox.TabStop = false;
            // 
            // TrackSlider
            // 
            TrackSlider.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            TrackSlider.Location = new Point(350, 26);
            TrackSlider.Name = "TrackSlider";
            TrackSlider.Size = new Size(312, 56);
            TrackSlider.TabIndex = 18;
            TrackSlider.TickStyle = TickStyle.Both;
            TrackSlider.Scroll += TrackBar_Scroll;
            TrackSlider.MouseDown += TrackBar_MouseDown;
            TrackSlider.MouseUp += TrackBar_MouseUp;
            // 
            // PauseBtn
            // 
            PauseBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            PauseBtn.BackColor = Color.SpringGreen;
            PauseBtn.BackgroundImageLayout = ImageLayout.Center;
            PauseBtn.Font = new Font("Showcard Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            PauseBtn.Location = new Point(266, 435);
            PauseBtn.Name = "PauseBtn";
            PauseBtn.Size = new Size(33, 30);
            PauseBtn.TabIndex = 18;
            PauseBtn.Text = "| |";
            PauseBtn.UseVisualStyleBackColor = false;
            PauseBtn.Visible = false;
            PauseBtn.Click += PauseBtn_Click;
            // 
            // Playlist_name
            // 
            Playlist_name.AutoSize = true;
            Playlist_name.BackColor = Color.SpringGreen;
            Playlist_name.Location = new Point(157, 0);
            Playlist_name.Name = "Playlist_name";
            Playlist_name.Size = new Size(95, 20);
            Playlist_name.TabIndex = 17;
            Playlist_name.Text = "PlaylistName";
            // 
            // VolumeSlider
            // 
            VolumeSlider.BackColor = Color.SpringGreen;
            VolumeSlider.Location = new Point(766, 355);
            VolumeSlider.Name = "VolumeSlider";
            VolumeSlider.Orientation = Orientation.Vertical;
            VolumeSlider.RightToLeft = RightToLeft.No;
            VolumeSlider.Size = new Size(56, 79);
            VolumeSlider.TabIndex = 19;
            VolumeSlider.TickStyle = TickStyle.None;
            VolumeSlider.Visible = false;
            VolumeSlider.Scroll += VolumeSlider_Scroll;
            // 
            // PlaylistContextMenuStrip
            // 
            PlaylistContextMenuStrip.ImageScalingSize = new Size(20, 20);
            PlaylistContextMenuStrip.Items.AddRange(new ToolStripItem[] { переместитьВверхToolStripMenuItem, удалитьТрекToolStripMenuItem, переместитьВнизToolStripMenuItem });
            PlaylistContextMenuStrip.Name = "PlaylistContextMenuStrip";
            PlaylistContextMenuStrip.Size = new Size(214, 104);
            // 
            // удалитьТрекToolStripMenuItem
            // 
            удалитьТрекToolStripMenuItem.Name = "удалитьТрекToolStripMenuItem";
            удалитьТрекToolStripMenuItem.Size = new Size(213, 24);
            удалитьТрекToolStripMenuItem.Text = "Удалить трек";
            удалитьТрекToolStripMenuItem.Click += RemoveTrack_Click;
            // 
            // переместитьВверхToolStripMenuItem
            // 
            переместитьВверхToolStripMenuItem.Name = "переместитьВверхToolStripMenuItem";
            переместитьВверхToolStripMenuItem.Size = new Size(213, 24);
            переместитьВверхToolStripMenuItem.Text = "Переместить вверх";
            переместитьВверхToolStripMenuItem.Click += TrackUp_Click;
            // 
            // переместитьВнизToolStripMenuItem
            // 
            переместитьВнизToolStripMenuItem.Name = "переместитьВнизToolStripMenuItem";
            переместитьВнизToolStripMenuItem.Size = new Size(213, 24);
            переместитьВнизToolStripMenuItem.Text = "Переместить вниз";
            переместитьВнизToolStripMenuItem.Click += TrackDown_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(882, 488);
            Controls.Add(VolumeSlider);
            Controls.Add(PauseBtn);
            Controls.Add(Playlist_name);
            Controls.Add(Next_Btn);
            Controls.Add(SpeedBtn);
            Controls.Add(TimerText);
            Controls.Add(Info_Btn);
            Controls.Add(PlaylistBox);
            Controls.Add(ExportPlaylist_Btn);
            Controls.Add(AddTrack_Btn);
            Controls.Add(CreatePlaylist_Btn);
            Controls.Add(OpenTrack_Btn);
            Controls.Add(OpenPlaylist_Btn);
            Controls.Add(Player_groupBox);
            MinimumSize = new Size(900, 535);
            Name = "Form1";
            Text = "Form1";
            Player_groupBox.ResumeLayout(false);
            Player_groupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)TrackSlider).EndInit();
            ((System.ComponentModel.ISupportInitialize)VolumeSlider).EndInit();
            PlaylistContextMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button OpenPlaylist_Btn;
        private Button OpenTrack_Btn;
        private Button CreatePlaylist_Btn;
        private Button AddTrack_Btn;
        private Button ExportPlaylist_Btn;
        private ListBox PlaylistBox;
        private Button Info_Btn;
        private Label TimerText;
        private Button Volume_Btn;
        private Button SpeedBtn;
        private Button PlayBtn;
        private Button Next_Btn;
        private Button Previous_Btn;
        private Label TBFileName;
        private GroupBox Player_groupBox;
        private Label Playlist_name;
        private TrackBar TrackSlider;
        private Button PauseBtn;
        private TrackBar VolumeSlider;
        private ContextMenuStrip PlaylistContextMenuStrip;
        private ToolStripMenuItem удалитьТрекToolStripMenuItem;
        private ToolStripMenuItem переместитьВверхToolStripMenuItem;
        private ToolStripMenuItem переместитьВнизToolStripMenuItem;
    }
}
