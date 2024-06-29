using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Demo_Encrypt_LoadFile.Feature
{
    class FolderEncryptor
    {
        private readonly byte[] key;
        private readonly byte[] iv;

        public FolderEncryptor(string password)
        {
            byte[] salt = Encoding.UTF8.GetBytes("YourSaltHere");
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(password, salt);
            key = pdb.GetBytes(32);
            iv = pdb.GetBytes(16);
        }

        public string EncryptFolderName(string folderName)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                byte[] encryptedBytes;

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(folderName);
                        }
                        encryptedBytes = msEncrypt.ToArray();
                    }
                }

                return Convert.ToBase64String(encryptedBytes);
            }
        }

        public string DecryptFolderName(string encryptedFolderName)
        {
            byte[] cipherText = Convert.FromBase64String(encryptedFolderName);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }

        public static void RenameFile(string oldFolderPath, string newFolderPath)
        {
            // Kiểm tra xem tệp tồn tại không
            if (Directory.Exists(oldFolderPath))
            {
                Directory.Move(oldFolderPath, newFolderPath);
            }
            else
            {
                Console.WriteLine("File does not exist: " + oldFolderPath);
            }
        }
    }
}
