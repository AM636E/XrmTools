using System;
using System.Collections.Generic;
using Xunit;
using DynamicaLabs.Tools;
using DynamicaLabs.Tools.Encryption;

namespace DynamicaLabs.XrmTools.Tests
{
    public class EncryptedSettingsProviderTests
    {
        private static readonly string Key = Guid.NewGuid().ToString();
        private readonly IEnctyptionService _enctyptionService = new RijndaelEncryptionService(Key);
        private readonly EncryptedSettingsProvider _provider;

        private readonly JsonSettingsProvider _sprovider = new JsonSettingsProvider(new PhysicalFileProvider(),
            "encsettings.json");

        public EncryptedSettingsProviderTests()
        {
            _provider =
                new EncryptedSettingsProvider(
                    _sprovider,
                    _enctyptionService,
                    new[]
                    {
                        "test1",
                        "test2"
                    });
        }

        [Fact]
        public void TestSetSingle()
        {
            var data = Guid.NewGuid().ToString();
            _provider.Set(new KeyValuePair<string, string>("test1", data));
            _provider.Set(new KeyValuePair<string, string>("test3", data));
            Assert.NotEqual(data, _sprovider.Get("test1"));
            Assert.Equal(data, _sprovider.Get("test3"));
            Assert.Equal(data, _enctyptionService.Decrypt(_sprovider.Get("test1")));
        }


        [Fact]
        public void TestGetMany()
        {
            var data = Guid.NewGuid().ToString();
            _provider.Set(new KeyValuePair<string, string>("test1", data));

            Assert.Equal(data, _provider.Get("test1"));
        }
    }
}