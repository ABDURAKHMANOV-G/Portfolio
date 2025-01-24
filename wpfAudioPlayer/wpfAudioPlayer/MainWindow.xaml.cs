using Microsoft.Win32;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
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




namespace wpfAudioPlayer
{
    public partial class AudioPlayerViewModel : Window
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
        private string SelTrack;
        private string trackNameListBox;

        public AudioPlayerViewModel()
        {
            InitializeComponent();
            AudioMain = new AudioPlayerMain();

            timer.Interval = TimeSpan.FromSeconds(1);
        }

        //Функции обработчики кнопок и событий
        private void BT_Click_Open(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Audio Files|*.mp3;*.wav;*.wma;*.flac",
                Title = "Select an Audio File"
            };
            if (openFileDialog.ShowDialog() == true) // Для WPF используем `true`
            {
                string filePath = openFileDialog.FileName;
                try
                {
                    AudioMain.OpenFile(filePath);
                    // Обновляем название трека в бегущей строке
                    TBFileName.Text = System.IO.Path.GetFileNameWithoutExtension(filePath);
                    // Запуск анимации
                    StartMarqueeAnimation();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error playing file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("No file selected.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void BT_Click_Play(object sender, RoutedEventArgs e)
        {
            playBtn.Visibility = Visibility.Hidden;
            pauseBtn.Visibility = Visibility.Visible;
            AudioMain?.Play();
            // Запуск таймера
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void BT_Click_Pause(object sender, RoutedEventArgs e)
        {
            plbcks = PlaybackState.Paused;
            playBtn.Visibility = Visibility.Visible;
            pauseBtn.Visibility = Visibility.Hidden;
            AudioMain?.Pause();
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        private void TrackSlider_Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (AudioMain != null && TrackSlider.IsMouseOver)
            {
                AudioMain.CurrentPosition = TimeSpan.FromSeconds(TrackSlider.Value);
            }
        }
        private void BT_Click_Volume(object sender, RoutedEventArgs e)
        {
            // Переключение видимости Popup
            if (VolumePopup.IsOpen)
            {
                VolumePopup.IsOpen = false; // Закрытие Popup
            }
            else
            {
                VolumePopup.IsOpen = true;  // Открытие Popup
            }
        }
        private void VolumeSlider_Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (AudioMain != null)
            {
                AudioMain.Volume = (float)VolumeSlider.Value;
            }
        }
        private void BT_Click_OpenPlaylist(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK && AudioMain.playlist != null)
                {
                    string selectedPath = dialog.SelectedPath;
                    AudioMain.LoadPlaylist(selectedPath);
                    //Название плейлиста
                    Playlist_name.Text = System.IO.Path.GetFileNameWithoutExtension(selectedPath);
                    if(AudioMain.playlist.Count > 0)
                    {
                        //Название играющих треков
                        TBFileName.Text = System.IO.Path.GetFileNameWithoutExtension(AudioMain.Playlist[0]);
                        StartMarqueeAnimation();
                    }
                    // Обновляем плейлист в интерфейсе
                    PlaylistLB_Update();
                }
            }
        }
        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void BT_Click_CreatePlaylist(object sender, RoutedEventArgs e)
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
        private void BT_Click_AddTrack(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Audio Files|*.mp3;*.wav;*.wma;*.flac",
                Title = "Select an Audio File"
            };
            if (openFileDialog.ShowDialog() == true) // Для WPF используем `true`
            {
                string filePath = openFileDialog.FileName;
                try
                {
                    AudioMain.AddTrackToPlaylist(filePath, actual_created_playlistPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error playing file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("No file selected.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            //Обновление ListBox'а
            PlaylistLB_Update();
        }
        private void BT_Click_Previous(object sender, RoutedEventArgs e)
        {
            AudioMain?.Previous();
            TrackSlider.Value = 0;
            //Название играющих треков
            TBFileName.Text = System.IO.Path.GetFileNameWithoutExtension(AudioMain.Playlist[AudioMain.currentTrackIndex]);
            StartMarqueeAnimation();
        }
        private void PlaylistBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point mousePosition = e.GetPosition(PlaylistBox);
            // Находим элемент, по которому кликнули
            var hitTest = VisualTreeHelper.HitTest(PlaylistBox, mousePosition);
            var clickedItem = hitTest.VisualHit;

            // Определяем индекс
            if (clickedItem != null)
            {
                var container = ItemsControl.ContainerFromElement(PlaylistBox, clickedItem) as ListBoxItem;
                if (container != null)
                {
                    if (e.ChangedButton == MouseButton.Left)
                    {
                        // Действия при клике левой кнопкой мыши
                        index = PlaylistBox.ItemContainerGenerator.IndexFromContainer(container);
                        AudioMain.PlaySelectedTrack(index);
                        //Замена кнопок Play и Pause
                        playBtn.Visibility = Visibility.Hidden;
                        pauseBtn.Visibility = Visibility.Visible;
                        //Название играющих треков
                        TBFileName.Text = System.IO.Path.GetFileNameWithoutExtension(AudioMain.Playlist[index]);
                        StartMarqueeAnimation();
                        // Запуск таймера
                        timer.Tick += Timer_Tick;
                        timer.Start();
                    }
                }    
            }
        }
        private void BT_Click_Speed(object sender, RoutedEventArgs e)
        {
            SpeedBtn.Content = AudioMain.ToggleSpeed();
        }
        private void BT_Click_Next(object sender, RoutedEventArgs e)
        {
            AudioMain?.Next();
            TrackSlider.Value = 0;
            //Название играющих треков
            TBFileName.Text = System.IO.Path.GetFileNameWithoutExtension(AudioMain.Playlist[AudioMain.currentTrackIndex]);
            StartMarqueeAnimation();
        }

        //Функции неявляющиеся обработчиками кнопок
        //Функция таймера
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (AudioMain != null && AudioMain.TotalDuration.TotalSeconds > 0)
            {
                TrackSlider.Maximum = AudioMain.TotalDuration.TotalSeconds;
                TrackSlider.Value = AudioMain.CurrentPosition.TotalSeconds;
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
                trackNameListBox = $"{ind} - {fileNameWithoutExtension}";
                ind++;
            }   
        }
        //Функция отвечающая за выделение элемента листбокса
        private ListBoxItem GetListBoxItemUnderMouse(MouseButtonEventArgs e)
        {
            // Получаем позицию мыши
            var point = e.GetPosition(PlaylistBox);

            // Ищем элемент, который находится под мышью
            var element = PlaylistBox.InputHitTest(point) as FrameworkElement;
            while (element != null && !(element is ListBoxItem))
            {
                element = VisualTreeHelper.GetParent(element) as FrameworkElement;
            }

            return element as ListBoxItem;
        }
        //Функция удаления трека
        private void RemoveTrack_Click(object sender, RoutedEventArgs e)
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
                MessageBox.Show("Трек не выбран.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void TrackUp_Click(object sender, RoutedEventArgs e)
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
                MessageBox.Show("Трек не выбран.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void TrackDown_Click(object sender, RoutedEventArgs e)
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
                MessageBox.Show("Трек не выбран.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            ChangeTheme("Dark"); 
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            ChangeTheme("Light");
        }

        private void Info_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Автор: Абдурахманов Гасан Гаджимагомедович\n" +
                "Студент Московского Политехнического Университета\n" +
                "Дата выпуска: 01.01.2025\n" +
                "Версия: 1.0.0.0",
                "О приложении",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        //private void BT_Click_ExportPlaylist(object sender, RoutedEventArgs e)
        //{
        //    // Убедимся, что в плейлисте есть файлы
        //    if (AudioMain.Playlist.Count == 0)
        //    {
        //        MessageBox.Show("Плейлист пуст. Нечего выгружать.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
        //        return;
        //    }

        //    // Открываем диалог сохранения файла
        //    Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
        //    {
        //        Title = "Сохранить плейлист как...",
        //        Filter = "ZIP Archive (*.zip)|*.zip",
        //        FileName = System.IO.Path.GetFileNameWithoutExtension(selectedPath)
        //    };

        //    if (saveFileDialog.ShowDialog() == true)
        //    {
        //        string zipPath = saveFileDialog.FileName; // Получаем путь для сохранения архива

        //        try
        //        {
        //            // Создаем временную директорию для копирования файлов
        //            string tempFolder = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "PlaylistTemp");
        //            Directory.CreateDirectory(tempFolder);

        //            // Копируем все треки из плейлиста в временную директорию
        //            foreach (var trackPath in AudioMain.Playlist)
        //            {
        //                string fileName = System.IO.Path.GetFileName(trackPath);
        //                string destinationPath = System.IO.Path.Combine(tempFolder, fileName);
        //                System.IO.File.Copy(trackPath, destinationPath, true);
        //            }

        //            // Создаем ZIP-архив из временной директории
        //            if (System.IO.File.Exists(zipPath))
        //                System.IO.File.Delete(zipPath); // Удаляем существующий файл, если он есть
        //            ZipFile.CreateFromDirectory(tempFolder, zipPath);

        //            // Удаляем временную директорию
        //            Directory.Delete(tempFolder, true);

        //            // Оповещаем пользователя об успешном создании архива
        //            MessageBox.Show($"Плейлист успешно выгружен в архив: {zipPath}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        //        }
        //        catch (Exception ex)
        //        {
        //            // Если произошла ошибка, сообщаем пользователю
        //            MessageBox.Show($"Ошибка при создании архива: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //    }
        //}



        // Метод для смены темы

        private void BT_Click_ExportPlaylist(object sender, RoutedEventArgs e)
        {
            // Убедимся, что в плейлисте есть файлы
            if (AudioMain.Playlist.Count == 0)
            {
                MessageBox.Show("Плейлист пуст. Нечего выгружать.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                    MessageBox.Show($"Плейлист успешно выгружен в архив: {zipPath}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    // Если произошла ошибка, сообщаем пользователю
                    MessageBox.Show($"Ошибка при создании архива: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ChangeTheme(string themeName)
        {
            switch (themeName)
            {
                case "Light":
                    var uri = new Uri("Themes/LightTheme.xaml", UriKind.Relative);
                    ResourceDictionary resourceDictionary = Application.LoadComponent(uri) as ResourceDictionary;
                    Application.Current.Resources.Clear();
                    Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
                    break;
                case "Dark":
                    uri = new Uri("Themes/DarkTheme.xaml", UriKind.Relative);
                    resourceDictionary = Application.LoadComponent(uri) as ResourceDictionary;
                    Application.Current.Resources.Clear();
                    Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
                    break;
            }
            
            
        }
        //Анимация бегущей строки
        private void StartMarqueeAnimation()
        {
            TBFileName.Dispatcher.InvokeAsync(() =>
            {
                double textWidth = TBFileName.ActualWidth;
                double containerWidth = 150;
                var marqueeAnimation = new DoubleAnimation
                {
                    From = containerWidth,
                    To = -textWidth,
                    Duration = TimeSpan.FromSeconds(5), // Длительность анимации
                    RepeatBehavior = RepeatBehavior.Forever // Повторять анимацию
                };
                MarqueeTransform.BeginAnimation(TranslateTransform.XProperty, marqueeAnimation);
            });
        }
    }
}