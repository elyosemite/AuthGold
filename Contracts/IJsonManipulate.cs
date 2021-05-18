using AuthGold.Models;

namespace AuthGold.Contracts
{
    public interface IJsonManipulate
    {
        void WriteEncryptedJson(string filepath, RequestTrace reqTrace);
        void WriteDecryptedJson(string filepath, string outputFilePath);
    }
}
