namespace AuthGold.Contracts
{
    public interface IAESEncryptation
    {
        byte[] EncryptData(byte[] textData, string Encryptionkey);  
        byte[] DecryptData(byte[] EncryptedText, string Encryptionkey);
    }
}
