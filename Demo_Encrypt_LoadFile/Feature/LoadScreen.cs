using AxWMPLib;
using DevExpress.XtraPdfViewer;
using DevExpress.XtraRichEdit;
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
using WMPLib;

namespace Demo_Encrypt_LoadFile.Feature
{
    public partial class LoadScreen : Form
    {
        #region Variables
        public string frm_key = string.Empty;
        public string fileName = string.Empty;
        private PictureBox pictureBox;
        private float zoomFactor = 1.0f;
        private const float zoomStep = 0.1f;
        Panel panel = new Panel();
        public string temp = string.Empty;

        private AxWindowsMediaPlayer axWindowsMediaPlayer = new AxWindowsMediaPlayer();
        #endregion

        #region Inits

        #region public LoadScreen()
        public LoadScreen()
        {
            InitializeComponent();
        }
        #endregion

        #region private void LoadScreen_Load(object sender, EventArgs e)
        private void LoadScreen_Load(object sender, EventArgs e)
        {
            try
            {
                #region Add panel           
                panel.Dock = DockStyle.Fill;

                this.Controls.Add(panel);
                #endregion

                //Lấy phần mở rộng của file
                string decodedFilePath = Path.GetExtension(Encrypt_FileName.DecodeFileNameInPath(fileName).Replace(".enc", ""));

                #region LoadFile
                if (decodedFilePath != null)
                {
                    switch (decodedFilePath.ToLower())
                    {
                        case ".jpg":
                        case ".jpeg":
                        case ".png":
                        case ".gif":
                        case ".bmp":
                            Picture();
                            break;
                        case ".mp4":
                        case ".avi":
                        case ".mov":
                        case ".wmv":
                        case ".mkv":
                            MediaPlayer();
                            break;
                        case ".pdf":
                        case ".pdfa":
                        case ".pdfx":
                        case ".pdfe":
                        case ".pdfua":
                            PDF();
                            break;
                        case ".pptx":
                        case ".ppt":
                        case ".pps":
                        case ".ppsx":
                        case ".potx":
                            PowerPoint();
                            break;
                        case ".doc":
                        case ".docx":
                        case ".dot":
                        case ".dotx":
                        case ".docm":
                        case ".dtm":
                            Doc();
                            break;
                        default:
                            MessageBox.Show("Không xác định được file");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Không tìm thấy đường dẫn");
                }
                #endregion
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #endregion

        #region Functions

        #region Picture

        #region private void Picture()
        private void Picture()
        {
            MemoryStream msImage = ENC.FileDecrypt(fileName, frm_key);
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
        private void MediaPlayer()
        {
            try
            {
                //AxWindowsMediaPlayer axWindowsMediaPlayer = new AxWindowsMediaPlayer();
                axWindowsMediaPlayer.Dock = DockStyle.Fill;
                panel.Controls.Add(axWindowsMediaPlayer);

                MemoryStream msMediaPlayer = ENC.FileDecrypt(fileName, frm_key);
                string tempFilePath = Path.GetTempFileName();
                File.WriteAllBytes(tempFilePath, msMediaPlayer.ToArray());
                temp = tempFilePath;

                axWindowsMediaPlayer.URL = tempFilePath;
                this.FormClosing += Form_FormClosing;
            }
            catch(Exception ex)
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
        private void PDF()
        {
            try
            {
                PdfViewer pdfViewer = new PdfViewer();
                pdfViewer.Dock = DockStyle.Fill;               
                pdfViewer.NavigationPanePageVisibility = PdfNavigationPanePageVisibility.None;

                panel.Controls.Add(pdfViewer);

                MemoryStream msPDF = ENC.FileDecrypt(fileName, frm_key);
                pdfViewer.LoadDocument(msPDF);

                pdfViewer.PopupMenuShowing += PdfViewer_PopupMenuShowing;
                pdfViewer.KeyDown += PdfViewer_KeyDown;
            }
            catch(Exception ex)
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
        private void PowerPoint()
        {
            frm_PowerPoint frm = new frm_PowerPoint();
            frm.RunPowerPoint(fileName, frm_key);

            this.Close();
        }
        #endregion

        #endregion

        #region Docx

        #region private void Doc()
        private void Doc()
        {
            try
            {
                RichEditControl richEdit = new RichEditControl();
                richEdit.Dock = DockStyle.Fill;
                panel.Controls.Add(richEdit);

                MemoryStream msDocx = ENC.FileDecrypt(fileName, frm_key);
                string tempFilePath = Path.GetTempFileName();
                File.WriteAllBytes(tempFilePath, msDocx.ToArray());
                temp = tempFilePath;

                richEdit.LoadDocument(tempFilePath, DocumentFormat.OpenXml);

                richEdit.ReadOnly = true;

                //Chặn sự kiện chuột phải -> copy
                richEdit.PopupMenuShowing += richEdit_PopupMenuShowing;
                this.FormClosed += FormDoc_FormClosing;
                richEdit.KeyDown += RichEdit_KeyDown;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region private void RichEdit_KeyDown(object sender, KeyEventArgs e)
        private void RichEdit_KeyDown(object sender, KeyEventArgs e)
        {
            // Ngăn chặn việc copy (Ctrl + C)
            if (e.Control && e.KeyCode == Keys.C)
            {
                e.Handled = true; // Ngăn chặn sự kiện được xử lý tiếp theo
            }
        }
        #endregion

        #region private void FormDoc_FormClosing(object sender, FormClosedEventArgs e)
        private void FormDoc_FormClosing(object sender, FormClosedEventArgs e)
        {
            File.Delete(temp);
        }
        #endregion

        #region private void richEdit_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        private void richEdit_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            // Ngăn chặn hiển thị menu ngữ cảnh mặc định
            e.Menu = null;
        }
        #endregion

        #endregion

        #endregion
    }
}
