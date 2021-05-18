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
        public string key = "28B20FC2E5D6731C";

        private readonly IAESEncryptation _aes;
        public WriteJsonProvider(IAESEncryptation aes)
        {
            _aes = aes;
        }

        public void WriteEncryptedJson(string filepath, RequestTrace reqTrace)
        {
            if(File.Exists(filepath))
            {
                byte[] encryptedData = BringBytes(filepath);
                byte[] decryptedData = DecryptedReqTrace(encryptedData);
                string dataInString = Convert.ToBase64String(decryptedData);
                //Console.WriteLine(dataInString);
                RequestTrace requestTrace = JsonSerializer.Deserialize<RequestTrace>(dataInString);

                using(FileStream fs = File.Open(filepath, FileMode.Open))
                {
                    fs.Seek(0, SeekOrigin.End);
                    AddEncryptedReqTrace(fs, requestTrace);
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
        public byte[] BringBytes(string filepath)
        {
            byte[] bytes;
            using(FileStream fs = File.Open(filepath, FileMode.Open))
            {
                bytes = new byte[fs.Length];
                fs.Read(bytes, 0, bytes.Length);
            }
            return bytes;
        }

        public void WriteDecryptedJson(string filepath, string outputFilePath)
        {
            if(File.Exists(filepath))
            {
                byte[] bytes = BringBytes(filepath);
                using(FileStream fs = File.Create(outputFilePath))
                {
                    byte[] decryptedData = DecryptedReqTrace(bytes);
                    Console.WriteLine(Convert.ToBase64String(decryptedData));
                    fs.Write(decryptedData, 0, decryptedData.Length);
                    Console.WriteLine("Estágio 5");
                }
            }
            else {
                Console.WriteLine("Arquivo não existe.");
            }
        }

        private void AddEncryptedReqTrace(FileStream fs, RequestTrace reqTrace)
        {
            string str = $"{JsonSerializer.Serialize<RequestTrace>(reqTrace)}\n";

            byte[] bytes = Encoding.UTF8.GetBytes(str);

            byte[] encryptedBytes = _aes.EncryptData(bytes, key);
            
            fs.Write(encryptedBytes, 0, encryptedBytes.Length);
        }

        private byte[] DecryptedReqTrace(byte[] encryptedData)
        {
            return _aes.DecryptData(encryptedData, key);
        }
    }
}
