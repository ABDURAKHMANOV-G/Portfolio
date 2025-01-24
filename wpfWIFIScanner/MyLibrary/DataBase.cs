using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary
{
    public class Networks
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string SSID { get; set; }
        public string SignalQuality { get; set; }
        public DateTime ScanTime { get; set; }
    }
}
