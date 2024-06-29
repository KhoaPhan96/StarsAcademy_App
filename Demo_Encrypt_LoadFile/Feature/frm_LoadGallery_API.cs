using Demo_Encrypt_LoadFile.Feature.Models;
using DevExpress.Utils;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraBars.Ribbon.Gallery;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo_Encrypt_LoadFile.Feature
{
    public partial class frm_LoadGallery_API : Form
    {
        #region API
        public bool isStatus = false;

        private const string ApiUrl = "https://knsngoisao.edu.vn/api/v1/me/check?api_token=";
        private string api_Key = "37741d319ba8deb7";
        #endregion

        #region Danh sách bài giảng, khóa học
        public DataTable tbl_Courses = new DataTable();
        public DataTable tbl_Lessions = new DataTable();
        public DataTable dt_Lessions = new DataTable();
        #endregion

        public string key = "chuacopass";

        public frm_LoadGallery_API()
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


        private async void frm_LoadGallery_API_Load(object sender, EventArgs e)
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

                    //Danh sách khóa học bài giảng XML
                    dt_Lessions.Columns.Add("uuid_KhoaHoc", typeof(string));
                    dt_Lessions.Columns.Add("title_KhoaHoc", typeof(string));
                    dt_Lessions.Columns.Add("description_KhoaHoc", typeof(string));
                    dt_Lessions.Columns.Add("image_url_KhoaHoc", typeof(string));
                    dt_Lessions.Columns.Add("image_Path_KhoaHoc", typeof(string));
                    dt_Lessions.Columns.Add("uuid", typeof(string));
                    dt_Lessions.Columns.Add("title", typeof(string));
                    dt_Lessions.Columns.Add("description", typeof(string));
                    dt_Lessions.Columns.Add("image_url", typeof(string));
                    dt_Lessions.Columns.Add("file_url", typeof(string));
                    dt_Lessions.Columns.Add("image_url_Path", typeof(string));
                    dt_Lessions.Columns.Add("file_url_Path", typeof(string));

                    dt_Lessions.TableName = "Products";

                    using (HttpClient client = new HttpClient())
                    {
                        string apiUrl = "https://knsngoisao.edu.vn/api/v1/me/courses?api_token=" + api_Key;
                        HttpResponseMessage response = await client.GetAsync(apiUrl);

                        //Danh sách các khóa học
                        if (response.IsSuccessStatusCode)
                        {
                            string jsonData = await response.Content.ReadAsStringAsync();
                            //var result = JsonConvert.DeserializeObject<ApiResponse>(jsonData);
                            var result = JsonConvert.DeserializeObject<DanhSachKhoaHocModels.DSData>(jsonData);

                            foreach (var item in result.data)
                                {

                                tbl_Courses.Rows.Add(new object[] { item.title, item.description, item.uuid, item.image_url });

                                //Danh sách các bài giảng
                                foreach (var lesson in item.lessions)
                                {
                                    tbl_Lessions.Rows.Add(new object[] { item.uuid, lesson.uuid, lesson.title, lesson.description, lesson.image_url, lesson.file });

                                    
                                    //Datatable để đẩy vào file XML
                                    dt_Lessions.Rows.Add(new object[] { item.uuid, item.title, item.description, item.image_url
                                                                        , Application.StartupPath + @"\File\" + Encrypt_FileName.EncodeFileNameInPath(Path.GetFileName(item.image_url) + ".enc")
                                                                        , lesson.uuid, lesson.title, lesson.description, lesson.image_url, lesson.file
                                                                        , Application.StartupPath + @"\File\" + Encrypt_FileName.EncodeFileNameInPath(Path.GetFileName(lesson.image_url) + ".enc")
                                                                        , Application.StartupPath + @"\File\" + Encrypt_FileName.EncodeFileNameInPath(Path.GetFileName(lesson.file) + ".enc")
                                                                        });
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Failed to retrieve data from API.");
                        }
                    }
                
                    foreach (DataRow dr in tbl_Courses.Rows)
                    {
                        GalleryItemGroup group = new GalleryItemGroup();
                        group.Caption = dr["title"].ToString();
                        galleryControl1.Gallery.Groups.Add(group);

                        DataTable tbl_Lessions_Temp = tbl_Lessions.Clone();
                        tbl_Lessions.Select(String.Format("uuid_Course = '{0}'", dr["uuid"])).CopyToDataTable(tbl_Lessions_Temp, LoadOption.PreserveChanges);

                        foreach(DataRow dr1 in tbl_Lessions_Temp.Rows)
                        {
                            Image img = await DownloadImageAsync(dr1["image_url"].ToString());

                            //API chi tiết bài giảng
                           // string API_ChiTietBaiGiang = "https://knsngoisao.edu.vn/api/v1/me/lessions/" + dr1["uuid"].ToString() + "?api_token=" + api_Key;
                            string API_ChiTietKhoaHoc = "https://knsngoisao.edu.vn/api/v1/me/courses/" + dr1["uuid_Course"].ToString() + "?api_token=" + api_Key;

                            //group.Items.Add(new GalleryItem(img, dr1["title"].ToString(), dr1["file_url"].ToString()));
                            group.Items.Add(new GalleryItem(img, dr1["title"].ToString(), API_ChiTietKhoaHoc));

                        }    
                    }
                    
                    galleryControl1.Gallery.ImageSize = new System.Drawing.Size(300, 300);
                    galleryControl1.Gallery.ItemImageLayout = DevExpress.Utils.Drawing.ImageLayoutMode.Squeeze;


                    // Thêm sự kiện để xử lý khi một mục trong GalleryControl được chọn
                    galleryControl1.Gallery.ItemClick += Gallery_ItemClick;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private async void Gallery_ItemClick(object sender, GalleryItemClickEventArgs e)
        {
            try
            {
                #region Kiểm tra token
                #endregion

                #region Thêm vào XML
                string filePath_XML = Path.Combine(Application.StartupPath + @"\File\", "data.xml");
                dt_Lessions.WriteXml(filePath_XML);
                #endregion

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(e.Item.Description);

                    //Danh sách các khóa học
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonData = await response.Content.ReadAsStringAsync();
                        //var result = JsonConvert.DeserializeObject<ApiResponse>(jsonData);
                        var result = JsonConvert.DeserializeObject<ChiTietKhoaHocModels.Data_ChiTietKH>(jsonData);

                        string folderName = result.data.title;

                        //Tạo thư mục khóa học (chứa các bài giảng)
                        //string folderPath = Application.StartupPath + @"\File\" + folderName;
                        string folderPath = Application.StartupPath + @"\File\";

                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                            MessageBox.Show("Folder đã được tạo thành công!");
                        }    

                        //Tải file ảnh của khóa học
                        //foreach(var item_Course in result.data.image_url)
                        //{
                            //Link ảnh
                            string image_url_Course = result.data.image_url;
                            //Lấy tên file + phần mở rộng
                            string fileName_image_url_Course = Path.GetFileName(image_url_Course);
                            //Nơi lưu ảnh
                            string image_url_Course_Path = Application.StartupPath + @"\File\" + fileName_image_url_Course;
                            //Tên sau khi mã hóa
                            string image_url_Course_Path_encode = Encrypt_FileName.EncodeFileNameInPath(fileName_image_url_Course + ".enc");

                            
                            if (!File.Exists(image_url_Course_Path_encode))
                            {
                                await DownloadFileAsync(image_url_Course, image_url_Course_Path);

                                //Mã hóa dữ liệu
                                ENC.FileEncrypt(image_url_Course_Path, image_url_Course_Path + ".enc", key);

                                //Mã hóa tên file
                                string originalFilePath = image_url_Course_Path + ".enc";
                                string encodedFilePath = Encrypt_FileName.EncodeFileNameInPath(originalFilePath);

                                if (File.Exists(encodedFilePath))
                                {
                                    File.Delete(encodedFilePath);
                                }

                                //Rename
                                Encrypt_FileName.RenameFile(originalFilePath, encodedFilePath);

                            }
                        //}    

                        foreach(var item in result.data.lessions)
                        {
                            //Lây link download
                            string fileUrl = item.file_url;
                            string image_url = item.image_url;

                            //Tách tên file
                            string fileName = Path.GetFileName(fileUrl);
                            string fileName_Image = Path.GetFileName(image_url);

                            //Lấy đuôi mở rộng
                            string extension = Path.GetExtension(fileUrl);

                            //Path

                            /* VER1 Cách cũ: file nằm trong thư mục của khóa học
                            string file_Path = folderPath + "\\" + item.uuid + "_" + fileName;
                            string image_thumbnail_Path = folderPath + "\\" + item.uuid  + "thumbnailApp_" + fileName_Image;
                            */

                            /* VER 2 để dùng cho file XML */
                            string file_Path = folderPath + @"\"  + fileName;
                            string image_thumbnail_Path = folderPath + @"\" + fileName_Image;
                           

                            string file_Path_encode = Encrypt_FileName.EncodeFileNameInPath(file_Path + ".enc");
                            string image_thumbnail_Path_encode = Encrypt_FileName.EncodeFileNameInPath(image_thumbnail_Path + ".enc");

                            //Tải và mã hóa
                            #region Mã hóa
                            //File dữ liệu
                            if (!File.Exists(file_Path_encode))
                            {
                                await DownloadFileAsync(fileUrl, file_Path);

                                //Mã hóa dữ liệu
                                ENC.FileEncrypt(file_Path, file_Path + ".enc", key);

                                //Mã hóa tên file
                                string originalFilePath = file_Path + ".enc";
                                string encodedFilePath = Encrypt_FileName.EncodeFileNameInPath(originalFilePath);

                                if (File.Exists(encodedFilePath))
                                {
                                    File.Delete(encodedFilePath);
                                }

                                //Rename
                                Encrypt_FileName.RenameFile(originalFilePath, encodedFilePath);
                            }

                            //File Thumbnail
                            if (!File.Exists(image_thumbnail_Path_encode))
                            {
                                //await DownloadFileAsync(image_url, image_thumbnail_Path);
                                await DownloadFileAsync(image_url, image_thumbnail_Path);

                                //Mã hóa dữ liệu
                                ENC.FileEncrypt(image_thumbnail_Path, image_thumbnail_Path + ".enc", key);

                                //Mã hóa tên file
                                string originalFilePath_Thumbnail = image_thumbnail_Path + ".enc";
                                string encodedFilePath_Thumbnail = Encrypt_FileName.EncodeFileNameInPath(originalFilePath_Thumbnail);
                                if (File.Exists(encodedFilePath_Thumbnail))
                                {
                                    File.Delete(encodedFilePath_Thumbnail);
                                }

                                //Rename
                                Encrypt_FileName.RenameFile(originalFilePath_Thumbnail, encodedFilePath_Thumbnail);
                            }


                            MessageBox.Show("Tải tệp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            #endregion

                        }
                    }
                    else
                    {
                        MessageBox.Show("Failed to retrieve data from API.");
                    }
                }    


                /* TẠM ẨN KHÚC NÀY
                //Lây link download
                string fileUrl = ((string)e.Item.Description);

                //Tách tên file
                string fileName = Path.GetFileName(fileUrl);
                //Lấy đuôi mở rộng
                string extension = Path.GetExtension(fileUrl);
                await DownloadFileAsync(fileUrl, @"C:\Users\bachd\OneDrive\Máy tính\test\" + fileName);

                ENC.FileEncrypt(@"C:\Users\bachd\OneDrive\Máy tính\test\" + fileName, @"C:\Users\bachd\OneDrive\Máy tính\test\" + fileName + ".enc", key);

                #region Mã hóa tên file
                // Đường dẫn và tên file ban đầu
                string originalFilePath = @"C:\Users\bachd\OneDrive\Máy tính\test\" + fileName + ".enc";

                // Mã hóa tên file trong đường dẫn
                string encodedFilePath = Encrypt_FileName.EncodeFileNameInPath(originalFilePath);

                // Rename tệp ban đầu thành tên đã mã hóa
                if (File.Exists(encodedFilePath))
                {
                    File.Delete(encodedFilePath);
                }

                Encrypt_FileName.RenameFile(originalFilePath, encodedFilePath);

                File.Delete(@"C:\Users\bachd\OneDrive\Máy tính\test\" + fileName);

                MessageBox.Show("Tải tệp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                #endregion
                */

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void galleryControl1_Gallery_ContextButtonCustomize(object sender, DevExpress.XtraBars.Ribbon.Gallery.GalleryContextButtonCustomizeEventArgs e)
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

        #region private async Task<Image> DownloadImageAsync(string imageUrl)
        private async Task<Image> DownloadImageAsync(string imageUrl)
        {
           
                using (HttpClient client = new HttpClient())
                {
                    using (HttpResponseMessage response = await client.GetAsync(imageUrl))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            using (Stream stream = await response.Content.ReadAsStreamAsync())
                            {
                                return Image.FromStream(stream);
                            }
                        }
                        else
                        {
                            throw new Exception("Không thể tải ảnh từ URL: " + imageUrl);
                        }
                    }
                }            
        }
        #endregion

        #region static async Task DownloadFileAsync(string fileUrl, string savePath)
        static async Task DownloadFileAsync(string fileUrl, string savePath)
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
                    //MessageBox.Show("Tải tệp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Console.WriteLine("Lỗi khi tải tệp: " + response.StatusCode);
                    MessageBox.Show("Lỗi khi tải tệp: " + response.StatusCode, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        #endregion
    }
}
