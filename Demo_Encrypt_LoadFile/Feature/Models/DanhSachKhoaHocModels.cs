using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_Encrypt_LoadFile.Feature.Models
{
    class DanhSachKhoaHocModels
    {
        public class Lessions
        {
            public string id { get; set; }
            public string uuid { get; set; }
            public string title { get; set; }
            public string description { get; set; }
            public string image_url { get; set; }
            public string meta_image_url { get; set; }

            public string file { get; set; }
        }

        public class Data
        {
            public string id { get; set; }
            public string title { get; set; }
            public string description { get; set; }
            public string uuid { get; set; }
            public string image_url { get; set; }
            public string meta_image_url { get; set; }
            public Lessions[] lessions { get; set; }
        }    

        public class DSData
        {
            public Data[] data { get; set; }
        }
    }
}
