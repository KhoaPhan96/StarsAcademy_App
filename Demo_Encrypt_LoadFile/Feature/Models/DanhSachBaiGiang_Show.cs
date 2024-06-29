using Demo_Encrypt_LoadFile.Feature.API;
using Demo_Encrypt_LoadFile.Feature.Functions;
using ImageMagick;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Demo_Encrypt_LoadFile.Feature.Models
{
    class DanhSachBaiGiang_Show
    {
        #region public static DataTable GetDanhSachDaTai(string path) (Lấy thông tin các file đã tải đã lưu trong file xml)
        public static DataTable GetDanhSachDaTai(string pathXML, string token)         
        {
            try
            {
                #region Ảnh default
               
                Image image;
                string default_Image = frm_StartAcademy.dafault_Image;

                byte[] imageBytes = File.ReadAllBytes(default_Image);

                #endregion

                using (MemoryStream memoryStream_image = new MemoryStream(imageBytes))
                {
                    image = Image.FromStream(memoryStream_image);

                    DataTable dt_DanhSachDaTai = new DataTable();

                    DataSet dataSet = new DataSet();

                    if (!File.Exists(pathXML))
                        return null;

                    // Đọc tệp XML vào DataSet
                    dataSet.ReadXml(pathXML);

                    DataTable loadedDataTable = new DataTable();
                    try
                    {
                        loadedDataTable = dataSet.Tables[0];
                    }
                    catch (Exception ex)
                    {

                    }

                    dt_DanhSachDaTai = loadedDataTable.Clone();

                    dt_DanhSachDaTai.Columns.Add("Image_Course_Show", typeof(Image));
                    dt_DanhSachDaTai.Columns.Add("Image_Lession_Show", typeof(Image));
                    dt_DanhSachDaTai.Columns.Add("IsSelected", typeof(bool));
                    dt_DanhSachDaTai.Columns.Add("Read", typeof(string));
                    dt_DanhSachDaTai.Columns.Add("Delete", typeof(string));

                    foreach (DataRow dr in loadedDataTable.Rows)
                    {
                        MemoryStream msImage_meta_image_url_Path_KhoaHoc = ENC.FileDecrypt(dr["meta_image_url_Path_KhoaHoc"].ToString(), FormMain.key);

                        Bitmap bitmap_meta_image_url_Path_KhoaHoc = null;
                        if (msImage_meta_image_url_Path_KhoaHoc != null)
                        {
                            msImage_meta_image_url_Path_KhoaHoc.Position = 0;

                            using (MagickImage magickImage = new MagickImage(msImage_meta_image_url_Path_KhoaHoc))
                            {
                                // Chuyển đổi từ MagickImage sang mảng byte (định dạng PNG)
                                byte[] pngBytes = magickImage.ToByteArray(MagickFormat.Png);

                                // Tạo MemoryStream từ mảng byte PNG
                                using (MemoryStream pngStream = new MemoryStream(pngBytes))
                                {
                                    // Tạo Bitmap từ MemoryStream PNG
                                    bitmap_meta_image_url_Path_KhoaHoc = new Bitmap(pngStream);
                                }
                            }
                        }

                        MemoryStream msImage_meta_image_url_Path = ENC.FileDecrypt(dr["meta_image_url_Path"].ToString(), FormMain.key);

                        Bitmap bitmap_meta_image_url_Path = null;

                        if (msImage_meta_image_url_Path != null)
                        {
                            msImage_meta_image_url_Path.Position = 0;

                            using (MagickImage magickImage = new MagickImage(msImage_meta_image_url_Path))
                            {
                                // Chuyển đổi từ MagickImage sang mảng byte (định dạng PNG)
                                byte[] pngBytes = magickImage.ToByteArray(MagickFormat.Png);

                                // Tạo MemoryStream từ mảng byte PNG
                                using (MemoryStream pngStream = new MemoryStream(pngBytes))
                                {
                                    // Tạo Bitmap từ MemoryStream PNG
                                    bitmap_meta_image_url_Path = new Bitmap(pngStream);
                                }
                            }
                        }



                        dt_DanhSachDaTai.Rows.Add(
                                    dr["id_KhoaHoc"].ToString(),
                                    dr["uuid_KhoaHoc"].ToString(),
                                    dr["title_KhoaHoc"].ToString(),
                                    dr["description_KhoaHoc"].ToString(),
                                    dr["meta_image_url_KhoaHoc"].ToString(),
                                    dr["meta_image_url_Path_KhoaHoc"].ToString(),

                                    dr["id"].ToString(),
                                    dr["uuid"].ToString(),
                                    dr["title"].ToString(),
                                    dr["description"].ToString(),
                                    dr["meta_image_url"].ToString(),
                                    dr["file"].ToString(),
                                    dr["meta_image_url_Path"].ToString(),
                                    dr["file_Path"].ToString(),
                                    token.ToString(),

                                    //Image.FromStream(ENC.FileDecrypt(dr["image_Path_KhoaHoc"].ToString(), FormMain.key)),
                                    //Image.FromStream(ENC.FileDecrypt(dr["image_url_Path"].ToString(), FormMain.key)),

                                    //Image.FromStream(ENC.FileDecrypt(dr["meta_image_url_Path_KhoaHoc"].ToString(), FormMain.key) == null ? memoryStream_image : ENC.FileDecrypt(dr["meta_image_url_Path_KhoaHoc"].ToString(), FormMain.key)),
                                    //Image.FromStream(ENC.FileDecrypt(dr["meta_image_url_Path"].ToString(), FormMain.key) == null ? memoryStream_image : ENC.FileDecrypt(dr["meta_image_url_Path"].ToString(), FormMain.key)),

                                    bitmap_meta_image_url_Path_KhoaHoc == null ? Image.FromStream(memoryStream_image) : bitmap_meta_image_url_Path_KhoaHoc,
                                    bitmap_meta_image_url_Path == null ? Image.FromStream(memoryStream_image) : bitmap_meta_image_url_Path,
                                    false,
                                    "",
                                    ""
                                    );
                    }
                
                return dt_DanhSachDaTai;
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
        #endregion

        #region Danh sách bài giảng khóa học load từ API
        public async static Task<DataTable> GetDanhSach_API(string ApiUrl, string apiKey, string path)
        {
            try
            {
                DataTable dt_API_XML = new DataTable("Products"), dt_API_Show = new DataTable("Products");

                bool isStatus = await CallAPI.CallApi(ApiUrl, apiKey);

                if (isStatus == false)
                {
                    MessageBox.Show("Tài khoản không được xác thực", "API Response", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    if (dt_API_XML.Columns.Contains("uuid_KhoaHoc"))
                    {
                        dt_API_XML.Clear();
                        dt_API_XML.Dispose();

                    }
                    else
                    {
                        dt_API_XML.Columns.Add("id_KhoaHoc", typeof(string));
                        dt_API_XML.Columns.Add("uuid_KhoaHoc", typeof(string));
                        dt_API_XML.Columns.Add("title_KhoaHoc", typeof(string));
                        dt_API_XML.Columns.Add("description_KhoaHoc", typeof(string));
                        dt_API_XML.Columns.Add("meta_image_url_KhoaHoc", typeof(string));
                        dt_API_XML.Columns.Add("meta_image_url_Path_KhoaHoc", typeof(string));

                        dt_API_XML.Columns.Add("id", typeof(string));
                        dt_API_XML.Columns.Add("uuid", typeof(string));
                        dt_API_XML.Columns.Add("title", typeof(string));
                        dt_API_XML.Columns.Add("description", typeof(string));
                        dt_API_XML.Columns.Add("meta_image_url", typeof(string));
                        dt_API_XML.Columns.Add("file", typeof(string));
                        dt_API_XML.Columns.Add("meta_image_url_Path", typeof(string));
                        dt_API_XML.Columns.Add("file_Path", typeof(string));

                        dt_API_XML.Columns.Add("Image_Course_Show", typeof(Image));
                        dt_API_XML.Columns.Add("Image_Lession_Show", typeof(Image));
                        dt_API_XML.Columns.Add("IsSelected", typeof(bool));
                        dt_API_XML.Columns.Add("token", typeof(string));
                    }

                    using (HttpClient client = new HttpClient())
                    {
                        string apiUrl = frm_StartAcademy.apiUrl_Courses + apiKey;
                        HttpResponseMessage response = await client.GetAsync(apiUrl);

                        if (response.IsSuccessStatusCode)
                        {
                            frm_Extention.credential = await frm_Extention.GetCredentialAsync();

                            string jsonData = await response.Content.ReadAsStringAsync();
                            //var result = JsonConvert.DeserializeObject<ApiResponse>(jsonData);
                            var result = JsonConvert.DeserializeObject<DanhSachKhoaHocModels.DSData>(jsonData);

                            foreach (var course in result.data)
                            {
                                foreach (var lesson in course.lessions)
                                {
                                    //Image img_Course = await CallAPI.DownloadImageAsync(course.meta_image_url);

                                    //Image img_Lesson = await CallAPI.DownloadImageAsync(lesson.meta_image_url);

                                    Image img_Course;
                                    byte[] imageBytes_img_Course = await CallAPI.DownloadImageAsync(course.meta_image_url);
                                    using (MagickImage magickImage = new MagickImage(imageBytes_img_Course))
                                    {
                                        img_Course = ConvertMagickImageToImage(magickImage);
                                    }

                                    Image img_Lesson;
                                    byte[] imageBytes_img_Lesson = await CallAPI.DownloadImageAsync(lesson.meta_image_url);
                                    using (MagickImage magickImage = new MagickImage(imageBytes_img_Lesson))
                                    {
                                        img_Lesson = ConvertMagickImageToImage(magickImage);
                                    }

                                    
                                    string fileId = frm_Extention.ReturnFileId(lesson.file);
                                    string fileName_UTF8 = await frm_Extention.GetFileNameAsync(fileId); // Lấy tên của tệp
                                    string fileName = frm_Extention.RemoveDiacritics(fileName_UTF8);//Chuyen sang khong dau

                                    //Datatable để đẩy vào file XML
                                    dt_API_XML.Rows.Add(new object[] { course.id ,course.uuid, course.title, course.description, course.meta_image_url
                                                                        , path + Encrypt_FileName.EncodeFileNameInPath(Path.GetFileName(course.meta_image_url) + ".enc")
                                                                        , lesson.id
                                                                        , lesson.uuid, lesson.title, lesson.description, lesson.meta_image_url, lesson.file
                                                                        , path + Encrypt_FileName.EncodeFileNameInPath(Path.GetFileName(lesson.meta_image_url) + ".enc")

                                                                        //, path + Encrypt_FileName.EncodeFileNameInPath(Path.GetFileName(lesson.file) + ".enc")
                                                                        , path + Encrypt_FileName.EncodeFileNameInPath(fileName + ".enc")

                                                                        //, path + Encrypt_FileName.EncodeFileNameInPath(frm_Extention.RemoveDiacritics(Path.GetFileName(lesson.file)) + ".enc")
                                                                        , img_Course, img_Lesson, false
                                                                        , apiKey
                                                                        });
                                }
                            }

                        }

                    }

                }
                //return await Task.FromResult(dt_API_XML);

                return dt_API_XML;
            }
            catch(Exception ex)
            {
                //return null;
                throw new Exception(ex.Message);
            }
        }
        #endregion

        public static Image ConvertMagickImageToImage(MagickImage magickImage)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Lưu MagickImage vào MemoryStream với định dạng PNG
                magickImage.Write(memoryStream, MagickFormat.Png);
                memoryStream.Position = 0; // Đặt lại vị trí của luồng về đầu
                                           // Chuyển MemoryStream thành đối tượng Image
                return Image.FromStream(memoryStream);
            }
        }

        #region Danh sách bài giảng khóa học hiển thị trên lưới
        public async static Task<DataTable> GetDanhSach_NotExists_API(string ApiUrl, string apiKey, string path)
        {
            try
            {
                DataTable dt_API_XML = new DataTable("Products");
                dt_API_XML = await GetDanhSach_API(ApiUrl, apiKey, path);

                DataTable dt_API_XML_NotExists = dt_API_XML.Clone();

                #region Kiểm tra file đã được tải chưa
                DataTable tbl_DanhSachDaTai = DanhSachBaiGiang_Show.GetDanhSachDaTai(frm_StartAcademy.path_XML, apiKey);

                foreach (DataRow drAPI in dt_API_XML.Rows)
                {
                    string uuid_KhoaHoc_API = drAPI["uuid_KhoaHoc"].ToString();
                    string uuid_API = drAPI["uuid"].ToString();
                    bool exists = false;

                    if (tbl_DanhSachDaTai == null)
                        tbl_DanhSachDaTai = dt_API_XML.Clone();

                    foreach (DataRow dr in tbl_DanhSachDaTai.Rows)
                    {
                        string uuid_KhoaHoc_DatTai = dr["uuid_KhoaHoc"].ToString();
                        string uuid_API_DatTai = dr["uuid"].ToString();

                        if (uuid_KhoaHoc_API == uuid_KhoaHoc_DatTai && uuid_API == uuid_API_DatTai)
                        {
                            exists = true;
                            break;
                        }
                    }

                    if (!exists)
                    {
                        Image image;
                        string default_Image = frm_StartAcademy.dafault_Image;

                        byte[] imageBytes = File.ReadAllBytes(default_Image);

                        using (MemoryStream memoryStream_image = new MemoryStream(imageBytes))
                        {
                            dt_API_XML_NotExists.Rows.Add(drAPI["id_KhoaHoc"].ToString(),
                                                      drAPI["uuid_KhoaHoc"].ToString(),
                                                      drAPI["title_KhoaHoc"].ToString(),
                                                      drAPI["description_KhoaHoc"].ToString(),
                                                      drAPI["meta_image_url_KhoaHoc"].ToString(),
                                                      drAPI["meta_image_url_Path_KhoaHoc"].ToString(),

                                                      drAPI["id"].ToString(),
                                                      drAPI["uuid"].ToString(),
                                                      drAPI["title"].ToString(),
                                                      drAPI["description"].ToString(),
                                                      drAPI["meta_image_url"].ToString(),
                                                      drAPI["file"].ToString(),
                                                      drAPI["meta_image_url_Path"].ToString(),
                                                      drAPI["file_Path"].ToString(),
                                                      drAPI["Image_Course_Show"],
                                                      drAPI["Image_Lession_Show"],
                                                      drAPI["IsSelected"].ToString(),
                                                      drAPI["token"].ToString()
                                                        );
                        }
                    }
                }

                #endregion

                return dt_API_XML_NotExists;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
       
    }
}
