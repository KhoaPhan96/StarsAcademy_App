using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Office.Interop.PowerPoint;
using Microsoft.Office.Core;
using System.Windows;
using Application = Microsoft.Office.Interop.PowerPoint.Application;

namespace Demo_Encrypt_LoadFile.Feature
{
    class frm_PowerPoint_MemoryStream
    {
        #region Variables
        public string frm_key = string.Empty;
        public string fileName = string.Empty;
        #endregion


        private void ReadPowerPointFromMemoryStream(MemoryStream memoryStream)
        {
            

            try
            {
                // Tạo một tệp tạm thời từ MemoryStream
                string tempFilePath = Path.GetTempFileName();
                File.WriteAllBytes(tempFilePath, memoryStream.ToArray());

                // Khởi tạo một ứng dụng PowerPoint
                Application pptApplication = new Application();

                // Tạo một Presentation từ tệp tạm thời
                Presentation pptPresentation = pptApplication.Presentations.Open(
                    tempFilePath, // Đường dẫn của tệp tạm thời
                    WithWindow: MsoTriState.msoFalse, ReadOnly: MsoTriState.msoTrue
                );

                // Chạy trình chiếu
                pptPresentation.SlideShowSettings.Run();



                //// Giải phóng tài nguyên
                //pptPresentation.Close();
                //pptApplication.Quit();

                // Xóa tệp tạm thời sau khi sử dụng
                File.Delete(tempFilePath);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }          
        }

        // Sử dụng để chạy ví dụ
        public void RunExample(string fileName, string key)
        {
            try
            {
                MemoryStream msPowerPoint = ENC.FileDecrypt(fileName, key);

                // Gọi phương thức để đọc PowerPoint từ MemoryStream
                ReadPowerPointFromMemoryStream(msPowerPoint);

                //// Tạo một MemoryStream từ tệp PowerPoint
                //using (MemoryStream memoryStream = new MemoryStream(File.ReadAllBytes(fileName)))
                //{
                //    // Gọi phương thức để đọc PowerPoint từ MemoryStream
                //    ReadPowerPointFromMemoryStream(memoryStream);
                //}
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }


}
