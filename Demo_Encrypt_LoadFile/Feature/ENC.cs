using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Demo_Encrypt_LoadFile.Feature
{
    class ENC
    {

        public static void FileEncrypt(string inputImagePath, string outputImagePath, string key)
        {
            using (FileStream fsInput = new FileStream(inputImagePath, FileMode.Open))
            using (FileStream fsOutput = new FileStream(outputImagePath, FileMode.Create))
            {
                byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(key);
                passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.Key = passwordBytes;
                    AES.Mode = CipherMode.ECB;

                    using (CryptoStream cs = new CryptoStream(fsOutput, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        fsInput.CopyTo(cs);
                    }
                }
            }
        }

        public static MemoryStream FileDecrypt(string inputImagePath, string key)
        {
            try
            {
                if (File.Exists(inputImagePath))
                {
                    FileStream fsInput = new FileStream(inputImagePath, FileMode.Open);
                    MemoryStream msOutput = new MemoryStream();

                    byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(key);
                    passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

                    using (RijndaelManaged AES = new RijndaelManaged())
                    {
                        AES.Key = passwordBytes;
                        AES.Mode = CipherMode.ECB;

                        using (CryptoStream cs = new CryptoStream(fsInput, AES.CreateDecryptor(), CryptoStreamMode.Read))
                        {
                            cs.CopyTo(msOutput);
                        }
                    }
                    return msOutput;
                }
                else
                    return null;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
