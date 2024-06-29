using DevExpress.XtraBars.Ribbon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars.Ribbon.Gallery;
using DevExpress.Utils;
using System.Net.Http;
using Newtonsoft.Json;

namespace Demo_Encrypt_LoadFile.Feature
{
    public partial class frm_TestGallery : Form
    {
        #region #region Variables  
        #region API
        public bool isStatus = false;

        private const string ApiUrl = "https://knsngoisao.edu.vn/api/v1/me/check?api_token=";
        private string api_Key = "37741d319ba8deb7";
        #endregion

        #region Danh sách bài giảng, khóa học
        public DataTable tbl_Courses = new DataTable();
        public DataTable tbl_Lessions = new DataTable();
        #endregion
        #endregion
        public frm_TestGallery()
        {
            InitializeComponent();
        }

        private async Task CallApi(string api_Key)
        {
            try
            {
                // Khởi tạo HttpClient
                using (HttpClient client = new HttpClient())
                {
                    // Gọi API
                    HttpResponseMessage response = await client.GetAsync(ApiUrl + api_Key);

                    // Xử lý kết quả
                    if (response.IsSuccessStatusCode)
                    {
                        // Đọc dữ liệu từ phản hồi
                        string responseBody = await response.Content.ReadAsStringAsync();

                        // Hiển thị kết quả trong một hộp thoại thông báo

                        if (response.StatusCode.ToString() == "OK")
                            isStatus = true;
                        else
                            isStatus = false;

                        //MessageBox.Show(response.StatusCode.ToString(), "API Response", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        //return response.StatusCode.ToString();
                    }
                    else
                    {
                        //Xử lý lỗi nếu có
                        MessageBox.Show("Error: " + response.StatusCode, "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu có
                // MessageBox.Show("Exception: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public async void DanhSachBaiGiang_KhoaHoc()
        {
            try
            {
                await CallApi(api_Key);

                if (isStatus == false)
                {
                    MessageBox.Show("Tài khoản không được xác thực", "API Response", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    //Tạo bảng chứa danh sách các khóa học   
                    //DataTable tbl_Courses = new DataTable();
                    tbl_Courses.Columns.Add("title", typeof(string));
                    tbl_Courses.Columns.Add("description", typeof(string));
                    tbl_Courses.Columns.Add("uuid", typeof(string));
                    tbl_Courses.Columns.Add("image_url", typeof(string));

                    //Tạo bảng chứa danh sách các bài giảng của khóa học
                    //DataTable tbl_Lessions = new DataTable();
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }

        }

        private async void frm_TestGallery_Load(object sender, EventArgs e)
        {

            #region Danh sách khóa học bài giảng
            try
            {
                await CallApi(api_Key);

                if (isStatus == false)
                {
                    MessageBox.Show("Tài khoản không được xác thực", "API Response", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    //Tạo bảng chứa danh sách các khóa học   
                    //DataTable tbl_Courses = new DataTable();
                    tbl_Courses.Columns.Add("title", typeof(string));
                    tbl_Courses.Columns.Add("description", typeof(string));
                    tbl_Courses.Columns.Add("uuid", typeof(string));
                    tbl_Courses.Columns.Add("image_url", typeof(string));

                    //Tạo bảng chứa danh sách các bài giảng của khóa học
                    //DataTable tbl_Lessions = new DataTable();
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


                    #endregion

                    GalleryItemGroup group1 = new GalleryItemGroup();
                    group1.Caption = "Cars";
                    galleryControl1.Gallery.Groups.Add(group1);

                    GalleryItemGroup group2 = new GalleryItemGroup();
                    group2.Caption = "People";
                    galleryControl1.Gallery.Groups.Add(group2);

                    Image im1 = Image.FromFile(@"C:\Users\bachd\OneDrive\Máy tính\test\chihiro014.jpg");
                    Image im2 = Image.FromFile(@"C:\Users\bachd\OneDrive\Máy tính\test\pexels-evgeny-tchebotarev-2187422.jpg");
                    Image im3 = Image.FromFile(@"C:\Users\bachd\OneDrive\Máy tính\test\pexels-olya-kobruseva-4679116.jpg");
                    Image im4 = Image.FromFile(@"C:\Users\bachd\OneDrive\Máy tính\test\thumb-1920-947353.JPG");
                    Image im5 = Image.FromFile(@"C:\Users\bachd\OneDrive\Máy tính\test\wp4761182.jpg");

                    group1.Items.Add(new GalleryItem(im1, "BMW", ""));
                    group1.Items.Add(new GalleryItem(im2, "Ford", ""));

                    group2.Items.Add(new GalleryItem(im3, "Mercedec-Benz", ""));
                    group2.Items.Add(new GalleryItem(im4, "Anne Dodsworth", ""));
                    group2.Items.Add(new GalleryItem(im5, "The Khoa", ""));

                    //galleryControl1.Gallery.ItemSize = new Size(200, 200);
                    galleryControl1.Gallery.ImageSize = new System.Drawing.Size(300, 300);
                    galleryControl1.Gallery.ItemImageLayout = DevExpress.Utils.Drawing.ImageLayoutMode.Squeeze;




                    // Thêm sự kiện để xử lý khi một mục trong GalleryControl được chọn
                    // galleryControl.Gallery.ItemClick += Gallery_ItemClick;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void galleryControl1_Gallery_ContextButtonCustomize(object sender, GalleryContextButtonCustomizeEventArgs e)
        {
            GalleryControlGallery gallery = sender as GalleryControlGallery;
            switch (e.Item.Name)
            {
                //case "itemRating":
                //    RatingContextButton rating = e.Item as RatingContextButton;
                //    if (!RatingValues.ContainsKey(e.GalleryItem))
                //    {
                //        rating.Rating = rand.Next(0, 6);
                //        RatingValues.Add(e.GalleryItem, (int)rating.Rating);
                //    }
                //    else rating.Rating = RatingValues[e.GalleryItem];
                //    break;
                case "itemCheck":
                    CheckContextButton check = e.Item as CheckContextButton;
                    //if (MarkedItems.Contains(e.GalleryItem))
                    check.Checked = true;
                    break;
                case "itemInfo":
                    ContextButton btn = e.Item as ContextButton;
                    btn.Caption = e.GalleryItem.Caption;
                    break;
                default:
                    break;
            }
        }

        private void galleryControl1_Gallery_ContextButtonClick(object sender, ContextItemClickEventArgs e)
        {
            //GalleryControlGallery gallery = sender as GalleryControlGallery;
            //switch (e.Item.Name)
            //{
            //    //case "itemRating":
            //    //    RatingContextButton rating = e.Item as RatingContextButton;
            //    //    if (!RatingValues.ContainsKey(e.GalleryItem))
            //    //    {
            //    //        rating.Rating = rand.Next(0, 6);
            //    //        RatingValues.Add(e.GalleryItem, (int)rating.Rating);
            //    //    }
            //    //    else rating.Rating = RatingValues[e.GalleryItem];
            //    //    break;
            //    case "itemCheck":
            //        CheckContextButton check = e.Item as CheckContextButton;
            //        //if (MarkedItems.Contains(e.GalleryItem))
            //        check.Checked = true;
            //        break;
            //    //case "itemInfo":
            //    //    ContextButton btn = e.Item as ContextButton;
            //    //    btn.Caption = e.GalleryItem.Caption;
            //    //    break;
            //    default:
            //        break;
            //}

            

        }
    }
}
