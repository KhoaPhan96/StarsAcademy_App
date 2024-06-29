using Demo_Encrypt_LoadFile.Feature;
//using DevExpress.DataAccess.Native.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Security.Cryptography;
using Google.Apis.Drive.v3;
using Google.Apis.Auth.OAuth2;
using System.Threading;
using Google.Apis.Util.Store;
using Google.Apis.Services;
using System.Text.RegularExpressions;
using System.Xml;

namespace Demo_Encrypt_LoadFile
{
    public partial class FormMain : Form
    {
        //test
        #region Ẩn taskbar
        // Import các hàm từ thư viện user32.dll
        [DllImport("user32.dll")]
        private static extern int FindWindow(string className, string windowText);

        [DllImport("user32.dll")]
        private static extern int ShowWindow(int hwnd, int command);

        // Hằng số cho ShowWindow
        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;
        #endregion

        #region Variables       
        public string fileName = string.Empty;
        public static string key = "chuacopass";
        private const string ApiUrl = "https://knsngoisao.edu.vn/api/v1/me/check?api_token=";
        private string api_Key = "37741d319ba8deb7";

        //Danh sách bài giảng, khóa học
        #region Danh sách bài giảng, khóa học
        public DataTable tbl_Courses = new DataTable();
        public DataTable tbl_Lessions = new DataTable();
        #endregion
        #endregion

        #region test drive
        private static string[] scopes = { DriveService.Scope.Drive };
        private static string applicationName = "Star Academy";
        private UserCredential credential;
        #endregion

        #region Inits

        #region  public FormMain()
        public FormMain()
        {
            InitializeComponent();
        }
        #endregion

        #region private void FormMain_Load(object sender, EventArgs e)
        private void FormMain_Load(object sender, EventArgs e)
        {
            try
            {
                //Hiển thị lại thanh taskbar
                int hwnd = FindWindow("Shell_TrayWnd", "");
                ShowWindow(hwnd, SW_SHOW);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #endregion

        #region Functions

        #region  private void btn_Brower_Click(object sender, EventArgs e)
        private void btn_Brower_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txt_input.Text = dlg.FileName;
                fileName = dlg.FileName;
            }
        }
        #endregion

