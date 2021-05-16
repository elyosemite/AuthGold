using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AuthGold.Contracts;
using AuthGold.Database;
using AuthGold.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthGold.Providers
{
    public class AESEncryptProvider : IAESEncryptation
    {
        public byte[] DecryptData(byte[] EncryptedText, string Encryptionkey)
        {
            RijndaelManaged objrij = new RijndaelManaged();  
            objrij.Mode = CipherMode.CBC;  
            objrij.Padding = PaddingMode.PKCS7;  
  
            objrij.KeySize = 0x80;  
            objrij.BlockSize = 0x80;
            byte[] encryptedTextByte = EncryptedText;  
            byte[] passBytes = Encoding.UTF8.GetBytes(Encryptionkey);  
            byte[] EncryptionkeyBytes = new byte[0x10];  
            int len = passBytes.Length;  
            if (len > EncryptionkeyBytes.Length)  
            {  
                len = EncryptionkeyBytes.Length;  
            }  
            Array.Copy(passBytes, EncryptionkeyBytes, len);  
            objrij.Key = EncryptionkeyBytes;  
            objrij.IV = EncryptionkeyBytes;  
            byte[] TextByte = objrij.CreateDecryptor().TransformFinalBlock(encryptedTextByte, 0, encryptedTextByte.Length);  
            return TextByte;
        }

        public byte[] EncryptData(byte[] textData, string Encryptionkey)
        {
            RijndaelManaged objrij = new RijndaelManaged();
            //set the mode for operation of the algorithm
            objrij.Mode = CipherMode.CBC;
            //set the padding mode used in the algorithm.
            objrij.Padding = PaddingMode.PKCS7;
            //set the size, in bits, for the secret key.
            objrij.KeySize = 0x80;
            //set the block size in bits for the cryptographic operation.   
            objrij.BlockSize = 0x80;
            //set the symmetric key that is used for encryption & decryption.    
            byte[] passBytes = Encoding.UTF8.GetBytes(Encryptionkey);  
            //set the initialization vector (IV) for the symmetric algorithm    
            byte[] EncryptionkeyBytes = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };  
  
            int len = passBytes.Length;  
            if (len > EncryptionkeyBytes.Length)  
            {  
                len = EncryptionkeyBytes.Length;  
            }  
            Array.Copy(passBytes, EncryptionkeyBytes, len);  
  
            objrij.Key = EncryptionkeyBytes;  
            objrij.IV = EncryptionkeyBytes;  
  
            //Creates a symmetric AES object with the current key and initialization vector IV.    
            ICryptoTransform objtransform = objrij.CreateEncryptor();  
            byte[] textDataByte = textData;  
            //Final transform the test string.  
            return objtransform.TransformFinalBlock(textDataByte, 0, textDataByte.Length);
        }
    }
}
