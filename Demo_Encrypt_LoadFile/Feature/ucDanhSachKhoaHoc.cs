using Demo_Encrypt_LoadFile.Feature.Functions;
using Demo_Encrypt_LoadFile.Feature.Models;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Tile;
using Microsoft.Office.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
//using System.Windows.Shapes;
using System.Xml.Linq;

namespace Demo_Encrypt_LoadFile.Feature
{
    public partial class ucDanhSachKhoaHoc : UserControl
    {
        #region Variables
        string ApiUrl, api_Key, path, path_XML_Course, folderPath_XML, uuid_Cource;

        public static string path_XML = Application.StartupPath + @"\File\data.xml";
        public static string path_XML_Lession = Application.StartupPath + @"\2505\Lession.xml"; //Nơi lưu thông tin bài giảng đã tải
        public static DataTable dt_New_Save = new DataTable("Products");
        int type = 0;
        public static int _checkExpired = 0;
        bool _check = false;

        public DataTable dt_DanhSachKhoaHoc = new DataTable("Course");
        public DataTable dt_DanhSachKhoaHoc_Return = new DataTable("Course");

        #endregion

        #region Inits

        public ucDanhSachKhoaHoc()
        {
            InitializeComponent();
        }

        public ucDanhSachKhoaHoc(string _ApiUrl, string _path, string _path_XML_Course, string _folderPath_XML)
        {
            this.ApiUrl = _ApiUrl;
            this.path = _path;
            this.path_XML_Course = _path_XML_Course;
            this.folderPath_XML = _folderPath_XML;
            InitializeComponent();
        }

        private async void ucDanhSachKhoaHoc_Load(object sender, EventArgs e)
        {
            getData();
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

                    #region Danh sách khóa học
                    //DataRow[] filteredRows_Cource = frm_StartAcademy_Home.dt_DanhSachKhoaHoc.Select("IsSelected = '" + true + "'");
                    DataRow[] filteredRows_Cource = dt_DanhSachKhoaHoc.Select("IsSelected = '" + true + "'");

                    if (filteredRows_Cource.Length == 0)
                    {
                        //thread_progress.Abort();
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
                                                             , dr_KhoaHoc["IsDownloaded"].ToString()
                                                             , dr_KhoaHoc["IsSelected"].ToString()
                                                            ); ;
                    }

                    #endregion

                    #region Danh sách bài giảng

                    DataTable dt_DanhSachBaiGiang_Selected = new DataTable("Lessions");

                    dt_DanhSachBaiGiang_Selected.Columns.Add("uuid_Course", typeof(string));
                    dt_DanhSachBaiGiang_Selected.Columns.Add("id", typeof(string));
                    dt_DanhSachBaiGiang_Selected.Columns.Add("uuid", typeof(string));
                    dt_DanhSachBaiGiang_Selected.Columns.Add("title", typeof(string));
                    dt_DanhSachBaiGiang_Selected.Columns.Add("description", typeof(string));
                    dt_DanhSachBaiGiang_Selected.Columns.Add("image_url", typeof(string));
                    dt_DanhSachBaiGiang_Selected.Columns.Add("image_Path", typeof(string));
                    dt_DanhSachBaiGiang_Selected.Columns.Add("image_Show", typeof(Image));
                    dt_DanhSachBaiGiang_Selected.Columns.Add("meta_image_url", typeof(string));
                    dt_DanhSachBaiGiang_Selected.Columns.Add("meta_image_Path", typeof(string));
                    dt_DanhSachBaiGiang_Selected.Columns.Add("meta_image_Show", typeof(Image));
                    dt_DanhSachBaiGiang_Selected.Columns.Add("file", typeof(string));
                    dt_DanhSachBaiGiang_Selected.Columns.Add("file_Path", typeof(string));
                    dt_DanhSachBaiGiang_Selected.Columns.Add("IsDownloaded", typeof(bool));
                    dt_DanhSachBaiGiang_Selected.Columns.Add("IsSelected", typeof(bool));


