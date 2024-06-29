using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_Encrypt_LoadFile.Feature
{
    class Encrypt_FileName
    {
        public static string EncodeFileNameInPath(string filePath)
        {
            string directory = Path.GetDirectoryName(filePath);
            string fileName = Path.GetFileName(filePath);
            string encodedFileName = EncodeFileName(fileName);
            return Path.Combine(directory, encodedFileName);
        }

        // Mã hóa tên file
        private static string EncodeFileName(string fileName)
        {
            byte[] fileNameBytes = Encoding.UTF8.GetBytes(fileName);
            string encodedFileName = Convert.ToBase64String(fileNameBytes);
            return encodedFileName;
        }

        // Giải mã tên file trong đường dẫn
        public static string DecodeFileNameInPath(string filePath)
        {
            string directory = Path.GetDirectoryName(filePath);
            string fileName = Path.GetFileName(filePath);
            string decodedFileName = DecodeFileName(fileName);
            return Path.Combine(directory, decodedFileName);
        }

        // Giải mã tên file
        private static string DecodeFileName(string encodedFileName)
        {           
            {
                byte[] encodedFileNameBytes = Convert.FromBase64String(encodedFileName);
                string decodedFileName = Encoding.UTF8.GetString(encodedFileNameBytes);
                return decodedFileName;
            }           
        }

        public static void RenameFile(string oldFilePath, string newFilePath)
        {
            // Kiểm tra xem tệp tồn tại không
            if (File.Exists(oldFilePath))
            {
                File.Move(oldFilePath, newFilePath);
            }
            else
            {
                Console.WriteLine("File does not exist: " + oldFilePath);
            }
        }


    }
}
