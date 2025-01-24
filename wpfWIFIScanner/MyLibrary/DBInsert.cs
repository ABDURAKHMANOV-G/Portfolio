using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.Data.SQLite;

namespace MyLibrary
{
    public class DBInsert
    {
        public readonly SQLite.SQLiteConnection db;
        public DBInsert()
        {
            db = new SQLite.SQLiteConnection("myDB.db");
            db.CreateTable<Networks>();
        }
        public void InsertNetworks(string SSIDname, string SignalQual)
        {
            var network = new Networks()
            {
                SSID = SSIDname,
                SignalQuality = SignalQual,
                ScanTime = DateTime.Now
            };

            // Вставляем в таблицу
            db.Insert(network);
        }

        public void Clear()
        {
            // Очищаем таблицу от всех записей
            db.DeleteAll<Networks>();
        }

        // Метод для получения всех данных из таблицы Networks
        public string GetAllNetworks()
        {
            string connectionString = "Data Source=myDB.db;";  // Укажите путь к вашей базе данных
            string query = "SELECT * FROM Networks;";  // Запрос для получения всех данных из таблицы

            StringBuilder sb = new StringBuilder();

            using (System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand(query, conn))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    string ssid = reader["SSID"].ToString();  // Замените на имя вашего столбца
                                    string signalQuality = reader["SignalQuality"].ToString();  // Замените на имя вашего столбца
                                    string scanTime = reader["ScanTime"].ToString();

                                    try
                                    {
                                        // Преобразование времени из Ticks
                                        long ticks = long.Parse(scanTime); // Преобразуем строку в long
                                        DateTime parsedTime = new DateTime(ticks); // Создаем объект DateTime из Ticks
                                        string formattedTime = parsedTime.ToString("dd.MM.yyyy HH:mm:ss"); // Читаемый формат времени

                                        sb.AppendLine($"Scan Time: {formattedTime}, SSID: {ssid}, Signal Quality: {signalQuality}");
                                    }
                                    catch (Exception ex)
                                    {
                                        sb.AppendLine($"Scan Time: Ошибка преобразования времени ({scanTime}), SSID: {ssid}, Signal Quality: {signalQuality}");
                                    }
                                }
                            }
                            else
                            {
                                sb.AppendLine("Нет данных в базе.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    sb.AppendLine($"Ошибка: {ex.Message}");
                }
            }

            return sb.ToString(); // Возвращаем результат в виде строки
        }


    }
}
