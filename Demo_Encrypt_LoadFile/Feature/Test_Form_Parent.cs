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
    // Biến để kiểm tra xem frm_TittleView đã được mở hay chưa

    public partial class Test_Form_Parent : Form
    {
        private bool isTittleViewOpened = false;
        private bool isLoadGalleryOpened = false;

        public Test_Form_Parent()
        {
            InitializeComponent();
        }

        private void accordionControlElement2_Click(object sender, EventArgs e)
        {
           

            // Đóng form frm_LoadGallery_API nếu nó đang mở
            foreach (Form frm in Application.OpenForms)
            {
                if (frm is frm_TittleView)
                {
                    frm.Close();
                    isTittleViewOpened = false;
                    break;
                }
            }

            try
            {
                if (!isLoadGalleryOpened)
                {
                    frm_LoadGallery_API frm = new frm_LoadGallery_API();
                    frm.MdiParent = this;
                    Test_Form_Parent fm = new Test_Form_Parent();
                    fm.Text = "Danh sách tải các bài giảng";
                    frm.Show();

                    isLoadGalleryOpened = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void accordionControlElement3_Click(object sender, EventArgs e)
        {
            try
            {
               

                // Đóng form frm_LoadGallery_API nếu nó đang mở
                foreach (Form frm_ in Application.OpenForms)
                {
                    if (frm_ is frm_LoadGallery_API)
                    {
                        frm_.Close();
                        isLoadGalleryOpened = false;
                        break;
                    }
                }

                if (!isTittleViewOpened)
                {
                    frm_TittleView frm = new frm_TittleView();
                    frm.MdiParent = this;
                    Test_Form_Parent fm = new Test_Form_Parent();
                    fm.Text = "Danh sách đã tải";
                    frm.Show();

                    // Đặt cờ để chỉ ra rằng frm_TittleView đã được mở
                    isTittleViewOpened = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
