using Demo_Encrypt_LoadFile.Feature.Functions;
using Demo_Encrypt_LoadFile.Feature.Models;
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
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Shapes;

namespace Demo_Encrypt_LoadFile.Feature
{
    public partial class ucDanhSachBaiGiangDaTai : UserControl
    {
        #region Variables
        string ApiUrl, api_Key, path, path_XML_Lession, folderPath_XML, uuid_Course;
        public DataTable dt_DanhSachBaiGiang = new DataTable("Lession");

        #endregion

        #region Inits
        public ucDanhSachBaiGiangDaTai()
        {
            InitializeComponent();
        }

        public ucDanhSachBaiGiangDaTai(string _ApiUrl, string _path, string _uuid_Cource, string _path_XML_Lession, string _folderPath_XML)
        {
            this.ApiUrl = _ApiUrl;
            this.path = _path;
            this.uuid_Course = _uuid_Cource;
            this.path_XML_Lession = _path_XML_Lession;
            this.folderPath_XML = _folderPath_XML;
            InitializeComponent();
        }
        #endregion

        #region Functions
        private void repositoryItemButtonEdit_delete_Click(object sender, EventArgs e)
        {
            try
            {
                this.UseWaitCursor = true;
                Application.DoEvents();

                string uuid_Selected = gridView1.GetFocusedDataRow()["uuid_BaiGiang"].ToString();

                string file_path = gridView1.GetFocusedDataRow()["file_Path"].ToString();
                string image_path = gridView1.GetFocusedDataRow()["image_Path"].ToString();
                string meta_image_path = gridView1.GetFocusedDataRow()["meta_image_Path"].ToString();

                frm_Delete.Delete_Lession(uuid_Selected, file_path, image_path, meta_image_path, frm_StartAcademy_Home.path_XML_Course);

                ucDanhSachBaiGiangDaTai_Load(null, null);
                MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void repositoryItemButtonEdit_Read_Click(object sender, EventArgs e)
        {
            try
            {
                this.UseWaitCursor = true;
                Application.DoEvents();

                #region Đọc file
                string fileName = gridView1.GetFocusedDataRow()["file_Path"].ToString();

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

        private async void ucDanhSachBaiGiangDaTai_Load(object sender, EventArgs e)
        {
            try
            {
                this.UseWaitCursor = true;
                Application.DoEvents();

                String test = Application.StartupPath;
                this.Text = "Danh sách bài giảng khóa học đã tải";
                string token_api = string.Empty;
               
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

                #region Hiển thị lên lưới

                dt_DanhSachBaiGiang = DanhSachDaTai_Show.DanhSachDaTai(frm_StartAcademy_Home.path_XML_Course);
                
                gridControl1.DataSource = dt_DanhSachBaiGiang;
                gridView1.BestFitColumns();

                stopwatch.Stop();
                Console.WriteLine("Time taken until bai giang: " + stopwatch.ElapsedMilliseconds + " ms");

                #endregion
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
