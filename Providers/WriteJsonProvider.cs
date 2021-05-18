using System;
using System.Collections.Generic;
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
                var requestTraces = new List<RequestTrace>();

                byte[] encryptedData = BringBytes(filepath);
                byte[] decryptedData = DecryptedReqTrace(encryptedData);
                string dataInString = Encoding.UTF8.GetString(decryptedData);
                
                var requestTrace1 = JsonSerializer.Deserialize<List<RequestTrace>>(dataInString);
                requestTrace1.Add(reqTrace);

                using(FileStream fs = File.Open(filepath, FileMode.Open))
                {
                    AddEncryptedReqTrace(fs, requestTrace1);
                }
            }
            else
            {
                var rt = new List<RequestTrace>();
                rt.Add(reqTrace);
                using(FileStream fs = File.Create(filepath))
                {
                    AddEncryptedReqTrace(fs, rt);
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
                    fs.Write(decryptedData, 0, decryptedData.Length);
                }
            }
            else {
                Console.WriteLine("Arquivo não existe.");
            }
        }

        private void AddEncryptedReqTrace(FileStream fs, List<RequestTrace> reqTrace)
        {
            string str = JsonSerializer.Serialize<List<RequestTrace>>(reqTrace);

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
