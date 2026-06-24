using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Project.Core.Extension
{
    public class CipherHelper
    {
        public static string Encrypt(string plainText)
        {   
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes("W920V2daSMBijylX");
                aes.IV = Encoding.UTF8.GetBytes("OUVyzD8pe3Ygdp41");

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(plainText);
                        }
                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
        }

        public static string Decrypt(string cipherText)
        {
            cipherText = cipherText.Replace(" ", "+");
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes("W920V2daSMBijylX");
                aes.IV = Encoding.UTF8.GetBytes("OUVyzD8pe3Ygdp41");

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
