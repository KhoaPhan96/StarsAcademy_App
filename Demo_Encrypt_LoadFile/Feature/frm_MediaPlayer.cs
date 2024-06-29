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
    public partial class frm_MediaPlayer : Form
    {
        #region Variables
        public string frm_key = string.Empty;
        public string fileName = string.Empty;
        private string temp = string.Empty;
        #endregion

        #region Inits

        #region public frm_MediaPlayer()
        public frm_MediaPlayer()
        {
            InitializeComponent();
        }
        #endregion

        #region private void frm_MediaPlayer_Load(object sender, EventArgs e)
        private void frm_MediaPlayer_Load(object sender, EventArgs e)
        {
            try
            {
                MemoryStream msMediaPlayer = ENC.FileDecrypt(fileName, frm_key);
                string tempFilePath = Path.GetTempFileName();
                File.WriteAllBytes(tempFilePath, msMediaPlayer.ToArray());
                temp = tempFilePath;

                axWindowsMediaPlayer.URL = tempFilePath;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        #endregion

        #region private void frm_MediaPlayer_FormClosed(object sender, FormClosedEventArgs e)
        private void frm_MediaPlayer_FormClosed(object sender, FormClosedEventArgs e)
        {
            axWindowsMediaPlayer.Ctlcontrols.stop();
            File.Delete(temp);
        }
        #endregion

        #endregion

    }
}
