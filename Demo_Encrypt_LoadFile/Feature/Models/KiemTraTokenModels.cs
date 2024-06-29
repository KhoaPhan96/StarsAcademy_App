using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_Encrypt_LoadFile.Feature.Models
{
    class KiemTraTokenModels
    {
        public class Data
        {
            public string uuid { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public string customer_type { get; set; }
            public string avatar_url { get; set; }
            public API_token api_Token { get; set; }
        }

        public class API_token
        {
            public string token { get; set; }
            public DateTime expired_at { get; set; }
            public bool is_active { get; set; }
            public bool is_valid { get; set; }
        }
    }
}
