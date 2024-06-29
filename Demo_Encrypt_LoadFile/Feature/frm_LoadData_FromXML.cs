using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo_Encrypt_LoadFile.Feature
{
    public partial class frm_LoadData_FromXML : Form
    {
        DataTable dt = new DataTable();
        public frm_LoadData_FromXML()
        {
            InitializeComponent();
        }

        private void frm_LoadData_FromXML_Load(object sender, EventArgs e)
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
                cardView1.Columns["Image_Lession_Show"].ColumnEdit = repositoryItemPictureEdit;
                cardView1.Columns["Read"].ColumnEdit = repositoryItemButtonEdit_Read;
                cardView1.Columns["Delete"].ColumnEdit = repositoryItemButtonEdit_Delete;

                cardView1.CardWidth = 300;
                cardView1.OptionsBehavior.FieldAutoHeight = true;
            }
            catch(Exception ex)
            {

            }
        }

        private void repositoryItemButtonEdit_Click(object sender, EventArgs e)
        {
            string fileName = cardView1.GetFocusedDataRow()["file_url_Path"].ToString();

            LoadScreen frm = new LoadScreen();
            frm.fileName = fileName;
            frm.frm_key = FormMain.key;
            frm.ShowDialog();
        }

        private void repositoryItemButtonEdit_Delete_Click(object sender, EventArgs e)
        {
            string a = cardView1.GetFocusedDataRow()["file_url_Path"].ToString();
        }

        private void cardView1_CustomDrawCardCaption(object sender, DevExpress.XtraGrid.Views.Card.CardCaptionCustomDrawEventArgs e)
        {
            string caption = cardView1.GetRowCellDisplayText(e.RowHandle, "title_KhoaHoc");
            e.CardCaption = caption;
        }
    }
}
