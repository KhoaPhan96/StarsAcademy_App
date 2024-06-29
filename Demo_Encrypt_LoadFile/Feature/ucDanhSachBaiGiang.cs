using Demo_Encrypt_LoadFile.Feature.Functions;
using Demo_Encrypt_LoadFile.Feature.Models;

//using Demo_Encrypt_LoadFile.Feature.Models;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Tile;
using Microsoft.Office.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
//using System.Windows.Shapes;
using System.Xml.Linq;
using DanhSachBaiGiang_Show = Demo_Encrypt_LoadFile.Feature.Functions.DanhSachBaiGiang_Show;

namespace Demo_Encrypt_LoadFile.Feature
{
    public partial class ucDanhSachBaiGiang : UserControl
    {
        #region Variables
        string ApiUrl, api_Key, path, folderPath_XML, uuid_Course, title_KhoaHoc, description_KhoaHoc
                , path_XML_Course = Application.StartupPath + @"\0511\Course.xml" //Nơi lưu thông tin khóa học đã tải
                , path_XML_Lession = Application.StartupPath + @"\2505\Lession.xml" //Nơi lưu thông tin bài giảng đã tải
                , returnChiTiet ;
        public static string path_XML = Application.StartupPath + @"\File\data.xml";
        public static DataTable dt_New_Save = new DataTable("Products");
        int type = 1;
        public static int _checkExpired = 0;
        bool _check = false;
        public DataTable dt_DanhSachBaiGiang = new DataTable("Lession");

        public static string path_Course = Application.StartupPath + @"\0511\"; //Thư mục chứa thông tin khóa học đã tải
        public static string path_Lession = Application.StartupPath + @"\2505\"; //Thư mục thông tin bài giảng đã tải
        #endregion

        #region Inits
        public ucDanhSachBaiGiang()
        {
            InitializeComponent();
        }

        public ucDanhSachBaiGiang(string _ApiUrl, string _path, string _path_XML_Course, string _folderPath_XML, string _uuid_Course, string _title_KhoaHoc, string _description_KhoaHoc, string _returnChiTiet)
        {
            this.ApiUrl = _ApiUrl;
            this.path = _path;
            this.path_XML_Course = _path_XML_Course;
            this.folderPath_XML = _folderPath_XML;
            this.uuid_Course = _uuid_Course;
            this.title_KhoaHoc = _title_KhoaHoc;
            this.description_KhoaHoc = _description_KhoaHoc;
            this.returnChiTiet = _returnChiTiet;
            InitializeComponent();
        }

