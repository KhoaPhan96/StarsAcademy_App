using DevExpress.XtraBars.Ribbon;
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
    public partial class frm_LoadFile_DaTai : Form
    {
        public frm_LoadFile_DaTai()
        {
            InitializeComponent();
        }

        private void frm_LoadFile_DaTai_Load(object sender, EventArgs e)
        {
            try
            {

                string rootPath = Application.StartupPath + @"\File";

                if (Directory.Exists(rootPath))
                {
                    //Danh sách thư mục
                    string[] subDirectories = Directory.GetDirectories(rootPath);
                    foreach (string subDirectory in subDirectories)
                    {
                        string folderName = Path.GetFileName(subDirectory);

                        GalleryItemGroup group = new GalleryItemGroup();
                        group.Caption = folderName.ToString();
                        galleryControl1.Gallery.Groups.Add(group);

                        //Danh sách file trong thư mục
                        string[] files = Directory.GetFiles(rootPath + @"\" + folderName);
                        foreach (string file in files)
                        {
                            //Chỉ lấy file ảnh có Thumbnail để hiển thị
                            string fileName = Path.GetFileName(file);

                            string decodedFilePath = Path.GetExtension(Encrypt_FileName.DecodeFileNameInPath(fileName).Replace(".enc", ""));

                            if (decodedFilePath.ToLower() == ".jpg"
                                || decodedFilePath.ToLower() == ".jpeg"
                                || decodedFilePath.ToLower() == ".png"
                                || decodedFilePath.ToLower() == ".gif"
                                || decodedFilePath.ToLower() == ".bmp"
                                )
                            {
                                string original_fileName = Encrypt_FileName.DecodeFileNameInPath(fileName);
                                if (original_fileName.Contains("thumbnailApp"))
                                {
                                    string a = string.Empty;

                                    int index = original_fileName.IndexOf("_");

                                    MemoryStream msImage = ENC.FileDecrypt(file, FormMain.key);
                                    // Đọc ảnh từ MemoryStream
                                    Image originalImage = Image.FromStream(msImage);
                                    group.Items.Add(new GalleryItem(originalImage, "123", ""));
                                }
                            }
                        }
                        string thumbnail_Path = string.Empty;

                        galleryControl1.Gallery.ImageSize = new System.Drawing.Size(300, 300);
                    }
                }
                else
                {
                    MessageBox.Show("Thư mục không tồn tại!");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
