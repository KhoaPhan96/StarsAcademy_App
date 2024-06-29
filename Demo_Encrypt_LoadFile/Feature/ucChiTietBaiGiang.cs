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
    public partial class ucChiTietBaiGiang : UserControl
    {
        #region Variables
        string ApiUrl, api_Key, path, folderPath_XML, uuid_Course, title_KhoaHoc, description_KhoaHoc
                , path_XML_Course = Application.StartupPath + @"\0511\Course.xml" //Nơi lưu thông tin khóa học đã tải
                , path_XML_Lession = Application.StartupPath + @"\2505\Lession.xml"; //Nơi lưu thông tin bài giảng đã tải
        public static string path_XML = Application.StartupPath + @"\File\data.xml";
        public static DataTable dt_New_Save = new DataTable("Products");
        int type = 1;
        public static int _checkExpired = 0;
        bool _check = false;
        public DataTable dt_DanhSachBaiGiang = new DataTable("Lession");

        public static string path_Course = Application.StartupPath + @"\0511\"; //Thư mục chứa thông tin khóa học đã tải
        public static string path_Lession = Application.StartupPath + @"\2505\"; //Thư mục thông tin bài giảng đã tải
       

        public string file_Path { get; }
        public string tile { get; }
        public object meta_Image_Show { get; }
        public string description { get; }
        #endregion

        #region Inits
        private async void ucChiTietBaiGiang_Load(object sender, EventArgs e)
        {
            string htmlContent = $@"
            <div class=""w3-panel w3-center w3-opacity"" style=""padding:30px 16px"">
                <h1 class=""w3-xlarge"">{tile}</h1>
                <p style=""font-size: 16px;"">{description}</p>
            </div>
            <div class=""item-info"">
                <div id=""detailButton"" class=""button"">Xem bài giảng</div>
            </div>";
            htmlContentControl1.HtmlTemplate.Template = htmlContent;
        }

        public ucChiTietBaiGiang()
        {
            InitializeComponent();
        }

        public ucChiTietBaiGiang(string _file_Path, string _tile, object _meta_image_Show, string _description, string _folderPath_XML, string _apiUrl, string _path)
        {
            file_Path = _file_Path;
            tile = _tile;
            meta_Image_Show = _meta_image_Show;
            description = _description;
            folderPath_XML = _folderPath_XML;
            ApiUrl = _apiUrl;
            path = _path;
            InitializeComponent();
        }
        #endregion

        #region Functions
        private async void btn_Download_Click(object sender, EventArgs e)
        {
        }

        private void chk_CheckAll_CheckedChanged(object sender, EventArgs e)
        {
        }

        private async void tileView1_HtmlElementMouseClick(object sender, TileViewHtmlElementMouseEventArgs e)
        {
        }

        private void htmlContentControl1_ElementMouseClick(object sender, DevExpress.Utils.Html.DxHtmlElementMouseEventArgs e)
        {
            //Thread thread_progress = new Thread(Graphical.CreateProgressbar);
            //thread_progress.Start();
            if (e.ElementId == "detailButton")
            {
                #region Đọc file
                string fileName = file_Path;

                if (!string.IsNullOrEmpty(fileName))
                {
                    ucLoadScreen frm = new ucLoadScreen();
                    frm.fileName = fileName;
                    frm.frm_key = FormMain.key;
                    xtrascroll.Controls.Clear();
                    frm.Dock = DockStyle.Fill;
                    xtrascroll.Controls.Add(frm);
                }
                else
                {
                    MessageBox.Show("Chưa tải bài giảng - Không có bài giảng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                #endregion
            }
            // thread_progress.Abort();
        }

        private async void tileView1_ContextButtonClick(object sender, DevExpress.Utils.ContextItemClickEventArgs e)
        {
        }

        private void tileView1_ItemClick(object sender, TileViewItemClickEventArgs e)
        {
        }

        private void btn_TroLai_Click(object sender, EventArgs e)
        {
            XtraScrollableControl xtrascroll = (XtraScrollableControl)this.Parent;
            ucDanhSachBaiGiang ucControl = new ucDanhSachBaiGiang(ApiUrl, path, path_XML_Course, folderPath_XML, uuid_Course, title_KhoaHoc, description_KhoaHoc, "1");
            xtrascroll.Controls.Clear();
            ucControl.Dock = DockStyle.Fill;
            xtrascroll.Controls.Add(ucControl);
        }

        private void tileView1_ContextButtonCustomize(object sender, TileViewContextButtonCustomizeEventArgs e)
        {   
            e.Item.Visibility = DevExpress.Utils.ContextItemVisibility.Hidden;
            e.Item.Enabled = false;           
        }
    
        private void btn_Delete_Click(object sender, EventArgs e)
        {
            //Thread thread_progress = new Thread(Graphical.CreateProgressbar);
            //thread_progress.Start();

            try
            {
                DataRow[] filteredRows = dt_DanhSachBaiGiang.Select("IsSelected = '" + true + "'");

                if (filteredRows.Length == 0)
                {
                    // thread_progress.Abort();
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
                    }
                    else //Nếu không có mạng thì xóa hiển thị
                    {
                        // Xóa các hàng tìm thấy
                        foreach (DataRow row in rowsToUpdate)
                        {
                            frm_StartAcademy_Home.dt_DanhSachBaiGiang.Rows.Remove(row);
                        }
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

                // thread_progress.Abort();
                MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                foreach (DataRow dr in frm_StartAcademy_Home.dt_DanhSachBaiGiang.Rows)
                {
                    dr["IsSelected"] = false;
                }
            }
            catch (Exception ex)
            {
                //thread_progress.Abort();
            }
        }
        #endregion
    }
}
