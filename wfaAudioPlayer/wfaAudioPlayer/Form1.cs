using Microsoft.Win32;
using System.Text;
using System.Windows;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using AudioPlayerLibrary;
using System;
using NAudio.Wave;
using TagLib;
using System.Windows.Media.Animation;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.IO.Compression;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;



namespace wfaAudioPlayer
{
    public partial class Form1 : Form
    {
        private readonly AudioPlayerMain AudioMain;
        private int draggedItemIndex = -1;
        public string filename, TBFileName_v, TBArtistName_v;
        public string[] file;
        public string fileNameWithoutExtension;
        private string actual_created_playlistPath;

        PlaybackState plbcks = PlaybackState.Stopped;
        public int index;
        DispatcherTimer timer = new DispatcherTimer();
        private string selectedPath;

        public Form1()
        {
            InitializeComponent();
            AudioMain = new AudioPlayerMain();

            timer.Interval = TimeSpan.FromSeconds(1);
        }

        private void Playlist_listBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void AddTrack_Btn_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog
            {
                Filter = "Audio Files|*.mp3;*.wav;*.wma;*.flac",
                Title = "Select an Audio File"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK) // ��� WinForms ���������� DialogResult.OK
            {
                string filePath = openFileDialog.FileName;
                try
                {
                    AudioMain.AddTrackToPlaylist(filePath, actual_created_playlistPath);
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show($"Error playing file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("No file selected.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            //���������� ListBox'�
            PlaylistLB_Update();
        }

        private void Info_Btn_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("�����: ������������ ����� ����������������\n" +
                "������� ����������� ���������������� ������������\n" +
                "���� �������: 01.01.2025\n" +
                "������: 1.0.0.0",
                "� ����������",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void OpenTrack_Btn_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog
            {
                Filter = "Audio Files|*.mp3;*.wav;*.wma;*.flac",
                Title = "Select an Audio File"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK) // ��� WPF ���������� `true`
            {
                string filePath = openFileDialog.FileName;
                try
                {
                    AudioMain.OpenFile(filePath);
                    // ��������� �������� ����� � ������� ������
                    TBFileName.Text = System.IO.Path.GetFileNameWithoutExtension(filePath);
                    // ������ ��������
                    //StartMarqueeAnimation();
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show($"Error playing file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("No file selected.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void OpenPlaylist_Btn_Click(object sender, EventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK && AudioMain.playlist != null)
                {
                    string selectedPath = dialog.SelectedPath;
                    AudioMain.LoadPlaylist(selectedPath);
                    //�������� ���������
                    Playlist_name.Text = System.IO.Path.GetFileNameWithoutExtension(selectedPath);
                    if (AudioMain.playlist.Count > 0)
                    {
                        //�������� �������� ������
                        TBFileName.Text = System.IO.Path.GetFileNameWithoutExtension(AudioMain.Playlist[0]);
                        //StartMarqueeAnimation();
                    }
                    // ��������� �������� � ����������
                    PlaylistLB_Update();
                }
            }
        }

        private void CreatePlaylist_Btn_Click(object sender, EventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    selectedPath = dialog.SelectedPath;
                    AudioMain.CreatePlaylist(selectedPath);
                    AudioMain.LoadPlaylist(selectedPath);
                    //�������� ���������
                    Playlist_name.Text = System.IO.Path.GetFileNameWithoutExtension(selectedPath);
                    //���������� ���� � �����
                    actual_created_playlistPath = selectedPath;
                    //���������� ListBox'�
                    PlaylistLB_Update();
                }
            }
        }

        private void ExportPlaylist_Btn_Click(object sender, EventArgs e)
        {
            // ��������, ��� � ��������� ���� �����
            if (AudioMain.Playlist.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("�������� ����. ������ ���������.", "������", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ��������� ������ ���������� �����
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Title = "��������� �������� ���...",
                Filter = "ZIP Archive (*.zip)|*.zip",
                FileName = System.IO.Path.GetFileNameWithoutExtension(selectedPath) // ��� ������ ��� � �������� �����
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string zipPath = saveFileDialog.FileName; // ���� ��� ���������� ������

                try
                {
                    // ������� ��������� ����� ��� ����� ���������
                    string tempFolder = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "PlaylistTemp");
                    Directory.CreateDirectory(tempFolder);

                    // �������� ���������� ��������� ��������� � ��������� �����
                    foreach (var trackPath in AudioMain.Playlist)
                    {
                        string fileName = System.IO.Path.GetFileName(trackPath);
                        string destinationPath = System.IO.Path.Combine(tempFolder, fileName);
                        System.IO.File.Copy(trackPath, destinationPath, true);
                    }

                    // �������������� ������ � ������������� ����� �� ������ ������
                    int ind = 1;
                    foreach (var trackPath in AudioMain.Playlist)
                    {
                        // �������� ������ ��� ����� (� �����������)
                        string oldFileName = System.IO.Path.GetFileName(trackPath);

                        // ��������� ����� ��� ����� (������ + ��� ��� ������������)
                        string newFileName = $"{ind} - {System.IO.Path.GetFileNameWithoutExtension(trackPath)}{System.IO.Path.GetExtension(trackPath)}";

                        // ��������� ������ ���� � ������� ����� � ��������� �����
                        string oldFilePath = System.IO.Path.Combine(tempFolder, oldFileName);

                        // ��������� ������ ���� � ������ ����� � ��������� �����
                        string newFilePath = System.IO.Path.Combine(tempFolder, newFileName);

                        // ��������������� ����
                        System.IO.File.Move(oldFilePath, newFilePath);

                        ind++;
                    }

                    // ������� ZIP-����� �� ��������� �����
                    if (System.IO.File.Exists(zipPath))
                        System.IO.File.Delete(zipPath); // ������� ������������ ����, ���� �� ����
                    ZipFile.CreateFromDirectory(tempFolder, zipPath);

                    // ������� ��������� �����
                    Directory.Delete(tempFolder, true);

                    // �������� ������������ �� �������� �������� ������
                    System.Windows.Forms.MessageBox.Show($"�������� ������� �������� � �����: {zipPath}", "�����", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    // ���� ��������� ������, �������� ������������
                    System.Windows.Forms.MessageBox.Show($"������ ��� �������� ������: {ex.Message}", "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Play_Btn_Click(object sender, EventArgs e)
        {
            PlayBtn.Visible = false;
            PauseBtn.Visible = true;
            AudioMain?.Play();
            // ������ �������
            timer.Tick += Timer_Tick;
            timer.Start();
        }


        //������� ������������ ������������� ������
        //������� �������
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (AudioMain != null && AudioMain.TotalDuration.TotalSeconds > 0)
            {
                TrackSlider.Maximum = (int)(AudioMain.TotalDuration.TotalSeconds);
                TrackSlider.Value = (int)(AudioMain.CurrentPosition.TotalSeconds);
            }
            TimerText.Text = $"{AudioMain.minutes}:{AudioMain.seconds}";
        }
        //������� ���������� ���������
        public void PlaylistLB_Update()
        {
            int ind = 1;
            PlaylistBox.Items.Clear();
            foreach (var track in AudioMain.Playlist)
            {
                fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(track);

                // ��������� ������������ ����� � ��������
                TimeSpan trackDuration = AudioMain.GetTrackDuration(track);
                string formattedDuration = trackDuration.ToString(@"mm\:ss");
                PlaylistBox.Items.Add($"{ind} - {fileNameWithoutExtension} - {formattedDuration}");

                ind++;
            }
        }
        private void PlaylistBox_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // ���������, ��� ���� ��� ����� ������� ����
            if (e.Button == MouseButtons.Left)
            {
                // ������� ������ ��������, �� �������� ��� ����
                int index = PlaylistBox.IndexFromPoint(e.Location);

                if (index != System.Windows.Forms.ListBox.NoMatches) // ���������, ��� ������ �������
                {
                    // �������� ��� ������ ��������
                    AudioMain.PlaySelectedTrack(index);

                    // ������ ������ Play � Pause
                    PlayBtn.Visible = false;
                    PauseBtn.Visible = true;

                    // ���������� �������� �����
                    TBFileName.Text = System.IO.Path.GetFileNameWithoutExtension(AudioMain.Playlist[index]);

                    // ���� �����, ��������� ��������
                    // StartMarqueeAnimation();

                    // ������ �������
                    timer.Tick += Timer_Tick;
                    timer.Start();
                }
            }
            else
            {
                // ������� ������ ��������, �� �������� ��� ����
                int index = PlaylistBox.IndexFromPoint(e.Location);

                if (index != System.Windows.Forms.ListBox.NoMatches) // ���������, ��� ���� �� ��������
                {
                    // �������� �������, �� �������� ��� ��������� ����
                    PlaylistBox.SelectedIndex = index;

                    // ���������� ����������� ����
                    PlaylistContextMenuStrip.Show(PlaylistBox, e.Location);
                }
            }
        }



        private bool isDragging = false; // ���� ��� ������������, ����� ������������ ��������������� � ���������
        private string SelTrack;

        private void TrackBar_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            isDragging = true; // ������������� ����, ����� ������������ �������� �������������
        }

        private void TrackBar_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (isDragging && AudioMain != null)
            {
                // ������������� ������� ����� � ������������ �� ��������� ��������
                AudioMain.CurrentPosition = TimeSpan.FromSeconds(TrackSlider.Value);
            }
            isDragging = false; // ���������� ���� ����� ���������� ��������������
        }

        private void TrackBar_Scroll(object sender, EventArgs e)
        {
            if (isDragging && AudioMain != null)
            {
                // ��������� ������� ������� ��� ��������� ��������
                AudioMain.CurrentPosition = TimeSpan.FromSeconds(TrackSlider.Value);
            }
        }


        private void Previous_Btn_Click(object sender, EventArgs e)
        {
            AudioMain?.Previous();
            TrackSlider.Value = 0;
            //�������� �������� ������
            TBFileName.Text = System.IO.Path.GetFileNameWithoutExtension(AudioMain.Playlist[AudioMain.currentTrackIndex]);
            //StartMarqueeAnimation();
        }

        private void Next_Btn_Click(object sender, EventArgs e)
        {
            AudioMain?.Next();
            TrackSlider.Value = 0;
            //�������� �������� ������
            TBFileName.Text = System.IO.Path.GetFileNameWithoutExtension(AudioMain.Playlist[AudioMain.currentTrackIndex]);
            //StartMarqueeAnimation();
        }

        private void SpeedCtrl_Btn_Click(object sender, EventArgs e)
        {
            SpeedBtn.Text = AudioMain.ToggleSpeed();
        }

        private void Volume_Btn_Click(object sender, EventArgs e)
        {
            // ����������� ��������� ��������
            VolumeSlider.Visible = !VolumeSlider.Visible;
        }

        private void PauseBtn_Click(object sender, EventArgs e)
        {
            plbcks = PlaybackState.Paused;
            PlayBtn.Visible = true;
            PauseBtn.Visible = false;
            AudioMain?.Pause();
        }

        private void VolumeSlider_Scroll(object sender, EventArgs e)
        {
            // ������������� ��������� ����� AudioMain
            AudioMain.Volume = VolumeSlider.Value;
        }

        private void RemoveTrack_Click(object sender, EventArgs e)
        {
            // ���������, ���� �� ��������� �������
            if (PlaylistBox.SelectedItem != null)
            {
                int selectedIndex = PlaylistBox.SelectedIndex;              // �������� ������

                //������� �� �����
                AudioMain.RemoveTrackFromPlaylist(AudioMain.playlist[selectedIndex]);
                PlaylistLB_Update();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("���� �� ������.", "������", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void TrackUp_Click(object sender, EventArgs e)
        {
            if (PlaylistBox.SelectedItem != null && PlaylistBox.SelectedIndex > 0)
            {
                int selectedIndex = PlaylistBox.SelectedIndex;
                SelTrack = AudioMain.playlist[selectedIndex];
                AudioMain.playlist[selectedIndex] = AudioMain.playlist[selectedIndex - 1];
                AudioMain.playlist[selectedIndex - 1] = SelTrack;
                PlaylistLB_Update();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("���� �� ������.", "������", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void TrackDown_Click(object sender, EventArgs e)
        {
            if (PlaylistBox.SelectedItem != null && PlaylistBox.SelectedIndex > 0)
            {
                int selectedIndex = PlaylistBox.SelectedIndex;
                SelTrack = AudioMain.playlist[selectedIndex];
                AudioMain.playlist[selectedIndex] = AudioMain.playlist[selectedIndex + 1];
                AudioMain.playlist[selectedIndex + 1] = SelTrack;
                PlaylistLB_Update();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("���� �� ������.", "������", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
