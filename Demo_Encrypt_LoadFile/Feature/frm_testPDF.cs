using DevExpress.Pdf;
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
    public partial class frm_testPDF : Form
    {
        public string fileName = string.Empty;
        public string key = string.Empty;
        public frm_testPDF()
        {
            InitializeComponent();
        }

        private void frm_testPDF_Load(object sender, EventArgs e)
        {
            try
            {
                MemoryStream msImage = ENC.FileDecrypt(fileName, key);

                // Tạo PdfDocument từ MemoryStream
                // PdfDocument document = PdfDocument.Load(msImage);

                // Gán document vào PdfViewer
                pdfViewer1.LoadDocument(msImage);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error decrypting image: " + ex.Message);
            }
        }
    }
}
