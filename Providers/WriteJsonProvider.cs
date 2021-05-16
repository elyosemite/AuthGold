using System;
using System.IO;
using System.Text;
using System.Text.Json;
using AuthGold.Contracts;
using AuthGold.Models;

namespace AuthGold.Providers
{
    public class WriteJsonProvider : IJsonManipulate
    {

        private readonly IAESEncryptation _aes;
        public WriteJsonProvider(IAESEncryptation aes)
        {
            _aes = aes;
        }

        public void WriteEncryptedJson(string filepath, RequestTrace reqTrace)
        {
            if(File.Exists(filepath))
            {
                using(FileStream fs = File.Open(filepath, FileMode.Open))
                {
                    fs.Seek(0, SeekOrigin.End);
                    AddEncryptedReqTrace(fs, reqTrace);
                }
            }
            else
            {
                using(FileStream fs = File.Create(filepath))
                {
                    AddEncryptedReqTrace(fs, reqTrace);
                }
            }
        }

        public void WriteDecryptedJson(string filepath)
        {
            if(File.Exists(filepath))
            {
                using(FileStream fs = File.Open(filepath, FileMode.Open))
                {
                    Console.WriteLine("Entrei no arquivo...");
                    byte[] bytes = new byte[fs.Length-1];
                    fs.Read(bytes, 0, bytes.Length);
                    Console.WriteLine(BitConverter.ToString(bytes[0..1024]));
                }
            }
            else {
                Console.WriteLine("Arquivo não existe.");
            }
        }

        private void AddEncryptedReqTrace(FileStream fs, RequestTrace reqTrace)
        {
            string str = $"{JsonSerializer.Serialize(reqTrace)}\n";
            string key = "28B20FC2E5D6731C";

            byte[] bytes = Encoding.Unicode.GetBytes(str);

            byte[] encryptedBytes = _aes.EncryptData(bytes, key);
            fs.Write(encryptedBytes, 0, bytes.Length);
        }

        private void AddDecryptedReqTrace(FileStream fs, RequestTrace reqTrace)
        {
            string str = $"{JsonSerializer.Serialize(reqTrace)}\n";
            string key = "28B20FC2E5D6731C";

            byte[] bytes = Encoding.Unicode.GetBytes(str);

            byte[] encryptedBytes = _aes.EncryptData(bytes, key);
            fs.Write(encryptedBytes, 0, bytes.Length);
        }
    }
}
