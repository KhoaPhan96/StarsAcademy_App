using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_Encrypt_LoadFile.Feature.Models
{
    class ChiTietKhoaHocModels
    {
        public class Lessions
        {
            public string uuid { get; set; }
            public string title { get; set; }
            public string description { get; set; }
            public string image_url { get; set; }
            public string file_url { get; set; }
        }

        public class Data
        {
            public string title { get; set; }
            public string description { get; set; }
            public string uuid { get; set; }
            public string image_url { get; set; }
            public Lessions[] lessions { get; set; }
        }

        public class Data_ChiTietKH
        {
            public Data data { get; set; }
        }
    }
}
