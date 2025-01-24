using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;
using NAudio.Wave.SampleProviders;

namespace AudioPlayerLibrary
{
    public class AudioPlayerMain
    {
        private IWavePlayer waveForPlaylist;
        private AudioFileReader audioFileReader;
        public List<string> playlist;
        public int currentTrackIndex;
        private bool autoNextEnabled;
        private PlaybackState playbackState = PlaybackState.Stopped;
        private SmbPitchShiftingSampleProvider pitchShifter;
        private IWavePlayer waveForOneTrack;
        private long currentPosition = 0;
        private readonly float[] speedFactors = { 1.0f, 1.5f, 2.0f, 0.5f, 0.75f };
        private int currentSpeedIndex = 0; // Индекс текущего значения скорости
        public System.Timers.Timer timer;

        public int minutes, seconds; 

        public TimeSpan TotalDuration => audioFileReader?.TotalTime ?? TimeSpan.Zero;


        public AudioPlayerMain()
        {
            waveForPlaylist = new WaveOutEvent();
            playlist = new List<string>();
            currentTrackIndex = -1;
            autoNextEnabled = true;

            timer = new System.Timers.Timer(1000); // Таймер с интервалом 1 секунда
            timer.Elapsed += UpdateRemainingTime;
        }


        //Обновление оставшегося времени воспроизведения трека
        private void UpdateRemainingTime(object sender, ElapsedEventArgs e)
        {
            if (audioFileReader == null) return;
            TimeSpan remainingTime = TotalDuration - CurrentPosition;
            minutes = remainingTime.Minutes;
            seconds = remainingTime.Seconds;
            if (remainingTime <= TimeSpan.Zero)
            {
                timer.Stop();
                if (AutoNextEnabled)
                {   
                    Next();
                }
            }
        }

        public IReadOnlyList<string> Playlist => playlist.AsReadOnly();

        public bool AutoNextEnabled
        {
            get => autoNextEnabled;
            set => autoNextEnabled = value;
        }

        //Открытие одного файла
        public void OpenFile(string filePath)
        {
                // Освобождаем ресурсы, если что-то уже загружено
                Dispose();
                // Открываем аудиофайл
                audioFileReader = new AudioFileReader(filePath);
        }



        public TimeSpan CurrentPosition
        {
            get => audioFileReader?.CurrentTime ?? TimeSpan.Zero;
            set
            {
                if (audioFileReader != null)
                {
                    audioFileReader.CurrentTime = value;
                }
            }
        }

        public float Volume
        {
            get => audioFileReader?.Volume ?? 1f;
            set
            {
                if (audioFileReader != null)
                {
                    audioFileReader.Volume = value;
                }
            }
        }

        public string CurrentTrack => currentTrackIndex >= 0 ? playlist[currentTrackIndex] : string.Empty;

        public void LoadPlaylist(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                playlist.Clear();
                playlist.AddRange(Directory.GetFiles(folderPath, "*.mp3"));
                currentTrackIndex = playlist.Count > 0 ? 0 : -1;
            } 
        }

        public void PlaySelectedTrack(int ind)
        {
            if (ind >= 0)
            {
                Stop();
                audioFileReader = new AudioFileReader(playlist[ind]);
                waveForPlaylist.Init(audioFileReader);
                audioFileReader.Position = currentPosition;
                playbackState = PlaybackState.Paused;

                if (playbackState != PlaybackState.Playing)
                {
                    waveForPlaylist.Play();
                    playbackState = PlaybackState.Playing;
                    timer.Start(); // Запускаем таймер
                }
            }
        }

