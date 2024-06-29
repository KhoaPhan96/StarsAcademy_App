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

namespace Demo_Encrypt_LoadFile.Feature
{
    public partial class frm_StartAcademy_Test : Form
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
        public DataTable dt_DanhSachKhoaHoc = new DataTable("Course");
        public DataTable dt_DanhSachBaiGiang = new DataTable("Lession");

        public static string API_URL_DanhSachKhoaHoc = "https://knsngoisao.edu.vn/api/v1/me/courses?api_token="; //Danh sách khóa học
        public static string API_URL_DanhSachBaiGiang = "https://knsngoisao.edu.vn/api/v1/me/courses/"; //Danh sách bài giảng của khóa học
        public static string API_URL_ChiTietBaiGiang = "https://knsngoisao.edu.vn/api/v1/me/lessions/"; //Chi tiết bài giảng của khóa học
        #endregion

        #region Inits

        #region public frm_StartAcademy_Test()
        public frm_StartAcademy_Test()
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

            chk_CheckAll.Checked = false;
            accordionControlElement_danhSach_Click(null, null);
        }
        #endregion

        #endregion

        #region Functions

        #region Danh sach tu API
        private async void accordionControlElement_danhSach_Click(object sender, EventArgs e)
        {
            try
            {
                chk_CheckAll.Checked = false;

                layout_CheckAll.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layout_Download.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

                this.Text = "Danh sách bài giảng khóa học";
                type = 0;

                #region Tạm bỏ
                #region Kiểm tra Key còn hạn không  
                //if (frm_InsertKeyAPI.IsInternetConnected())
                //{
                //    #region Cập nhật lại thông tin token API nếu có internet

                //    if (!File.Exists(folderPath_XML))
                //        return;

                //    DataTable tbl_Token_API = new DataTable();
                //    tbl_Token_API = await data_API_Token.GetData_API_Token(folderPath_XML);

                //    DataTable tbl_auto_checkTokenApi = new DataTable();
                //    tbl_auto_checkTokenApi = await frm_Extention.AutoUpdata_TokenAPI(frm_InsertKeyAPI.link_check_token_API + tbl_Token_API.Rows[0]["token"].ToString(), frm_InsertKeyAPI.folderPath);

                //    tbl_auto_checkTokenApi.WriteXml(folderPath_XML);
                //    #endregion
                //}

                //_checkExpired = frm_Extention.Check_Expired_FromXML(frm_InsertKeyAPI.folderPath_XML);

                //if (_checkExpired != 1)
                //{
                //    MessageBox.Show("Tài khoản đã hết hạn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    return;
                //}

                #endregion
                #endregion

                #region Lấy token_api

                if (!File.Exists(folderPath_XML))
                {
                    return;
                }

                DataTable tbl_Return_TokenAPI = new DataTable();
                tbl_Return_TokenAPI = await data_API_Token.GetData_API_Token(folderPath_XML);

                api_Key = tbl_Return_TokenAPI.Rows[0]["token"].ToString();
                #endregion



                //#region Hiển thị lên lưới   
                ////Kiểm tra nếu chưa tồn tại bài giảng thì hiển thị, đã tồn tại thì null
                //dt_API_XML = await DanhSachBaiGiang_Show.GetDanhSach_NotExists_API(ApiUrl, api_Key, path);

                //gridControl1.DataSource = dt_API_XML;
                //tileView1.Columns["Image_Lession_Show"].ColumnEdit = repositoryItemPictureEdit;
                //#endregion

                string t = string.Empty;

                Stopwatch stopwatch = new Stopwatch();

                stopwatch.Start();

                //dt_DanhSachKhoaHoc = await DanhSachKhoaHoc_Show.GetDanhSachKhoaHoc_API(ApiUrl, api_Key, path);
                //dt_DanhSachKhoaHoc = DanhSachKhoaHoc_Show.GetDanhSachKhoaHoc_DaTai(path_XML_Course, api_Key);

                dt_DanhSachKhoaHoc = await DanhSachKhoaHoc_Show.GetDanhSachKhoaHoc_Show(ApiUrl, api_Key, path, path_XML_Course, null);
                gridControl1.DataSource = dt_DanhSachKhoaHoc;
                stopwatch.Stop();
                Console.WriteLine("Time taken until khoa hoc: " + stopwatch.ElapsedMilliseconds + " ms");

                string test = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Danh sach da tai
        private async void accordionControlElement_danhSachDaTai_Click(object sender, EventArgs e)
        {
            try
            {
                String test = Application.StartupPath;
                layout_CheckAll.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layout_Download.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

                this.Text = "Danh sách bài giảng khóa học đã tải";
                type = 1;
                string token_api = string.Empty;

                #region Tạm bỏ
                #region Kiểm tra Key còn hạn không  
                //if (frm_InsertKeyAPI.IsInternetConnected())
                //{
                //    #region Cập nhật lại thông tin token API nếu có internet

                //    if (!File.Exists(folderPath_XML))
                //        return;

                //    DataTable tbl_Token_API = new DataTable();
                //    tbl_Token_API = await data_API_Token.GetData_API_Token(folderPath_XML);

                //    DataTable tbl_auto_checkTokenApi = new DataTable();
                //    tbl_auto_checkTokenApi = await frm_Extention.AutoUpdata_TokenAPI(frm_InsertKeyAPI.link_check_token_API + tbl_Token_API.Rows[0]["token"].ToString(), frm_InsertKeyAPI.folderPath);

                //    tbl_auto_checkTokenApi.WriteXml(folderPath_XML);
                //    #endregion
                //}

                //_checkExpired = frm_Extention.Check_Expired_FromXML(frm_InsertKeyAPI.folderPath_XML);

                //if (_checkExpired != 1)
                //{
                //    MessageBox.Show("Tài khoản đã hết hạn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    return;
                //}

                #endregion
                #endregion

                Stopwatch stopwatch = new Stopwatch();

                stopwatch.Start();

                #region Hiển thị lên lưới
                //dt_DanhSachDaTai = DanhSachBaiGiang_Show.GetDanhSachDaTai(path_XML, api_Key);
                dt_DanhSachBaiGiang = await Functions.DanhSachBaiGiang_Show.GetDanhSachBaiGiang_Show(ApiUrl, api_Key, path, "7d3f8d674b77b1fa", path_XML_Lession, null);
                gridControl1.DataSource = dt_DanhSachBaiGiang;
                //tileView1.Columns["Image_Lession_Show"].ColumnEdit = repositoryItemPictureEdit;

                stopwatch.Stop();
                Console.WriteLine("Time taken until bai giang: " + stopwatch.ElapsedMilliseconds + " ms");

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
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

        #region private async void tileView1_ContextButtonClick(object sender, DevExpress.Utils.ContextItemClickEventArgs e)
        private async void tileView1_ContextButtonClick(object sender, DevExpress.Utils.ContextItemClickEventArgs e)
        {
            try
            {
                #region Focus tileview ngay tại bị trí click chuột
                TileView view_Read = sender as TileView;
                TileViewItem item_Read = e.DataItem as TileViewItem;
                if (item_Read != null)
                    view_Read.FocusedRowHandle = item_Read.RowHandle;
                #endregion

                #region Danh sách bài giảng load từ API
                if (type == 0)
                {
                    switch(e.Item.Name)
                    {
                        case "Download":

                            ////Lấy thư mục lưu file (thư mục khóa học - bài giảng)
                            ////Lây danh sách khóa học - theo mã uuid
                            ////Lấy danh sách bài giảng - theo mã uuid_KhoaHoc %% uuid_baigiang

                            //string uuid_Course = tileView1.GetFocusedDataRow()["uuid_Course"].ToString();
                            //string uuid = tileView1.GetFocusedDataRow()["uuid"].ToString();

                            //DataTable dt_DanhSachKhoaHoc_Selected = new DataTable("Course");
                            //dt_DanhSachKhoaHoc_Selected = dt_DanhSachKhoaHoc.Clone();

                            //DataTable dt_DanhSachBaiGiang_Selected = new DataTable("Lession");
                            //dt_DanhSachBaiGiang_Selected = dt_DanhSachBaiGiang.Clone();

                            //foreach (DataRow dr_KhoaHoc in dt_DanhSachKhoaHoc.Rows)
                            //{
                            //    if(dr_KhoaHoc["uuid"].ToString() == uuid_Course)
                            //    {
                            //        dt_DanhSachKhoaHoc_Selected.Rows.Add(dr_KhoaHoc["id"].ToString()
                            //                                             , dr_KhoaHoc["title"].ToString()
                            //                                             , dr_KhoaHoc["description"].ToString()
                            //                                             , dr_KhoaHoc["uuid"].ToString()
                            //                                             , dr_KhoaHoc["image_url"].ToString()
                            //                                             , dr_KhoaHoc["image_Path"].ToString()
                            //                                             , dr_KhoaHoc["image_Show"].ToString()
                            //                                             , dr_KhoaHoc["meta_image_url"].ToString()
                            //                                             , dr_KhoaHoc["meta_image_Path"].ToString()
                            //                                             , dr_KhoaHoc["meta_image_Show"].ToString()
                            //                                             , dr_KhoaHoc["IsSelected"].ToString()
                            //                                            );
                            //    }    
                            //}    

                            //foreach(DataRow dr_BaiGiang in dt_DanhSachBaiGiang.Rows)
                            //{
                            //    if(dr_BaiGiang["uuid_Course"].ToString() == uuid_Course && dr_BaiGiang["uuid"].ToString() == uuid)
                            //    {
                            //        dt_DanhSachBaiGiang_Selected.Rows.Add(dr_BaiGiang["uuid_Course"].ToString()
                            //                                              , dr_BaiGiang["id"].ToString()
                            //                                              , dr_BaiGiang["uuid"].ToString()
                            //                                              , dr_BaiGiang["title"].ToString()
                            //                                              , dr_BaiGiang["description"].ToString()
                            //                                              , dr_BaiGiang["image_url"].ToString()
                            //                                              , dr_BaiGiang["image_Path"].ToString()
                            //                                              , dr_BaiGiang["image_Show"].ToString()
                            //                                              , dr_BaiGiang["meta_image_url"].ToString()
                            //                                              , dr_BaiGiang["meta_image_Path"].ToString()
                            //                                              , dr_BaiGiang["meta_image_Show"].ToString()
                            //                                              , dr_BaiGiang["meta_image_Show"].ToString()
                            //                                              , dr_BaiGiang["file"].ToString()
                            //                                              , dr_BaiGiang["file_Path"].ToString()
                            //                                              , dr_BaiGiang["IsDownloaded"].ToString()
                            //                                              , dr_BaiGiang["IsSelected"].ToString()                                                                         
                            //                                             );
                            //    }    
                            //}    

                            //frm_Download_New.Download_New(path_Course, path_Lession, dt_DanhSachKhoaHoc_Selected, dt_DanhSachBaiGiang_Selected);

                            #region bỏ

                            //#region Kiểm tra Key còn hạn không  

                            //_checkExpired = frm_Extention.Check_Expired_FromXML(frm_InsertKeyAPI.folderPath_XML);

                            //if (_checkExpired != 1)
                            //{
                            //    MessageBox.Show("Tài khoản đã hết hạn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            //    return;
                            //}
                            //#endregion

                            //#region Tải file

                            //DataRow rowsToDelete = tileView1.GetFocusedDataRow();
                            //DataTable dt_Select_XML = new DataTable("Products");
                            //dt_Select_XML = rowsToDelete.Table.Clone();
                            //dt_Select_XML.ImportRow(rowsToDelete);


                            //frm_Extention.Download_Ver2(path, dt_Select_XML, "", path_XML, "", "", "", dt_New_Save);

                            ////frm_Extention.Download_Ver2(path, dt_Select_XML, "", path_XML, "", "", "", dt_New_Save);
                            //#endregion 

                            //#region Tạm bỏ
                            ////frm_Extention.Download(path, 
                            ////                       dt_API_XML, 
                            ////                       tileView1.GetFocusedDataRow()["uuid"].ToString(),
                            ////                       path_XML,
                            ////                       tileView1.GetFocusedDataRow()["image_url_KhoaHoc"].ToString(),
                            ////                       tileView1.GetFocusedDataRow()["file_url"].ToString(),
                            ////                       tileView1.GetFocusedDataRow()["image_url"].ToString()
                            ////                       );

                            ////if (dt_API_XML.Columns.Contains("Image_Course_Show"))
                            ////    dt_API_XML.Columns.Remove("Image_Course_Show");

                            ////if (dt_API_XML.Columns.Contains("Image_Lession_Show"))
                            ////    dt_API_XML.Columns.Remove("Image_Lession_Show");

                            ////if (dt_API_XML.Columns.Contains("IsSelected"))
                            ////    dt_API_XML.Columns.Remove("IsSelected");

                            ////dt_API_XML.WriteXml(path_XML);

                            //#endregion

                            //accordionControlElement_danhSach_Click(null, null);
                            //MessageBox.Show("Tải tệp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            //#region bỏ

                            ////#region Tải về

                            //////Tạo thư mục khóa học (chứa các bài giảng)
                            //////string folderPath = Application.StartupPath + @"\File\";

                            ////if (!Directory.Exists(path))
                            ////{
                            ////    Directory.CreateDirectory(path);
                            ////}



                            ////#region Thêm vào XML
                            //////string filePath_XML = Path.Combine(Application.StartupPath + @"\File\", "data.xml");

                            //////DataTable dt_XML = dt_API_XML;
                            //////dt_XML.Clone();

                            ////DataTable dt_XML = dt_API_XML.Clone();
                            ////string uuid = tileView1.GetFocusedDataRow()["uuid"].ToString();

                            ////// Lọc các hàng trong dt_API_XML dựa trên giá trị của uuid và sao chép chúng vào dt_XML
                            ////DataRow[] filteredRows = dt_API_XML.Select("uuid = '" + uuid + "'");
                            ////foreach (DataRow row in filteredRows)
                            ////{
                            ////    dt_XML.ImportRow(row);
                            ////}

                            //////string uuid = tileView1.GetFocusedDataRow()["uuid"].ToString();
                            //////dt_API_XML.Select("uuid =  " + uuid).CopyToDataTable(dt_XML, LoadOption.PreserveChanges);

                            ////// Loại bỏ cột Bitmap trước khi ghi lại DataTable vào tệp XML
                            ////dt_XML.Columns.Remove("Image_Course_Show");
                            ////dt_XML.Columns.Remove("Image_Lession_Show");
                            ////dt_XML.Columns.Remove("IsSelected");

                            //////dt_XML.WriteXml(filePath_XML);
                            ////dt_XML.WriteXml(path_XML);
                            ////#endregion

                            ////#region Tải file về máy

                            //////Link ảnh
                            ////string image_url_Course = tileView1.GetFocusedDataRow()["image_url_KhoaHoc"].ToString();
                            //////Lấy tên file + phần mở rộng
                            ////string fileName_image_url_Course = Path.GetFileName(image_url_Course);
                            //////Nơi lưu ảnh
                            ////string image_url_Course_Path = Application.StartupPath + @"\File\" + fileName_image_url_Course;
                            //////Tên sau khi mã hóa
                            ////string image_url_Course_Path_encode = Encrypt_FileName.EncodeFileNameInPath(fileName_image_url_Course + ".enc");

                            ////if (!File.Exists(image_url_Course_Path_encode))
                            ////{
                            ////    await DownloadFileAsync(image_url_Course, image_url_Course_Path);

                            ////    //Mã hóa dữ liệu
                            ////    ENC.FileEncrypt(image_url_Course_Path, image_url_Course_Path + ".enc", FormMain.key);

                            ////    //Mã hóa tên file
                            ////    string originalFilePath = image_url_Course_Path + ".enc";
                            ////    string encodedFilePath = Encrypt_FileName.EncodeFileNameInPath(originalFilePath);

                            ////    if (File.Exists(encodedFilePath))
                            ////    {
                            ////        File.Delete(encodedFilePath);
                            ////    }

                            ////    //Rename
                            ////    Encrypt_FileName.RenameFile(originalFilePath, encodedFilePath);

                            ////}

                            //////Lây link download
                            ////string fileUrl = tileView1.GetFocusedDataRow()["file_url"].ToString();
                            ////string image_url = tileView1.GetFocusedDataRow()["image_url"].ToString();

                            //////Tách tên file
                            ////string fileName = Path.GetFileName(fileUrl);
                            ////string fileName_Image = Path.GetFileName(image_url);

                            //////Lấy đuôi mở rộng
                            ////string extension = Path.GetExtension(fileUrl);

                            //////Path

                            /////* VER1 Cách cũ: file nằm trong thư mục của khóa học
                            ////string file_Path = folderPath + "\\" + item.uuid + "_" + fileName;
                            ////string image_thumbnail_Path = folderPath + "\\" + item.uuid  + "thumbnailApp_" + fileName_Image;
                            ////*/

                            /////* VER 2 để dùng cho file XML */
                            ////string file_Path = path + @"\" + fileName;
                            ////string image_thumbnail_Path = path + @"\" + fileName_Image;


                            ////string file_Path_encode = Encrypt_FileName.EncodeFileNameInPath(file_Path + ".enc");
                            ////string image_thumbnail_Path_encode = Encrypt_FileName.EncodeFileNameInPath(image_thumbnail_Path + ".enc");

                            //////Tải và mã hóa

                            //////File dữ liệu
                            ////if (!File.Exists(file_Path_encode))
                            ////{
                            ////    await DownloadFileAsync(fileUrl, file_Path);

                            ////    //Mã hóa dữ liệu
                            ////    ENC.FileEncrypt(file_Path, file_Path + ".enc", FormMain.key);

                            ////    //Mã hóa tên file
                            ////    string originalFilePath = file_Path + ".enc";
                            ////    string encodedFilePath = Encrypt_FileName.EncodeFileNameInPath(originalFilePath);

                            ////    if (File.Exists(encodedFilePath))
                            ////    {
                            ////        File.Delete(encodedFilePath);
                            ////    }

                            ////    //Rename
                            ////    Encrypt_FileName.RenameFile(originalFilePath, encodedFilePath);
                            ////}

                            //////File Thumbnail
                            ////if (!File.Exists(image_thumbnail_Path_encode))
                            ////{
                            ////    //await DownloadFileAsync(image_url, image_thumbnail_Path);
                            ////    await DownloadFileAsync(image_url, image_thumbnail_Path);

                            ////    //Mã hóa dữ liệu
                            ////    ENC.FileEncrypt(image_thumbnail_Path, image_thumbnail_Path + ".enc", FormMain.key);

                            ////    //Mã hóa tên file
                            ////    string originalFilePath_Thumbnail = image_thumbnail_Path + ".enc";
                            ////    string encodedFilePath_Thumbnail = Encrypt_FileName.EncodeFileNameInPath(originalFilePath_Thumbnail);
                            ////    if (File.Exists(encodedFilePath_Thumbnail))
                            ////    {
                            ////        File.Delete(encodedFilePath_Thumbnail);
                            ////    }

                            ////    //Rename
                            ////    Encrypt_FileName.RenameFile(originalFilePath_Thumbnail, encodedFilePath_Thumbnail);
                            ////}

                            ////accordionControlElement_danhSach_Click(null, null);
                            ////MessageBox.Show("Tải tệp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            ////#endregion


                            ////#endregion

                            //#endregion


                           

                            #endregion

                            break;
                        case "Check":
                            CheckContextButton checkItem1 = e.Item as CheckContextButton;
                            if (checkItem1.Checked == true)
                                _check = true;
                            else
                                _check = false;                          
                            break;
                        default:
                            break;
                    }    
                }
                #endregion

                #region Danh sách bài giảng đã tải
                if (type == 1) 
                {
                    #region Kiểm tra Key còn hạn không  

                    _checkExpired = frm_Extention.Check_Expired_FromXML(frm_InsertKeyAPI.folderPath_XML);

                    if (_checkExpired != 1)
                    {
                        MessageBox.Show("Tài khoản đã hết hạn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    #endregion

                    switch (e.Item.Name)
                    {
                        case "Download":
                            string uuid_Course = tileView1.GetFocusedDataRow()["uuid_Course"].ToString();
                            string uuid_ = tileView1.GetFocusedDataRow()["uuid"].ToString();

                            DataTable dt_DanhSachKhoaHoc_Selected = new DataTable("Courses");
                            dt_DanhSachKhoaHoc_Selected = dt_DanhSachKhoaHoc.Clone();

                            DataTable dt_DanhSachBaiGiang_Selected = new DataTable("Lessions");
                            dt_DanhSachBaiGiang_Selected = dt_DanhSachBaiGiang.Clone();

                            foreach (DataRow dr_KhoaHoc in dt_DanhSachKhoaHoc.Rows)
                            {
                                byte[] imageBytes;

                                if (dr_KhoaHoc["uuid"].ToString() == uuid_Course)
                                {
                                    dt_DanhSachKhoaHoc_Selected.Rows.Add(dr_KhoaHoc["id"].ToString()
                                                                         , dr_KhoaHoc["title"].ToString()
                                                                         , dr_KhoaHoc["description"].ToString()
                                                                         , dr_KhoaHoc["uuid"].ToString()
                                                                         , dr_KhoaHoc["image_url"].ToString()
                                                                         , dr_KhoaHoc["image_Path"].ToString()
                                                                         , dr_KhoaHoc["image_Show"]
                                                                         , dr_KhoaHoc["meta_image_url"].ToString()
                                                                         , dr_KhoaHoc["meta_image_Path"].ToString()
                                                                         , dr_KhoaHoc["meta_image_Show"]
                                                                         , dr_KhoaHoc["IsSelected"].ToString()
                                                                        );
                                }
                            }

                            foreach (DataRow dr_BaiGiang in dt_DanhSachBaiGiang.Rows)
                            {
                                if (dr_BaiGiang["uuid_Course"].ToString() == uuid_Course && dr_BaiGiang["uuid"].ToString() == uuid_)
                                {
                                    dt_DanhSachBaiGiang_Selected.Rows.Add(dr_BaiGiang["uuid_Course"].ToString()
                                                                          , dr_BaiGiang["id"].ToString()
                                                                          , dr_BaiGiang["uuid"].ToString()
                                                                          , dr_BaiGiang["title"].ToString()
                                                                          , dr_BaiGiang["description"].ToString()
                                                                          , dr_BaiGiang["image_url"].ToString()
                                                                          , dr_BaiGiang["image_Path"].ToString()
                                                                          , dr_BaiGiang["image_Show"]
                                                                          , dr_BaiGiang["meta_image_url"].ToString()
                                                                          , dr_BaiGiang["meta_image_Path"].ToString()
                                                                          , dr_BaiGiang["meta_image_Show"]
                                                                          , dr_BaiGiang["file"].ToString()
                                                                          , dr_BaiGiang["file_Path"].ToString()
                                                                          , dr_BaiGiang["IsDownloaded"].ToString()
                                                                          , dr_BaiGiang["IsSelected"].ToString()
                                                                         );
                                }
                            }

                            await frm_Download_New.Download_New(path_Course, path_XML_Course, path_Lession, path_XML_Lession, dt_DanhSachKhoaHoc_Selected, dt_DanhSachBaiGiang_Selected);
                            {
                                MessageBox.Show("Tải thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            break;

                        case "Read":
                            #region Đọc file
                            fileName = tileView1.GetFocusedDataRow()["file_Path"].ToString();
                            LoadScreen frm = new LoadScreen();
                            frm.fileName = fileName;
                            frm.frm_key = FormMain.key;
                            frm.ShowDialog();
                            #endregion
                            break;
                        case "Delete":
                            #region Xóa file
                            //Focus tileview ngay tại bị trí click chuột
                            TileView view_Delete = sender as TileView;
                            TileViewItem item_Delete = e.DataItem as TileViewItem;
                            if (view_Delete != null)
                                view_Delete.FocusedRowHandle = item_Delete.RowHandle;

                            #region Xóa file
                            string file_url_Path = tileView1.GetFocusedDataRow()["file_Path"].ToString();
                            string image_Path_KhoaHoc = tileView1.GetFocusedDataRow()["meta_image_url_Path_KhoaHoc"].ToString();
                            string image_url_Path = tileView1.GetFocusedDataRow()["meta_image_url_Path"].ToString();
                            string uuid = tileView1.GetFocusedDataRow()["uuid"].ToString();

                            if (File.Exists(image_Path_KhoaHoc))
                            {
                                File.Delete(image_Path_KhoaHoc);
                            }

                            if (File.Exists(image_url_Path))
                            {
                                File.Delete(image_url_Path);
                            }

                            if (File.Exists(file_url_Path))
                            {
                                File.Delete(file_url_Path);
                            }
                            #endregion

                            //DataRow rowsToDelete = tileView1.GetFocusedDataRow();
                            //if (rowsToDelete != null)
                            //{
                            //    rowsToDelete.Delete();
                            //}

                            XDocument doc = XDocument.Load(path_XML);
                            doc.Descendants("Products")
                               .Where(p => (string)p.Element("uuid") == uuid)
                               .Remove();

                            doc.Save(path_XML);

                            //// Loại bỏ cột Bitmap trước khi ghi lại DataTable vào tệp XML
                            //dt_DanhSachDaTai.Columns.Remove("Image_Course_Show");
                            //dt_DanhSachDaTai.Columns.Remove("Image_Lession_Show");
                            //dt_DanhSachDaTai.Columns.Remove("IsSelected");
                            //dt_DanhSachDaTai.Columns.Remove("Read");
                            //dt_DanhSachDaTai.Columns.Remove("Delete");

                            //dt_DanhSachDaTai.WriteXml(Application.StartupPath + @"\File\data.xml");

                            MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            accordionControlElement_danhSachDaTai_Click(null, null);
                            #endregion
                            break;                       
                        default:
                            break;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            catch(Exception ex)
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
        }
        #endregion

        #region private void tileView1_ItemClick(object sender, TileViewItemClickEventArgs e)
        private void tileView1_ItemClick(object sender, TileViewItemClickEventArgs e)
        {
            try
            {
                bool check = (bool)tileView1.GetRowCellValue(e.Item.RowHandle, "IsSelected");
                tileView1.SetRowCellValue(e.Item.RowHandle, "IsSelected", !check);
                //e.Item.Checked = !e.Item.Checked;


                //string uuid = tileView1.GetFocusedDataRow()["uuid"].ToString();
                //string sss = tileView1.GetFocusedDataRow()["IsSelected"].ToString();

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
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

        #region private void chk_CheckAll_CheckedChanged(object sender, EventArgs e)
        private void chk_CheckAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                // Lấy index của cột mà bạn muốn cập nhật
                int columnIndex = tileView1.Columns["IsSelected"].VisibleIndex;

                for(int i = 0; i < tileView1.RowCount; i++)
                {
                    //// Lấy handle của hàng hiện tại (ví dụ: hàng đầu tiên)
                    int rowHandle = tileView1.GetVisibleRowHandle(i);

                    // Lấy dữ liệu của hàng tương ứng
                    DataRowView rowData = (DataRowView)tileView1.GetRow(rowHandle);

                    // Thay đổi giá trị của cột trong dữ liệu
                    rowData["IsSelected"] = chk_CheckAll.EditValue.ToString();

                    // Gọi RefreshRow để cập nhật giao diện người dùng
                    tileView1.RefreshRow(rowHandle);
                }                                                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
      
        #endregion

        #region Check all - Download
        private async void btn_Download_Click(object sender, EventArgs e)
        {
            try
            {
                //Tạo thư mục khóa học (chứa các bài giảng)
                string folderPath = Application.StartupPath + @"\File\";

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                #region Kiểm tra Key còn hạn không  

                _checkExpired = frm_Extention.Check_Expired_FromXML(frm_InsertKeyAPI.folderPath_XML);

                if (_checkExpired != 1)
                {
                    MessageBox.Show("Tài khoản đã hết hạn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                #endregion

                #region Thêm vào XML - Tải file

                DataRow[] filteredRows = dt_API_XML.Select("IsSelected = '" + true + "'");

                if (filteredRows.Length == 0)
                {
                    MessageBox.Show("Chưa chọn khóa học để tải", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DataTable dt_Select_XML = dt_API_XML.Clone();
                Boolean isSelected = bool.Parse(tileView1.GetFocusedDataRow()["IsSelected"].ToString());

                foreach (DataRow row in filteredRows)
                {
                    dt_Select_XML.ImportRow(row);
                }

                if (dt_Select_XML.Rows.Count == 0)
                    return;

                try
                {
                    frm_Extention.Download_Ver2(path, dt_Select_XML, "", path_XML, "", "", "", dt_New_Save);

                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                #endregion

                #region Tạm bỏ

                ///////////////////////////////////////////////////////////
                /////
                ////DataTable dt_Select_XML = dt_API_XML.Clone();
                ////Boolean isSelected = bool.Parse(tileView1.GetFocusedDataRow()["IsSelected"].ToString());

                ////DataRow[] filteredRows = dt_API_XML.Select("IsSelected = '" + true + "'");

                ////if(filteredRows.Length == 0)
                ////{
                ////    MessageBox.Show("Chưa chọn khóa học để tải", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ////}    

                ////foreach (DataRow row in filteredRows)
                ////{
                ////    dt_Select_XML.ImportRow(row);
                ////}

                ////if (dt_Select_XML.Rows.Count == 0)
                ////    return;


                //DataTable dt_XML = dt_Select_XML.Clone();

                //foreach (DataRow row in dt_Select_XML.Rows)
                //{
                //    dt_XML.ImportRow(row);
                //}

                //dt_XML.Columns.Remove("Image_Course_Show");
                //dt_XML.Columns.Remove("Image_Lession_Show");
                //dt_XML.Columns.Remove("IsSelected");

                //// Đọc dữ liệu từ tệp XML vào một DataTable mới
                ////DataTable existingDataTable = new DataTable();
                ////existingDataTable.ReadXml(path_XML);


                //DataTable existingDataTable = new DataTable();

                //DataSet dataSet = new DataSet();

                ////if (!File.Exists(path_XML))
                ////    return null;

                //if (File.Exists(path_XML))
                //{
                //    // Đọc tệp XML vào DataSet
                //    dataSet.ReadXml(path_XML);
                //    try
                //    {
                //        existingDataTable = dataSet.Tables[0];
                //    }
                //    catch (Exception ex)
                //    {

                //    }

                //    if (existingDataTable.Rows.Count == 0)
                //    {
                //        foreach (DataRow dr in dt_XML.Rows)
                //        {
                //            frm_Extention.Download(path,
                //                                          dt_XML,
                //                                          dr["uuid"].ToString(),
                //                                          path_XML,
                //                                          dr["image_url_KhoaHoc"].ToString(),
                //                                          dr["file_url"].ToString(),
                //                                          dr["image_url"].ToString()
                //                                          );


                //        }

                //        if (dt_XML.Columns.Contains("Image_Course_Show"))
                //            dt_XML.Columns.Remove("Image_Course_Show");

                //        if (dt_XML.Columns.Contains("Image_Lession_Show"))
                //            dt_XML.Columns.Remove("Image_Lession_Show");

                //        if (dt_XML.Columns.Contains("IsSelected"))
                //            dt_XML.Columns.Remove("IsSelected");

                //        dt_XML.WriteXml(path_XML);
                //    }
                //    else
                //    {

                //        List<DataRow> dtExists = new List<DataRow>();
                //        DataTable dt_New_Download = new DataTable();
                //        dt_New_Download = existingDataTable.Clone();

                //        foreach (DataRow dr_exists in existingDataTable.Rows)
                //        {
                //            dtExists.Add(dr_exists);
                //        }

                //        foreach (DataRow dr_New in dt_XML.Rows)
                //        {
                //            foreach (DataRow dr_exists in existingDataTable.Rows)
                //            {
                //                if (dr_exists["uuid_KhoaHoc"].ToString() != dr_New["uuid_KhoaHoc"].ToString() && dr_exists["uuid"].ToString() != dr_New["uuid"].ToString())
                //                {
                //                    dtExists.Add(dr_New); //insert thêm vào file xml - không ghi đè
                //                    dt_New_Download.ImportRow(dr_New); //dt này dùng để tải file về máy - không ghi đè file đã tải
                //                }
                //            }
                //        }

                //        #region Tải file máy                       
                //        if (dt_New_Download.Rows.Count > 0)
                //        {
                //            foreach (DataRow dr_DL in dt_New_Download.Rows)
                //            {
                //                frm_Extention.Download(path,
                //                                               dt_New_Download,
                //                                               dr_DL["uuid"].ToString(),
                //                                               path_XML,
                //                                               dr_DL["image_url_KhoaHoc"].ToString(),
                //                                               dr_DL["file_url"].ToString(),
                //                                               dr_DL["image_url"].ToString()
                //                                               );
                //            }

                //            // Đảm bảo có ít nhất một DataRow trong List<DataRow>
                //            if (dtExists.Count > 0)
                //            {
                //                // Thêm các cột từ DataRow đầu tiên vào DataTable
                //                foreach (DataColumn column in dtExists[0].Table.Columns)
                //                {
                //                    dt_New_Save.Columns.Add(column.ColumnName, column.DataType);
                //                }

                //                // Thêm các DataRow từ List<DataRow> vào DataTable
                //                foreach (DataRow row in dtExists)
                //                {
                //                    dt_New_Save.ImportRow(row);
                //                }
                //            }

                //            if (dt_New_Save.Columns.Contains("Image_Course_Show"))
                //                dt_New_Save.Columns.Remove("Image_Course_Show");

                //            if (dt_New_Save.Columns.Contains("Image_Lession_Show"))
                //                dt_New_Save.Columns.Remove("Image_Lession_Show");

                //            if (dt_New_Save.Columns.Contains("IsSelected"))
                //                dt_New_Save.Columns.Remove("IsSelected");

                //            dt_New_Save.WriteXml(path_XML);


                //            #region Bỏ
                //            //if (!Directory.Exists(path))
                //            //{
                //            //    Directory.CreateDirectory(path);
                //            //}

                //            //#region Kiểm tra Key còn hạn không  

                //            //_checkExpired = frm_Extention.Check_Expired_FromXML(frm_InsertKeyAPI.folderPath_XML);

                //            //if (_checkExpired != 1)
                //            //{
                //            //    MessageBox.Show("Tài khoản đã hết hạn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //            //    return;
                //            //}
                //            //#endregion

                //            //foreach (DataRow dr in dt_New_Download.Rows)
                //            //{
                //            //    //Link ảnh
                //            //    string image_url_Course = dr["image_url_KhoaHoc"].ToString();
                //            //    //Lấy tên file + phần mở rộng
                //            //    string fileName_image_url_Course = Path.GetFileName(image_url_Course);
                //            //    //Nơi lưu ảnh
                //            //    string image_url_Course_Path = Application.StartupPath + @"\File\" + fileName_image_url_Course;
                //            //    //Tên sau khi mã hóa
                //            //    string image_url_Course_Path_encode = Encrypt_FileName.EncodeFileNameInPath(fileName_image_url_Course + ".enc");

                //            //    if (!File.Exists(image_url_Course_Path_encode))
                //            //    {
                //            //        await DownloadFileAsync(image_url_Course, image_url_Course_Path);

                //            //        //Mã hóa dữ liệu
                //            //        ENC.FileEncrypt(image_url_Course_Path, image_url_Course_Path + ".enc", FormMain.key);

                //            //        //Mã hóa tên file
                //            //        string originalFilePath = image_url_Course_Path + ".enc";
                //            //        string encodedFilePath = Encrypt_FileName.EncodeFileNameInPath(originalFilePath);

                //            //        if (File.Exists(encodedFilePath))
                //            //        {
                //            //            File.Delete(encodedFilePath);
                //            //        }

                //            //        //Rename
                //            //        Encrypt_FileName.RenameFile(originalFilePath, encodedFilePath);

                //            //    }

                //            //    //Lây link download
                //            //    string fileUrl = dr["file_url"].ToString();
                //            //    string image_url = dr["image_url"].ToString();

                //            //    //Tách tên file
                //            //    string fileName = Path.GetFileName(fileUrl);
                //            //    string fileName_Image = Path.GetFileName(image_url);

                //            //    //Lấy đuôi mở rộng
                //            //    string extension = Path.GetExtension(fileUrl);

                //            //    //Path

                //            //    /* VER1 Cách cũ: file nằm trong thư mục của khóa học
                //            //    string file_Path = folderPath + "\\" + item.uuid + "_" + fileName;
                //            //    string image_thumbnail_Path = folderPath + "\\" + item.uuid  + "thumbnailApp_" + fileName_Image;
                //            //    */

                //            //    /* VER 2 để dùng cho file XML */
                //            //    string file_Path = path + @"\" + fileName;
                //            //    string image_thumbnail_Path = path + @"\" + fileName_Image;


                //            //    string file_Path_encode = Encrypt_FileName.EncodeFileNameInPath(file_Path + ".enc");
                //            //    string image_thumbnail_Path_encode = Encrypt_FileName.EncodeFileNameInPath(image_thumbnail_Path + ".enc");

                //            //    //Tải và mã hóa

                //            //    //File dữ liệu
                //            //    if (!File.Exists(file_Path_encode))
                //            //    {
                //            //        await DownloadFileAsync(fileUrl, file_Path);

                //            //        //Mã hóa dữ liệu
                //            //        ENC.FileEncrypt(file_Path, file_Path + ".enc", FormMain.key);

                //            //        //Mã hóa tên file
                //            //        string originalFilePath = file_Path + ".enc";
                //            //        string encodedFilePath = Encrypt_FileName.EncodeFileNameInPath(originalFilePath);

                //            //        if (File.Exists(encodedFilePath))
                //            //        {
                //            //            File.Delete(encodedFilePath);
                //            //        }

                //            //        //Rename
                //            //        Encrypt_FileName.RenameFile(originalFilePath, encodedFilePath);
                //            //    }

                //            //    //File Thumbnail
                //            //    if (!File.Exists(image_thumbnail_Path_encode))
                //            //    {
                //            //        //await DownloadFileAsync(image_url, image_thumbnail_Path);
                //            //        await DownloadFileAsync(image_url, image_thumbnail_Path);

                //            //        //Mã hóa dữ liệu
                //            //        ENC.FileEncrypt(image_thumbnail_Path, image_thumbnail_Path + ".enc", FormMain.key);

                //            //        //Mã hóa tên file
                //            //        string originalFilePath_Thumbnail = image_thumbnail_Path + ".enc";
                //            //        string encodedFilePath_Thumbnail = Encrypt_FileName.EncodeFileNameInPath(originalFilePath_Thumbnail);
                //            //        if (File.Exists(encodedFilePath_Thumbnail))
                //            //        {
                //            //            File.Delete(encodedFilePath_Thumbnail);
                //            //        }

                //            //        //Rename
                //            //        Encrypt_FileName.RenameFile(originalFilePath_Thumbnail, encodedFilePath_Thumbnail);
                //            //    }


                //            //}


                //            //// Đảm bảo có ít nhất một DataRow trong List<DataRow>
                //            //if (dtExists.Count > 0)
                //            //{
                //            //    // Thêm các cột từ DataRow đầu tiên vào DataTable
                //            //    foreach (DataColumn column in dtExists[0].Table.Columns)
                //            //    {
                //            //        dt_New_Save.Columns.Add(column.ColumnName, column.DataType);
                //            //    }

                //            //    // Thêm các DataRow từ List<DataRow> vào DataTable
                //            //    foreach (DataRow row in dtExists)
                //            //    {
                //            //        dt_New_Save.ImportRow(row);
                //            //    }
                //            //}

                //            //dt_New_Save.WriteXml(path_XML);
                //            //MessageBox.Show("Tải tệp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //            //accordionControlElement_danhSach_Click(null, null);

                //            #endregion
                //        }
                //        #endregion
                //    }

                //}
                //else
                //{
                //    foreach (DataRow dr in dt_XML.Rows)
                //    {
                //        frm_Extention.Download(path,
                //                                      dt_XML,
                //                                      dr["uuid"].ToString(),
                //                                      path_XML,
                //                                      dr["image_url_KhoaHoc"].ToString(),
                //                                      dr["file_url"].ToString(),
                //                                      dr["image_url"].ToString()
                //                                      );
                //    }

                //    dt_XML.WriteXml(path_XML);
                //}

                #endregion

                accordionControlElement_danhSach_Click(null, null);
                MessageBox.Show("Tải tệp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Tìm kiếm bài giảng
        private void textEdit_Search_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string key_Search = textEdit_Search.EditValue.ToString();
                string filterExpression = string.Format("title LIKE '%{0}%'", key_Search.Replace("'", "''"));

                DataTable dt_filter = new DataTable();
                DataRow[] filteredRows;
                if (type == 0)
                {
                    filteredRows = dt_DanhSachKhoaHoc.Select(filterExpression);

                    dt_filter = dt_DanhSachKhoaHoc.Clone();                                     
                }
                else
                {
                    filteredRows = dt_DanhSachDaTai.Select(filterExpression);

                    dt_filter = dt_DanhSachDaTai.Clone();                    
                }

                foreach (DataRow row in filteredRows)
                {
                    dt_filter.ImportRow(row);
                }

                gridControl1.DataSource = dt_filter;
                //tileView1.Columns["Image_Lession_Show"].ColumnEdit = repositoryItemPictureEdit;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: ", ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #endregion
    }
}