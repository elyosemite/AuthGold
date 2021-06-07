using System;
using System.Text;
using System.Security.Cryptography;

namespace AuthGold.Providers
{
    public static class MD5Provider
    {
        public static string GetMD5Hash(string key)
        {

            if(key.Length <= 0)
            {
                throw new Exception("You must put one key");
            }

            using (HashAlgorithm md5 = MD5.Create())
            {
                byte[] bytesOfKey = Encoding.UTF8.GetBytes(key);
                byte[] generatedBytesFromKey = Iteration(md5, 10000, bytesOfKey);

                StringBuilder hexHash = new StringBuilder();
                for(int i = 0; i < generatedBytesFromKey.Length; i++)
                {
                    hexHash.Append(generatedBytesFromKey[i].ToString("X2"));
                }

                return hexHash.ToString();
            }
        }

        public static byte[] Iteration(HashAlgorithm hashAlgorithm, int times, byte[] buffer)
        {
            byte[] generatedBytesFromKey = buffer;
            //var str = Encoding.UTF8.GetString(buffer);

            for (int i = 0; i < times; i++)
            {
                generatedBytesFromKey = hashAlgorithm.ComputeHash(generatedBytesFromKey);
                Console.WriteLine(ShowByte(generatedBytesFromKey));
            }

            return generatedBytesFromKey;
        }

        public static string ShowByte(byte[] buffer)
        {
            StringBuilder hexHash = new StringBuilder();
            for (int i = 0; i < buffer.Length; i++)
            {
                hexHash.Append(buffer[i].ToString("X2"));
            }
            return hexHash.ToString();
        }
    }
}
