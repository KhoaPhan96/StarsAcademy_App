using Demo_Encrypt_LoadFile.Feature.Models;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Navigation;
using DevExpress.XtraGrid.Views.Tile;
using DevExpress.XtraGrid.Views.Tile.ViewInfo;
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
using Demo_Encrypt_LoadFile.Feature.Functions;
using DevExpress.Utils;
using DevExpress.XtraGrid.Columns;
using System.Reflection;
using System.Xml.Linq;
using System.Diagnostics;
using DevExpress.Utils.CodedUISupport;
namespace Demo_Encrypt_LoadFile.Feature
{
    public partial class frm_StartAcademy_Home : Form
    {
        #region Variables     

        //Nơi lưu thông tin token api
        public static string path_XML = Application.StartupPath + @"\File\data.xml"; //Nơi lưu thông tin đường dẫn các file
        string path = Application.StartupPath + @"\File\";
        string fileName = string.Empty;

        public DataTable dt_DanhSachDaTai = new DataTable("Products");
        public DataTable dt_API_XML = new DataTable("Products"), dt_API_Show = new DataTable("Products");
        string folderPath_XML = Application.StartupPath + @"\Data\data.xml"; //Nơi lưu thông tin token
        public static DataTable dt_New_Save = new DataTable("Products");
        int type = 0;

