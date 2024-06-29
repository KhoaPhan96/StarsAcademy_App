using DevExpress.Utils;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Tile;
using DevExpress.XtraGrid.Views.Tile.ViewInfo;
//using DevExpress.XtraGrid.Views.Tile;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo_Encrypt_LoadFile.Feature
{
    public partial class frm_TittleView : Form
    {
        DataTable dt = new DataTable();
        string fileName = string.Empty;
        public frm_TittleView()
        {
            InitializeComponent();
            
        }

        private void frm_TittleView_Load(object sender, EventArgs e)
        {
            try
            {
                DataSet dataSet = new DataSet();

                // Đọc tệp XML vào DataSet
                dataSet.ReadXml(Application.StartupPath + @"\File\data.xml");

                DataTable loadedDataTable = dataSet.Tables[0];

                dt = loadedDataTable.Clone();
                dt.Columns.Add("Image_Course_Show", typeof(Image));
                dt.Columns.Add("Image_Lession_Show", typeof(Image));
                dt.Columns.Add("IsSelected", typeof(bool));
                dt.Columns.Add("Read", typeof(string));
                dt.Columns.Add("Delete", typeof(string));

                foreach (DataRow dr in loadedDataTable.Rows)
                {
                    dt.Rows.Add(dr["uuid_KhoaHoc"].ToString(),
                                dr["title_KhoaHoc"].ToString(),
                                dr["description_KhoaHoc"].ToString(),
                                dr["image_url_KhoaHoc"].ToString(),
                                dr["image_Path_KhoaHoc"].ToString(),
                                dr["uuid"].ToString(),
                                dr["title"].ToString(),
                                dr["description"].ToString(),
                                dr["image_url"].ToString(),
                                dr["file_url"].ToString(),
                                dr["image_url_Path"].ToString(),
                                dr["file_url_Path"].ToString(),
                                Image.FromStream(ENC.FileDecrypt(dr["image_Path_KhoaHoc"].ToString(), FormMain.key)),
                                Image.FromStream(ENC.FileDecrypt(dr["image_url_Path"].ToString(), FormMain.key)),
                                false,
                                "",
                                ""
                                );
                }

                gridControl1.DataSource = dt;
                tileView1.Columns["Image_Lession_Show"].ColumnEdit = repositoryItemPictureEdit;

                //Read.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
            catch (Exception ex)
            {

            }
        }

        private void deleteButton_Click(object sender, ContextItemClickEventArgs e)
        {
            try
            {
                // Lấy chỉ mục của tileview được chọn
                int focusedRowHandle = tileView1.FocusedRowHandle;

                // Kiểm tra nếu chỉ mục hợp lệ
                if (focusedRowHandle >= 0 && focusedRowHandle < tileView1.RowCount)
                {
                    // Lấy dữ liệu của tileview được chọn
                    DataRow selectedRow = tileView1.GetDataRow(focusedRowHandle);

                    // Kiểm tra nếu dữ liệu không rỗng
                    if (selectedRow != null)
                    {
                        // Xóa dòng dữ liệu khỏi DataTable
                        dt.Rows.Remove(selectedRow);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void repositoryItemButtonEdit_Read_Click(object sender, EventArgs e)
        {
            string a = string.Empty;
        }

        private void tileView1_ContextButtonClick(object sender, DevExpress.Utils.ContextItemClickEventArgs e)
        {
            try
            {                
                switch (e.Item.Name)
                {

                    case "Read":
                        fileName = tileView1.GetFocusedDataRow()["file_url_Path"].ToString();
                        LoadScreen frm = new LoadScreen();
                        frm.fileName = fileName;
                        frm.frm_key = FormMain.key;
                        frm.ShowDialog();
                        break;
                    case "Delete":
                     
                        #region Xóa tileview
                        //Focus tileview ngay tại bị trí click chuột
                        TileView view = sender as TileView;
                        TileViewItem item = e.DataItem as TileViewItem;
                        if (item != null)
                            view.FocusedRowHandle = item.RowHandle;

                        #region Xóa file
                        string file_url_Path = tileView1.GetFocusedDataRow()["file_url_Path"].ToString();
                        string image_Path_KhoaHoc = tileView1.GetFocusedDataRow()["image_Path_KhoaHoc"].ToString();
                        string image_url_Path = tileView1.GetFocusedDataRow()["image_url_Path"].ToString();
                        string test = tileView1.GetFocusedDataRow()["uuid"].ToString();

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

                        DataRow rowsToDelete = tileView1.GetFocusedDataRow();
                        if (rowsToDelete != null)
                        {
                            rowsToDelete.Delete();
                        }

                        

                        // Loại bỏ cột Bitmap trước khi ghi lại DataTable vào tệp XML
                        dt.Columns.Remove("Image_Course_Show");
                        dt.Columns.Remove("Image_Lession_Show");
                        dt.Columns.Remove("IsSelected");
                        dt.Columns.Remove("Read");
                        dt.Columns.Remove("Delete");
                       
                        dt.WriteXml(Application.StartupPath + @"\File\data.xml");

                        frm_TittleView_Load(null, null);
                        #endregion
                        break;
                    default:
                        break;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }

        private void tileView1_Click(object sender, EventArgs e)
        {

            MouseEventArgs mouseArgs = (e as MouseEventArgs);
            TileViewHitInfo hitInfo = tileView1.CalcHitInfo(mouseArgs.Location);

            if (hitInfo.InItem)
            {
                object SelectedValue = tileView1.GetRowCellValue(hitInfo.RowHandle, "uuid");
            }
        }
       
        private void Delete_File(string uuid_KhoaHoc, string uuid)
        {
            DataTable dtTemp = new DataTable();
            dtTemp = dt.Clone();

            DataRow[] rowsToDelete = dt.AsEnumerable().Where(row => row.Field<string>("uuid_KhoaHoc") == uuid_KhoaHoc && row.Field<string>("uuid") == uuid).ToArray();

            if (rowsToDelete.Length > 0)
            {
                // Xóa dòng thỏa mãn điều kiện
                foreach (DataRow row in rowsToDelete)
                {
                    row.Delete();
                }

                // Gọi phương thức AcceptChanges để lưu thay đổi vào DataTable
                dt.AcceptChanges();

                dt.WriteXml(Application.StartupPath + @"\File\data.xml");
                //MessageBox.Show("Dữ liệu đã được lưu vào tệp XML thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);                
            }
        }

        private void tileView1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) // Kiểm tra xem người dùng đã click chuột trái chưa
            {
                TileView tileView = sender as TileView;

                // Lấy vị trí click của chuột
                Point clickLocation = new Point(e.X, e.Y);

                // Tính toán thông tin về vị trí click trên TileView
                TileViewHitInfo hitInfo = tileView.CalcHitInfo(clickLocation);

                // Kiểm tra xem vị trí click có nằm trên một tile hay không
                if (hitInfo.InItem)
                {
                    // Đặt FocusedRowHandle vào chỉ mục của tile được click
                    tileView.FocusedRowHandle = hitInfo.RowHandle;
                }

                //tileView1.ContextButtonClick += tileView1_ContextButtonClick;
            }
        }

        private void tileView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                fileName = tileView1.GetFocusedDataRow()["file_url_Path"].ToString();
                LoadScreen frm = new LoadScreen();
                frm.fileName = fileName;
                frm.frm_key = FormMain.key;
                frm.ShowDialog();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tileView1_ContextButtonCustomize(object sender, TileViewContextButtonCustomizeEventArgs e)
        {
            //if (e.Item.Name == "Read")
            //{
            //    e.Item.Visibility = ContextItemVisibility.Hidden;
            //}    
        }
    }
}