        private async void ucDanhSachBaiGiang_Load(object sender, EventArgs e)
        {
            try
            {
                this.UseWaitCursor = true;
                Application.DoEvents();

                chk_CheckAll.Checked = false;
                layout_CheckAll.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layout_Download.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                txt_Mota.Text = description_KhoaHoc;
                lblTieuDe.Text = "Bài giảng - Khoá học: " + title_KhoaHoc;
                txt_Mota.Text = description_KhoaHoc;
                txt_Mota.SelectionAlignment = HorizontalAlignment.Center;

                #region Lấy token_api

                if (!File.Exists(folderPath_XML))
                {
                    return;
                }

                DataTable tbl_Return_TokenAPI = new DataTable();
                tbl_Return_TokenAPI = await data_API_Token.GetData_API_Token(folderPath_XML);

                api_Key = tbl_Return_TokenAPI.Rows[0]["token"].ToString();
                #endregion

                Stopwatch stopwatch = new Stopwatch();

                stopwatch.Start();

                if (frm_StartAcademy_Home.dt_DanhSachBaiGiang != null && frm_StartAcademy_Home.dt_DanhSachBaiGiang.Rows.Count > 0 && returnChiTiet == "1")
                {
                    tileView1.HtmlImages = svgImageCollection1;
                    gridControl1.DataSource = frm_StartAcademy_Home.dt_DanhSachBaiGiang;
                    stopwatch.Stop();
                    Console.WriteLine("Time taken until khoa hoc: " + stopwatch.ElapsedMilliseconds + " ms");
                }
                else
                {

                    dt_DanhSachBaiGiang = await Functions.DanhSachBaiGiang_Show.GetDanhSachBaiGiang_Show(ApiUrl, api_Key, path, uuid_Course, path_XML_Course, svgImageCollection1);

                    frm_StartAcademy_Home.dt_DanhSachBaiGiang = dt_DanhSachBaiGiang;
                    tileView1.HtmlImages = svgImageCollection1;
                    gridControl1.DataSource = dt_DanhSachBaiGiang;
                    stopwatch.Stop();
                    Console.WriteLine("Time taken until khoa hoc: " + stopwatch.ElapsedMilliseconds + " ms");
                    string test = string.Empty;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.UseWaitCursor = false;
                Application.DoEvents();
            }
        }

        #endregion

        #region Functions

        private async void btn_Download_Click(object sender, EventArgs e)
        {           
            try
            {
                this.UseWaitCursor = true;
                Application.DoEvents();
               
                if (frm_Extention.IsInternetConnected()) //Kiểm tra máy có internet không
                {

                    #region Kiểm tra Key còn hạn không  

                    _checkExpired = frm_Extention.Check_Expired_FromXML(frm_InsertKeyAPI.folderPath_XML);

                    if (_checkExpired != 1)
                    {
                        MessageBox.Show("Tài khoản đã hết hạn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    #endregion

                    #region Tải file về máy


                    string uuid_Course = tileView1.GetFocusedDataRow()["uuid_Course"].ToString();

                    #region Lấy danh sách khóa học
                    DataRow[] filteredRows_Cource = frm_StartAcademy_Home.dt_DanhSachKhoaHoc.Select("uuid = '" + uuid_Course + "'");

                    if (filteredRows_Cource.Length == 0)
                    {
                        MessageBox.Show("Chưa chọn bài giảng để tải", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    DataTable dt_DanhSachKhoaHoc_filteredRows_Cource = new DataTable("Course");
                    dt_DanhSachKhoaHoc_filteredRows_Cource = frm_StartAcademy_Home.dt_DanhSachKhoaHoc.Clone();

                    DataTable dt_DanhSachKhoaHoc_Selected = new DataTable("Course");
                    dt_DanhSachKhoaHoc_Selected = dt_DanhSachKhoaHoc_filteredRows_Cource.Clone();

                    foreach (DataRow row_Cource in filteredRows_Cource)
                    {
                        dt_DanhSachKhoaHoc_filteredRows_Cource.ImportRow(row_Cource);
                    }

                    foreach (DataRow dr_KhoaHoc in dt_DanhSachKhoaHoc_filteredRows_Cource.Rows)
                    {
                        if (dr_KhoaHoc["uuid"].ToString() == uuid_Course)
                        {
                            dt_DanhSachKhoaHoc_Selected.Rows.Add(dr_KhoaHoc["id"].ToString()
                                                                 , dr_KhoaHoc["title"].ToString()
                                                                 , dr_KhoaHoc["description"].ToString()
                                                                 , dr_KhoaHoc["uuid"].ToString()
                                                                 , dr_KhoaHoc["image_url"].ToString()
                                                                 , dr_KhoaHoc["image_Path"].ToString()
                                                                 //, dr_KhoaHoc["image_Show"].ToString()
                                                                 , null
                                                                 , dr_KhoaHoc["meta_image_url"].ToString()
                                                                 , dr_KhoaHoc["meta_image_Path"].ToString()
                                                                 // , dr_KhoaHoc["meta_image_Show"].ToString()
                                                                 , null
                                                                 , dr_KhoaHoc["IsSelected"].ToString()
                                                                );
                        }
                    }

                    #endregion

                    #region Lấy danh sách bài giảng

                    DataRow[] filteredRows_Lession = dt_DanhSachBaiGiang.Select("IsSelected = '" + true + "'");

                    if (filteredRows_Lession.Length == 0)
                    {
                        MessageBox.Show("Chưa chọn bài giảng để tải", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    DataTable dt_DanhSachBaiGiang_filteredRows_Lession = new DataTable("Lession");
                    dt_DanhSachBaiGiang_filteredRows_Lession = dt_DanhSachBaiGiang.Clone();

                    DataTable dt_DanhSachBaiGiang_Selected = new DataTable("Lession");
                    dt_DanhSachBaiGiang_Selected = dt_DanhSachBaiGiang_filteredRows_Lession.Clone();

                    foreach (DataRow row_Lession in filteredRows_Lession)
                    {
                        dt_DanhSachBaiGiang_filteredRows_Lession.ImportRow(row_Lession);
                    }

                    foreach (DataRow dr_BaiGiang in dt_DanhSachBaiGiang_filteredRows_Lession.Rows)
                    {
                        if (dr_BaiGiang["uuid_Course"].ToString() == uuid_Course)
                        {
                            dt_DanhSachBaiGiang_Selected.Rows.Add(dr_BaiGiang["uuid_Course"].ToString()
                                                                          , dr_BaiGiang["id"].ToString()
                                                                          , dr_BaiGiang["uuid"].ToString()
                                                                          , dr_BaiGiang["title"].ToString()
                                                                          , dr_BaiGiang["description"].ToString()
                                                                          , dr_BaiGiang["image_url"].ToString()
                                                                          , dr_BaiGiang["image_Path"].ToString()
                                                                          //, dr_BaiGiang["image_Show"].ToString()
                                                                          , null
                                                                          , dr_BaiGiang["meta_image_url"].ToString()
                                                                          , dr_BaiGiang["meta_image_Path"].ToString()
                                                                          //, dr_BaiGiang["meta_image_Show"].ToString()
                                                                          , null
                                                                          , dr_BaiGiang["file"].ToString()
                                                                          , dr_BaiGiang["file_Path"].ToString()
                                                                          , dr_BaiGiang["IsDownloaded"].ToString()
                                                                          , dr_BaiGiang["IsSelected"].ToString()
                                                                         );
                        }
                    }


                    #endregion

                    await frm_Download_New.Download_New(path_Course, path_XML_Course, path_Lession, path_XML_Lession, dt_DanhSachKhoaHoc_Selected, dt_DanhSachBaiGiang_Selected);
                    {
                        chk_CheckAll.EditValue = false;
                        MessageBox.Show("Tải tệp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ucDanhSachBaiGiang_Load(null, null);
                    }

                    #endregion                  
                }
                else
                {
                    MessageBox.Show("Vui lòng kiểm tra lại Internet", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.UseWaitCursor = false;
                Application.DoEvents();
            }
        }

        private void chk_CheckAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                // Lấy index của cột mà bạn muốn cập nhật
                int columnIndex = tileView1.Columns["IsSelected"].VisibleIndex;

                for (int i = 0; i < tileView1.RowCount; i++)
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

        private async void tileView1_HtmlElementMouseClick(object sender, TileViewHtmlElementMouseEventArgs e)
        {
            try
            {
                if (e.ElementId == "readButton")
                {
                    #region Đọc file
                    string fileName = tileView1.GetFocusedDataRow()["file_Path"].ToString();

                    if (!string.IsNullOrEmpty(fileName))
                    {
                        LoadScreen frm = new LoadScreen();
                        frm.fileName = fileName;
                        frm.frm_key = FormMain.key;
                        frm.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Chưa tải bài giảng - Không có bài giảng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    #endregion
                }
                else
                    if (e.ElementId == "downloadButton")
                {

                    if (frm_Extention.IsInternetConnected()) //Kiểm tra máy có internet không
                    {

                        ////Lấy thư mục lưu file (thư mục khóa học - bài giảng)
                        ////Lây danh sách khóa học - theo mã uuid
                        ////Lấy danh sách bài giảng - theo mã uuid_KhoaHoc %% uuid_baigiang

                        #region Tải file về máy

                        string uuid_Course = tileView1.GetFocusedDataRow()["uuid_Course"].ToString();
                        string uuid = tileView1.GetFocusedDataRow()["uuid"].ToString();

                        DataTable dt_DanhSachKhoaHoc_Selected = new DataTable("Course");
                        dt_DanhSachKhoaHoc_Selected = frm_StartAcademy_Home.dt_DanhSachKhoaHoc.Clone();

                        DataTable dt_DanhSachBaiGiang_Selected = new DataTable("Lession");
                        dt_DanhSachBaiGiang_Selected = dt_DanhSachBaiGiang.Clone();

                        foreach (DataRow dr_KhoaHoc in frm_StartAcademy_Home.dt_DanhSachKhoaHoc.Rows)
                        {
                            if (dr_KhoaHoc["uuid"].ToString() == uuid_Course)
                            {
                                dt_DanhSachKhoaHoc_Selected.Rows.Add(dr_KhoaHoc["id"].ToString()
                                                                     , dr_KhoaHoc["title"].ToString()
                                                                     , dr_KhoaHoc["description"].ToString()
                                                                     , dr_KhoaHoc["uuid"].ToString()
                                                                     , dr_KhoaHoc["image_url"].ToString()
                                                                     , dr_KhoaHoc["image_Path"].ToString()
                                                                     //, dr_KhoaHoc["image_Show"].ToString()
                                                                     , null
                                                                     , dr_KhoaHoc["meta_image_url"].ToString()
                                                                     , dr_KhoaHoc["meta_image_Path"].ToString()
                                                                     //, dr_KhoaHoc["meta_image_Show"].ToString()
                                                                     , null
                                                                     , dr_KhoaHoc["IsSelected"].ToString()
                                                                    );
                            }
                        }

                        foreach (DataRow dr_BaiGiang in dt_DanhSachBaiGiang.Rows)
                        {
                            if (dr_BaiGiang["uuid_Course"].ToString() == uuid_Course && dr_BaiGiang["uuid"].ToString() == uuid)
                            {
                                dt_DanhSachBaiGiang_Selected.Rows.Add(dr_BaiGiang["uuid_Course"].ToString()
                                                                      , dr_BaiGiang["id"].ToString()
                                                                      , dr_BaiGiang["uuid"].ToString()
                                                                      , dr_BaiGiang["title"].ToString()
                                                                      , dr_BaiGiang["description"].ToString()
                                                                      , dr_BaiGiang["image_url"].ToString()
                                                                      , dr_BaiGiang["image_Path"].ToString()
                                                                      //, dr_BaiGiang["image_Show"].ToString()
                                                                      , null
                                                                      , dr_BaiGiang["meta_image_url"].ToString()
                                                                      , dr_BaiGiang["meta_image_Path"].ToString()
                                                                      //, dr_BaiGiang["meta_image_Show"].ToString()
                                                                      , null
                                                                      , dr_BaiGiang["file"].ToString()
                                                                      , dr_BaiGiang["file_Path"].ToString()
                                                                      , dr_BaiGiang["IsDownloaded"].ToString()
                                                                      , dr_BaiGiang["IsSelected"].ToString()
                                                                     );
                            }
                        }
                        #endregion

                        await frm_Download_New.Download_New(path_Course, path_XML_Course, path_Lession, path_XML_Lession, dt_DanhSachKhoaHoc_Selected, dt_DanhSachBaiGiang_Selected);
                        {
                            #region Cập nhật file_Path lại lưới
                            //DataTable dt_DanhSachBaiGiang_Return = new DataTable();
                            //dt_DanhSachBaiGiang_Return = await Functions.DanhSachBaiGiang_Show.GetDanhSachBaiGiang_Show(ApiUrl, api_Key, path, uuid_Course, path_XML_Course);

                            //frm_StartAcademy_Home.dt_DanhSachBaiGiang = dt_DanhSachBaiGiang_Return;

                            //gridControl1.DataSource = dt_DanhSachBaiGiang_Return;
                            ucDanhSachBaiGiang_Load(null, null);
                            #endregion
                        }

                        //thread_progress.Abort();
                        MessageBox.Show("Tải tệp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);                      
                    }
                    else
                    {
                        //thread_progress.Abort();
                        MessageBox.Show("Vui lòng kiểm tra lại Internet", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                    if (e.ElementId == "deleteButton")
                {
                    #region xóa file
                    TileView view_Delete = sender as TileView;
                    //TileViewItem item_Delete = e.DataItem as TileViewItem;
                    //if (view_Delete != null)
                    //    view_Delete.FocusedRowHandle = item_Delete.RowHandle;

                    string uuid_Selected = tileView1.GetFocusedDataRow()["uuid"].ToString();

                    string file_path = tileView1.GetFocusedDataRow()["file_Path"].ToString();
                    string image_path = tileView1.GetFocusedDataRow()["image_Path"].ToString();
                    string meta_image_path = tileView1.GetFocusedDataRow()["meta_image_Path"].ToString();

                    frm_Delete.Delete_Lession(uuid_Selected, file_path, image_path, meta_image_path, path_XML_Course);
                   
                    #endregion

                    #region Cập nhật lại lưới

                    DataRow[] rowsToUpdate = frm_StartAcademy_Home.dt_DanhSachBaiGiang.Select("uuid = '" + uuid_Selected + "'");

                    if (frm_Extention.IsInternetConnected()) //Kiểm tra máy có internet không - Nếu có mạng thì cập nhập IsDownloaded = false, file_Path = null
                    {

                       // Cập nhật giá trị của cột "IsDownloaded"
                        foreach (DataRow row in rowsToUpdate)
                        {
                            row["IsDownloaded"] = false;
                            row["file_Path"] = null;

                        }

                        gridControl1.DataSource = frm_StartAcademy_Home.dt_DanhSachBaiGiang;
                    }
                    else //Nếu không có mạng thì xóa hiển thị
                    {
                        // Xóa các hàng tìm thấy
                        foreach (DataRow row in rowsToUpdate)
                        {
                            frm_StartAcademy_Home.dt_DanhSachBaiGiang.Rows.Remove(row);
                        }

                        gridControl1.DataSource = frm_StartAcademy_Home.dt_DanhSachBaiGiang;
                    }
                    #endregion

                    ucDanhSachBaiGiang_Load(null, null);
                    //thread_progress.Abort();
                    MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                    if (e.ElementId == "detailButton")
                {
                    string file_path = tileView1.GetFocusedDataRow()["file_path"].ToString();
                    string uuid = tileView1.GetFocusedDataRow()["uuid"].ToString();
                    string title = tileView1.GetFocusedDataRow()["title"].ToString();
                    string description = tileView1.GetFocusedDataRow()["description"].ToString();

                    XtraScrollableControl xtrascroll = (XtraScrollableControl)this.Parent;
                    ucChiTietBaiGiang ucControl = new ucChiTietBaiGiang(file_path, title, tileView1.GetFocusedDataRow()["meta_Image_Show"], description, folderPath_XML, ApiUrl, path);

                    xtrascroll.Controls.Clear();
                    ucControl.Dock = DockStyle.Fill;
                    xtrascroll.Controls.Add(ucControl);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.UseWaitCursor = false;
                Application.DoEvents();
            }

        }

        private async void tileView1_ContextButtonClick(object sender, DevExpress.Utils.ContextItemClickEventArgs e)
        {
            //Thread thread_progress = new Thread(Graphical.CreateProgressbar);
            //thread_progress.Start();

            try
            {
                this.UseWaitCursor = true;
                Application.DoEvents();

                #region Focus tileview ngay tại bị trí click chuột
                TileView view_Read = sender as TileView;
                TileViewItem item_Read = e.DataItem as TileViewItem;
                if (item_Read != null)
                    view_Read.FocusedRowHandle = item_Read.RowHandle;
                #endregion

                #region Danh sách bài giảng load từ API
                //if (type == 0)
                //{
                switch (e.Item.Name)
                {
                    case "Download":

                        if (frm_Extention.IsInternetConnected()) //Kiểm tra máy có internet không
                        {

                            ////Lấy thư mục lưu file (thư mục khóa học - bài giảng)
                            ////Lây danh sách khóa học - theo mã uuid
                            ////Lấy danh sách bài giảng - theo mã uuid_KhoaHoc %% uuid_baigiang

                            #region Tải file về máy

                            string uuid_Course = tileView1.GetFocusedDataRow()["uuid_Course"].ToString();
                            string uuid = tileView1.GetFocusedDataRow()["uuid"].ToString();

                            DataTable dt_DanhSachKhoaHoc_Selected = new DataTable("Course");
                            dt_DanhSachKhoaHoc_Selected = frm_StartAcademy_Home.dt_DanhSachKhoaHoc.Clone();

                            DataTable dt_DanhSachBaiGiang_Selected = new DataTable("Lession");
                            dt_DanhSachBaiGiang_Selected = dt_DanhSachBaiGiang.Clone();

                            foreach (DataRow dr_KhoaHoc in frm_StartAcademy_Home.dt_DanhSachKhoaHoc.Rows)
                            {
                                if (dr_KhoaHoc["uuid"].ToString() == uuid_Course)
                                {
                                    dt_DanhSachKhoaHoc_Selected.Rows.Add(dr_KhoaHoc["id"].ToString()
                                                                         , dr_KhoaHoc["title"].ToString()
                                                                         , dr_KhoaHoc["description"].ToString()
                                                                         , dr_KhoaHoc["uuid"].ToString()
                                                                         , dr_KhoaHoc["image_url"].ToString()
                                                                         , dr_KhoaHoc["image_Path"].ToString()
                                                                         //, dr_KhoaHoc["image_Show"].ToString()
                                                                         , null
                                                                         , dr_KhoaHoc["meta_image_url"].ToString()
                                                                         , dr_KhoaHoc["meta_image_Path"].ToString()
                                                                         //, dr_KhoaHoc["meta_image_Show"].ToString()
                                                                         , null
                                                                         , dr_KhoaHoc["IsSelected"].ToString()
                                                                        );
                                }
                            }

                            foreach (DataRow dr_BaiGiang in dt_DanhSachBaiGiang.Rows)
                            {
                                if (dr_BaiGiang["uuid_Course"].ToString() == uuid_Course && dr_BaiGiang["uuid"].ToString() == uuid)
                                {
                                    dt_DanhSachBaiGiang_Selected.Rows.Add(dr_BaiGiang["uuid_Course"].ToString()
                                                                          , dr_BaiGiang["id"].ToString()
                                                                          , dr_BaiGiang["uuid"].ToString()
                                                                          , dr_BaiGiang["title"].ToString()
                                                                          , dr_BaiGiang["description"].ToString()
                                                                          , dr_BaiGiang["image_url"].ToString()
                                                                          , dr_BaiGiang["image_Path"].ToString()
                                                                          //, dr_BaiGiang["image_Show"].ToString()
                                                                          , null
                                                                          , dr_BaiGiang["meta_image_url"].ToString()
                                                                          , dr_BaiGiang["meta_image_Path"].ToString()
                                                                          //, dr_BaiGiang["meta_image_Show"].ToString()
                                                                          , null
                                                                          , dr_BaiGiang["file"].ToString()
                                                                          , dr_BaiGiang["file_Path"].ToString()
                                                                          , dr_BaiGiang["IsDownloaded"].ToString()
                                                                          , dr_BaiGiang["IsSelected"].ToString()
                                                                         );
                                }
                            }
                            #endregion

                            await frm_Download_New.Download_New(path_Course, path_XML_Course, path_Lession, path_XML_Lession, dt_DanhSachKhoaHoc_Selected, dt_DanhSachBaiGiang_Selected);
                            {
                                #region Cập nhật file_Path lại lưới
                                //DataTable dt_DanhSachBaiGiang_Return = new DataTable();
                                //dt_DanhSachBaiGiang_Return = await Functions.DanhSachBaiGiang_Show.GetDanhSachBaiGiang_Show(ApiUrl, api_Key, path, uuid_Course, path_XML_Course);

                                //frm_StartAcademy_Home.dt_DanhSachBaiGiang = dt_DanhSachBaiGiang_Return;

                                //gridControl1.DataSource = dt_DanhSachBaiGiang_Return;
                                ucDanhSachBaiGiang_Load(null, null);
                                #endregion
                            }

                            //thread_progress.Abort();
                            MessageBox.Show("Tải tệp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);                                                   
                        }
                        else
                        {
                            //thread_progress.Abort();
                            MessageBox.Show("Vui lòng kiểm tra lại Internet", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        break;
                    case "Check":
                        CheckContextButton checkItem1 = e.Item as CheckContextButton;
                        if (checkItem1.Checked == true)
                            _check = true;
                        else
                            _check = false;

                        //thread_progress.Abort();
                        break;
                    case "Read":
                        #region Đọc file
                        string fileName = tileView1.GetFocusedDataRow()["file_Path"].ToString();

                        if (!string.IsNullOrEmpty(fileName))
                        {
                            LoadScreen frm = new LoadScreen();
                            frm.fileName = fileName;
                            frm.frm_key = FormMain.key;
                            frm.ShowDialog();
                            //thread_progress.Abort();
                        }
                        else
                        {
                            //thread_progress.Abort();
                            MessageBox.Show("Chưa tải bài giảng - Không có bài giảng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        #endregion
                        break;
                    case "Delete":
                        #region xóa file
                        TileView view_Delete = sender as TileView;
                        TileViewItem item_Delete = e.DataItem as TileViewItem;
                        if (view_Delete != null)
                            view_Delete.FocusedRowHandle = item_Delete.RowHandle;

                        string uuid_Selected = tileView1.GetFocusedDataRow()["uuid"].ToString();

                        string file_path = tileView1.GetFocusedDataRow()["file_Path"].ToString();
                        string image_path = tileView1.GetFocusedDataRow()["image_Path"].ToString();
                        string meta_image_path = tileView1.GetFocusedDataRow()["meta_image_Path"].ToString();

                        frm_Delete.Delete_Lession(uuid_Selected, file_path, image_path, meta_image_path, path_XML_Course);

                      
                        #endregion

                        #region Cập nhật lại lưới

                        DataRow[] rowsToUpdate = frm_StartAcademy_Home.dt_DanhSachBaiGiang.Select("uuid = '" + uuid_Selected + "'");

                        if (frm_Extention.IsInternetConnected()) //Kiểm tra máy có internet không - Nếu có mạng thì cập nhập IsDownloaded = false, file_Path = null
                        {


                            // Cập nhật giá trị của cột "IsDownloaded"
                            foreach (DataRow row in rowsToUpdate)
                            {
                                row["IsDownloaded"] = false;
                                row["file_Path"] = null;

                            }

                            gridControl1.DataSource = frm_StartAcademy_Home.dt_DanhSachBaiGiang;
                        }
                        else //Nếu không có mạng thì xóa hiển thị
                        {
                            // Xóa các hàng tìm thấy
                            foreach (DataRow row in rowsToUpdate)
                            {
                                frm_StartAcademy_Home.dt_DanhSachBaiGiang.Rows.Remove(row);
                            }

                            gridControl1.DataSource = frm_StartAcademy_Home.dt_DanhSachBaiGiang;
                        }
                        #endregion

                        ucDanhSachBaiGiang_Load(null, null);
                        //thread_progress.Abort();
                        MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        break;
                    default:
                        break;
                }
                // }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Đổi con trỏ chuột trở lại bình thường
                this.UseWaitCursor = false;
                Application.DoEvents();
            }
        }

        private void tileView1_ItemClick(object sender, TileViewItemClickEventArgs e)
        {
            try
            {
                //bool check = (bool)tileView1.GetRowCellValue(e.Item.RowHandle, "IsSelected");
                //tileView1.SetRowCellValue(e.Item.RowHandle, "IsSelected", !check);

                //e.Item.Checked = !e.Item.Checked;


                //string uuid = tileView1.GetFocusedDataRow()["uuid"].ToString();
                //string sss = tileView1.GetFocusedDataRow()["IsSelected"].ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_TroLai_Click(object sender, EventArgs e)
        {
            XtraScrollableControl xtrascroll = (XtraScrollableControl)this.Parent;
            ucDanhSachKhoaHoc ucControl = new ucDanhSachKhoaHoc(ApiUrl, path, path_XML_Course, folderPath_XML);
            xtrascroll.Controls.Clear();
            ucControl.Dock = DockStyle.Fill;
            xtrascroll.Controls.Add(ucControl);
        }

        private void tileView1_ContextButtonCustomize(object sender, TileViewContextButtonCustomizeEventArgs e)
        {          
            e.Item.Visibility = DevExpress.Utils.ContextItemVisibility.Hidden;
            e.Item.Enabled = false;        
        }
     
        private async void accordionControlElement_danhSach_Click(object sender, EventArgs e)
        {          
            try
            {
                chk_CheckAll.Checked = false;

                layout_CheckAll.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layout_Download.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

                this.Text = "Danh sách bài giảng khóa học";
                type = 0;
               
                #region Lấy token_api

                if (!File.Exists(folderPath_XML))
                {
                    return;
                }

                DataTable tbl_Return_TokenAPI = new DataTable();
                tbl_Return_TokenAPI = await data_API_Token.GetData_API_Token(folderPath_XML);

                api_Key = tbl_Return_TokenAPI.Rows[0]["token"].ToString();
                #endregion

                dt_DanhSachBaiGiang = await DanhSachBaiGiang_Show.GetDanhSachBaiGiang_ThuocKhoaHoc_API(ApiUrl, api_Key, path, uuid_Course, svgImageCollection1);
                gridControl1.DataSource = dt_DanhSachBaiGiang;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
       
        private void btn_Delete_Click(object sender, EventArgs e)
        {         
            try
            {
                this.UseWaitCursor = true;
                Application.DoEvents();

                DataRow[] filteredRows = dt_DanhSachBaiGiang.Select("IsSelected = '" + true + "'");

                if (filteredRows.Length == 0)
                {
                    MessageBox.Show("Chưa chọn bài giảng để xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DataTable dt_DanhSachBaiGiang_filteredRows = dt_DanhSachBaiGiang.Clone();

                foreach (DataRow row_Cource in filteredRows)
                {
                    dt_DanhSachBaiGiang_filteredRows.ImportRow(row_Cource);
                }

                foreach (DataRow dr in dt_DanhSachBaiGiang_filteredRows.Rows)
                {
                    string uuid_Selected = dr["uuid"].ToString();

                    string file_path = dr["file_Path"].ToString();
                    string image_path = dr["image_Path"].ToString();
                    string meta_image_path = dr["meta_image_Path"].ToString();

                    frm_Delete.Delete_Lession(uuid_Selected, file_path, image_path, meta_image_path, path_XML_Course);

                    DataRow[] rowsToUpdate = dt_DanhSachBaiGiang.Select("uuid = '" + uuid_Selected + "'");

                    if (frm_Extention.IsInternetConnected()) //Kiểm tra máy có internet không - Nếu có mạng thì cập nhập IsDownloaded = false, file_Path = null
                    {

                        // Cập nhật giá trị của cột "IsDownloaded"
                        foreach (DataRow row in rowsToUpdate)
                        {
                            row["IsDownloaded"] = false;
                            row["file_Path"] = null;
                            row["IsSelected"] = false;

                        }
                        gridControl1.DataSource = frm_StartAcademy_Home.dt_DanhSachBaiGiang;
                    }
                    else //Nếu không có mạng thì xóa hiển thị
                    {
                        // Xóa các hàng tìm thấy
                        foreach (DataRow row in rowsToUpdate)
                        {
                            frm_StartAcademy_Home.dt_DanhSachBaiGiang.Rows.Remove(row);
                        }

                        gridControl1.DataSource = frm_StartAcademy_Home.dt_DanhSachBaiGiang;
                    }
                }

                #region Khúc này để xóa khóa học khi đã xóa hết bài giảng của khóa học (còn >= 1 bài giảng thì sẽ không vào khúc này)
                //DataTable dt_DanhSachBaiGiang_DaTai = new DataTable(); //Xét để xóa khóa học

                //dt_DanhSachBaiGiang_DaTai = Functions.DanhSachBaiGiang_Show.GetDanhSachBaiGiang_ThuocKhoaHoc_DaTai(path_XML_Lession,  uuid_Course, null);

                //if(dt_DanhSachBaiGiang_DaTai == null)
                //{
                //   // DataTable dt_DanhSachKhoaHoc_DaTai = Functions.DanhSachKhoaHoc_Show.GetDanhSachKhoaHoc_DaTai(path_XML_Course, api_Key, null, null);
                //    DataTable dt_DanhSachKhoaHoc_DaTai = Functions.DanhSachKhoaHoc_Show.GetDanhSachKhoaHoc_DaTai_Delete( path_XML_Course,  api_Key, uuid_Course);



                //}    

                #endregion

                MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                chk_CheckAll.EditValue = false;

                foreach (DataRow dr in frm_StartAcademy_Home.dt_DanhSachBaiGiang.Rows)
                {
                    dr["IsSelected"] = false;
                }

                gridControl1.DataSource = frm_StartAcademy_Home.dt_DanhSachBaiGiang;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.UseWaitCursor = false;
                Application.DoEvents();
            }
        }

        #endregion
    }
}
