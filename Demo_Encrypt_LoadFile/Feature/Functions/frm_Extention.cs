using Demo_Encrypt_LoadFile.Feature.API;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using ImageMagick;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
//using System.Windows;
using System.Windows.Forms;
using System.Xml;

namespace Demo_Encrypt_LoadFile.Feature.Functions
{
    class frm_Extention
    {
        private static string[] scopes = { DriveService.Scope.Drive };
        private static string applicationName = "Star Academy";
        public static UserCredential credential;
        public static string fileName_Now = string.Empty;
        public static string fileName_image_url_Course_Now = string.Empty;
        public static string fileName_Image_Now = string.Empty;

        #region Kiểm tra xem key còn hạn không
        public static int Check_Expired(string dateString)
        {
            try
            {
                string dateString1 = dateString;

                // Lấy ngày giờ hiện tại
                DateTime now = DateTime.Now;
                string dateString2 = now.ToString("dd-MM-yyyy HH:mm:ss tt");

                // Chuyển đổi chuỗi ngày giờ sang kiểu DateTime
                DateTime date1 = DateTime.ParseExact(dateString1, "dd-MM-yyyy hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);
                DateTime date2 = DateTime.ParseExact(dateString2, "dd-MM-yyyy HH:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);

                // Hiển thị kết quả
                Console.WriteLine("Ngày giờ 1: " + date1);
                Console.WriteLine("Ngày giờ 2: " + date2);

                // So sánh hai ngày giờ
                int result = DateTime.Compare(date1, date2); /* result = 1: còn hạn */

                /*
                Hiển thị kết quả
                if (result1 < 0)
                    Console.WriteLine("{0} sớm hơn {1}", date1, date2);
                else if (result1 == 0)
                    Console.WriteLine("{0} và {1} là cùng một thời điểm", date1, date2);
                else
                    Console.WriteLine("{0} muộn hơn {1}", date1, date2);
                */

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Kiểm tra key còn hạn không từ file XML
        public static int Check_Expired_FromXML(string folderPath_XML)
        {
            try
            {
                DataSet dataSet = new DataSet();

                if (!File.Exists(folderPath_XML))
                    return 0;

                // Đọc tệp XML vào DataSet
                dataSet.ReadXml(folderPath_XML);

                DataTable loadedDataTable = new DataTable();
                try
                {
                    loadedDataTable = dataSet.Tables[0];
                }
                catch (Exception ex)
                {

                }

                string dateString = loadedDataTable.Rows[0]["expired_at"].ToString();
                //int result = Check_Expired(dateString);
                int is_expired = Check_Expired(dateString);

                int result = 0;
                if (is_expired == 1 && bool.Parse(loadedDataTable.Rows[0]["is_active"].ToString()) == true)
                {
                    result = 1; //Còn hạn
                }
                else
                    result = 0; //Hết hạn

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
                    //System.Windows.MessageBox.Show("Lỗi khi tải tệp: " + response.StatusCode, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        #endregion

        #region Tải file về máy
        public static async void Download(string path, DataTable dt_API_XML, string uuid, string path_XML, string meta_image_url_KhoaHoc, string file, string meta_image_url)
        {
            try
            {
                #region Thư mục chứa bài giảng
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                #endregion

                #region Thêm vào XML

                DataTable dt_XML = dt_API_XML.Clone();
                //string uuid = tileView1.GetFocusedDataRow()["uuid"].ToString();

                // Lọc các hàng trong dt_API_XML dựa trên giá trị của uuid và sao chép chúng vào dt_XML
                DataRow[] filteredRows = dt_API_XML.Select("uuid = '" + uuid + "'");
                foreach (DataRow row in filteredRows)
                {
                    dt_XML.ImportRow(row);
                }

                // Loại bỏ cột Bitmap trước khi ghi lại DataTable vào tệp XML
                if (dt_XML.Columns.Contains("Image_Course_Show"))
                    dt_XML.Columns.Remove("Image_Course_Show");

                if (dt_XML.Columns.Contains("Image_Lession_Show"))
                    dt_XML.Columns.Remove("Image_Lession_Show");

                if (dt_XML.Columns.Contains("IsSelected"))
                    dt_XML.Columns.Remove("IsSelected");

                ////dt_XML.WriteXml(filePath_XML);
                //dt_XML.WriteXml(path_XML);
                #endregion

                #region Tải file về máy
                //Link ảnh
                string image_url_Course = meta_image_url_KhoaHoc;
                //Lấy tên file + phần mở rộng
                string fileName_image_url_Course_UTF8 = Path.GetFileName(image_url_Course);
                string fileName_image_url_Course = RemoveDiacritics(fileName_image_url_Course_UTF8); //Chuyển sang tiếng việt không dấu

                //Nơi lưu ảnh
                string image_url_Course_Path = path + fileName_image_url_Course;
                //Tên sau khi mã hóa
                string image_url_Course_Path_encode = Encrypt_FileName.EncodeFileNameInPath(fileName_image_url_Course + ".enc");



                string fileId = ReturnFileId(file);
                credential = await GetCredentialAsync();
                string fileName_UTF8 = await GetFileNameAsync(fileId); // Lấy tên của tệp
                string fileName = RemoveDiacritics(fileName_UTF8);//Chuyen sang khong dau

                //Tách tên file
                // string fileName = Path.GetFileName(file);
                string fileName_Image_UTF8 = Path.GetFileName(meta_image_url);
                string fileName_Image = RemoveDiacritics(fileName_Image_UTF8);//Chuyen sang khong dau

                //Lấy đuôi mở rộng
                string extension = Path.GetExtension(file);

                string file_Path = path + @"\" + fileName;

                string image_thumbnail_Path = path + @"\" + fileName_Image;

                string file_Path_encode = Encrypt_FileName.EncodeFileNameInPath(file_Path + ".enc");



                string image_thumbnail_Path_encode = Encrypt_FileName.EncodeFileNameInPath(image_thumbnail_Path + ".enc");

                //File dữ liệu
                if (fileName.ToString() != string.Empty)
                {
                    if (!File.Exists(file_Path_encode))
                    {
                        await DownloadFileAsync_GoogleDrive(fileId, file_Path);


                        //await DownloadFileAsync(file, file_Path);
                        //await DownloadFileAsync(file, @"F:\Lam viec\freelance\Demo_Encrypt_LoadFile\Demo_Encrypt_LoadFile\bin\Debug\File\test22.pptx");

                        //Mã hóa dữ liệu
                        ENC.FileEncrypt(file_Path, file_Path + ".enc", FormMain.key);

                        //Mã hóa tên file
                        string originalFilePath = file_Path + ".enc";
                        string encodedFilePath = Encrypt_FileName.EncodeFileNameInPath(originalFilePath);

                        fileName_Now = encodedFilePath;

                        if (File.Exists(file_Path))
                        {
                            File.Delete(file_Path);
                        }

                        if (File.Exists(encodedFilePath))
                        {
                            File.Delete(encodedFilePath);
                        }

                        //Rename
                        Encrypt_FileName.RenameFile(originalFilePath, encodedFilePath);
                    }
                }

                //File Thumbnail
                if (fileName_Image.ToString() != string.Empty)
                {
                    if (!File.Exists(image_thumbnail_Path_encode))
                    {
                        byte[] imageBytes_img_Course = await CallAPI.DownloadImageAsync(meta_image_url);
                        using (MemoryStream stream = new MemoryStream(imageBytes_img_Course))
                        using (var fileStream = new System.IO.FileStream(image_thumbnail_Path, System.IO.FileMode.Create))
                        {
                            await stream.CopyToAsync(fileStream);
                        }

                        //await DownloadFileAsync(image_url, image_thumbnail_Path);
                        // await DownloadFileAsync(meta_image_url, image_thumbnail_Path);

                        //Mã hóa dữ liệu
                        ENC.FileEncrypt(image_thumbnail_Path, image_thumbnail_Path + ".enc", FormMain.key);

                        //Mã hóa tên file
                        string originalFilePath_Thumbnail = image_thumbnail_Path + ".enc";
                        string encodedFilePath_Thumbnail = Encrypt_FileName.EncodeFileNameInPath(originalFilePath_Thumbnail);

                        fileName_Image_Now = encodedFilePath_Thumbnail;

                        if (File.Exists(image_thumbnail_Path))
                        {
                            File.Delete(image_thumbnail_Path);
                        }

                        if (File.Exists(encodedFilePath_Thumbnail))
                        {
                            File.Delete(encodedFilePath_Thumbnail);
                        }

                        //Rename
                        Encrypt_FileName.RenameFile(originalFilePath_Thumbnail, encodedFilePath_Thumbnail);
                    }
                }

                #region Tạm bỏ
                //if (image_url_Course.ToString() != string.Empty)
                //{
                //    if (!File.Exists(image_url_Course_Path_encode))
                //    {
                //        byte[] imageBytes_img_Course = await CallAPI.DownloadImageAsync(meta_image_url_KhoaHoc);
                //        using (MemoryStream stream = new MemoryStream(imageBytes_img_Course))
                //        using (var fileStream = new System.IO.FileStream(image_url_Course_Path, System.IO.FileMode.Create))
                //        {
                //            await stream.CopyToAsync(fileStream);
                //        }

                //        // await DownloadFileAsync(image_url_Course, image_url_Course_Path);

                //        //Mã hóa dữ liệu
                //        ENC.FileEncrypt(image_url_Course_Path, image_url_Course_Path + ".enc", FormMain.key);

                //        //Mã hóa tên file
                //        string originalFilePath = image_url_Course_Path + ".enc";
                //        string encodedFilePath = Encrypt_FileName.EncodeFileNameInPath(originalFilePath);

                //        fileName_image_url_Course_Now = encodedFilePath;

                //        if (File.Exists(image_url_Course_Path))
                //        {
                //            File.Delete(image_url_Course_Path);
                //        }

                //        if (File.Exists(encodedFilePath))
                //        {
                //            File.Delete(encodedFilePath);
                //        }

                //        //Rename
                //        Encrypt_FileName.RenameFile(originalFilePath, encodedFilePath);

                //    }
                //}
                #endregion


                //// Load tài liệu XML
                //XmlDocument doc = new XmlDocument();
                //doc.Load(path_XML);

                //// Lấy danh sách tất cả các phần tử có tên "Products"
                //XmlNodeList productsList = doc.SelectNodes("//Products");

                //// Duyệt qua từng phần tử "Products"
                //foreach (XmlNode product in productsList)
                //{
                //    // Lấy giá trị của phần tử "uuid"
                //    XmlNode uuidNode = product.SelectSingleNode("uuid");
                //    if (uuidNode != null && uuidNode.InnerText == uuid)
                //    {
                //        XmlNode fileNode_file_Path = product.SelectSingleNode("file_Path");
                //        if (fileNode_file_Path != null)
                //        {
                //            // Thực hiện chỉnh sửa
                //            fileNode_file_Path.InnerText = fileName_Now;
                //        }

                //        //XmlNode fileNode_meta_image_url_Path_KhoaHoc = product.SelectSingleNode("meta_image_url_Path_KhoaHoc");
                //        //if (fileNode_file_Path != null)
                //        //{
                //        //    // Thực hiện chỉnh sửa
                //        //    fileNode_meta_image_url_Path_KhoaHoc.InnerText = fileName_image_url_Course_Now;
                //        //}

                //        XmlNode fileNode_meta_image_url_Path = product.SelectSingleNode("meta_image_url_Path");
                //        if (fileNode_file_Path != null)
                //        {
                //            // Thực hiện chỉnh sửa
                //            fileNode_meta_image_url_Path.InnerText = fileName_Image_Now;
                //        }
                //    }
                //}

                //// Lưu lại tệp XML sau khi đã chỉnh sửa
                //doc.Save(path_XML);

                //// Chọn node có tên là "file_Path"
                //XmlNode fileNode_file_Path = doc.SelectSingleNode("//file_Path");
                //XmlNode fileNode_meta_image_url_Path_KhoaHoc = doc.SelectSingleNode("//meta_image_url_Path_KhoaHoc");
                //XmlNode fileNode_meta_image_url_Path = doc.SelectSingleNode("//meta_image_url_Path");

                //if (fileNode_file_Path != null)
                //{
                //    // Thay đổi nội dung của node
                //    fileNode_file_Path.InnerText = fileName_Now; // Thay đổi thành đường dẫn mới của file
                //}

                //if (fileNode_meta_image_url_Path_KhoaHoc != null)
                //{
                //    // Thay đổi nội dung của node
                //    fileNode_meta_image_url_Path_KhoaHoc.InnerText = fileName_image_url_Course_Now; // Thay đổi thành đường dẫn mới của file
                //}

                //if (fileNode_meta_image_url_Path != null)
                //{
                //    // Thay đổi nội dung của node
                //    fileNode_meta_image_url_Path.InnerText = fileName_Image_Now; // Thay đổi thành đường dẫn mới của file
                //}

                //// Lưu lại tài liệu XML sau khi thay đổi
                //doc.Save(path_XML);

                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                MessageBox.Show("Lỗi:" + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region public static async void Download_Ver2(string path, DataTable dt_Select_XML, string uuid, string path_XML, string meta_image_url_KhoaHoc, string file, string meta_image_url, DataTable dt_New_Save)
        public static async void Download_Ver2(string path, DataTable dt_Select_XML, string uuid, string path_XML, string meta_image_url_KhoaHoc, string file, string meta_image_url, DataTable dt_New_Save)
        {
            try
            {
                #region Thư mục chứa bài giảng
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                #endregion

                //Danh sách bài giảng
                DataTable dt_XML = dt_Select_XML.Clone();
                foreach (DataRow row in dt_Select_XML.Rows)
                {
                    dt_XML.ImportRow(row);
                }

                if (dt_XML.Columns.Contains("Image_Course_Show"))
                    dt_XML.Columns.Remove("Image_Course_Show");

                if (dt_XML.Columns.Contains("Image_Lession_Show"))
                    dt_XML.Columns.Remove("Image_Lession_Show");

                if (dt_XML.Columns.Contains("IsSelected"))
                    dt_XML.Columns.Remove("IsSelected");

                DataTable existingDataTable = new DataTable();

                DataSet dataSet = new DataSet();

                if (File.Exists(path_XML))
                {
                    // Đọc tệp XML vào DataSet
                    dataSet.ReadXml(path_XML);
                    try
                    {
                        existingDataTable = dataSet.Tables[0];
                    }
                    catch (Exception ex)
                    {

                    }

                    //Trường hợp chưa tải bài giảng nào
                    if (existingDataTable.Rows.Count == 0)
                    {
                        foreach (DataRow dr in dt_XML.Rows)
                        {

                            DataTable dt_XML_temp = dt_XML.Clone();

                            DataRow[] filteredRows = dt_XML.Select("uuid = '" + dr["uuid"].ToString() + "'");
                            foreach (DataRow row in filteredRows)
                            {
                                dt_XML_temp.ImportRow(row);
                            }

                            Download(path,
                                                          dt_XML_temp,
                                                          dr["uuid"].ToString(),
                                                          path_XML,
                                                          dr["meta_image_url_KhoaHoc"].ToString(),
                                                          dr["file"].ToString(),
                                                          dr["meta_image_url"].ToString()
                                                          );


                        }

                        if (dt_XML.Columns.Contains("Image_Course_Show"))
                            dt_XML.Columns.Remove("Image_Course_Show");

                        if (dt_XML.Columns.Contains("Image_Lession_Show"))
                            dt_XML.Columns.Remove("Image_Lession_Show");

                        if (dt_XML.Columns.Contains("IsSelected"))
                            dt_XML.Columns.Remove("IsSelected");

                        dt_XML.WriteXml(path_XML);
                    }
                    else  //Trường đã tải bài giảng rồi, cập nhật thêm bài giảng mới
                    {

                        List<DataRow> dtExists = new List<DataRow>();
                        DataTable dt_New_Download = new DataTable();
                        dt_New_Download = existingDataTable.Clone();

                        foreach (DataRow dr_exists in existingDataTable.Rows)
                        {
                            dtExists.Add(dr_exists);
                        }

                        foreach (DataRow dr_New in dt_XML.Rows)
                        {
                            int dem = 0;
                            foreach (DataRow dr_exists in existingDataTable.Rows)
                            {
                                //if (dr_exists["uuid_KhoaHoc"].ToString() != dr_New["uuid_KhoaHoc"].ToString() && dr_exists["uuid"].ToString() != dr_New["uuid"].ToString())
                                if (dr_exists["uuid"].ToString() != dr_New["uuid"].ToString())
                                {

                                    dem = dem + 1;
                                    dtExists.Add(dr_New); //insert thêm vào file xml - không ghi đè
                                    dt_New_Download.ImportRow(dr_New); //dt này dùng để tải file về máy - không ghi đè file đã tải

                                    if (dem >= dt_XML.Rows.Count)
                                        break;
                                }
                            }
                        }

                        #region Tải file về máy                       
                        if (dt_New_Download.Rows.Count > 0)
                        {
                            foreach (DataRow dr_DL in dt_New_Download.Rows)
                            {
                                DataTable dt_XML_temp = dt_New_Download.Clone();

                                DataRow[] filteredRows = dt_New_Download.Select("uuid = '" + dr_DL["uuid"].ToString() + "'");
                                foreach (DataRow row in filteredRows)
                                {
                                    dt_XML_temp.ImportRow(row);
                                }


                                Download(path,
                                                               dt_XML_temp,
                                                               dr_DL["uuid"].ToString(),
                                                               path_XML,
                                                               dr_DL["meta_image_url_KhoaHoc"].ToString(),
                                                               dr_DL["file"].ToString(),
                                                               dr_DL["meta_image_url"].ToString()
                                                               );
                            }

                            dt_New_Save.Clear();
                            dt_New_Save.Reset();

                            if (dtExists.Count > 0)
                            {

                                // Thêm các cột từ DataRow đầu tiên vào DataTable
                                foreach (DataColumn column in dtExists[0].Table.Columns)
                                {
                                    dt_New_Save.Columns.Add(column.ColumnName, column.DataType);
                                }

                                foreach (DataRow row in dtExists)
                                {
                                    dt_New_Save.ImportRow(row);
                                }
                            }

                            if (dt_New_Save.Columns.Contains("Image_Course_Show"))
                                dt_New_Save.Columns.Remove("Image_Course_Show");

                            if (dt_New_Save.Columns.Contains("Image_Lession_Show"))
                                dt_New_Save.Columns.Remove("Image_Lession_Show");

                            if (dt_New_Save.Columns.Contains("IsSelected"))
                                dt_New_Save.Columns.Remove("IsSelected");

                            dt_New_Save.WriteXml(path_XML);

                        }
                        #endregion
                    }

                }
                else
                {
                    //Trường hợp chưa tải bài giảng nào
                    foreach (DataRow dr in dt_XML.Rows)
                    {

                        DataTable dt_XML_temp = dt_XML.Clone();

                        DataRow[] filteredRows = dt_XML.Select("uuid = '" + dr["uuid"].ToString() + "'");
                        foreach (DataRow row in filteredRows)
                        {
                            dt_XML_temp.ImportRow(row);
                        }

                        Download(path,
                                                      dt_XML_temp,
                                                      dr["uuid"].ToString(),
                                                      path_XML,
                                                      dr["meta_image_url"].ToString(),
                                                      dr["file"].ToString(),
                                                      dr["meta_image_url"].ToString()
                                                      );
                    }

                    dt_XML.WriteXml(path_XML);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        public static bool IsInternetConnected()
        {
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface networkInterface in interfaces)
            {
                if (networkInterface.OperationalStatus == OperationalStatus.Up &&
                    (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                    networkInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet))
                {
                    IPInterfaceProperties properties = networkInterface.GetIPProperties();
                    foreach (GatewayIPAddressInformation gateway in properties.GatewayAddresses)
                    {
                        Ping ping = new Ping();
                        PingReply reply = ping.Send(gateway.Address);
                        if (reply.Status == IPStatus.Success)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public async static Task<DataTable> AutoUpdata_TokenAPI(string tokenAPI, string folderPath)
        {
            try
            {
                DataTable tbl_auto_checkTokenApi = new DataTable();
                if (IsInternetConnected())
                {
                    tbl_auto_checkTokenApi = await CallAPI.CheckTokenAPI(tokenAPI);

                    if (tbl_auto_checkTokenApi.Rows.Count > 0)
                    {
                        if (tbl_auto_checkTokenApi.Rows[0]["token"].ToString() != "")
                        {
                            string avatar_url = tbl_auto_checkTokenApi.Rows[0]["avatar_url"].ToString();
                            string fileName_avatar = Path.GetFileName(avatar_url) + ".jpg";
                            string image_avatar_Path = folderPath + @"\" + fileName_avatar;
                            string image_avatar_Path_encode = Encrypt_FileName.EncodeFileNameInPath(image_avatar_Path + ".enc");

                            string originalFilePath_avatar = "";
                            string encodedFilePath_avatar = "";

                            await CallAPI.DownloadFileAsync(avatar_url, image_avatar_Path);

                            //Mã hóa dữ liệu
                            ENC.FileEncrypt(image_avatar_Path, image_avatar_Path + ".enc", FormMain.key);

                            //Mã hóa tên file
                            originalFilePath_avatar = image_avatar_Path + ".enc";
                            encodedFilePath_avatar = Encrypt_FileName.EncodeFileNameInPath(originalFilePath_avatar);

                            if (File.Exists(encodedFilePath_avatar))
                            {
                                File.Delete(encodedFilePath_avatar);
                            }

                            if (File.Exists(image_avatar_Path))
                            {
                                File.Delete(image_avatar_Path);
                            }

                            //Rename
                            Encrypt_FileName.RenameFile(originalFilePath_avatar, encodedFilePath_avatar);

                            if (!tbl_auto_checkTokenApi.Columns.Contains("image_avatar_Path"))
                            {
                                tbl_auto_checkTokenApi.Columns.Add("image_avatar_Path", typeof(string));
                                tbl_auto_checkTokenApi.Rows[0]["image_avatar_Path"] = encodedFilePath_avatar;
                            }
                            else
                            {
                                tbl_auto_checkTokenApi.Rows[0]["image_avatar_Path"] = encodedFilePath_avatar;
                            }
                        }
                    }
                }

                return tbl_auto_checkTokenApi;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                return null;
            }
        }

        #region Tải file từ Link google drive
        public static async Task<UserCredential> GetCredentialAsync()
        {
            //using (var stream = new FileStream("client_secret_123769918993-sge5nmf8mpl3v6btc6crr75cnrs9fujk.apps.googleusercontent.com.json", FileMode.Open, FileAccess.Read))
            using (var stream = new FileStream(Path.Combine(Application.StartupPath + @"\Resources\", "client_secret_123769918993-sge5nmf8mpl3v6btc6crr75cnrs9fujk.apps.googleusercontent.com.json"), FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json"; // Thay đổi tên tệp theo nhu cầu của bạn
                return await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true));
            }
        }

        private static async Task DownloadFileAsync_GoogleDrive(string fileId, string saveFilePath)
        {
            // Tạo dịch vụ Google Drive
            var service = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = applicationName
            });

            // Tải tệp từ Google Drive
            using (var stream = new FileStream(saveFilePath, FileMode.Create))
            {
                await service.Files.Get(fileId).DownloadAsync(stream);
            }
        }

        public static async Task<string> GetFileNameAsync(string fileId)
        {
            // Tạo dịch vụ Google Drive
            var service = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = applicationName
            });

            // Gửi yêu cầu lấy thông tin về tệp
            var file = await service.Files.Get(fileId).ExecuteAsync();

            // Lấy tên của tệp
            string fileName = file.Name;

            return fileName;
        }

        public async void DownFile_From_GoogleDrive()
        {
            try
            {
                credential = await GetCredentialAsync();
                string fileId = "1AvtVjjeCCB9vdweNqt44tKhMtO5G2mGy"; // ID của tệp bạn muốn tải xuống từ Google Drive
                string fileName = await GetFileNameAsync(fileId); // Lấy tên của tệp
                //string saveFilePath = @"F:\Lam viec\freelance\Demo_Encrypt_LoadFile\Demo_Encrypt_LoadFile\bin\Debug\File\test22.pptx"; // Đường dẫn đến nơi bạn muốn lưu trữ tệp được tải xuống
                string saveFilePath = Path.Combine(Application.StartupPath + @"\File\", fileName); // Đường dẫn đến nơi bạn muốn lưu trữ tệp được tải xuống
                await DownloadFileAsync(fileId, saveFilePath);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public static string ReturnFileId(string url)
        {
            string fileId = string.Empty;
            string pattern = @"id=([a-zA-Z0-9_-]+)";
            Regex regex = new Regex(pattern);
            Match match = regex.Match(url);
            if (match.Success)
            {
                fileId = match.Groups[1].Value;
                Console.WriteLine(fileId);
            }
            else
            {
                Console.WriteLine("Không tìm thấy chuỗi ID");
            }

            return fileId;
        }

        public static string RemoveDiacritics(string text)
        {
            string normalizedString = text.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
        #endregion

        #region Đọc ảnh định dạng Webp
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
        #endregion

        #region Load ảnh từ API - New
        private static readonly Lazy<HttpClient> lazyHttpClient = new Lazy<HttpClient>(() =>
        {
            HttpClient client = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(30) // Bạn có thể điều chỉnh thời gian timeout
            };
            return client;
        });

        private static HttpClient HttpClientInstance => lazyHttpClient.Value;

        // Phương thức tải ảnh
        public static async Task<byte[]> DownloadImageAsync_New(string imageUrl)
        {
            try
            {
                using (HttpResponseMessage response = await HttpClientInstance.GetAsync(imageUrl))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsByteArrayAsync();
                    }
                    else
                    {
                        throw new Exception("Không thể tải ảnh từ URL: " + imageUrl);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error downloading image: " + ex.Message);
                throw;
            }
        }
        #endregion

        #region Update_Datatable
        public static void Update_Datatable(DataTable dataTableTarget, DataTable dataTableSource)
        {
            bool hasFilePath = dataTableTarget.Columns.Contains("file_Path");
            //bool hasIsDownloaded = dataTableTarget.Columns.Contains("IsDownloaded");

            foreach (DataRow dr in dataTableSource.Rows)
            {
                DataRow[] foundRows = dataTableTarget.Select("uuid = '" + dr["uuid"] + "'");
                // Nếu tìm thấy hàng, cập nhật giá trị của cột targetColumnName
                if (foundRows.Length > 0)
                {
                    foreach (DataRow rowTarget in foundRows)
                    {
                        rowTarget["image_Path"] = dr["image_Path"];
                        //rowTarget["image_Show"] = dr["image_Show"];
                        rowTarget["meta_image_Path"] = dr["meta_image_Path"];
                        //rowTarget["meta_image_Show"] = dr["meta_image_Show"];
                        //rowTarget["IsDownloaded"] = dr["IsDownloaded"];

                        if (hasFilePath)
                        {
                            rowTarget["file_Path"] = dr["file_Path"];
                            rowTarget["IsDownloaded"] = dr["IsDownloaded"];
                        }

                    }
                }
            }
        }
        #endregion

        public static Image Return_Image_Default()
        {
            Image image_Default = null;
            string default_Image = frm_StartAcademy.dafault_Image;
            byte[] imageBytes = File.ReadAllBytes(default_Image);
            using (MemoryStream memoryStream_image = new MemoryStream(imageBytes))
            {
                image_Default = Image.FromStream(memoryStream_image);
            }

            return image_Default;
        }

        #region Download_New
        public static async void Download_New(string path, string path_XML_Course, DataTable dt_DanhSachKhoaHoc)
        {
            try
            {
                #region Thư mục chứa bài giảng
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                #endregion


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Lấy đường dẫn file
        public static string ReturnFilePath(string folderPath)
        {
            try
            {
                string filePath = string.Empty;
                // Lấy danh sách tất cả các tệp trong thư mục
                string[] files = Directory.GetFiles(folderPath);
                if (files.Length > 0)
                {
                    // Lấy tệp đầu tiên
                    filePath = files[0];
                }
                else
                {
                    Console.WriteLine("Thư mục không chứa tệp nào.");
                }

                return filePath;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Tải file ảnh
        /// <summary>
        /// Tải file về máy (Chia làm 2 thư mục: khóa học - bài giảng)
        /// </summary>
        /// <param name="image_url">Link để tải ảnh</param>
        /// <param name="uuid">Mã khóa học</param>
        /// <param name="path_Course_Lession">Nơi lưu</param>        
        /// <returns></returns>
        public async static Task<string> Download_Image(string image_url, string uuid, string path_Course_Lession)
        {
            try
            {
                string image_Name_temp = null;
                string image_Name_KhongDau = null;
                string image_Name_Path = null;
                string image_Name_Path_Encode = null;

                if (image_url != null)
                {
                    image_Name_temp = uuid + "_" + Path.GetFileName(image_url); //Lấy tên và phần mở rộng
                    image_Name_KhongDau = frm_Extention.RemoveDiacritics(image_Name_temp); //Chuyển sang tên file không dấu
                    image_Name_Path = path_Course_Lession + @"\" + image_Name_KhongDau; //Nơi lưu file ảnh
                    image_Name_Path_Encode = Encrypt_FileName.EncodeFileNameInPath(image_Name_Path + ".enc"); //Nơi lưu file ảnh đã mã hóa

                    //Lưu về máy
                    byte[] imageBytes_img_Course = await CallAPI.DownloadImageAsync(image_url);
                    using (MemoryStream stream = new MemoryStream(imageBytes_img_Course))
                    using (var fileStream = new System.IO.FileStream(image_Name_Path, System.IO.FileMode.Create))
                    {
                        await stream.CopyToAsync(fileStream);
                    }

                    //Mã hóa dữ liệu
                    ENC.FileEncrypt(image_Name_Path, image_Name_Path + ".enc", FormMain.key);

                    //Mã hóa tên file
                    string originalFilePath_Thumbnail = image_Name_Path + ".enc";
                    string encodedFilePath_Thumbnail = Encrypt_FileName.EncodeFileNameInPath(originalFilePath_Thumbnail);

                    fileName_Image_Now = encodedFilePath_Thumbnail;

                    if (File.Exists(image_Name_Path))
                    {
                        File.Delete(image_Name_Path);
                    }

                    if (File.Exists(image_Name_Path_Encode))
                    {
                        File.Delete(image_Name_Path_Encode);
                    }

                    //Rename
                    Encrypt_FileName.RenameFile(originalFilePath_Thumbnail, encodedFilePath_Thumbnail);

                }

                return image_Name_Path_Encode;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Tải file bài giảng
        public static async Task<string> Download_File(string file, string uuid, string path_Course_Lession)
        {
            try
            {
                string file_Name_temp = null;
                string file_Name_KhongDau = null;
                string file_Name_Path = null;
                string file_Name_Path_Encode = null;

                string fileId = ReturnFileId(file);

                file_Name_temp = await GetFileNameAsync(fileId); // Lấy tên của tệp
                file_Name_KhongDau = RemoveDiacritics(file_Name_temp);//Chuyen sang khong dau

                string file_Path = path_Course_Lession + @"\" + file_Name_KhongDau;

                await DownloadFileAsync_GoogleDrive(fileId, file_Path);


                //await DownloadFileAsync(file, file_Path);
                //await DownloadFileAsync(file, @"F:\Lam viec\freelance\Demo_Encrypt_LoadFile\Demo_Encrypt_LoadFile\bin\Debug\File\test22.pptx");

                //Mã hóa dữ liệu
                ENC.FileEncrypt(file_Path, file_Path + ".enc", FormMain.key);

                //Mã hóa tên file
                file_Name_Path = file_Path + ".enc";
                file_Name_Path_Encode = Encrypt_FileName.EncodeFileNameInPath(file_Name_Path);

                if (File.Exists(file_Path))
                {
                    File.Delete(file_Path);
                }

                if (File.Exists(file_Name_Path_Encode))
                {
                    File.Delete(file_Name_Path_Encode);
                }

                //Rename
                Encrypt_FileName.RenameFile(file_Name_Path, file_Name_Path_Encode);

                return file_Name_Path_Encode;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
