using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_Encrypt_LoadFile.Feature.Functions
{
    class Graphical
    {
        public static void CreateProgressbar()
        {
            try
            {
                using (frm_Marquees progress = new frm_Marquees())
                {
                    progress.TopMost = true;
                    progress.ShowDialog();
                }
            }
            catch { }
        }
    }
}
