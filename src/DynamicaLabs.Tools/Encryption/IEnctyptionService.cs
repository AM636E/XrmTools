namespace DynamicaLabs.Tools.Encryption
{
    public interface IEnctyptionService
    {
        string Encrypt(string data);
        byte[] Encrypt(byte[] data);
        string Decrypt(string data);
        byte[] Decrypt(byte[] data);
        void Encrypt(string fileIn, string fileOut);
        void Decrypt(string fileIn, string fileOut);
    }
}