        #region private void btn_encrypt_Click(object sender, EventArgs e)
        private void btn_encrypt_Click(object sender, EventArgs e)
        {
            try
            {
                ENC.FileEncrypt(txt_input.Text, txt_input.Text + ".enc", key);

                //EncryptImage(inputImagePath, outputImagePath, key);
                //MessageBox.Show("Image encrypted successfully.");

                #region Mã hóa tên file
                // Đường dẫn và tên file ban đầu
                string originalFilePath = txt_input.Text + ".enc";

                // Mã hóa tên file trong đường dẫn
                string encodedFilePath = Encrypt_FileName.EncodeFileNameInPath(originalFilePath);

                // Rename tệp ban đầu thành tên đã mã hóa
                Encrypt_FileName.RenameFile(originalFilePath, encodedFilePath);

                MessageBox.Show("Mã hóa thành công.");
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
        #endregion

        #region  private void btn_image_Click(object sender, EventArgs e)
        private void btn_image_Click(object sender, EventArgs e)
        {
            frm_Image frm = new frm_Image();
            frm.fileName = txt_input.Text;
            frm.frm_key = key;
            frm.ShowDialog();
        }
        #endregion

        #region private void btn_mediaPlayer_Click(object sender, EventArgs e)
        private void btn_mediaPlayer_Click(object sender, EventArgs e)
        {
            frm_MediaPlayer frm = new frm_MediaPlayer();
            frm.fileName = txt_input.Text;
            frm.frm_key = key;
            frm.ShowDialog();
        }

        #endregion

        #region private void btn_PDF_Click(object sender, EventArgs e)
        private void btn_PDF_Click(object sender, EventArgs e)
        {
            frm_PDF frm = new frm_PDF();
            frm.fileName = txt_input.Text;
            frm.frm_key = key;
            frm.ShowDialog();
        }

        #endregion

        #region private void btn_powerPoint_Click(object sender, EventArgs e)
        private void btn_powerPoint_Click(object sender, EventArgs e)
        {
            frm_PowerPoint frm = new frm_PowerPoint();
            frm.RunPowerPoint(txt_input.Text, key);
        }

        #endregion

        #region private void btn_powerPoint_MemoryStream_Click(object sender, EventArgs e)
        private void btn_powerPoint_MemoryStream_Click(object sender, EventArgs e)
        {
            frm_PowerPoint_MemoryStream frm = new frm_PowerPoint_MemoryStream();
            frm.RunExample(txt_input.Text, key);
        }
        #endregion

        #region private void btn_LoadDocx_Click(object sender, EventArgs e)
        private void btn_LoadDocx_Click(object sender, EventArgs e)
        {
            frm_Docx frm = new frm_Docx();
            frm.fileName = txt_input.Text;
            frm.key = key;
            frm.ShowDialog();
        }
        #endregion

        #endregion

        #region test giao dien
        private void button1_Click(object sender, EventArgs e)
        {
            frm_StartAcademy frm = new frm_StartAcademy();
            frm.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Demo_GiaoDien frm = new Demo_GiaoDien();
            frm.ShowDialog();
        }
        #endregion

        #region test mã hóa tên file

        /* Khi mã hóa tên file thì vẫn đọc được mà không cần phải giải mã lại */

        //public static string EncodeFileName(string fileName)
        //{
        //    byte[] fileNameBytes = Encoding.UTF8.GetBytes(fileName);
        //    string encodedFileName = Convert.ToBase64String(fileNameBytes);
        //    return encodedFileName;
        //}

        // Giải mã tên file
        //public static string DecodeFileName(string encodedFileName)
        //{
        //    byte[] encodedFileNameBytes = Convert.FromBase64String(encodedFileName);
        //    string decodedFileName = Encoding.UTF8.GetString(encodedFileNameBytes);
        //    return decodedFileName;
        //}

        // Rename tệp
        //public static void RenameFile(string oldFileName, string newFileName)
        //{
        //    // Kiểm tra xem tệp tồn tại không
        //    if (File.Exists(oldFileName))
        //    {
        //        File.Move(oldFileName, newFileName);
        //    }
        //    else
        //    {
        //        Console.WriteLine("File does not exist: " + oldFileName);
        //    }
        //}

        // Mã hóa tên file trong đường dẫn
        public static string EncodeFileNameInPath(string filePath)
        {
            string directory = Path.GetDirectoryName(filePath);
            string fileName = Path.GetFileName(filePath);
            string encodedFileName = EncodeFileName(fileName);
            return Path.Combine(directory, encodedFileName);
        }

        // Mã hóa tên file
        public static string EncodeFileName(string fileName)
        {
            byte[] fileNameBytes = Encoding.UTF8.GetBytes(fileName);
            string encodedFileName = Convert.ToBase64String(fileNameBytes);
            return encodedFileName;
        }

        // Giải mã tên file trong đường dẫn
        public static string DecodeFileNameInPath(string filePath)
        {
            string directory = Path.GetDirectoryName(filePath);
            string fileName = Path.GetFileName(filePath);
            string decodedFileName = DecodeFileName(fileName);
            return Path.Combine(directory, decodedFileName);
        }

        // Giải mã tên file
        public static string DecodeFileName(string encodedFileName)
        {
            byte[] encodedFileNameBytes = Convert.FromBase64String(encodedFileName);
            string decodedFileName = Encoding.UTF8.GetString(encodedFileNameBytes);
            return decodedFileName;
        }

        public static void RenameFile(string oldFilePath, string newFilePath)
        {
            // Kiểm tra xem tệp tồn tại không
            if (File.Exists(oldFilePath))
            {
                File.Move(oldFilePath, newFilePath);
            }
            else
            {
                Console.WriteLine("File does not exist: " + oldFilePath);
            }
        }

        private void btn_MaHoaTenFile_Click(object sender, EventArgs e)
        {
            #region Cách 1
            //string encodedFileName = EncodeFileName(txt_input.Text);

            //// Rename tệp ban đầu thành tên đã mã hóa
            //RenameFile(txt_input.Text, encodedFileName);
            #endregion

            #region Cách 2
            // Đường dẫn và tên file ban đầu
            string originalFilePath = txt_input.Text;

            // Mã hóa tên file trong đường dẫn
            string encodedFilePath = EncodeFileNameInPath(originalFilePath);

            // Rename tệp ban đầu thành tên đã mã hóa
            RenameFile(originalFilePath, encodedFilePath);
            #endregion
        }

        private void btn_GiaiMaTenFile_Click(object sender, EventArgs e)
        {
            #region Cách 1
            //// Giải mã tên file
            //string decodedFileName = DecodeFileName(txt_input.Text);

            //// Rename lại tên file khi đã giải mã
            //RenameFile(txt_input.Text, decodedFileName);
            #endregion

            #region Cách 2
            // Giải mã tên file
            string decodedFilePath = DecodeFileNameInPath(txt_input.Text);

            // Rename lại tên file khi đã giải mã
            RenameFile(txt_input.Text, decodedFilePath);
            #endregion
        }


        #endregion

        private void btn_LoadScreen_Click(object sender, EventArgs e)
        {
            LoadScreen frm = new LoadScreen();
            frm.fileName = txt_input.Text;
            frm.frm_key = key;
            frm.ShowDialog();

            //LoadFile loadFile = new LoadFile();
            //loadFile.Load(txt_input.Text, key);
        }

        #region Định dạng file
        private void btn_DinhDangFile_Click(object sender, EventArgs e)
        {
            // Lấy phần mở rộng của tệp tin
            string fileExtension = Path.GetExtension(txt_input.Text);

            if (fileExtension != null)
            {
                switch (fileExtension.ToLower())
                {
                    case ".jpg":
                    case ".jpeg":
                    case ".png":
                    case ".gif":
                    case ".bmp":
                        MessageBox.Show("Ảnh");
                        break;
                    case ".mp4":
                    case ".avi":
                    case ".mov":
                    case ".wmv":
                    case ".mkv":
                        MessageBox.Show("Video");
                        break;
                    case ".pdf":
                    case ".pdfa":
                    case ".pdfx":
                    case ".pdfe":
                    case ".pdfua":
                        MessageBox.Show("Pdf");
                        break;
                    case ".pptx":                 
                    case ".ppt":                 
                    case ".pps":                 
                    case ".ppsx":                 
                    case ".potx":                 
                        MessageBox.Show("PowerPoint");
                        break;
                    case ".doc": 
                    case ".docx": 
                    case ".dot": 
                    case ".dotx": 
                    case ".docm": 
                    case ".dtm": 
                        MessageBox.Show("Word");
                        break;
                    default:
                        MessageBox.Show("Không biết");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Không tìm thấy đường dẫn");
            }
        }
        #endregion


        #region API

        #region private async Task CallApi(string api_Key)
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
                        //MessageBox.Show(responseBody, "API Response", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        MessageBox.Show(response.StatusCode.ToString(), "API Response", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // Xử lý lỗi nếu có
                        MessageBox.Show("Error: " + response.StatusCode, "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu có
                MessageBox.Show("Exception: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region private async void btn_KiemTraToken_Click(object sender, EventArgs e)
        private async void btn_KiemTraToken_Click(object sender, EventArgs e)
        {
            // Gọi API khi nút được nhấn
            await CallApi(api_Key);
        }
        #endregion

        #region  private async void btn_DanhSachKhoaHoc_Click(object sender, EventArgs e)
        private async void btn_DanhSachKhoaHoc_Click(object sender, EventArgs e)
        {
            try
            {
                // Địa chỉ API endpoint để lấy dữ liệu
                string apiUrl = "https://knsngoisao.edu.vn/api/v1/me/courses/5ddc57eb97b2b0fe?api_token=37741d319ba8deb7";

                // Tạo một đối tượng HttpClient để gửi yêu cầu HTTP GET
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        // Gửi yêu cầu GET đến API endpoint và nhận phản hồi
                        HttpResponseMessage response = await client.GetAsync(apiUrl);

                        // Kiểm tra tính hợp lệ của phản hồi
                        if (response.IsSuccessStatusCode)
                        {
                            // Đọc nội dung từ phản hồi dưới dạng chuỗi JSON
                            string responseBody = await response.Content.ReadAsStringAsync();

                            // Phân tích chuỗi JSON thành một đối tượng JObject
                            JObject json = JObject.Parse(responseBody);

                            // Lấy dữ liệu từ trường "data" trong JSON
                            JObject data = (JObject)json["data"];

                            // Lấy các giá trị từ trường "data"
                            string title = (string)data["title"];
                            string description = (string)data["description"];
                            string uuid = (string)data["uuid"];
                            string imageUrl = (string)data["image_url"];

                            Console.WriteLine("Title: " + title);
                            Console.WriteLine("Description: " + description);
                            Console.WriteLine("UUID: " + uuid);
                            Console.WriteLine("Image URL: " + imageUrl);

                            // Lấy danh sách các bài học từ trường "lessions"
                            JArray lessonsArray = (JArray)data["lessions"];
                            foreach (JObject lesson in lessonsArray)
                            {
                                string lessonUuid = (string)lesson["uuid"];
                                string lessonTitle = (string)lesson["title"];
                                string lessonDescription = (string)lesson["description"];
                                string lessonImageUrl = (string)lesson["image_url"];
                                string lessonFileUrl = (string)lesson["file_url"];

                                Console.WriteLine("Lesson UUID: " + lessonUuid);
                                Console.WriteLine("Lesson Title: " + lessonTitle);
                                Console.WriteLine("Lesson Description: " + lessonDescription);
                                Console.WriteLine("Lesson Image URL: " + lessonImageUrl);
                                Console.WriteLine("Lesson File URL: " + lessonFileUrl);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Failed to get data. Status code: " + response.StatusCode);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #endregion

        private async void btn_DanhSachKhoaHoc_Cach2_Click(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            frm_LoadAPI frm = new frm_LoadAPI();
            frm.ShowDialog();
        }

        private void btn_TestFormGallery_Click(object sender, EventArgs e)
        {
            frm_TestGallery frm = new frm_TestGallery();
            frm.ShowDialog();
        }

        private void btn_MaHoaDuLieu_TenFile_Click(object sender, EventArgs e)
        {
            try
            {
                ENC.FileEncrypt(txt_input.Text, txt_input.Text + ".enc", key);

                //EncryptImage(inputImagePath, outputImagePath, key);
                //MessageBox.Show("Image encrypted successfully.");

                #region Mã hóa tên file
                // Đường dẫn và tên file ban đầu
                string originalFilePath = txt_input.Text + ".enc";

                // Mã hóa tên file trong đường dẫn
                string encodedFilePath = Encrypt_FileName.EncodeFileNameInPath(originalFilePath);

                // Rename tệp ban đầu thành tên đã mã hóa
                Encrypt_FileName.RenameFile(originalFilePath, encodedFilePath);

                MessageBox.Show("Mã hóa thành công.");
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btn_TestFormGallery_API_Click(object sender, EventArgs e)
        {
            frm_LoadGallery_API frm = new frm_LoadGallery_API();
            frm.ShowDialog();
        }

        private void bnt_MaHoaThuMuc_Click(object sender, EventArgs e)
        {
            try
            {
                string originalFolderPath = @"F:\Khoa\Freelance\testmahoa";

                FolderEncryptor encryptor = new FolderEncryptor(key);
                string encryptedName = encryptor.EncryptFolderName(Path.GetFileName(originalFolderPath));

                // Đường dẫn mới với tên đã mã hóa
                string newFolderPath = Path.Combine(Path.GetDirectoryName(originalFolderPath), encryptedName);

                FolderEncryptor.RenameFile(originalFolderPath, newFolderPath);

                //Đổi tên thư mục gốc thành tên đã mã hóa
               // Directory.Move(originalFolderPath, newFolderPath);

                Console.WriteLine("Folder renamed successfully.");

            }
            catch (Exception ex)
            {
                
            }

        }

        private void btn_GiaiMaThuMuc_Click(object sender, EventArgs e)
        {
            try
            {
                string originalFolderPath = @"C:\Users\bachd\OneDrive\Máy tính\test\4oGdFT8wrjKlALwpjD+w3w==";

                FolderEncryptor encryptor = new FolderEncryptor(key);
                string encryptedName = encryptor.DecryptFolderName(Path.GetFileName(originalFolderPath));

                // Đường dẫn mới với tên đã mã hóa
                string newFolderPath = Path.Combine(Path.GetDirectoryName(originalFolderPath), encryptedName);

                FolderEncryptor.RenameFile(originalFolderPath, newFolderPath);

                //Đổi tên thư mục gốc thành tên đã mã hóa
                // Directory.Move(originalFolderPath, newFolderPath);

                Console.WriteLine("Folder renamed successfully.");

            }
            catch (Exception ex)
            {

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            frm_LoadFile_DaTai frm = new frm_LoadFile_DaTai();
            frm.ShowDialog();
        }

        private void btn_TestXML_Click(object sender, EventArgs e)
        {
            frm_TestXML frm = new frm_TestXML();
            frm.ShowDialog();

        }

        private void btn_LoadData_FromXML_Click(object sender, EventArgs e)
        {
            frm_LoadData_FromXML frm = new frm_LoadData_FromXML();
            frm.ShowDialog();
        }

        private void btn_TlieView_Click(object sender, EventArgs e)
        {
            frm_TittleView frm = new frm_TittleView();
            frm.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Test_Form_Parent frm = new Test_Form_Parent();
            frm.Show();
        }

        private async void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {

                string url = "https://drive.google.com/uc?export=download&id=1AvtVjjeCCB9vdweNqt44tKhMtO5G2mGy";
                string pattern = @"id=([a-zA-Z0-9_-]+)";

                Regex regex = new Regex(pattern);
                Match match = regex.Match(url);

                if (match.Success)
                {
                    string fileId_ = match.Groups[1].Value;
                    Console.WriteLine(fileId_);
                }
                else
                {
                    Console.WriteLine("Không tìm thấy chuỗi ID");
                }

                credential = await GetCredentialAsync();
                string fileId = "1AvtVjjeCCB9vdweNqt44tKhMtO5G2mGy"; // ID của tệp bạn muốn tải xuống từ Google Drive
                string fileName = await GetFileNameAsync(fileId); // Lấy tên của tệp
                //string saveFilePath = @"F:\Lam viec\freelance\Demo_Encrypt_LoadFile\Demo_Encrypt_LoadFile\bin\Debug\File\test22.pptx"; // Đường dẫn đến nơi bạn muốn lưu trữ tệp được tải xuống
                string saveFilePath = Path.Combine(Application.StartupPath + @"\File\", fileName); // Đường dẫn đến nơi bạn muốn lưu trữ tệp được tải xuống
                await DownloadFileAsync(fileId, saveFilePath);
                MessageBox.Show("Tệp " + fileName + " đã được tải xuống thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private async Task<UserCredential> GetCredentialAsync()
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

        private async Task DownloadFileAsync(string fileId, string saveFilePath)
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

        private async Task<string> GetFileNameAsync(string fileId)
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

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                string filename = txt_input.Text;

                MemoryStream decryptedStream = ENC.FileDecrypt(fileName, key);

                if (decryptedStream != null)
                {
                    // Chuyển MemoryStream thành XmlDocument
                    decryptedStream.Seek(0, SeekOrigin.Begin);
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(decryptedStream);


                    // Step 3: Thực hiện các thay đổi trong XmlDocument
                    // Ví dụ: Thay đổi giá trị của một phần tử
                    XmlNodeList titleNodes = xmlDoc.SelectNodes("Courses");
                    foreach (XmlNode titleNode in titleNodes)
                    {
                        titleNode.InnerText = "New Title";
                    }

                    MemoryStream updatedStream = new MemoryStream();
                    xmlDoc.Save(updatedStream);
                    updatedStream.Seek(0, SeekOrigin.Begin);

                    ENC.FileEncrypt(txt_input.Text, txt_input.Text + ".enc", key);

                    //FileEncrypt(outputImagePath, updatedStream, key);
                }

                //if (decryptedStream != null)
                //{
                //    // Chuyển MemoryStream thành XmlDocument
                //    decryptedStream.Seek(0, SeekOrigin.Begin);
                //    XmlDocument xmlDoc = new XmlDocument();
                //    xmlDoc.Load(decryptedStream);

                //    DataSet dataSet = new DataSet();
                //    // Chuyển đổi XmlDocument thành DataSet
                //    StringReader xmlStringReader = new StringReader(xmlDoc.OuterXml);

                //    dataSet.ReadXml(xmlStringReader);

                //    if(dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                //    {
                //        dataSet.Tables[0].Rows[0]["id"] = "7";
                //    }

                //    // Lưu các thay đổi trở lại tệp tin XML
                //    StringWriter xmlStringWriter = new StringWriter();
                //    dataSet.WriteXml(xmlStringWriter);

                //    // Load XmlDocument từ chuỗi XML đã cập nhật
                //    xmlDoc.LoadXml(xmlStringWriter.ToString());



                //    // Lưu lại tệp tin XML đã cập nhật
                //    xmlDoc.Save(filename);
                //}






            }
            catch(Exception ex)
            {

            }
        }
    }


    public class Lesson
    {
        public string uuid { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string image_url { get; set; }
        public string file_url { get; set; }
    }

    public class Data
    {
        public string title { get; set; }
        public string description { get; set; }
        public string uuid { get; set; }
        public string image_url { get; set; }
        public Lesson[] lessions { get; set; }
    }

    public class ApiResponse
    {
        public Data[] data { get; set; }
    }


}
