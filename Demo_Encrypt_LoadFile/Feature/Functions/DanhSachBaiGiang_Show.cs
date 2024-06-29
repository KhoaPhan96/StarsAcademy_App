using Demo_Encrypt_LoadFile.Feature.API;
using Demo_Encrypt_LoadFile.Feature.Models;
using DevExpress.Utils;
using DevExpress.Utils.Svg;
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


namespace Demo_Encrypt_LoadFile.Feature.Functions
{
    class DanhSachBaiGiang_Show
    {
        #region Danh Sách bài giảng thuộc khóa học - Load từ API
        public async static Task<DataTable> GetDanhSachBaiGiang_ThuocKhoaHoc_API(string ApiUrl, string api_Key, string path, string uuid_Course, SvgImageCollection svgImages)
        {
            try
            {
                DataTable dt_DanhSachBaiGiang_API = new DataTable("Lessions"); //Danh sách Khóa học lấy từ API
                bool isStatus = await CallAPI.CallApi(ApiUrl, api_Key);

                if (isStatus == false)
                {
                    //MessageBox.Show("Tài khoản không được xác thực", "API Response", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    dt_DanhSachBaiGiang_API.Columns.Add("uuid_Course", typeof(string));
                    dt_DanhSachBaiGiang_API.Columns.Add("id", typeof(string));
                    dt_DanhSachBaiGiang_API.Columns.Add("uuid", typeof(string));
                    dt_DanhSachBaiGiang_API.Columns.Add("title", typeof(string));
                    dt_DanhSachBaiGiang_API.Columns.Add("description", typeof(string));
                    dt_DanhSachBaiGiang_API.Columns.Add("image_url", typeof(string));
                    dt_DanhSachBaiGiang_API.Columns.Add("image_Path", typeof(string));
                    dt_DanhSachBaiGiang_API.Columns.Add("image_Show", typeof(Image));
                    dt_DanhSachBaiGiang_API.Columns.Add("meta_image_url", typeof(string));
                    dt_DanhSachBaiGiang_API.Columns.Add("meta_image_Path", typeof(string));
                    dt_DanhSachBaiGiang_API.Columns.Add("meta_image_Show", typeof(Image));
                    dt_DanhSachBaiGiang_API.Columns.Add("file", typeof(string));
                    dt_DanhSachBaiGiang_API.Columns.Add("file_Path", typeof(string));
                    dt_DanhSachBaiGiang_API.Columns.Add("IsDownloaded", typeof(bool));
                    dt_DanhSachBaiGiang_API.Columns.Add("IsSelected", typeof(bool));

                    dt_DanhSachBaiGiang_API.PrimaryKey = new DataColumn[] { dt_DanhSachBaiGiang_API.Columns["uuid"] };

                    if (frm_InsertKeyAPI.IsInternetConnected()) //Nếu có Internet
                    {
                        #region Ảnh default
                        Image image_Default = null;
                        image_Default = frm_Extention.Return_Image_Default();
                        #endregion

                        using (HttpClient client = new HttpClient())
                        {
                            string url_API = frm_StartAcademy_Test.API_URL_DanhSachBaiGiang + uuid_Course + "/lessions?api_token=" + api_Key;
                            HttpResponseMessage response = await client.GetAsync(url_API);
                            if (response.IsSuccessStatusCode)
                            {
                                string jsonData = await response.Content.ReadAsStringAsync();
                                var result = JsonConvert.DeserializeObject<DanhSachBaiGiangModels.DSData>(jsonData);

                                foreach (var lession in result.data)
                                {
                                    #region image
                                    Image image = null;
                                    //if (lession.image_url != null)
                                    //{
                                    //    byte[] imageBytes_image = await frm_Extention.DownloadImageAsync_New(lession.image_url); //Đoạn này load ảnh nhanh hơn :'>
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
                                        if (lession.meta_image_url != null)
                                        {
                                            byte[] imageBytes_meta_image = await frm_Extention.DownloadImageAsync_New(lession.meta_image_url); //Đoạn này load ảnh nhanh hơn :'>
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

                                    dt_DanhSachBaiGiang_API.Rows.Add(uuid_Course
                                                                     , lession.id
                                                                     , lession.uuid
                                                                     , lession.title
                                                                     , lession.description
                                                                     , lession.image_url
                                                                     , null //Nơi lưu image khi đã tải
                                                                     , image == null ? image_Default : image //Dữ liệu image khi đã tải
                                                                     , lession.meta_image_url //Link ảnh Thumbmail của bài giảng
                                                                     , null
                                                                     , meta_image == null ? image_Default : meta_image //Nơi lưu dữ liệu ảnh Thumbnail khi đã tải
                                                                     , lession.file
                                                                     , null //Nơi lưu file
                                                                     , false //Đã tải xuống hay chưa
                                                                     , false
                                                                    );

                                }
                            }    
                        }    
                    }
                }    
                return dt_DanhSachBaiGiang_API;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
        }
        #endregion

        public static DataTable GetDanhSachBaiGiang_ThuocKhoaHoc_DaTai(string path_XML_Lession, string uuid_Course, SvgImageCollection svgImages)
        {
            try
            {
                if (!File.Exists(path_XML_Lession))
                    return null;

                #region Ảnh Default
                Image image_Default = null;
                image_Default = frm_Extention.Return_Image_Default();
                #endregion

                DataTable dt_DanhSachBaiGiang_DaTai = new DataTable("Lessions"); //Danh sách Khóa học đã tải

                // Đọc tệp XML vào DataSet
                DataSet dataSet = new DataSet();
                dataSet.ReadXml(path_XML_Lession);

                DataTable loadedDataTable = new DataTable();

                if (dataSet != null || dataSet.Tables.Count != 0)
                {
                    loadedDataTable = dataSet.Tables["Lessions"];
                 
                    if (loadedDataTable != null )
                    {
                        DataRow[] selectedRows = loadedDataTable.Select("uuid_Course = '" + uuid_Course + "'"); //Danh sách bài giảng theo khóa học

                        if (selectedRows.Length != 0)
                        {
                            if (loadedDataTable.Rows.Count != 0)
                            {
                                dt_DanhSachBaiGiang_DaTai = loadedDataTable.Clone();

                                dt_DanhSachBaiGiang_DaTai.Columns.Add("meta_image_Show", typeof(Image));

                                foreach (DataRow dr in selectedRows)
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
                                        //MemoryStream ms_Image = ENC.FileDecrypt(dr["image_Path"].ToString(), FormMain.key);

                                        //if (ms_Image != null)
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

                                        dt_DanhSachBaiGiang_DaTai.Rows.Add(uuid_Course
                                                                           , dr["id"].ToString()
                                                                           , dr["uuid"].ToString()
                                                                           , dr["title"].ToString()
                                                                           , dr["description"].ToString()
                                                                           , dr["image_url"].ToString()
                                                                           , dr["image_Path"].ToString()
                                                                           //, image == null ? image_Default : image //Dữ liệu image khi đã tải
                                                                           , dr["meta_image_url"].ToString() //Link ảnh Thumbmail của khóa học 
                                                                           , dr["meta_image_path"].ToString() //Nơi lưu ảnh Thumbnail khi đã tải
                                                                                                              //, meta_image == null ? image_Default : meta_image //Nơi lưu dữ liệu ảnh Thumbnail khi đã tải
                                                                           , dr["file"].ToString()
                                                                           , dr["file_Path"].ToString()
                                                                           , bool.Parse(dr["IsDownloaded"].ToString())
                                                                           //, false
                                                                           , meta_image == null ? image_Default : meta_image //Nơi lưu dữ liệu ảnh Thumbnail khi đã tải
                                                                           );
                                    }
                                    else
                                    {
                                        //foreach (DataRow dr_ in foundRows)
                                        //{
                                        //    dt_DanhSachBaiGiang_DaTai.ImportRow(dr_);
                                        //}
                                        return null;
                                    }
                                }
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
                    else
                    {
                        return null;
                    }    
                }
                    return dt_DanhSachBaiGiang_DaTai;
            }
            catch(Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
        }

        public async static Task<DataTable> GetDanhSachBaiGiang_Show(string ApiUrl, string api_Key, string path, string uuid_Course, string path_XML_Lession, SvgImageCollection svgImageCollection1)
        {
            try
            {
                DataTable dt_DanhSachBaiGiang_Show = new DataTable("Lessions");
                DataTable dt_DanhSachBaiGiang_API = new DataTable();
                DataTable dt_DanhSachBaiGiang_DaTai = new DataTable();

                if (frm_InsertKeyAPI.IsInternetConnected()) //Nếu có Internet
                {
                    dt_DanhSachBaiGiang_API = await GetDanhSachBaiGiang_ThuocKhoaHoc_API(ApiUrl, api_Key, path, uuid_Course, svgImageCollection1);
                    dt_DanhSachBaiGiang_DaTai = GetDanhSachBaiGiang_ThuocKhoaHoc_DaTai(path_XML_Lession, uuid_Course, svgImageCollection1);

                    if (dt_DanhSachBaiGiang_API is null || dt_DanhSachBaiGiang_API.Rows.Count == 0)
                    {
                        return null;
                    }
                    else if (dt_DanhSachBaiGiang_DaTai is null || dt_DanhSachBaiGiang_DaTai.Rows.Count == 0)
                    {
                        return dt_DanhSachBaiGiang_API;
                    }
                    else
                    {
                        frm_Extention.Update_Datatable(dt_DanhSachBaiGiang_API, dt_DanhSachBaiGiang_DaTai);
                        return dt_DanhSachBaiGiang_API;
                    }
                }
                else
                {
                    return GetDanhSachBaiGiang_ThuocKhoaHoc_DaTai(path_XML_Lession, uuid_Course, svgImageCollection1);
                }
               
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async static Task<DataTable> GetDanhSachBaiGiang_ThuocKhoaHoc_API_XetDeTaiFile(string ApiUrl, string api_Key, string path, string uuid_Course, SvgImageCollection svgImages)
        {
            try
            {
                DataTable dt_DanhSachBaiGiang_API = new DataTable("Lessions"); //Danh sách Khóa học lấy từ API
                bool isStatus = await CallAPI.CallApi(ApiUrl, api_Key);

                if (isStatus == false)
                {
                    //MessageBox.Show("Tài khoản không được xác thực", "API Response", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    dt_DanhSachBaiGiang_API.Columns.Add("uuid_Course", typeof(string));
                    dt_DanhSachBaiGiang_API.Columns.Add("id", typeof(string));
                    dt_DanhSachBaiGiang_API.Columns.Add("uuid", typeof(string));
                    dt_DanhSachBaiGiang_API.Columns.Add("title", typeof(string));
                    dt_DanhSachBaiGiang_API.Columns.Add("description", typeof(string));
                    dt_DanhSachBaiGiang_API.Columns.Add("image_url", typeof(string));
                    dt_DanhSachBaiGiang_API.Columns.Add("image_Path", typeof(string));
                    dt_DanhSachBaiGiang_API.Columns.Add("image_Show", typeof(Image));
                    dt_DanhSachBaiGiang_API.Columns.Add("meta_image_url", typeof(string));
                    dt_DanhSachBaiGiang_API.Columns.Add("meta_image_Path", typeof(string));
                    dt_DanhSachBaiGiang_API.Columns.Add("meta_image_Show", typeof(Image));
                    dt_DanhSachBaiGiang_API.Columns.Add("file", typeof(string));
                    dt_DanhSachBaiGiang_API.Columns.Add("file_Path", typeof(string));
                    dt_DanhSachBaiGiang_API.Columns.Add("IsDownloaded", typeof(bool));
                    dt_DanhSachBaiGiang_API.Columns.Add("IsSelected", typeof(bool));

                    dt_DanhSachBaiGiang_API.PrimaryKey = new DataColumn[] { dt_DanhSachBaiGiang_API.Columns["uuid"] };

                    if (frm_InsertKeyAPI.IsInternetConnected()) //Nếu có Internet
                    {
                        #region Ảnh default
                        Image image_Default = null;
                        image_Default = frm_Extention.Return_Image_Default();
                        #endregion

                        using (HttpClient client = new HttpClient())
                        {
                            string url_API = frm_StartAcademy_Test.API_URL_DanhSachBaiGiang + uuid_Course + "/lessions?api_token=" + api_Key;
                            HttpResponseMessage response = await client.GetAsync(url_API);
                            if (response.IsSuccessStatusCode)
                            {
                                string jsonData = await response.Content.ReadAsStringAsync();
                                var result = JsonConvert.DeserializeObject<DanhSachBaiGiangModels.DSData>(jsonData);

                                foreach (var lession in result.data)
                                {
                                  
                                    dt_DanhSachBaiGiang_API.Rows.Add(uuid_Course
                                                                     , lession.id
                                                                     , lession.uuid
                                                                     , lession.title
                                                                     , lession.description
                                                                     , lession.image_url
                                                                     , null //Nơi lưu image khi đã tải
                                                                     //, image == null ? image_Default : image //Dữ liệu image khi đã tải
                                                                     , null //Dữ liệu image khi đã tải
                                                                     , lession.meta_image_url //Link ảnh Thumbmail của bài giảng
                                                                     , null //Nơi lưu
                                                                     //, meta_image == null ? image_Default : meta_image //Dữ liệu ảnh Thumbnail khi đã tải
                                                                     , null //Dữ liệu ảnh Thumbnail khi đã tải
                                                                     , lession.file
                                                                     , null //Nơi lưu file
                                                                     , false //Đã tải xuống hay chưa
                                                                     , false
                                                                    );

                                }
                            }
                        }
                    }
                }
                return dt_DanhSachBaiGiang_API;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
        }

    }
}