        public void Play()
        {
            if (currentTrackIndex >= 0)
            {
                Stop();
                audioFileReader = new AudioFileReader(playlist[currentTrackIndex]);
                
                audioFileReader.Position = currentPosition;
                // Создаем объект для изменения скорости
                var sampleProvider = audioFileReader.ToSampleProvider();
                pitchShifter = new SmbPitchShiftingSampleProvider(sampleProvider)
                {
                    PitchFactor = speedFactors[currentSpeedIndex] // Устанавливаем текущую скорость
                };
                waveForPlaylist.Init(pitchShifter);
                if (playbackState != PlaybackState.Playing)
                {
                    waveForPlaylist.Play();
                    playbackState = PlaybackState.Playing;
                    timer.Start(); // Запускаем таймер
                }
            }
            else
            {
                try
                {
                    
                    if (audioFileReader == null)
                    {
                        Console.WriteLine("No file loaded to play.");
                        return;
                    }

                    if (waveForOneTrack == null)
                    {
                        var sampleProvider = audioFileReader.ToSampleProvider();
                        pitchShifter = new SmbPitchShiftingSampleProvider(sampleProvider);
                        //Создаем устройство для воспроизведения звука
                        waveForOneTrack = new WaveOutEvent();
                        waveForOneTrack.Init(pitchShifter);
                    }

                    if (playbackState != PlaybackState.Playing)
                    {
                        waveForOneTrack.Play();
                        playbackState = PlaybackState.Playing;
                        timer.Start(); // Запускаем таймер
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error during playback: " + ex.Message);
                }
            }
        }

        public void Pause()
        {
            try
            {
                if (playbackState == PlaybackState.Playing)
                {
                    currentPosition = audioFileReader.Position;
                    timer.Stop(); // Останавливаем таймер

                    if (waveForOneTrack != null)
                    {
                        waveForOneTrack.Pause();
                    }
                    else if (waveForPlaylist != null)
                    {
                        waveForPlaylist.Pause();
                    }
                    playbackState = PlaybackState.Paused;
                    Console.WriteLine("Paused.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during pause: " + ex.Message);
            }
        }

        public void Stop()
        {
            //currentPosition = audioFileReader.Position;
            waveForPlaylist.Stop();
            audioFileReader?.Dispose();
            audioFileReader = null;
        }

        public void Next()
        {
            if (playlist.Count > 0)
            {
                currentPosition = 0;
                currentTrackIndex = (currentTrackIndex + 1) % playlist.Count;
                playbackState = PlaybackState.Paused;
                Play();
            }
        }

        public void Previous()
        {
            if (playlist.Count > 0)
            {
                currentPosition = 0;
                currentTrackIndex = (currentTrackIndex - 1 + playlist.Count) % playlist.Count;
                playbackState = PlaybackState.Paused;
                Play();
            }
        }

        public void CreatePlaylist(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                playlist.Clear();
                playlist.AddRange(Directory.GetFiles(folderPath, "*.mp3"));
                currentTrackIndex = playlist.Count > 0 ? 0 : -1;
            }
        }


        public void AddTrackToPlaylist(string trackPath, string playlistFolderPath)
        {
            if (File.Exists(trackPath) && Directory.Exists(playlistFolderPath))
            {
                string destinationPath = Path.Combine(playlistFolderPath, Path.GetFileName(trackPath));
                File.Copy(trackPath, destinationPath, true);
                playlist.Add(destinationPath);
            }

        }

        public void RemoveTrackFromPlaylist(string trackPath)
        {
            if (playlist.Contains(trackPath))
            {
                playlist.Remove(trackPath);
                File.Delete(trackPath);
            }
        }


        // Метод для получения длительности трека
        public TimeSpan GetTrackDuration(string trackPath)
        {
            try
            {
                var file = TagLib.File.Create(trackPath);
                return file.Properties.Duration;
            }
            catch (Exception ex)
            {
                // Обработка ошибок, если файл не поддерживает получение длительности
                Console.WriteLine($"Ошибка при получении длительности: {ex.Message}");
                return TimeSpan.Zero;
            }
        }

        // Переключение скорости
        public string ToggleSpeed()
        {
            // Переключаем индекс на следующий
            currentSpeedIndex = (currentSpeedIndex + 1) % speedFactors.Length;

            // Если воспроизведение уже идет, обновляем скорость
            if (pitchShifter != null)
            {
                pitchShifter.PitchFactor = speedFactors[currentSpeedIndex];
            }

            // Отображаем текущую скорость в интерфейсе (для удобства)
            return speedFactors[currentSpeedIndex].ToString() + "x";
        }

        private void OnPlaybackStopped(object sender, StoppedEventArgs e)
        {
            if (AutoNextEnabled && waveForPlaylist.PlaybackState == PlaybackState.Stopped)
            {
                Next();
            }
        }

        public void Dispose()
        {
            Stop();
            waveForPlaylist.Dispose();
        }
    }
}
    
