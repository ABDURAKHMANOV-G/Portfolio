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
            if (openFileDialog.ShowDialog() == DialogResult.OK) // Для WinForms используем DialogResult.OK
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
            //Обновление ListBox'а
            PlaylistLB_Update();
        }

        private void Info_Btn_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Автор: Абдурахманов Гасан Гаджимагомедович\n" +
                "Студент Московского Политехнического Университета\n" +
                "Дата выпуска: 01.01.2025\n" +
                "Версия: 1.0.0.0",
                "О приложении",
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
            if (openFileDialog.ShowDialog() == DialogResult.OK) // Для WPF используем `true`
            {
                string filePath = openFileDialog.FileName;
                try
                {
                    AudioMain.OpenFile(filePath);
                    // Обновляем название трека в бегущей строке
                    TBFileName.Text = System.IO.Path.GetFileNameWithoutExtension(filePath);
                    // Запуск анимации
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
                    //Название плейлиста
                    Playlist_name.Text = System.IO.Path.GetFileNameWithoutExtension(selectedPath);
                    if (AudioMain.playlist.Count > 0)
                    {
                        //Название играющих треков
                        TBFileName.Text = System.IO.Path.GetFileNameWithoutExtension(AudioMain.Playlist[0]);
                        //StartMarqueeAnimation();
                    }
                    // Обновляем плейлист в интерфейсе
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
                    //Название плейлиста
                    Playlist_name.Text = System.IO.Path.GetFileNameWithoutExtension(selectedPath);
                    //Актулаьный путь к папке
                    actual_created_playlistPath = selectedPath;
                    //Обновление ListBox'а
                    PlaylistLB_Update();
                }
            }
        }

        private void ExportPlaylist_Btn_Click(object sender, EventArgs e)
        {
            // Убедимся, что в плейлисте есть файлы
            if (AudioMain.Playlist.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Плейлист пуст. Нечего выгружать.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Открываем диалог сохранения файла
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Title = "Сохранить плейлист как...",
                Filter = "ZIP Archive (*.zip)|*.zip",
                FileName = System.IO.Path.GetFileNameWithoutExtension(selectedPath) // Имя архива как у основной папки
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string zipPath = saveFileDialog.FileName; // Путь для сохранения архива

                try
                {
                    // Создаем временную папку для копии плейлиста
                    string tempFolder = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "PlaylistTemp");
                    Directory.CreateDirectory(tempFolder);

                    // Копируем содержимое исходного плейлиста в временную папку
                    foreach (var trackPath in AudioMain.Playlist)
                    {
                        string fileName = System.IO.Path.GetFileName(trackPath);
                        string destinationPath = System.IO.Path.Combine(tempFolder, fileName);
                        System.IO.File.Copy(trackPath, destinationPath, true);
                    }

                    // Переименование треков в дублированной папке на основе списка
                    int ind = 1;
                    foreach (var trackPath in AudioMain.Playlist)
                    {
                        // Получаем старое имя файла (с расширением)
                        string oldFileName = System.IO.Path.GetFileName(trackPath);

                        // Формируем новое имя файла (индекс + имя без длительности)
                        string newFileName = $"{ind} - {System.IO.Path.GetFileNameWithoutExtension(trackPath)}{System.IO.Path.GetExtension(trackPath)}";

                        // Формируем полный путь к старому файлу в временной папке
                        string oldFilePath = System.IO.Path.Combine(tempFolder, oldFileName);

                        // Формируем полный путь к новому файлу в временной папке
                        string newFilePath = System.IO.Path.Combine(tempFolder, newFileName);

                        // Переименовываем файл
                        System.IO.File.Move(oldFilePath, newFilePath);

                        ind++;
                    }

                    // Создаем ZIP-архив из временной папки
                    if (System.IO.File.Exists(zipPath))
                        System.IO.File.Delete(zipPath); // Удаляем существующий файл, если он есть
                    ZipFile.CreateFromDirectory(tempFolder, zipPath);

                    // Удаляем временную папку
                    Directory.Delete(tempFolder, true);

                    // Сообщаем пользователю об успешном создании архива
                    System.Windows.Forms.MessageBox.Show($"Плейлист успешно выгружен в архив: {zipPath}", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    // Если произошла ошибка, сообщаем пользователю
                    System.Windows.Forms.MessageBox.Show($"Ошибка при создании архива: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Play_Btn_Click(object sender, EventArgs e)
        {
            PlayBtn.Visible = false;
            PauseBtn.Visible = true;
            AudioMain?.Play();
            // Запуск таймера
            timer.Tick += Timer_Tick;
            timer.Start();
        }


        //Функции неявляющиеся обработчиками кнопок
        //Функция таймера
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (AudioMain != null && AudioMain.TotalDuration.TotalSeconds > 0)
            {
                TrackSlider.Maximum = (int)(AudioMain.TotalDuration.TotalSeconds);
                TrackSlider.Value = (int)(AudioMain.CurrentPosition.TotalSeconds);
            }
            TimerText.Text = $"{AudioMain.minutes}:{AudioMain.seconds}";
        }
        //Функция обновления плейлиста
        public void PlaylistLB_Update()
        {
            int ind = 1;
            PlaylistBox.Items.Clear();
            foreach (var track in AudioMain.Playlist)
            {
                fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(track);

                // Добавляем длительность трека в название
                TimeSpan trackDuration = AudioMain.GetTrackDuration(track);
                string formattedDuration = trackDuration.ToString(@"mm\:ss");
                PlaylistBox.Items.Add($"{ind} - {fileNameWithoutExtension} - {formattedDuration}");

                ind++;
            }
        }
        private void PlaylistBox_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // Проверяем, что клик был левой кнопкой мыши
            if (e.Button == MouseButtons.Left)
            {
                // Находим индекс элемента, по которому был клик
                int index = PlaylistBox.IndexFromPoint(e.Location);

                if (index != System.Windows.Forms.ListBox.NoMatches) // Проверяем, что индекс валиден
                {
                    // Действия при выборе элемента
                    AudioMain.PlaySelectedTrack(index);

                    // Замена кнопок Play и Pause
                    PlayBtn.Visible = false;
                    PauseBtn.Visible = true;

                    // Отображаем название трека
                    TBFileName.Text = System.IO.Path.GetFileNameWithoutExtension(AudioMain.Playlist[index]);

                    // Если нужно, запускаем анимацию
                    // StartMarqueeAnimation();

                    // Запуск таймера
                    timer.Tick += Timer_Tick;
                    timer.Start();
                }
            }
            else
            {
                // Находим индекс элемента, по которому был клик
                int index = PlaylistBox.IndexFromPoint(e.Location);

                if (index != System.Windows.Forms.ListBox.NoMatches) // Проверяем, что клик на элементе
                {
                    // Выделяем элемент, по которому был произведён клик
                    PlaylistBox.SelectedIndex = index;

                    // Показываем контекстное меню
                    PlaylistContextMenuStrip.Show(PlaylistBox, e.Location);
                }
            }
        }



        private bool isDragging = false; // Флаг для отслеживания, когда пользователь взаимодействует с трекбаром
        private string SelTrack;

        private void TrackBar_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            isDragging = true; // Устанавливаем флаг, когда пользователь начинает перетаскивать
        }

        private void TrackBar_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (isDragging && AudioMain != null)
            {
                // Устанавливаем позицию трека в соответствии со значением трекбара
                AudioMain.CurrentPosition = TimeSpan.FromSeconds(TrackSlider.Value);
            }
            isDragging = false; // Сбрасываем флаг после завершения перетаскивания
        }

        private void TrackBar_Scroll(object sender, EventArgs e)
        {
            if (isDragging && AudioMain != null)
            {
                // Обновляем текущую позицию при прокрутке трекбара
                AudioMain.CurrentPosition = TimeSpan.FromSeconds(TrackSlider.Value);
            }
        }


        private void Previous_Btn_Click(object sender, EventArgs e)
        {
            AudioMain?.Previous();
            TrackSlider.Value = 0;
            //Название играющих треков
            TBFileName.Text = System.IO.Path.GetFileNameWithoutExtension(AudioMain.Playlist[AudioMain.currentTrackIndex]);
            //StartMarqueeAnimation();
        }

        private void Next_Btn_Click(object sender, EventArgs e)
        {
            AudioMain?.Next();
            TrackSlider.Value = 0;
            //Название играющих треков
            TBFileName.Text = System.IO.Path.GetFileNameWithoutExtension(AudioMain.Playlist[AudioMain.currentTrackIndex]);
            //StartMarqueeAnimation();
        }

        private void SpeedCtrl_Btn_Click(object sender, EventArgs e)
        {
            SpeedBtn.Text = AudioMain.ToggleSpeed();
        }

        private void Volume_Btn_Click(object sender, EventArgs e)
        {
            // Переключаем видимость трекбара
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
            // Устанавливаем громкость через AudioMain
            AudioMain.Volume = VolumeSlider.Value;
        }

        private void RemoveTrack_Click(object sender, EventArgs e)
        {
            // Проверяем, есть ли выбранный элемент
            if (PlaylistBox.SelectedItem != null)
            {
                int selectedIndex = PlaylistBox.SelectedIndex;              // Получаем индекс

                //Удаляем из папки
                AudioMain.RemoveTrackFromPlaylist(AudioMain.playlist[selectedIndex]);
                PlaylistLB_Update();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Трек не выбран.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                System.Windows.Forms.MessageBox.Show("Трек не выбран.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                System.Windows.Forms.MessageBox.Show("Трек не выбран.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
