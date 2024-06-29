using AxWMPLib;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Application = Microsoft.Office.Interop.PowerPoint.Application;
using Shape = Microsoft.Office.Interop.PowerPoint.Shape;
namespace Demo_Encrypt_LoadFile.Feature
{
    class frm_PowerPoint
    {
        #region Ẩn taskbar
        // Import các hàm từ thư viện user32.dll
        [DllImport("user32.dll")]
        private static extern int FindWindow(string className, string windowText);

        [DllImport("user32.dll")]
        private static extern int ShowWindow(int hwnd, int command);

        // Hằng số cho ShowWindow
        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;
        #endregion

        #region Variables  
        private Application pptApp;
        private Presentation pptPresentation;
        SlideShowSettings pptSlideShowSettings = null;
        public string fileName = string.Empty;
        public string frm_key = string.Empty;
        private string temp = string.Empty;
        #endregion

        #region Functions

        #region public void RunPowerPoint(string fileName, string key)
        public void RunPowerPoint(string fileName, string key)
        {
            try
            {
                MemoryStream msPowerPoint = ENC.FileDecrypt(fileName, key);

                // Gọi phương thức để đọc PowerPoint từ MemoryStream
                OpenPowerPointFile(msPowerPoint);                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region public void OpenPowerPointFile(string filePath)
        private void OpenPowerPointFile(MemoryStream msPowerPoint)
        {
            try
            {
                // Tạo một tệp tạm thời từ MemoryStream
                string tempFilePath = Path.GetTempFileName();
                File.WriteAllBytes(tempFilePath, msPowerPoint.ToArray());
                temp = tempFilePath;

                pptApp = new Application();

                //pptPresentation = pptApp.Presentations.Open(textBox1.Text, ReadOnly: MsoTriState.msoTrue);
                //pptPresentation = pptApp.Presentations.Open(filePath, WithWindow: MsoTriState.msoFalse, ReadOnly: MsoTriState.msoTrue);

                // Tạo một Presentation từ tệp tạm thời
                pptPresentation = pptApp.Presentations.Open(
                    tempFilePath, // Đường dẫn của tệp tạm thời
                    WithWindow: MsoTriState.msoFalse, ReadOnly: MsoTriState.msoTrue
                );

                // Thiết lập cấu hình trình chiếu (Nếu có màn hình thứ 2 thì sẽ tự hiện màn hình presenter view)
                pptSlideShowSettings = pptPresentation.SlideShowSettings;
                pptSlideShowSettings.ShowPresenterView = MsoTriState.msoTrue; // Thiết lập chế độ presenter view
                pptSlideShowSettings.ShowType = PpSlideShowType.ppShowTypeSpeaker;
                pptSlideShowSettings.RangeType = PpSlideShowRangeType.ppShowAll;

                // Chạy trình chiếu
                pptPresentation.SlideShowSettings.Run();


                // Đăng ký sự kiện BeforeSave
                pptApp.PresentationBeforeSave += PptApp_PresentationBeforeSave;

                // Đăng ký sự kiện SlideShowEnd
                pptApp.SlideShowEnd += PptApp_SlideShowEnd;

                //// Hiển thị ứng dụng PowerPoint
                //pptApp.Visible = MsoTriState.msoTrue;

                //pptProcess = Process.Start(startInfo);

                //// Ẩn taskbar
                //int hwnd = FindWindow("Shell_TrayWnd", "");
                //ShowWindow(hwnd, SW_HIDE);

            }
            catch (Exception ex)
            {
                ////Hiển thị lại thanh taskbar
                //int hwnd = FindWindow("Shell_TrayWnd", "");
                //ShowWindow(hwnd, SW_SHOW);

                MessageBox.Show(ex.Message);
            }            
        }
           
        #endregion

        #region public void PptApp_PresentationBeforeSave(Presentation Pres, ref bool Cancel)
        public void PptApp_PresentationBeforeSave(Presentation Pres, ref bool Cancel)
        {
            try
            {
                // Hủy lưu tệp
                Cancel = true;

                // Chặn việc mở cửa sổ chuẩn bị (preparation window)
                Pres.SlideShowSettings.ShowPresenterView = MsoTriState.msoFalse;

                MessageBox.Show("Không được phép lưu tệp PowerPoint này.");
            }
            catch(Exception ex)
            {
                ////Hiển thị lại thanh taskbar
                //int hwnd = FindWindow("Shell_TrayWnd", "");
                //ShowWindow(hwnd, SW_SHOW);

                MessageBox.Show(ex.Message);
            }
           
        }
        #endregion

        #region public void PptApp_SlideShowEnd(Presentation Pres)         
        public void PptApp_SlideShowEnd(Presentation Pres)
        {
            try
            {
                Process[] pros = Process.GetProcesses();
                for (int i = 0; i < pros.Count(); i++)
                {
                    if (pros[i].ProcessName.ToLower().Contains("powerpnt"))
                    {
                        pros[i].Kill();
                    }
                }

                File.Delete(temp);
                             
                ////Hiển thị lại thanh taskbar
                //int hwnd = FindWindow("Shell_TrayWnd", "");
                //ShowWindow(hwnd, SW_SHOW);
            }
            catch (Exception ex)
            {
                ////Hiển thị lại thanh taskbar
                //int hwnd = FindWindow("Shell_TrayWnd", "");
                //ShowWindow(hwnd, SW_SHOW);
            }
        }
        #endregion

        #endregion
    }
}
