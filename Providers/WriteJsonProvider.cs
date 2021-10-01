using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using AuthGold.Contracts;
using AuthGold.Models;
using Microsoft.Extensions.Configuration;

namespace AuthGold.Providers
{
    public class WriteJsonProvider : IJsonManipulate
    {
        public string key { get; set; }
        private readonly IAESEncryptation _aes;
        private readonly IConfiguration _configuration;
        
        public WriteJsonProvider(IAESEncryptation aes, IConfiguration configuration)
        {
            _aes = aes;
            _configuration = configuration;
            key = _configuration.GetSection("SecuritySession").GetSection("SecretKey").Value;
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

                using(FileStream fs = File.Open(filepath, FileMode.Open, FileAccess.Write, FileShare.None))
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
            else 
            {
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

        public static async void CreateFile(string filepath)
        {
            // Create
            if(!File.Exists(filepath))
            {
                byte[] bytesToWrite = Encoding.Unicode.GetBytes("Yuri dos Santos Melo");
                using(FileStream flst = File.Create(filepath))
                {
                    await flst.WriteAsync(bytesToWrite, 0, bytesToWrite.Length);
                }
            }
            else
            {
                // Open and Write

                byte[] bytesToWrite2 = Encoding.Unicode.GetBytes("\nIgor dos Santos Melo");
                using(FileStream fs = File.Open(filepath, FileMode.Append, FileAccess.Write))
                {
                    await fs.WriteAsync(bytesToWrite2, 0, bytesToWrite2.Length);
                }
            }
        }
    }
}
