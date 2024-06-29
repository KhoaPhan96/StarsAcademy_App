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

namespace Demo_Encrypt_LoadFile.Feature
{
    public partial class frm_Docx : Form
    {
        public string fileName = string.Empty;
        public string key = string.Empty;
        private string temp = string.Empty;
        public frm_Docx()
        {
            InitializeComponent();
        }

       

        private void frm_Docx_Load(object sender, EventArgs e)
        {
            MemoryStream msDocx = ENC.FileDecrypt(fileName, key);
            string tempFilePath = Path.GetTempFileName();
            File.WriteAllBytes(tempFilePath, msDocx.ToArray());
            temp = tempFilePath;

            richEditControl1.LoadDocument(tempFilePath, DocumentFormat.OpenXml);

            ////richEditControl1.LoadDocument(fileName);

            //// Tạo một đối tượng MemoryStream và ghi dữ liệu vào đó
            //MemoryStream stream = new MemoryStream();
            //byte[] data = File.ReadAllBytes(fileName); // Thay đổi đường dẫn đến tài liệu của bạn
            //stream.Write(data, 0, data.Length);
            //stream.Seek(0, SeekOrigin.Begin); // Đặt lại vị trí của stream về đầu

            //// Load tài liệu từ MemoryStream vào RichEditControl
            //richEditControl1.LoadDocument(stream, DocumentFormat.OpenXml);
            ///////////////////////////////////////
            richEditControl1.ReadOnly = true;

            //Chặn sự kiện chuột phải -> copy
            richEditControl1.PopupMenuShowing += RichEditControl1_PopupMenuShowing;
        }

        private void RichEditControl1_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            // Ngăn chặn hiển thị menu ngữ cảnh mặc định
            e.Menu = null;
        }

        private void richEditControl1_KeyDown(object sender, KeyEventArgs e)
        {
            // Ngăn chặn việc copy (Ctrl + C)
            if (e.Control && e.KeyCode == Keys.C)
            {
                e.Handled = true; // Ngăn chặn sự kiện được xử lý tiếp theo
            }
        }

        private void frm_Docx_FormClosed(object sender, FormClosedEventArgs e)
        {
            File.Delete(temp);
        }
    }
}