        public static int _checkExpired = 0; /* Kiểm tra key còn hạn không  = 1: còn hạn*/
        bool _check = false;
        public static string dafault_Image = Path.Combine(Application.StartupPath + @"\Resources\", "Default-image.jpg");
        #endregion

        #region API        
        private const string ApiUrl = "https://knsngoisao.edu.vn/api/v1/me/check?api_token="; //Kiểm tra thông tin tài khoản
        public static string apiUrl_Courses = "https://knsngoisao.edu.vn/api/v1/me/courses?api_token="; //Danh sách khóa học - bài giảng của tài khoản
                                                                                                        // private string api_Key = "37741d319ba8deb7";
        private string api_Key = string.Empty;
        #endregion

        #region Link_API_New
        public static string path_Course = Application.StartupPath + @"\0511\"; //Thư mục chứa thông tin khóa học đã tải
        public static string path_Lession = Application.StartupPath + @"\2505\"; //Thư mục thông tin bài giảng đã tải
        public static string path_XML_Course = Application.StartupPath + @"\0511\Course.xml"; //Nơi lưu thông tin khóa học đã tải
        public static string path_XML_Lession = Application.StartupPath + @"\2505\Lession.xml"; //Nơi lưu thông tin bài giảng đã tải
        public static DataTable dt_DanhSachKhoaHoc = new DataTable("Course");
        public static DataTable dt_DanhSachBaiGiang = new DataTable("Lession");

        public static string API_URL_DanhSachKhoaHoc = "https://knsngoisao.edu.vn/api/v1/me/courses?api_token="; //Danh sách khóa học
        public static string API_URL_DanhSachBaiGiang = "https://knsngoisao.edu.vn/api/v1/me/courses/"; //Danh sách bài giảng của khóa học
        #endregion

        #region Inits

        #region public frm_StartAcademy_Home()
        public frm_StartAcademy_Home()
        {
            InitializeComponent();
        }
        #endregion

        #region private async void Frm_StartAcademy_Load(object sender, EventArgs e)
        private async void Frm_StartAcademy_Load(object sender, EventArgs e)
        {
            #region Kiểm tra Key còn hạn không  
            if (frm_InsertKeyAPI.IsInternetConnected())
            {
                #region Cập nhật lại thông tin token API nếu có internet

                if (!File.Exists(folderPath_XML))
                    return;

                DataTable tbl_Token_API = new DataTable();
                tbl_Token_API = await data_API_Token.GetData_API_Token(folderPath_XML);

                DataTable tbl_auto_checkTokenApi = new DataTable();
                tbl_auto_checkTokenApi = await frm_Extention.AutoUpdata_TokenAPI(frm_InsertKeyAPI.link_check_token_API + tbl_Token_API.Rows[0]["token"].ToString(), frm_InsertKeyAPI.folderPath);

                tbl_auto_checkTokenApi.WriteXml(folderPath_XML);
                #endregion
            }

            _checkExpired = frm_Extention.Check_Expired_FromXML(frm_InsertKeyAPI.folderPath_XML);

            if (_checkExpired != 1)
            {
                MessageBox.Show("Tài khoản đã hết hạn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            #endregion

            #region Lấy token_api
            DataTable tbl_Return_TokenAPI = new DataTable();
            tbl_Return_TokenAPI = await data_API_Token.GetData_API_Token(folderPath_XML);

            api_Key = tbl_Return_TokenAPI.Rows[0]["token"].ToString();
            #endregion

            #region Xác thực Google Drive API
            if (frm_InsertKeyAPI.IsInternetConnected()) //Nếu có Internet
            {
                frm_Extention.credential = await frm_Extention.GetCredentialAsync();
            }
            #endregion

            dt_DanhSachKhoaHoc = null;

            accordionControlElement_danhSach_Click(null, null);

        }
        #endregion

        #endregion

        #region Functions

        #region Danh sach tu API
        private void accordionControlElement_danhSach_Click(object sender, EventArgs e)
        {
            ucDanhSachKhoaHoc ucControl = new ucDanhSachKhoaHoc(ApiUrl, path, path_XML_Course, folderPath_XML);
            xtrascroll.Controls.Clear();
            ucControl.Dock = DockStyle.Fill;
            xtrascroll.Controls.Add(ucControl);
        }
        #endregion

        #region Danh sach da tai
        private async void accordionControlElement_danhSachDaTai_Click(object sender, EventArgs e)
        {
            ucDanhSachBaiGiangDaTai ucControl = new ucDanhSachBaiGiangDaTai(ApiUrl, path, "aa88f7bb81d1def3", path_XML_Lession, folderPath_XML);
            xtrascroll.Controls.Clear();
            ucControl.Dock = DockStyle.Fill;
            xtrascroll.Controls.Add(ucControl);
        }
        #endregion

        #region private void accordionControlElement1_Click(object sender, EventArgs e)
        private void accordionControlElement1_Click(object sender, EventArgs e)
        {
            string directoryPath = @"C:\Users\ADMIN\OneDrive\Máy tính\test encrypt"; // Đường dẫn thư mục bạn muốn tải
            LoadDirectoryToAccordion(directoryPath, accordionControlElement1);
        }
        #endregion

        #region private void LoadDirectoryToAccordion(string directoryPath, AccordionControlElement parentElement)
        private void LoadDirectoryToAccordion(string directoryPath, AccordionControlElement parentElement)
        {
            // Kiểm tra xem thư mục có tồn tại không
            if (!Directory.Exists(directoryPath))
            {
                MessageBox.Show("Thư mục không tồn tại!");
                return;
            }

            // Lấy danh sách các thư mục con
            string[] subDirectories = Directory.GetDirectories(directoryPath);

            // Lặp qua từng thư mục con
            foreach (string subDirectory in subDirectories)
            {
                // Lấy tên thư mục
                string directoryName = Path.GetFileName(subDirectory);

                // Tạo một AccordionControlItem cho thư mục này
                AccordionControlElement directoryItem = new AccordionControlElement();
                directoryItem.Text = directoryName;

                // Thêm thư mục con vào thư mục cha
                parentElement.Elements.Add(directoryItem);

                // Đệ quy để tải các thư mục con
                LoadDirectoryToAccordion(subDirectory, directoryItem);
            }

            // Lấy danh sách các tệp tin trong thư mục
            string[] files = Directory.GetFiles(directoryPath);

            // Lặp qua từng tệp tin và thêm chúng vào AccordionControlElement
            foreach (string file in files)
            {
                // Lấy tên tệp
                string fileName = Path.GetFileName(file);

                // Tạo một AccordionControlElement cho tệp này
                AccordionControlElement fileElement = new AccordionControlElement();
                fileElement.Text = fileName;

                // Thêm tệp tin vào thư mục cha
                parentElement.Elements.Add(fileElement);
            }

        }
        #endregion

        #region private void tileView1_ContextButtonCustomize(object sender, DevExpress.XtraGrid.Views.Tile.TileViewContextButtonCustomizeEventArgs e)
        private void tileView1_ContextButtonCustomize(object sender, DevExpress.XtraGrid.Views.Tile.TileViewContextButtonCustomizeEventArgs e)
        {
            #region Danh sách từ API
            if (type == 0)
            {
                if (e.Item.Name == "Read")
                {
                    e.Item.Visibility = DevExpress.Utils.ContextItemVisibility.Hidden;
                    e.Item.Enabled = false;
                }

                if (e.Item.Name == "Delete")
                {
                    e.Item.Visibility = DevExpress.Utils.ContextItemVisibility.Hidden;
                    e.Item.Enabled = false;
                }

                if (e.Item.Name == "Check")
                {
                    e.Item.Visibility = DevExpress.Utils.ContextItemVisibility.Hidden;
                    e.Item.Enabled = false;
                }

                if (e.Item.Name == "Download")
                {
                    e.Item.Visibility = DevExpress.Utils.ContextItemVisibility.Hidden;
                    e.Item.Enabled = false;
                }

            }
            #endregion

            #region Danh sách đã tải
            if (type == 1)
            {
                if (e.Item.Name == "Download")
                {
                    //e.Item.Visibility = DevExpress.Utils.ContextItemVisibility.Hidden;
                    //e.Item.Enabled = false;
                }

                if (e.Item.Name == "Check")
                {
                    e.Item.Visibility = DevExpress.Utils.ContextItemVisibility.Hidden;
                    e.Item.Enabled = false;
                }

                //if (e.Item.Name == "Check")
                //{
                //    CheckContextButton checkItem = e.Item as CheckContextButton;
                //    checkItem.Checked = _check;

                //}

            }
            #endregion
        }
        #endregion

        #region private void tileView1_DoubleClick(object sender, EventArgs e)
        private void tileView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                //if (type == 1)
                //{
                //    fileName = tileView1.GetFocusedDataRow()["file_url_Path"].ToString();
                //    LoadScreen frm = new LoadScreen();
                //    frm.fileName = fileName;
                //    frm.frm_key = FormMain.key;
                //    frm.ShowDialog();
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Nhập key
        private void InsertKey(object sender, EventArgs e)
        {
            frm_InsertKeyAPI frm = new frm_InsertKeyAPI();
            frm.ShowDialog();

            Frm_StartAcademy_Load(null, null);
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


        #endregion
    }
}