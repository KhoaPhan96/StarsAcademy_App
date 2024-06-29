using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Demo_Encrypt_LoadFile.Feature
{
    class ChiTietKhoaHoc
    {

        public class Lesson
        {
            public string uuid { get; set; }
            public string title { get; set; }
            public string description { get; set; }
            public string image_url { get; set; }
            public string file_url { get; set; }
        }

        public class CourseData
        {
            public string title { get; set; }
            public string description { get; set; }
            public string uuid { get; set; }
            public string image_url { get; set; }
            public Lesson[] lessions { get; set; }
        }

        public class aa
        {
            public CourseData Data { set; get; }
        }

        public async void LoadData()
        {
            string apiUrl = "https://knsngoisao.edu.vn/api/v1/me/courses/5ddc57eb97b2b0fe?api_token=37741d319ba8deb7";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();

                        // Deserialize JSON thành đối tượng CourseData
                        var courseData = JsonConvert.DeserializeObject<aa>(jsonResponse);

                        //// Đặt dữ liệu vào DataGridView
                        //dataGridView1.AutoGenerateColumns = false;
                        //dataGridView1.DataSource = courseData.Lessons;
                    }
                    else
                    {
                        MessageBox.Show("Không thể kết nối đến API");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
                }
            }
        }
    }
}
