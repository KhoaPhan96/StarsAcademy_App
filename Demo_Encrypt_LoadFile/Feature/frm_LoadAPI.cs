using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo_Encrypt_LoadFile.Feature
{
    public partial class frm_LoadAPI : Form
    {
        private const string ApiUrl = "https://knsngoisao.edu.vn/api/v1/me/check?api_token=";
        private string api_Key = "37741d319ba8deb7";

        public frm_LoadAPI()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //Tạo bảng chứa danh sách các khóa học   
                DataTable tbl_Courses = new DataTable();
                tbl_Courses.Columns.Add("title", typeof(string));
                tbl_Courses.Columns.Add("description", typeof(string));
                tbl_Courses.Columns.Add("uuid", typeof(string));
                tbl_Courses.Columns.Add("image_url", typeof(string));

                //Tạo bảng chứa danh sách các bài giảng của khóa học
                DataTable tbl_Lessions = new DataTable();
                tbl_Lessions.Columns.Add("uuid_Course", typeof(string));
                tbl_Lessions.Columns.Add("uuid", typeof(string));
                tbl_Lessions.Columns.Add("title", typeof(string));
                tbl_Lessions.Columns.Add("description", typeof(string));
                tbl_Lessions.Columns.Add("image_url", typeof(string));
                tbl_Lessions.Columns.Add("file_url", typeof(string));

                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = "https://knsngoisao.edu.vn/api/v1/me/courses?api_token=" + api_Key;
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    //Danh sách các khóa học
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonData = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<ApiResponse>(jsonData);

                        foreach (var item in result.data)
                        {

                            tbl_Courses.Rows.Add(new object[] { item.title, item.description, item.uuid, item.image_url });

                            pictureEdit1.Image = new Bitmap(await client.GetStreamAsync(item.image_url));


                            //Danh sách các bài giảng
                            foreach (var lesson in item.lessions)
                            {
                                tbl_Lessions.Rows.Add(new object[] { item.uuid, lesson.uuid, lesson.title, lesson.description, lesson.image_url, lesson.file_url });
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Failed to retrieve data from API.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void frm_LoadAPI_Load(object sender, EventArgs e)
        {

        }
    }
}
