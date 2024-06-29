using AxWMPLib;
using DevExpress.XtraPdfViewer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo_Encrypt_LoadFile.Feature
{
    class LoadFile
    {
        #region Variables
        //public string frm_key = string.Empty;
        //public string fileName = string.Empty;
        private PictureBox pictureBox;
        private float zoomFactor = 1.0f;
        private const float zoomStep = 0.1f;
        Panel panel = new Panel();
        public string temp = string.Empty;
        Form form = new Form();
        public AxWindowsMediaPlayer axWindowsMediaPlayer = new AxWindowsMediaPlayer();
        #endregion

        #region Inits
        public void Load(string fileName, string key)
        {
            form.Dock = DockStyle.Fill;
            form.ShowDialog();
            #region Add panel           
            panel.Dock = DockStyle.Fill;

            form.Controls.Add(panel);
            #endregion

            //Picture(fileName, key);
            MediaPlayer(fileName, key);
            //PDF(fileName, key);
            //PowerPoint(fileName, key);

           
        }
        #endregion

        #region Functions

        #region Picture

        #region private void Picture()
        private void Picture(string fileName, string key)
        {
            MemoryStream msImage = ENC.FileDecrypt(fileName, key);
            // Đọc ảnh từ MemoryStream
            Image originalImage = Image.FromStream(msImage);

            // Load ảnh vào PictureBox
            pictureBox = new PictureBox();
            pictureBox.Image = originalImage;
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.Dock = DockStyle.Fill;

            // Thêm PictureBox vào Form
            panel.Controls.Add(pictureBox);

            // Kết nối sự kiện MouseWheel
            pictureBox.MouseWheel += PictureBox_MouseWheel;
        }
        #endregion

        #region private void PictureBox_MouseWheel(object sender, MouseEventArgs e)
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
        #endregion

        #region private void UpdatePictureBoxSize()
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

        #region MediaPlayer

        #region private void MediaPlayer()
        private void MediaPlayer(string fileName, string key)
        {
            try
            {
                //AxWindowsMediaPlayer axWindowsMediaPlayer = new AxWindowsMediaPlayer();
                axWindowsMediaPlayer.Dock = DockStyle.Fill;
                panel.Controls.Add(axWindowsMediaPlayer);

                MemoryStream msMediaPlayer = ENC.FileDecrypt(fileName, key);
                string tempFilePath = Path.GetTempFileName();
                File.WriteAllBytes(tempFilePath, msMediaPlayer.ToArray());
                temp = tempFilePath;

                axWindowsMediaPlayer.URL = tempFilePath;
                form.FormClosing += Form_FormClosing;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region private void Form_FormClosing(object sender, FormClosingEventArgs e)
        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            axWindowsMediaPlayer.Ctlcontrols.stop();
            File.Delete(temp);
        }
        #endregion

        #endregion

        #region PDF

        #region private void PDF()
        private void PDF(string fileName, string key)
        {
            try
            {
                PdfViewer pdfViewer = new PdfViewer();
                pdfViewer.Dock = DockStyle.Fill;
                pdfViewer.NavigationPanePageVisibility = PdfNavigationPanePageVisibility.None;

                panel.Controls.Add(pdfViewer);

                MemoryStream msPDF = ENC.FileDecrypt(fileName, key);
                pdfViewer.LoadDocument(msPDF);

                pdfViewer.PopupMenuShowing += PdfViewer_PopupMenuShowing;
                pdfViewer.KeyDown += PdfViewer_KeyDown;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #region private void PdfViewer_KeyDown(object sender, KeyEventArgs e)
        private void PdfViewer_KeyDown(object sender, KeyEventArgs e)
        {
            // Ngăn chặn việc copy (Ctrl + C)
            if (e.Control && e.KeyCode == Keys.C)
            {
                e.Handled = true; // Ngăn chặn sự kiện được xử lý tiếp theo
            }
        }
        #endregion

        #region private void PdfViewer1_PopupMenuShowing(object sender, PdfPopupMenuShowingEventArgs e)
        private void PdfViewer_PopupMenuShowing(object sender, PdfPopupMenuShowingEventArgs e)
        {
            // Ngăn chặn hiển thị context menu mặc định           
            e.ItemLinks.Clear();
        }
        #endregion

        #endregion

        #endregion

        #region PowerPoint

        #region private void PowerPoint()
        private void PowerPoint(string fileName, string key)
        {
            frm_PowerPoint frm = new frm_PowerPoint();
            frm.RunPowerPoint(fileName, key);
        }
        #endregion

        #endregion

        #endregion
    }
}
