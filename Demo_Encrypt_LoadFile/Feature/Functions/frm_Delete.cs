using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Demo_Encrypt_LoadFile.Feature.Functions
{
    class frm_Delete
    {
        #region Xóa bài giảng
        public static void Delete_Lession(string uuid_Selected, string file_path, string image_path, string meta_image_path, string path_XML_Course)
        {
            try
            {
                if (File.Exists(file_path))
                {
                    File.Delete(file_path);
                }

                if (File.Exists(image_path))
                {
                    File.Delete(image_path);
                }

                if (File.Exists(meta_image_path))
                {
                    File.Delete(meta_image_path);
                }

                XDocument doc = XDocument.Load(path_XML_Course);
                doc.Descendants("Lessions")
                   .Where(p => (string)p.Element("uuid") == uuid_Selected)
                   .Remove();

                doc.Save(path_XML_Course);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Xóa khóa học
        public static void Delete_Cource(string uuid_Selected, string file_path, string image_path, string meta_image_path, string path_XML_Course)
        {
            try
            {               
                if (File.Exists(image_path))
                {
                    File.Delete(image_path);
                }

                if (File.Exists(meta_image_path))
                {
                    File.Delete(meta_image_path);
                }

                XDocument doc = XDocument.Load(path_XML_Course);
                doc.Descendants("Courses")
                   .Where(p => (string)p.Element("uuid") == uuid_Selected)
                   .Remove();

                doc.Save(path_XML_Course);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

    }
}
