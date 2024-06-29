using DevExpress.Utils.Menu;
using DevExpress.XtraPdfViewer;
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
    public partial class frm_PDF : Form
    {
        #region Variables
        public string frm_key = string.Empty;
        public string fileName = string.Empty;
        #endregion

        #region Inits

        #region public frm_PDF()
        public frm_PDF()
        {
            InitializeComponent();
        }
        #endregion

        #region private void frm_PDF_Load(object sender, EventArgs e)
        private void frm_PDF_Load(object sender, EventArgs e)
        {
            try
            {
                MemoryStream msPDF = ENC.FileDecrypt(fileName, frm_key);

                pdfViewer.LoadDocument(msPDF);

                //pdfViewer.PopupMenuShowing += RichEditControl1_PopupMenuShowing;
                pdfViewer.PopupMenuShowing += PdfViewer1_PopupMenuShowing;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error decrypting image: " + ex.Message);
            }
        }

        private void PdfViewer1_PopupMenuShowing(object sender, PdfPopupMenuShowingEventArgs e)
        {
            // Ngăn chặn hiển thị context menu mặc định           
            e.ItemLinks.Clear();
        }

        #endregion

        #endregion

        #region Functions

        #region private void pdfViewer_KeyDown(object sender, KeyEventArgs e)
        private void pdfViewer_KeyDown(object sender, KeyEventArgs e)
        {
            // Ngăn chặn việc copy (Ctrl + C)
            if (e.Control && e.KeyCode == Keys.C)
            {
                e.Handled = true; // Ngăn chặn sự kiện được xử lý tiếp theo
            }
        }
        #endregion

        #endregion
    }
}
