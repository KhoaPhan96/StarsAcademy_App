using Demo_Encrypt_LoadFile.Feature.API;
using Demo_Encrypt_LoadFile.Feature.Functions;
using Demo_Encrypt_LoadFile.Feature.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo_Encrypt_LoadFile.Feature
{
    public partial class frm_InsertKeyAPI : Form
    {
        #region Variables
        DataTable dt_TokenAPI = new DataTable("TokenAPI");
        public static string folderPath = Application.StartupPath + @"\Data\";
        public static string folderPath_XML = Application.StartupPath + @"\Data\data.xml";
        public static string link_check_token_API = "https://knsngoisao.edu.vn/api/v1/me/check?api_token=";
        #endregion

        #region Inits

        #region public frm_InsertKeyAPI()
        public frm_InsertKeyAPI()
        {
            InitializeComponent();
        }
        #endregion

        #region private async void frm_InsertKeyAPI_Load(object sender, EventArgs e) (Tự động cập nhật thông tin API khi có internet)
        private async void frm_InsertKeyAPI_Load(object sender, EventArgs e)
        {
            try
            {
                #region Load thông tin token api từ file XML
                if (!File.Exists(folderPath_XML))
                    return;
              
                DataTable tbl_Token_API = new DataTable();

                tbl_Token_API = await data_API_Token.GetData_API_Token(folderPath_XML);
                #endregion

                txt_ToKenAPI.Text = tbl_Token_API.Rows[0]["token"].ToString();
                txt_FullName.Text = tbl_Token_API.Rows[0]["name"].ToString();
                chk_Active.Checked = Convert.ToBoolean(tbl_Token_API.Rows[0]["is_active"].ToString());
                txt_Expired.Text = tbl_Token_API.Rows[0]["expired_at"].ToString();
                pictureEdit_Avatar.Image = Image.FromStream(ENC.FileDecrypt(tbl_Token_API.Rows[0]["image_avatar_Path"].ToString(), FormMain.key));

                #region Tự động cập nhật Token API

                #region Bỏ
                //if (IsInternetConnected())
                //{
                //    DataTable tbl_auto_checkTokenApi = await CallAPI.CheckTokenAPI(link_check_token_API + txt_ToKenAPI.Text);

                //    if (tbl_auto_checkTokenApi.Rows.Count > 0)
                //    {
                //        if (tbl_auto_checkTokenApi.Rows[0]["token"].ToString() != "")
                //        {
                //            string avatar_url = tbl_auto_checkTokenApi.Rows[0]["avatar_url"].ToString();
                //            string fileName_avatar = Path.GetFileName(avatar_url) + ".jpg";
                //            string image_avatar_Path = folderPath + @"\" + fileName_avatar;
                //            string image_avatar_Path_encode = Encrypt_FileName.EncodeFileNameInPath(image_avatar_Path + ".enc");

                //            string originalFilePath_avatar = "";
                //            string encodedFilePath_avatar = "";

                //            //if (!File.Exists(image_avatar_Path_encode))
                //            //{
                //            await CallAPI.DownloadFileAsync(avatar_url, image_avatar_Path);

                //            //Mã hóa dữ liệu
                //            ENC.FileEncrypt(image_avatar_Path, image_avatar_Path + ".enc", FormMain.key);

                //            //Mã hóa tên file
                //            originalFilePath_avatar = image_avatar_Path + ".enc";
                //            encodedFilePath_avatar = Encrypt_FileName.EncodeFileNameInPath(originalFilePath_avatar);

                //            if (File.Exists(encodedFilePath_avatar))
                //            {
                //                File.Delete(encodedFilePath_avatar);
                //            }

                //            if (File.Exists(image_avatar_Path))
                //            {
                //                File.Delete(image_avatar_Path);
                //            }

                //            //Rename
                //            Encrypt_FileName.RenameFile(originalFilePath_avatar, encodedFilePath_avatar);
                //            //}

                //            if (!tbl_auto_checkTokenApi.Columns.Contains("image_avatar_Path"))
                //            {
                //                tbl_auto_checkTokenApi.Columns.Add("image_avatar_Path", typeof(string));
                //                tbl_auto_checkTokenApi.Rows[0]["image_avatar_Path"] = encodedFilePath_avatar;
                //            }
                //            else
                //            {
                //                tbl_auto_checkTokenApi.Rows[0]["image_avatar_Path"] = encodedFilePath_avatar;
                //            }

                //            tbl_auto_checkTokenApi.WriteXml(folderPath_XML);

                //            txt_ToKenAPI.Text = loadedDataTable.Rows[0]["token"].ToString();
                //            btn_Check_Click(null, null);
                //        }
                //    }
                //}
                #endregion

                if (frm_Extention.IsInternetConnected())
                {
                    DataTable tbl_auto_checkTokenApi = new DataTable();
                    tbl_auto_checkTokenApi = await frm_Extention.AutoUpdata_TokenAPI(link_check_token_API + tbl_Token_API.Rows[0]["token"].ToString(), folderPath);

                    tbl_auto_checkTokenApi.WriteXml(folderPath_XML);
                    txt_ToKenAPI.Text = tbl_auto_checkTokenApi.Rows[0]["token"].ToString();
                    btn_Check_Click(null, null);
                }    

                #endregion

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #endregion

        #region Functions

        #region private void btn_OK_Click(object sender, EventArgs e)
        private async void btn_OK_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string filePath_XML = folderPath_XML;

                #region Tải avatar về máy

                if (dt_TokenAPI.Rows.Count > 0)
                {
                    
                    string avatar_url = dt_TokenAPI.Rows[0]["avatar_url"].ToString();
                    string fileName_avatar = Path.GetFileName(avatar_url) + ".jpg";
                    string image_avatar_Path = folderPath + @"\" + fileName_avatar;
                    string image_avatar_Path_encode = Encrypt_FileName.EncodeFileNameInPath(image_avatar_Path + ".enc");

                    string originalFilePath_avatar = "";
                    string encodedFilePath_avatar = "";

                    //if (!File.Exists(image_avatar_Path_encode))
                    //{
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
                    //}

                    if (!dt_TokenAPI.Columns.Contains("image_avatar_Path"))
                    {
                        dt_TokenAPI.Columns.Add("image_avatar_Path", typeof(string));
                        dt_TokenAPI.Rows[0]["image_avatar_Path"] = encodedFilePath_avatar;
                    }   
                    else
                    {
                        dt_TokenAPI.Rows[0]["image_avatar_Path"] = encodedFilePath_avatar;
                    }                                
                }
                else
                {
                    MessageBox.Show("Vui lòng bấm nút Kiểm tra", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                #endregion

                #region Tạo file xml để lưu thông tin key               
                dt_TokenAPI.WriteXml(filePath_XML);
                #endregion

                MessageBox.Show("Lưu dữ liệu thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region private async void btn_Check_Click(object sender, EventArgs e)
        private async void btn_Check_Click(object sender, EventArgs e)
        {
            try
            {                
                if (IsInternetConnected())
                {
                    using (HttpClient client = new HttpClient())
                    {
                        string apiUrl = link_check_token_API + txt_ToKenAPI.Text;
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

                            if (dt_TokenAPI.Columns.Contains("uuid"))
                            {
                                dt_TokenAPI.Clear();
                                dt_TokenAPI.Dispose();
                            }
                            else
                            {
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
                            }
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
                            
                            txt_FullName.Text = result.data.name;
                            txt_Expired.Text = result.data.api_token.expired_at.ToString("dd-MM-yyyy HH:mm:ss tt");
                            chk_Active.EditValue = result.data.api_token.is_active;

                            // Tải hình ảnh từ URL và hiển thị trong PictureBox
                            using (WebClient webClient = new WebClient())
                            {
                                byte[] imageData = await webClient.DownloadDataTaskAsync(result.data.avatar_url);
                                using (MemoryStream ms = new MemoryStream(imageData))
                                {
                                    pictureEdit_Avatar.Image = Image.FromStream(ms);
                                }
                            }

                            #region Kiểm tra Key còn hạn không                           
                            int result1 = frm_Extention.Check_Expired(txt_Expired.Text);
                            frm_StartAcademy._checkExpired = result1;

                            if (result1 == 1)
                            {
                                txt_ThongTin.Text = "Còn hạn";
                                
                            }
                            else
                            {
                                txt_ThongTin.Text = "Hết hạn";
                            }                                                            
                            #endregion
                        }
                        else
                        {
                            MessageBox.Show("Key không đúng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Không có kết nối Internet!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }    
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Kiem tra internet
        public static bool IsInternetConnected()
        {
            // Kiểm tra tình trạng kết nối mạng của tất cả các giao diện
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface networkInterface in interfaces)
            {
                // Kiểm tra nếu giao diện mạng đang hoạt động và có kết nối Internet
                if (networkInterface.OperationalStatus == OperationalStatus.Up &&
                    (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                    networkInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet))
                {
                    // Kiểm tra nếu có thể truy cập Internet qua giao diện mạng này
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
        #endregion

        #endregion
    }
}
