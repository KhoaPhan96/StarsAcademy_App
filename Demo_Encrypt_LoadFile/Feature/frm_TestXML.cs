using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
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
    public partial class frm_TestXML : Form
    {
        public DataTable dt = new DataTable("Products");
        public frm_TestXML()
        {
            InitializeComponent();

            dt.Columns.Add("ProductID", typeof(int));
            dt.Columns.Add("ProductName", typeof(string));
            dt.Columns.Add("UnitPrice", typeof(decimal));
            dt.Columns.Add("Image", typeof(string));

            // Thêm dữ liệu mẫu vào DataTable
            Image image = Image.FromFile(@"C:\Users\ADMIN\OneDrive\Máy tính\test encrypt\pexels-olya-kobruseva-4679116.jpg");

            dt.Rows.Add(1, "Product 1", 10.5, @"C:\Users\ADMIN\OneDrive\Máy tính\test encrypt\pexels-olya-kobruseva-4679116.jpg");
            dt.Rows.Add(2, "Product 2", 20.75, @"C:\Users\ADMIN\OneDrive\Máy tính\test encrypt\pexels-olya-kobruseva-4679116.jpg");
            dt.Rows.Add(3, "Product 3", 15.0, @"C:\Users\ADMIN\OneDrive\Máy tính\test encrypt\pexels-olya-kobruseva-4679116.jpg");

        }

        private void btn_ThemXml_Click(object sender, EventArgs e)
        {
            //DataRow[] existingRows = dt.Select("ProductID =" + 4);

            DataRow[] existingRows = dt.AsEnumerable().Where(row => row.Field<int>("ProductID") == 4 && row.Field<string>("ProductName") == "Test thử nha").ToArray();

            if (existingRows.Length > 0)
            {
                MessageBox.Show("Đã tồn tại, không thể thêm", "Thông báo!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }    

            Image image = Image.FromFile(@"C:\Users\ADMIN\OneDrive\Máy tính\test encrypt\hd-unravel-2-wallpaper-68862-71207-hd-wallpapers.jpg");
            dt.Rows.Add(4, "Test thử nha", 15.0, @"C:\Users\ADMIN\OneDrive\Máy tính\test encrypt\hd-unravel-2-wallpaper-68862-71207-hd-wallpapers.jpg");
            
            dt.WriteXml(Application.StartupPath + @"\XML\data.xml");
            //MessageBox.Show("Dữ liệu đã được lưu vào tệp XML thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btn_LoadXML_Click(null, null);

        }

        private void btn_DeleteXML_Click(object sender, EventArgs e)
        {
            try
            {
                int productIdToDelete = 2;
                DataRow[] rowsToDelete = dt.Select("ProductID = " + productIdToDelete);

                if (rowsToDelete.Length > 0)
                {
                    // Xóa dòng thỏa mãn điều kiện
                    foreach (DataRow row in rowsToDelete)
                    {
                        row.Delete();
                    }

                    // Gọi phương thức AcceptChanges để lưu thay đổi vào DataTable
                    dt.AcceptChanges();

                    
                    dt.WriteXml(Application.StartupPath + @"\XML\data.xml");
                    MessageBox.Show("Dữ liệu đã được lưu vào tệp XML thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btn_LoadXML_Click(null, null);
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi lưu dữ liệu vào tệp XML: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_LoadXML_Click(object sender, EventArgs e)
        {
            try
            {               
                DataSet dataSet = new DataSet();

                // Đọc tệp XML vào DataSet
                dataSet.ReadXml(Application.StartupPath + @"\XML\data.xml");

                DataTable loadedDataTable = dataSet.Tables[0];

                DataTable dtTemp = new DataTable();
                dtTemp.Columns.Add("ProductID", typeof(int));
                dtTemp.Columns.Add("ProductName", typeof(string));
                dtTemp.Columns.Add("UnitPrice", typeof(decimal));
                dtTemp.Columns.Add("Image", typeof(Image));

                foreach (DataRow dr in loadedDataTable.Rows)
                {
                    dtTemp.Rows.Add(dr["ProductID"].ToString(), dr["ProductName"].ToString(), dr["UnitPrice"].ToString(), Image.FromFile(dr["Image"].ToString()));
                }    

                // Hiển thị dữ liệu lên DataGridView
                gridControl1.DataSource = dtTemp;

                // Thiết lập cột hình ảnh để hiển thị hình ảnh
                //RepositoryItemPictureEdit repositoryItemImage = new RepositoryItemPictureEdit();
                cardView1.Columns["Image"].ColumnEdit = repositoryItemPictureEdit1;
               //repositoryItemPictureEdit1.SizeMode = PictureSizeMode.Squeeze; // Thay đổi kích thước ảnh để vừa với cột
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi tải dữ liệu từ tệp XML: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btn_SaveXML_Click(object sender, EventArgs e)
        {
            dt.WriteXml(Application.StartupPath + @"\XML\data.xml");
            //MessageBox.Show("Dữ liệu đã được lưu vào tệp XML thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btn_LoadXML_Click(null, null);
        }

        private void btn_ThemFileXML_Click(object sender, EventArgs e)
        {
            try
            {
                using (FolderBrowserDialog dialog = new FolderBrowserDialog())
                {
                    DialogResult result = dialog.ShowDialog();

                    if(result == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
                    {                        
                            string filePath = Path.Combine(dialog.SelectedPath, "data.xml");
                            dt.WriteXml(filePath);
                            MessageBox.Show("Dữ liệu đã được lưu vào tệp XML thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);                      
                    }    
                }    
            }
            catch(Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
