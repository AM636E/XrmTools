using System;
using System.IO;
using System.Text;
using Xunit;
using DynamicaLabs.Tools.Encryption;

namespace DynamicaLabs.XrmTools.Tests
{
    public class RijndaelEncryptionServiceTests
    {
        private readonly RijndaelEncryptionService _encryptionService =
            new RijndaelEncryptionService(Guid.NewGuid().ToString());

        [Fact]
        public void TestEncrypt()
        {
            var data = Guid.NewGuid().ToString();

            var enc = _encryptionService.Encrypt(data);

            Assert.Equal(data, _encryptionService.Decrypt(enc));
        }

        [Fact]
        public void TestEncryptBytes()
        {
            var data = Encoding.Unicode.GetBytes(Guid.NewGuid().ToString());

            var enc = _encryptionService.Encrypt(data);

            Assert.Equal(Encoding.Unicode.GetString(data), Encoding.Unicode.GetString(_encryptionService.Decrypt(enc)));
        }

        [Fact]
        public void TestEncryptFile()
        {
            var data = Guid.NewGuid().ToString();
            File.WriteAllText("data.test", data);
            _encryptionService.Encrypt("data.test", "encrypted.test");
            _encryptionService.Decrypt("encrypted.test", "decrypted.test");
            Assert.Equal(data, File.ReadAllText("decrypted.test"));
        }
    }
}