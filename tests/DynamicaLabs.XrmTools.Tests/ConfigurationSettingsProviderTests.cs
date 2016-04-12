using DynamicaLabs.Tools;
using System;
using Xunit;
using System.Linq;

namespace DynamicaLabs.XrmTools.Tests
{
    public class ConfigurationSettingsProviderTests
    {
        [Fact]
        public void TestGetSingle()
        {
            var provider = new ConfigurationSettingsProvider();
            Assert.Equal("value1", provider.Get("test1"));
        }

        [Fact]
        public void TestGetSingleWithPrefix()
        {
            var provider = new ConfigurationSettingsProvider("pr");
            Assert.Equal("prvalue1", provider.Get("test1"));
        }

        [Fact]
        public void TestGetMany()
        {
            var provider = new ConfigurationSettingsProvider();
            var settings = provider.GetMany(new[] {"test1", "test2", "test3"}).ToList();
            Assert.Equal(3, settings.Count);
            Assert.False(settings.Any(it => it.Value == null));
            for (var i = 1; i <= 3; i++)
            {
                Assert.Contains($"value{i}", settings.Select(it => it.Value));
            }
        }

        [Fact]
        public void TestWithPrefix()
        {
            var provider = new ConfigurationSettingsProvider("pr");
            var settings = provider.GetMany(new[] {"test1", "test2"}).ToList();
            Assert.Equal(2, settings.Count);
            Assert.False(settings.Any(it => it.Value == null));
            for (var i = 1; i <= 2; i++)
            {
                Assert.Contains($"prvalue{i}", settings.Select(it => it.Value));
            }
        }

        [Fact]
        public void TestWithPrefixDots()
        {
            var provider = new ConfigurationSettingsProvider("pr:");
            var settings = provider.GetMany(new[] {"test1", "test2"}).ToList();
            Assert.Equal(2, settings.Count);
            Assert.False(settings.Any(it => it.Value == null));
            for (var i = 1; i <= 2; i++)
            {
                Assert.Equal($"prvalue{i}", settings[i - 1].Value);
            }
        }

        [Fact]
        public void TestSetMany()
        {
            var provider = new ConfigurationSettingsProvider("pr:");
            var ex = Assert.Throws<Exception>(() => { provider.SetMany(null); });

            Assert.Equal("Configuration doesn't support this option.", ex.Message);
        }
    }
}