                    foreach (DataRow dr in dt_DanhSachKhoaHoc_Selected.Rows)
                    {
                        DataTable dt_DanhSachBaiGiang_temp = await Functions.DanhSachBaiGiang_Show.GetDanhSachBaiGiang_ThuocKhoaHoc_API_XetDeTaiFile(ApiUrl, api_Key, path, dr["uuid"].ToString(), null);

                        foreach (DataRow dr_temp in dt_DanhSachBaiGiang_temp.Rows)
                        {
                            dt_DanhSachBaiGiang_Selected.ImportRow(dr_temp);
                        }

                    }
                    #endregion

                    await frm_Download_New.Download_New(frm_StartAcademy_Home.path_Course, frm_StartAcademy_Home.path_XML_Course, frm_StartAcademy_Home.path_Lession, frm_StartAcademy_Home.path_XML_Lession, dt_DanhSachKhoaHoc_Selected, dt_DanhSachBaiGiang_Selected);
                    {                       
                        MessageBox.Show("Tải tệp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
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

        private async void tileView1_ContextButtonClick(object sender, DevExpress.Utils.ContextItemClickEventArgs e)
        {        
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

                switch (e.Item.Name)
                {
                    case "Download":

                        if (frm_Extention.IsInternetConnected()) //Kiểm tra máy có internet không
                        {

                            ////Lấy thư mục lưu file (thư mục khóa học - bài giảng)
                            ////Lây danh sách khóa học - theo mã uuid
                            ////Lấy danh sách bài giảng - theo mã uuid_KhoaHoc %% uuid_baigiang

                            #region Danh sách khóa học
                            string uuid_Course = tileView1.GetFocusedDataRow()["uuid"].ToString();
                            //string uuid = tileView1.GetFocusedDataRow()["uuid"].ToString();

                            DataTable dt_DanhSachKhoaHoc_Selected = new DataTable("Course");
                            dt_DanhSachKhoaHoc_Selected = frm_StartAcademy_Home.dt_DanhSachKhoaHoc.Clone();



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
                                                                         // , dr_KhoaHoc["meta_image_Show"].ToString()
                                                                         , null
                                                                         , dr_KhoaHoc["IsSelected"].ToString()
                                                                        );
                                }
                            }
                            #endregion

                            #region Danh sách bài giảng


                            //dt_DanhSachBaiGiang_Selected = dt_DanhSachBaiGiang.Clone();

                            #region Kiểm tra khoa học có bài giảng không
                            //using (HttpClient client = new HttpClient())
                            //{
                            //    string url_API = frm_StartAcademy_Test.API_URL_DanhSachBaiGiang + uuid_Course + "/lessions?api_token=" + api_Key;
                            //    HttpResponseMessage response = await client.GetAsync(url_API);
                            //    if (response.IsSuccessStatusCode)
                            //    {
                            //        string jsonData = await response.Content.ReadAsStringAsync();
                            //        var result = JsonConvert.DeserializeObject<DanhSachBaiGiangModels.DSData>(jsonData);

                            //        if(result == null)
                            //        {
                            //            MessageBox.Show("Không có bài giảng để tải", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //            return;
                            //        }    
                            //    }
                            //}
                            #endregion

                            DataTable dt_DanhSachBaiGiang_temp = await Functions.DanhSachBaiGiang_Show.GetDanhSachBaiGiang_ThuocKhoaHoc_API_XetDeTaiFile(ApiUrl, api_Key, path, uuid_Course, null);

                            if (dt_DanhSachBaiGiang_temp == null)
                            {
                                // thread_progress.Abort();
                                MessageBox.Show("Không có bài giảng để tải", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                DataTable dt_DanhSachBaiGiang_filteredRows_Lession = new DataTable("Lession");
                                dt_DanhSachBaiGiang_filteredRows_Lession = dt_DanhSachBaiGiang_temp.Clone();

                                DataTable dt_DanhSachBaiGiang_Selected = new DataTable("Lession");
                                dt_DanhSachBaiGiang_Selected = dt_DanhSachBaiGiang_filteredRows_Lession.Clone();

                                foreach (DataRow dr_BaiGiang in dt_DanhSachBaiGiang_temp.Rows)
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

                                await frm_Download_New.Download_New(frm_StartAcademy_Home.path_Course, frm_StartAcademy_Home.path_XML_Course, frm_StartAcademy_Home.path_Lession, frm_StartAcademy_Home.path_XML_Lession, dt_DanhSachKhoaHoc_Selected, dt_DanhSachBaiGiang_Selected);
                                {
                                    // thread_progress.Abort();
                                    MessageBox.Show("Tải thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }

                            }
                            #endregion
                        
                        }
                        else
                        {
                            MessageBox.Show("Vui lòng kiểm tra lại Internet", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        break;
                    case "Check":
                        CheckContextButton checkItem1 = e.Item as CheckContextButton;
                        if (checkItem1.Checked == true)
                            _check = true;
                        else
                            _check = false;

                        break;
                    case "Read":
                        #region Xem danh sách bài giảng của khóa học
                        string uuid = tileView1.GetFocusedDataRow()["uuid"].ToString();
                        string title = tileView1.GetFocusedDataRow()["title"].ToString();
                        string description = tileView1.GetFocusedDataRow()["description"].ToString();

                        XtraScrollableControl xtrascroll = (XtraScrollableControl)this.Parent;
                        ucDanhSachBaiGiang ucControl = new ucDanhSachBaiGiang(ApiUrl, path, path_XML_Course, folderPath_XML, uuid, title, description, "0");
                        xtrascroll.Controls.Clear();
                        ucControl.Dock = DockStyle.Fill;
                        xtrascroll.Controls.Add(ucControl);

                        //    thread_progress.Abort();
                        #endregion
                        break;

                    case "Delete":
                        #region Xóa các bài giảng của khóa học

                        #region Xóa danh sách khóa học của bài giảng

                        string uuid_KhoaHoc_Delete = tileView1.GetFocusedDataRow()["uuid"].ToString();

                        DataTable dt_DanhSachBaiGiang_DaTai = Functions.DanhSachBaiGiang_Show.GetDanhSachBaiGiang_ThuocKhoaHoc_DaTai(path_XML_Course, uuid_KhoaHoc_Delete, null);

                        if (dt_DanhSachBaiGiang_DaTai != null)
                        {
                            foreach (DataRow dr_KhoaHoc in dt_DanhSachBaiGiang_DaTai.Rows)
                            {
                                string uuid_BaiGiang = dr_KhoaHoc["uuid"].ToString();
                                string file_path_BaiGiang = dr_KhoaHoc["file_Path"].ToString();
                                string image_path_BaiGiang = dr_KhoaHoc["image_Path"].ToString();
                                string meta_image_path_BaiGiang = dr_KhoaHoc["meta_image_Path"].ToString();

                                frm_Delete.Delete_Lession(uuid_BaiGiang, file_path_BaiGiang, image_path_BaiGiang, meta_image_path_BaiGiang, path_XML_Course);
                            }
                        }

                        #endregion

                        #region Xóa khóa học

                        string image_path_KhoaHoc = tileView1.GetFocusedDataRow()["image_Path"].ToString();
                        string meta_image_path_KhoaHoc = tileView1.GetFocusedDataRow()["meta_image_Path"].ToString();

                        frm_Delete.Delete_Cource(uuid_KhoaHoc_Delete, "", image_path_KhoaHoc, meta_image_path_KhoaHoc, path_XML_Course);

                        #endregion

                        #region Cập nhật lại lưới
                        DataRow[] rowsToUpdate = dt_DanhSachKhoaHoc.Select("uuid = '" + uuid_KhoaHoc_Delete + "'");

                        if (frm_Extention.IsInternetConnected()) //Kiểm tra máy có internet không - Nếu có mạng thì cập nhập IsDownloaded = false, file_Path = null
                        {

                            // Cập nhật giá trị của cột "IsDownloaded"
                            foreach (DataRow row in rowsToUpdate)
                            {
                                row["IsDownloaded"] = false;
                                row["IsSelected"] = false;
                            }
                            gridControl1.DataSource = dt_DanhSachKhoaHoc;
                        }
                        else //Nếu không có mạng thì xóa hiển thị
                        {                      
                            frm_StartAcademy_Home.dt_DanhSachKhoaHoc.Reset();
                            ucDanhSachKhoaHoc_Load(null, null);
                        }

                        #endregion

                        //  thread_progress.Abort();
                        MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        #endregion
                        break;

                    default:
                        break;
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

        private void tileView1_ItemClick(object sender, TileViewItemClickEventArgs e)
        {
            try
            {
                //bool check = (bool)tileView1.GetRowCellValue(e.Item.RowHandle, "IsSelected");
                //tileView1.SetRowCellValue(e.Item.RowHandle, "IsSelected", !check);



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tileView1_ContextButtonCustomize(object sender, TileViewContextButtonCustomizeEventArgs e)
        {
            if (e.Item.Name == "Download")
            {
                e.Item.Visibility = DevExpress.Utils.ContextItemVisibility.Hidden;
                e.Item.Enabled = true;
            }

            if (e.Item.Name == "Read")
            {
                e.Item.Visibility = DevExpress.Utils.ContextItemVisibility.Hidden;
                e.Item.Enabled = true;
            }

            if (e.Item.Name == "Delete")
            {
                e.Item.Visibility = DevExpress.Utils.ContextItemVisibility.Hidden;
                e.Item.Enabled = true;
            }
        }
     
        async void getData()
        {
            try
            {
                this.UseWaitCursor = true;
                Application.DoEvents();

                chk_CheckAll.Checked = false;
                layout_CheckAll.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layout_Download.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

                this.Text = "Danh sách bài giảng khóa học";

                #region Lấy token_api

                if (!File.Exists(folderPath_XML))
                {
                    //thread_progress.Abort();
                    return;
                }

                DataTable tbl_Return_TokenAPI = new DataTable();
                tbl_Return_TokenAPI = await data_API_Token.GetData_API_Token(folderPath_XML);

                api_Key = tbl_Return_TokenAPI.Rows[0]["token"].ToString();
                #endregion
           
                if (frm_StartAcademy_Home.dt_DanhSachKhoaHoc != null)
                {
                    if (frm_StartAcademy_Home.dt_DanhSachKhoaHoc.Rows.Count != 0)
                    {                      
                        dt_DanhSachKhoaHoc = frm_StartAcademy_Home.dt_DanhSachKhoaHoc.Copy();

                        foreach (DataRow dr in dt_DanhSachKhoaHoc.Rows)
                        {
                            dr["IsSelected"] = false;
                        }                     
                    }
                    else
                    {
                        dt_DanhSachKhoaHoc = await DanhSachKhoaHoc_Show.GetDanhSachKhoaHoc_Show(ApiUrl, api_Key, path, path_XML_Course, svgImageCollection1);
                      
                    }
                }
                else
                {
                    dt_DanhSachKhoaHoc = await DanhSachKhoaHoc_Show.GetDanhSachKhoaHoc_Show(ApiUrl, api_Key, path, path_XML_Course, svgImageCollection1);                 
                }
                frm_StartAcademy_Home.dt_DanhSachKhoaHoc = dt_DanhSachKhoaHoc;
                tileView1.HtmlImages = svgImageCollection1;
                gridControl1.DataSource = dt_DanhSachKhoaHoc;               
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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void tileView1_MouseEnter(object sender, EventArgs e)
        {
            TileView tileView = new TileView();
            tileView = sender as TileView;
            ContextItemCollection items = tileView.ContextButtons;
            foreach (ContextItem item in items)
            {
                if (item.Name == "Delete")
                {
                    item.Visibility = DevExpress.Utils.ContextItemVisibility.Hidden;
                    item.Enabled = false;
                }

                if (item.Name == "Check")
                {
                    item.Visibility = DevExpress.Utils.ContextItemVisibility.Hidden;
                    item.Enabled = false;
                }

                if (item.Name == "Download")
                {
                    item.Visibility = DevExpress.Utils.ContextItemVisibility.Hidden;
                    item.Enabled = false;
                }
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
            if (e.ElementId == "detailButton")
            {
                string uuid = tileView1.GetFocusedDataRow()["uuid"].ToString();
                string title = tileView1.GetFocusedDataRow()["title"].ToString();
                string description = tileView1.GetFocusedDataRow()["description"].ToString();

                XtraScrollableControl xtrascroll = (XtraScrollableControl)this.Parent;
                ucDanhSachBaiGiang ucControl = new ucDanhSachBaiGiang(ApiUrl, path, path_XML_Course, folderPath_XML, uuid, title, description, "0");
                xtrascroll.Controls.Clear();
                ucControl.Dock = DockStyle.Fill;
                xtrascroll.Controls.Add(ucControl);
            }
            else
                if (e.ElementId == "downloadButton")
            {
                if (frm_Extention.IsInternetConnected()) //Kiểm tra máy có internet không
                {

                    ////Lấy thư mục lưu file (thư mục khóa học - bài giảng)
                    ////Lây danh sách khóa học - theo mã uuid
                    ////Lấy danh sách bài giảng - theo mã uuid_KhoaHoc %% uuid_baigiang

                    #region Danh sách khóa học
                    string uuid_Course = tileView1.GetFocusedDataRow()["uuid"].ToString();
                    //string uuid = tileView1.GetFocusedDataRow()["uuid"].ToString();

                    DataTable dt_DanhSachKhoaHoc_Selected = new DataTable("Course");
                    dt_DanhSachKhoaHoc_Selected = frm_StartAcademy_Home.dt_DanhSachKhoaHoc.Clone();



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
                                                                 // , dr_KhoaHoc["meta_image_Show"].ToString()
                                                                 , null
                                                                 , dr_KhoaHoc["IsSelected"].ToString()
                                                                );
                        }
                    }
                    #endregion

                    #region Danh sách bài giảng


                    //dt_DanhSachBaiGiang_Selected = dt_DanhSachBaiGiang.Clone();

                    #region Kiểm tra khoa học có bài giảng không
                    //using (HttpClient client = new HttpClient())
                    //{
                    //    string url_API = frm_StartAcademy_Test.API_URL_DanhSachBaiGiang + uuid_Course + "/lessions?api_token=" + api_Key;
                    //    HttpResponseMessage response = await client.GetAsync(url_API);
                    //    if (response.IsSuccessStatusCode)
                    //    {
                    //        string jsonData = await response.Content.ReadAsStringAsync();
                    //        var result = JsonConvert.DeserializeObject<DanhSachBaiGiangModels.DSData>(jsonData);

                    //        if(result == null)
                    //        {
                    //            MessageBox.Show("Không có bài giảng để tải", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //            return;
                    //        }    
                    //    }
                    //}
                    #endregion

                    DataTable dt_DanhSachBaiGiang_temp = await Functions.DanhSachBaiGiang_Show.GetDanhSachBaiGiang_ThuocKhoaHoc_API_XetDeTaiFile(ApiUrl, api_Key, path, uuid_Course, null);

                    if (dt_DanhSachBaiGiang_temp == null)
                    {
                        // thread_progress.Abort();
                        MessageBox.Show("Không có bài giảng để tải", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        DataTable dt_DanhSachBaiGiang_filteredRows_Lession = new DataTable("Lession");
                        dt_DanhSachBaiGiang_filteredRows_Lession = dt_DanhSachBaiGiang_temp.Clone();

                        DataTable dt_DanhSachBaiGiang_Selected = new DataTable("Lession");
                        dt_DanhSachBaiGiang_Selected = dt_DanhSachBaiGiang_filteredRows_Lession.Clone();

                        foreach (DataRow dr_BaiGiang in dt_DanhSachBaiGiang_temp.Rows)
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

                        await frm_Download_New.Download_New(frm_StartAcademy_Home.path_Course, frm_StartAcademy_Home.path_XML_Course, frm_StartAcademy_Home.path_Lession, frm_StartAcademy_Home.path_XML_Lession, dt_DanhSachKhoaHoc_Selected, dt_DanhSachBaiGiang_Selected);
                        {
                            // thread_progress.Abort();
                            MessageBox.Show("Tải thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                    }
                    #endregion

                }
                else
                {
                    // thread_progress.Abort();
                    MessageBox.Show("Vui lòng kiểm tra lại Internet", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            else
                if (e.ElementId == "deleteButton")
            {
                #region Xóa các bài giảng của khóa học

                #region Xóa danh sách khóa học của bài giảng

                string uuid_KhoaHoc_Delete = tileView1.GetFocusedDataRow()["uuid"].ToString();

                DataTable dt_DanhSachBaiGiang_DaTai = Functions.DanhSachBaiGiang_Show.GetDanhSachBaiGiang_ThuocKhoaHoc_DaTai(path_XML_Course, uuid_KhoaHoc_Delete, null);

                if (dt_DanhSachBaiGiang_DaTai != null)
                {
                    foreach (DataRow dr_KhoaHoc in dt_DanhSachBaiGiang_DaTai.Rows)
                    {
                        string uuid_BaiGiang = dr_KhoaHoc["uuid"].ToString();
                        string file_path_BaiGiang = dr_KhoaHoc["file_Path"].ToString();
                        string image_path_BaiGiang = dr_KhoaHoc["image_Path"].ToString();
                        string meta_image_path_BaiGiang = dr_KhoaHoc["meta_image_Path"].ToString();

                        frm_Delete.Delete_Lession(uuid_BaiGiang, file_path_BaiGiang, image_path_BaiGiang, meta_image_path_BaiGiang, path_XML_Course);
                    }
                }

                #endregion

                #region Xóa khóa học

                string image_path_KhoaHoc = tileView1.GetFocusedDataRow()["image_Path"].ToString();
                string meta_image_path_KhoaHoc = tileView1.GetFocusedDataRow()["meta_image_Path"].ToString();

                frm_Delete.Delete_Cource(uuid_KhoaHoc_Delete, "", image_path_KhoaHoc, meta_image_path_KhoaHoc, path_XML_Course);

                #endregion

                #region Cập nhật lại lưới
                DataRow[] rowsToUpdate = dt_DanhSachKhoaHoc.Select("uuid = '" + uuid_KhoaHoc_Delete + "'");

                if (frm_Extention.IsInternetConnected()) //Kiểm tra máy có internet không - Nếu có mạng thì cập nhập IsDownloaded = false, file_Path = null
                {

                    // Cập nhật giá trị của cột "IsDownloaded"
                    foreach (DataRow row in rowsToUpdate)
                    {
                        row["IsDownloaded"] = false;
                        row["IsSelected"] = false;

                    }
                    gridControl1.DataSource = dt_DanhSachKhoaHoc;
                }
                else //Nếu không có mạng thì xóa hiển thị
                {                 
                    frm_StartAcademy_Home.dt_DanhSachKhoaHoc.Reset();
                    ucDanhSachKhoaHoc_Load(null, null);
                }

                #endregion

                //  thread_progress.Abort();
                MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                #endregion
            }
        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {           
            XtraScrollableControl xtrascroll = (XtraScrollableControl)this.Parent;
            uuid_Cource = tileView1.GetFocusedDataRow()["uuid"].ToString();
            ucDanhSachBaiGiangDaTai ucControl = new ucDanhSachBaiGiangDaTai(ApiUrl, path, uuid_Cource, path_XML_Lession, folderPath_XML);
            xtrascroll.Controls.Clear();
            ucControl.Dock = DockStyle.Fill;
            xtrascroll.Controls.Add(ucControl);
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

                Stopwatch stopwatch = new Stopwatch();

                stopwatch.Start();
          
                dt_DanhSachKhoaHoc = await DanhSachKhoaHoc_Show.GetDanhSachKhoaHoc_Show(ApiUrl, api_Key, path, path_XML_Course, svgImageCollection1);
                tileView1.HtmlImages = svgImageCollection1;
                gridControl1.DataSource = dt_DanhSachKhoaHoc;
                stopwatch.Stop();
                Console.WriteLine("Time taken until khoa hoc: " + stopwatch.ElapsedMilliseconds + " ms");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_DongBo_Click(object sender, EventArgs e)
        {
            frm_StartAcademy_Home.dt_DanhSachKhoaHoc.Reset();
            getData();
        }
    
        private void btn_Delete_Click(object sender, EventArgs e)
        {         
            try
            {
                this.UseWaitCursor = true;
                Application.DoEvents();

                DataRow[] filteredRows = dt_DanhSachKhoaHoc.Select("IsSelected = '" + true + "'");

                if (filteredRows.Length == 0)
                {
                    MessageBox.Show("Chưa chọn khóa học để xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DataTable dt_DanhSachKhoaHoc_filteredRows = dt_DanhSachKhoaHoc.Clone();

                foreach (DataRow row_Cource in filteredRows)
                {
                    dt_DanhSachKhoaHoc_filteredRows.ImportRow(row_Cource);
                }


                foreach (DataRow dr in dt_DanhSachKhoaHoc_filteredRows.Rows)
                {
                    #region Xóa danh sách bài giảng thuộc khóa học
                    //DataTable dt_DanhSachBaiGiang_DaTai = Functions.DanhSachKhoaHoc_Show.GetDanhSachBaiGiang_ThuocKhoaHoc_API_XetDeTaiFile(path_XML_Course, api_Key, dr["uuid"].ToString());
                    DataTable dt_DanhSachBaiGiang_DaTai = Functions.DanhSachBaiGiang_Show.GetDanhSachBaiGiang_ThuocKhoaHoc_DaTai(path_XML_Course, dr["uuid"].ToString(), null);

                    if (dt_DanhSachBaiGiang_DaTai != null)
                    {
                        foreach (DataRow dr_KhoaHoc in dt_DanhSachBaiGiang_DaTai.Rows)
                        {
                            string uuid_BaiGiang = dr_KhoaHoc["uuid"].ToString();
                            string file_path_BaiGiang = dr_KhoaHoc["file_Path"].ToString();
                            string image_path_BaiGiang = dr_KhoaHoc["image_Path"].ToString();
                            string meta_image_path_BaiGiang = dr_KhoaHoc["meta_image_Path"].ToString();

                            frm_Delete.Delete_Lession(uuid_BaiGiang, file_path_BaiGiang, image_path_BaiGiang, meta_image_path_BaiGiang, path_XML_Course);
                        }
                    }
                    #endregion

                    #region Xóa khóa học đã chọn

                    string uuid_KhoaHoc = dr["uuid"].ToString();
                    string image_path_KhoaHoc = dr["image_Path"].ToString();
                    string meta_image_path_KhoaHoc = dr["meta_image_Path"].ToString();

                    frm_Delete.Delete_Cource(uuid_KhoaHoc, "", image_path_KhoaHoc, meta_image_path_KhoaHoc, path_XML_Course);

                    #endregion

                    #region Cập nhật lại lưới danh sách khóa học

                    DataRow[] rowsToUpdate = dt_DanhSachKhoaHoc.Select("uuid = '" + dr["uuid"].ToString() + "'");

                    if (frm_Extention.IsInternetConnected()) //Kiểm tra máy có internet không - Nếu có mạng thì cập nhập IsDownloaded = false, file_Path = null
                    {

                        // Cập nhật giá trị của cột "IsDownloaded"
                        foreach (DataRow row in rowsToUpdate)
                        {
                            row["IsDownloaded"] = false;
                            row["IsSelected"] = false;

                        }
                        gridControl1.DataSource = dt_DanhSachKhoaHoc;
                    }
                    else //Nếu không có mạng thì xóa hiển thị
                    {                       
                        frm_StartAcademy_Home.dt_DanhSachKhoaHoc.Reset();
                        ucDanhSachKhoaHoc_Load(null, null);

                    }
                    #endregion
                }

                MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                chk_CheckAll.EditValue = false;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
