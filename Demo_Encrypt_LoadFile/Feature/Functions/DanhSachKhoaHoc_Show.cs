using Demo_Encrypt_LoadFile.Feature.API;
using Demo_Encrypt_LoadFile.Feature.Models;
using DevExpress.Utils;
using DevExpress.Utils.Svg;
using ImageMagick;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Demo_Encrypt_LoadFile.Feature.Functions
{
    class DanhSachKhoaHoc_Show
    {

        #region | Màn hình danh sách khóa học |

        #region Danh khóa học load từ API
        public async static Task<DataTable> GetDanhSachKhoaHoc_API(string ApiUrl, string api_Key, string path, SvgImageCollection svgImages)
        {
            try
            {
                DataTable dt_DanhSachKhoaHoc_API = new DataTable("Courses"); //Danh sách Khóa học lấy từ API
                bool isStatus = await CallAPI.CallApi(ApiUrl, api_Key);

                if (isStatus == false)
                {
                    //MessageBox.Show("Tài khoản không được xác thực", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    dt_DanhSachKhoaHoc_API.Columns.Add("id", typeof(string));
                    dt_DanhSachKhoaHoc_API.Columns.Add("title", typeof(string));
                    dt_DanhSachKhoaHoc_API.Columns.Add("description", typeof(string));
                    dt_DanhSachKhoaHoc_API.Columns.Add("uuid", typeof(string));
                    dt_DanhSachKhoaHoc_API.Columns.Add("image_url", typeof(string));
                    dt_DanhSachKhoaHoc_API.Columns.Add("image_Path", typeof(string));
                    dt_DanhSachKhoaHoc_API.Columns.Add("image_Show", typeof(Image));
                    dt_DanhSachKhoaHoc_API.Columns.Add("meta_image_url", typeof(string));
                    dt_DanhSachKhoaHoc_API.Columns.Add("meta_image_Path", typeof(string));
                    dt_DanhSachKhoaHoc_API.Columns.Add("meta_image_Show", typeof(Image));
                    dt_DanhSachKhoaHoc_API.Columns.Add("IsDownloaded", typeof(bool));
                    dt_DanhSachKhoaHoc_API.Columns.Add("IsSelected", typeof(bool));

                    dt_DanhSachKhoaHoc_API.PrimaryKey = new DataColumn[] { dt_DanhSachKhoaHoc_API.Columns["uuid"] };

                    if (frm_InsertKeyAPI.IsInternetConnected()) //Nếu có Internet
                    {
                        //#region Ảnh default
                        //Image image_Default = null;
                        //image_Default = frm_Extention.Return_Image_Default();
                        //#endregion

                        //frm_Extention.credential = await frm_Extention.GetCredentialAsync();
                        using (HttpClient client = new HttpClient())
                        {
                            HttpResponseMessage response = await client.GetAsync(frm_StartAcademy_Test.API_URL_DanhSachKhoaHoc + api_Key);
                            if (response.IsSuccessStatusCode)
                            {
                                string jsonData = await response.Content.ReadAsStringAsync();
                                var result = JsonConvert.DeserializeObject<DanhSachKhoaHocModels.DSData>(jsonData);

                               
                                foreach (var course in result.data)
                                {
                                    
                                    #region image
                                    Image image = null;

                                    //if(course.image_url != null)
                                    //{
                                    //    byte[] imageBytes_image = await frm_Extention.DownloadImageAsync_New(course.image_url); //Đoạn này load ảnh nhanh hơn :'>

                                    //    using (MagickImage magickImage = new MagickImage(imageBytes_image))
                                    //    {
                                    //        image = frm_Extention.ConvertMagickImageToImage(magickImage);
                                    //    }
                                    //}   
                                   
                                    
                                    #endregion

                                    #region meta_image
                                    Image meta_image = null;

                                    try
                                    {
                                        if (course.meta_image_url != null)
                                        {
                                            byte[] imageBytes_meta_image = await frm_Extention.DownloadImageAsync_New(course.meta_image_url); //Đoạn này load ảnh nhanh hơn :'>
                                            using (MagickImage magickImage = new MagickImage(imageBytes_meta_image))
                                            {
                                                meta_image = frm_Extention.ConvertMagickImageToImage(magickImage);
                                                using (var memoryStream = new MemoryStream())
                                                {
                                                    magickImage.Write(memoryStream, MagickFormat.Svg);
                                                    memoryStream.Position = 0;
                                                    svgImages.Add(SvgImage.FromStream(memoryStream));
                                                }
                                            }
                                        }
                                    }
                                    catch { }

                                    #endregion

                                    dt_DanhSachKhoaHoc_API.Rows.Add(course.id
                                                                    , course.title
                                                                    , course.description
                                                                    , course.uuid
                                                                    , course.image_url
                                                                    , null //Nơi lưu image khi đã tải
                                                                    , image //Dữ liệu image khi đã tải
                                                                    , course.meta_image_url //Link ảnh Thumbmail của khóa học
                                                                    , null //Nơi lưu ảnh Thumbnail khi đã tải
                                                                    , meta_image //Nơi lưu dữ liệu ảnh Thumbnail khi đã tải
                                                                    , false //IsDownloaded
                                                                    , false //IsSelected
                                                                    );
                                }                                                                
                            }
                        }
                    }

                }    

                return dt_DanhSachKhoaHoc_API;
            }
            catch(Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                Console.WriteLine("Stack Trace: " + ex.StackTrace);

                return null;
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Danh sách khóa học đã tải
        public static DataTable GetDanhSachKhoaHoc_DaTai(string path_XML_Course, string api_Key, DataTable dt_DanhSachKhoaHoc_API, SvgImageCollection svgImages)
        {
            try
            {
                if (!File.Exists(path_XML_Course))
                    return null;

                #region Ảnh Default
                Image image_Default = null;
                image_Default = frm_Extention.Return_Image_Default();

                //Image image_Default = null;
                //string default_Image = frm_StartAcademy.dafault_Image;
                //byte[] imageBytes = File.ReadAllBytes(default_Image);
                //using (MemoryStream memoryStream_image = new MemoryStream(imageBytes))
                //{
                //    image_Default = Image.FromStream(memoryStream_image);
                //}
                #endregion

                DataTable dt_DanhSachKhoaHoc_DaTai = new DataTable("Courses"); //Danh sách Khóa học đã tải

                // Đọc tệp XML vào DataSet
                DataSet dataSet = new DataSet();
                dataSet.ReadXml(path_XML_Course);

                DataTable loadedDataTable = new DataTable();

                //if(dataSet != null || dataSet.Tables.Count != 0)
                if(dataSet.Tables.Count != 0)
                {
                    loadedDataTable = dataSet.Tables["Courses"];

                    if (loadedDataTable != null)
                    {

                        //dt_DanhSachKhoaHoc_DaTai = dt_DanhSachKhoaHoc_API.Clone();
                        dt_DanhSachKhoaHoc_DaTai = loadedDataTable.Clone();
                        dt_DanhSachKhoaHoc_DaTai.Columns.Add("meta_image_Show", typeof(Image));
                        dt_DanhSachKhoaHoc_DaTai.Columns.Add("IsSelected", typeof(bool));

                        foreach (DataRow dr in loadedDataTable.Rows)
                        {
                            string uuid = dr["uuid"].ToString();
                            DataRow[] foundRows = loadedDataTable.Select($"uuid = '{uuid}'");
                            if (foundRows.Length != 0)
                            {
                                #region image
                                //Image image = null;
                                //if (dr["image_Path"].ToString() != null)
                                //{
                                //    string a = dr["image_Path"].ToString();
                                //    MemoryStream ms_image = ENC.FileDecrypt(dr["image_Path"].ToString(), FormMain.key);
                                //    //Image image = Image.FromStream(ms_image);


                                //    using (MagickImage magickImage = new MagickImage(ms_image))
                                //    {
                                //        image = frm_Extention.ConvertMagickImageToImage(magickImage);
                                //    }
                                //}

                                Bitmap image = null;
                                //MemoryStream  ms_Image = ENC.FileDecrypt(dr["image_Path"].ToString(), FormMain.key);                          
                                //if(ms_Image != null)
                                //{
                                //    ms_Image.Position = 0;
                                //    using (MagickImage magickImage = new MagickImage(ms_Image))
                                //    {
                                //        // Chuyển đổi từ MagickImage sang mảng byte (định dạng PNG)
                                //        byte[] pngBytes = magickImage.ToByteArray(MagickFormat.Png);

                                //        // Tạo MemoryStream từ mảng byte PNG
                                //        using (MemoryStream pngStream = new MemoryStream(pngBytes))
                                //        {
                                //            // Tạo Bitmap từ MemoryStream PNG
                                //            image = new Bitmap(pngStream);
                                //        }
                                //    }    
                                //}
                                #endregion

                                #region meta_image
                                //Image meta_image = null;
                                //if (dr["meta_image_path"].ToString() != null)
                                //{
                                //    MemoryStream ms_meta_image = ENC.FileDecrypt(dr["meta_image_path"].ToString(), FormMain.key);
                                //    //Image meta_image = Image.FromStream(ms_meta_image);


                                //    using (MagickImage magickImage = new MagickImage(ms_meta_image))
                                //    {
                                //        meta_image = frm_Extention.ConvertMagickImageToImage(magickImage);
                                //    }
                                //}

                                Bitmap meta_image = null;

                                try
                                {
                                    MemoryStream ms_meta_Image = ENC.FileDecrypt(dr["meta_image_Path"].ToString(), FormMain.key);
                                    if (svgImages != null)
                                    {
                                        if (ms_meta_Image != null)
                                        {
                                            ms_meta_Image.Position = 0;
                                            using (MagickImage magickImage = new MagickImage(ms_meta_Image))
                                            {
                                                // Chuyển đổi từ MagickImage sang mảng byte (định dạng PNG)
                                                byte[] pngBytes = magickImage.ToByteArray(MagickFormat.Png);

                                                // Tạo MemoryStream từ mảng byte PNG
                                                using (MemoryStream pngStream = new MemoryStream(pngBytes))
                                                {
                                                    // Tạo Bitmap từ MemoryStream PNG
                                                    meta_image = new Bitmap(pngStream);
                                                    using (var memoryStream = new MemoryStream())
                                                    {
                                                        magickImage.Write(memoryStream, MagickFormat.Svg);
                                                        memoryStream.Position = 0;
                                                        svgImages.Add(SvgImage.FromStream(memoryStream));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch { }

                                #endregion

                                dt_DanhSachKhoaHoc_DaTai.Rows.Add(dr["id"].ToString()
                                                                  , dr["title"].ToString()
                                                                  , dr["description"].ToString()
                                                                  , dr["uuid"].ToString()
                                                                  , dr["image_url"].ToString()
                                                                  , dr["image_Path"].ToString() //Nơi lưu image khi đã tải
                                                                                                //, image == null ? image_Default : image //Dữ liệu image khi đã tải
                                                                  , dr["meta_image_url"].ToString() //Link ảnh Thumbmail của khóa học
                                                                  , dr["meta_image_path"].ToString() //Nơi lưu ảnh Thumbnail khi đã tải
                                                                                                     //, meta_image == null ? image_Default : meta_image //Nơi lưu dữ liệu ảnh Thumbnail khi đã tải
                                                                  , dr["IsDownloaded"]
                                                                  , meta_image
                                                                  , false //IsSelected
                                                                  );
                            }
                            else
                            {
                                //foreach (DataRow dr_ in foundRows)
                                //{
                                //    dt_DanhSachKhoaHoc_DaTai.ImportRow(dr_);
                                //}
                                return null;
                            }
                        }
                    }
                    else
                    {
                        return null;
                    }    

                    return dt_DanhSachKhoaHoc_DaTai;
                }    
                else
                {
                    return null;
                }                                 
            }
            catch(Exception ex)
            
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Danh sách khóa học hiển thị lên lưới

        public async static Task<DataTable> GetDanhSachKhoaHoc_Show(string ApiUrl, string api_Key, string path, string path_XML_Course, SvgImageCollection svgImageCollection1)
        {
            try
            {
                DataTable dt_DanhSachKhoaHoc_Show = new DataTable("Courses");
                DataTable dt_DanhSachKhoaHoc_API = new DataTable();
                DataTable dt_DanhSachKhoaHoc_DaTai = new DataTable();

                if (frm_InsertKeyAPI.IsInternetConnected()) //Nếu có Internet
                {
                    dt_DanhSachKhoaHoc_API = await GetDanhSachKhoaHoc_API(ApiUrl, api_Key, path, svgImageCollection1);
                    dt_DanhSachKhoaHoc_DaTai = GetDanhSachKhoaHoc_DaTai(path_XML_Course, api_Key, dt_DanhSachKhoaHoc_API, svgImageCollection1);

                    if (dt_DanhSachKhoaHoc_API is null || dt_DanhSachKhoaHoc_API.Rows.Count == 0)
                    {
                        return null;
                    }
                    else if (dt_DanhSachKhoaHoc_DaTai is null || dt_DanhSachKhoaHoc_DaTai.Rows.Count == 0)
                    {
                        return dt_DanhSachKhoaHoc_API;
                    }
                    else
                    {
                        frm_Extention.Update_Datatable(dt_DanhSachKhoaHoc_API, dt_DanhSachKhoaHoc_DaTai);
                        return dt_DanhSachKhoaHoc_API;
                    }
                }
                else
                {
                    return GetDanhSachKhoaHoc_DaTai(path_XML_Course, api_Key, dt_DanhSachKhoaHoc_API, svgImageCollection1);
                }

            }
            catch(Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
        }

        #endregion

        public static DataTable GetDanhSachKhoaHoc_DaTai_Delete(string path_XML_Course, string api_Key, string uuid )
        {
            try
            {
                if (!File.Exists(path_XML_Course))
                    return null;

                DataTable dt_DanhSachKhoaHoc_DaTai = new DataTable("Courses"); //Danh sách Khóa học đã tải
                DataTable dt_DanhSachKhoaHoc_DaTai_Temp = new DataTable("Courses"); //Danh sách Khóa học đã tải
                
                // Đọc tệp XML vào DataSet
                DataSet dataSet = new DataSet();
                dataSet.ReadXml(path_XML_Course);

                DataTable loadedDataTable = new DataTable();

                if (dataSet.Tables.Count != 0)
                {
                    loadedDataTable = dataSet.Tables["Courses"];

                    if (loadedDataTable != null)
                    {
                        dt_DanhSachKhoaHoc_DaTai = loadedDataTable.Clone();

                        DataRow[] foundRows = loadedDataTable.Select($"uuid = '{uuid}'");

                        foreach (DataRow dr in foundRows)
                        {
                            dt_DanhSachKhoaHoc_DaTai.ImportRow(dr);
                        }

                        return dt_DanhSachKhoaHoc_DaTai;
                    }
                    else
                    {
                        return null;
                    }    
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion


    }
}
