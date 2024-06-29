using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_Encrypt_LoadFile.Feature.Models
{
    class API_Token_Models
    {
        public class Data
        {
            public string id { get; set; }
            public string uuid { get; set; }
            public string name { get; set; }
            public string avatar_url { get; set; }
            public string description { get; set; }
            public string phone { get; set; }
            public string email { get; set; }
            public string address { get; set; }
            public string customer_type { get; set; }         
            public ApiToken api_token { get; set; }
        }

        public class ApiToken
        {
            public string id { get; set; }
            public string token { get; set; }
            public DateTime expired_at { get; set; }
            public bool is_active { get; set; }
            public bool is_valid { get; set; }
        }

        public class Data_TokenAPI
        {
            public Data data { get; set; }
        }
    }
}
