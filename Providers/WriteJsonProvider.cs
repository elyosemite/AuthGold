using System.IO;
using System.Text;
using System.Text.Json;
using AuthGold.Models;

namespace AuthGold.Providers
{
    public class WriteJsonProvider
    {
        public void WriteJson(string filepath, RequestTrace reqTrace)
        {
            if(File.Exists(filepath))
            {
                using(FileStream fs = File.Open(filepath, FileMode.Open))
                {
                    fs.Seek(0, SeekOrigin.End);
                    AddReqTrace(fs, reqTrace);
                }
            }
            else
            {
                using(FileStream fs = File.Create(filepath))
                {
                    AddReqTrace(fs, reqTrace);
                }
            }
        }

        private void AddReqTrace(FileStream fs, RequestTrace reqTrace)
        {
            string str = $"{JsonSerializer.Serialize(reqTrace)}\n";
            byte[] bytes = Encoding.Unicode.GetBytes(str);
            fs.Write(bytes, 0, bytes.Length);
        }
    }
}
