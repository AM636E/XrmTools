namespace DynamicaLabs.Tools.Encryption
{
    public sealed class RijndaelEncryptionService : IEnctyptionService
    {
        private readonly string _key;

        public RijndaelEncryptionService(string key)
        {
            _key = key;
        }

        public string Encrypt(string data)
        {
            return RijndaelHelper.Encrypt(data, _key);
        }

        public byte[] Encrypt(byte[] data)
        {
            return RijndaelHelper.Encrypt(data, _key);
        }

        public string Decrypt(string data)
        {
            return RijndaelHelper.Decrypt(data, _key);
        }

        public byte[] Decrypt(byte[] data)
        {
            return RijndaelHelper.Decrypt(data, _key);
        }

        public void Encrypt(string fileIn, string fileOut)
        {
            RijndaelHelper.Encrypt(fileIn, fileOut, _key);
        }

        public void Decrypt(string fileIn, string fileOut)
        {
            RijndaelHelper.Decrypt(fileIn, fileOut, _key);
        }
    }
}