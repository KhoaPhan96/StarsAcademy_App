using Demo_Encrypt_LoadFile.Feature.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo_Encrypt_LoadFile.Feature.API
{   
    public class CallAPI
    {
        #region Variables   

        public bool isStatus = false;

        private const string ApiUrl = "https://knsngoisao.edu.vn/api/v1/me/check?api_token=";
        private string api_Key = "37741d319ba8deb7";
        
        #region Danh sách bài giảng, khóa học
        public DataTable tbl_Courses = new DataTable();
        public DataTable tbl_Lessions = new DataTable();
        #endregion

        #endregion

        #region Functions

        //public async Task CallApi(string api_Key)
        //{
        //    try
        //    {
        //        // Khởi tạo HttpClient
        //        using (HttpClient client = new HttpClient())
        //        {
        //            // Gọi API
        //            HttpResponseMessage response = await client.GetAsync(ApiUrl + api_Key);

        //            // Xử lý kết quả
        //            if (response.IsSuccessStatusCode)
        //            {
        //                // Đọc dữ liệu từ phản hồi
        //                string responseBody = await response.Content.ReadAsStringAsync();

        //                // Hiển thị kết quả trong một hộp thoại thông báo

        //                if (response.StatusCode.ToString() == "OK")
        //                    isStatus = true;
        //                else
        //                    isStatus = false;


        //                MessageBox.Show(response.StatusCode.ToString(), "API Response", MessageBoxButton.OK, MessageBoxImage.Information);

        //                //return response.StatusCode.ToString();
        //            }
        //            else
        //            {
        //                //Xử lý lỗi nếu có
        //                MessageBox.Show("Error: " + response.StatusCode, "API Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Xử lý ngoại lệ nếu có
        //       // MessageBox.Show("Exception: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        public static async Task<bool> CallApi(string ApiUrl, string apiKey)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(ApiUrl + apiKey);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        // Xử lý dữ liệu từ responseBody nếu cần

                        return true; // hoặc trả về giá trị phù hợp tùy vào kết quả nhận được từ API
                    }
                    else
                    {
                        return false; // hoặc xử lý lỗi và trả về giá trị phù hợp
                    }
                }
            }
            catch (Exception ex)
            {
                return false; // hoặc xử lý ngoại lệ và trả về giá trị phù hợp
            }
        }

        public static async Task<byte[]> DownloadImageAsync(string imageUrl)
        {

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(imageUrl))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ////using (Stream stream = await response.Content.ReadAsStreamAsync())
                        //using (Stream stream = await response.Content.ReadAsByteArrayAsync())
                        //{
                        //    return Image.FromStream(stream);
                        //}

                        return await response.Content.ReadAsByteArrayAsync();
                    }
                    else
                    {
                        throw new Exception("Không thể tải ảnh từ URL: " + imageUrl);
                    }
                }
            }
        }
   
        public static async Task DownloadFileAsync(string fileUrl, string savePath)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(fileUrl);

                if (response.IsSuccessStatusCode)
                {
                    using (var stream = await response.Content.ReadAsStreamAsync())
                    using (var fileStream = new System.IO.FileStream(savePath, System.IO.FileMode.Create))
                    {
                        await stream.CopyToAsync(fileStream);
                    }

                    Console.WriteLine("Tải tệp thành công!");
                }
                else
                {
                    Console.WriteLine("Lỗi khi tải tệp: " + response.StatusCode);
                    MessageBox.Show("Lỗi khi tải tệp: " + response.StatusCode, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public async static Task<DataTable> CheckTokenAPI(string apiUrl)
        {
            DataTable dt_TokenAPI = new DataTable("TokenAPI");

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string jsonData = await response.Content.ReadAsStringAsync();

                    // Định nghĩa một JsonSerializerSettings với định dạng ngày tháng được sử dụng trong JSON
                    var settings = new JsonSerializerSettings
                    {
                        DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffZ",
                        Culture = CultureInfo.InvariantCulture
                    };

                    var result = JsonConvert.DeserializeObject<API_Token_Models.Data_TokenAPI>(jsonData, settings);

                    dt_TokenAPI.Columns.Add("id", typeof(string));
                    dt_TokenAPI.Columns.Add("uuid", typeof(string));
                    dt_TokenAPI.Columns.Add("name", typeof(string));
                    dt_TokenAPI.Columns.Add("description", typeof(string));
                    dt_TokenAPI.Columns.Add("customer_type", typeof(string));
                    dt_TokenAPI.Columns.Add("avatar_url", typeof(string));
                    dt_TokenAPI.Columns.Add("token", typeof(string));
                    dt_TokenAPI.Columns.Add("expired_at", typeof(string));
                    dt_TokenAPI.Columns.Add("is_active", typeof(bool));
                    dt_TokenAPI.Columns.Add("is_valid", typeof(bool));

                    dt_TokenAPI.Columns.Add("phone", typeof(string));
                    dt_TokenAPI.Columns.Add("email", typeof(string));
                    dt_TokenAPI.Columns.Add("address", typeof(string));

                    dt_TokenAPI.Rows.Add(new object[] { result.data.id,
                                                        result.data.uuid,
                                                        result.data.name,
                                                        result.data.description,
                                                        result.data.customer_type,
                                                        result.data.avatar_url,
                                                        result.data.api_token.token,
                                                        result.data.api_token.expired_at.ToString("dd-MM-yyyy HH:mm:ss tt"),
                                                        result.data.api_token.is_active,
                                                        result.data.api_token.is_valid,

                                                        result.data.phone,
                                                        result.data.email,
                                                        result.data.address,
                                                          });


                }
            }    
             
            return dt_TokenAPI;
        }

        #endregion
    }
}
