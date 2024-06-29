using ImageMagick;
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
    public partial class frm_Image : Form
    {
        #region Variables
        public string frm_key = string.Empty;
        public string fileName = string.Empty;
        private PictureBox pictureBox;
        private float zoomFactor = 1.0f;
        private const float zoomStep = 0.1f;
        #endregion

        #region Inits

        #region public LoadImage()
        public frm_Image()
        {
            InitializeComponent();          
        }
        #endregion

        #region private void LoadImage_Load(object sender, EventArgs e)
        private void LoadImage_Load(object sender, EventArgs e)
        {
            try
            {
                MemoryStream msImage = ENC.FileDecrypt(fileName, frm_key);
                //pictureBox.Image = System.Drawing.Image.FromStream(msImage);

                //// Đọc ảnh từ MemoryStream
                //using (Image originalImage = Image.FromStream(msImage))
                //{
                //    // Tính toán tỉ lệ thay đổi kích thước
                //    float ratioWidth = (float)pictureBox.Width / (float)originalImage.Width;
                //    float ratioHeight = (float)pictureBox.Height / (float)originalImage.Height;
                //    float ratio = Math.Min(ratioWidth, ratioHeight);

                //    // Tạo một Bitmap mới với kích thước đã điều chỉnh
                //    int newWidth = (int)(originalImage.Width * ratio);
                //    int newHeight = (int)(originalImage.Height * ratio);
                //    Bitmap resizedImage = new Bitmap(newWidth, newHeight);
                //    using (Graphics g = Graphics.FromImage(resizedImage))
                //    {
                //        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                //        g.DrawImage(originalImage, 0, 0, newWidth, newHeight);
                //    }

                //    //Hiển thị ảnh đã điều chỉnh trong PictureBox
                //    pictureBox.Image = resizedImage;


                //}

                //test
                // Đọc ảnh từ MemoryStream bằng ImageMagick

                msImage.Position = 0;
                Bitmap bitmap;
                using (MagickImage magickImage = new MagickImage(msImage))
                {
                    // Chuyển đổi từ MagickImage sang mảng byte (định dạng PNG)
                    byte[] pngBytes = magickImage.ToByteArray(MagickFormat.Png);

                    // Tạo MemoryStream từ mảng byte PNG
                    using (MemoryStream pngStream = new MemoryStream(pngBytes))
                    {
                        // Tạo Bitmap từ MemoryStream PNG
                        bitmap = new Bitmap(pngStream);

                        // Hiển thị ảnh trong PictureBox                       
                    }
                }

                // Đọc ảnh từ MemoryStream
                //Image originalImage = Image.FromStream(msImage);

                // Load ảnh vào PictureBox
                pictureBox = new PictureBox();
               //pictureBox.Image = originalImage;
               pictureBox.Image = bitmap;
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox.Dock = DockStyle.Fill;

                // Thêm PictureBox vào Form
                this.Controls.Add(pictureBox);

                // Kết nối sự kiện MouseWheel
                pictureBox.MouseWheel += PictureBox_MouseWheel;

            }
            catch(Exception ex)
            {
                MessageBox.Show("Error decrypting image: " + ex.Message);
            }
        }

        private void PictureBox_MouseWheel(object sender, MouseEventArgs e)
        {
            // Tính toán tỷ lệ thay đổi dựa trên hướng cuộn chuột
            if (e.Delta > 0)
            {
                zoomFactor += zoomStep;
            }
            else
            {
                zoomFactor -= zoomStep;
                // Đảm bảo tỷ lệ không nhỏ hơn 0.1
                zoomFactor = Math.Max(zoomFactor, 0.1f);
            }

            // Cập nhật kích thước ảnh trong PictureBox
            UpdatePictureBoxSize();
        }

        private void UpdatePictureBoxSize()
        {
            // Tính toán kích thước mới của ảnh dựa trên tỷ lệ zoomFactor
            int newWidth = (int)(pictureBox.Image.Width * zoomFactor);
            int newHeight = (int)(pictureBox.Image.Height * zoomFactor);

            // Cập nhật kích thước của PictureBox và ảnh
            pictureBox.Width = newWidth;
            pictureBox.Height = newHeight;
        }
        #endregion

        #endregion
    }
}
