using System;
using System.Collections.Generic;
using System.Globalization;
using Xunit;
using DynamicaLabs.Tools;
using System.Linq;

namespace DynamicaLabs.XrmTools.Tests
{
    public class JsonSettingsProviderTests
    {
        private static readonly IFileProvider Provider = new PhysicalFileProvider();

        private static readonly JsonSettingsProvider JsonSettingsProvider = new JsonSettingsProvider(Provider,
            "settings.json");

        [Fact]
        public void TestGetSingle()
        {
            Assert.Equal("value1", JsonSettingsProvider.Get("test1"));
        }

        [Fact]
        public void TestSetSingle()
        {
            JsonSettingsProvider.Set(new KeyValuePair<string, string>("test11", "value2"));
            Assert.Equal("value2", JsonSettingsProvider.Get("test11"));
        }


        [Fact]
        public void TestGetMany()
        {
            var settings = JsonSettingsProvider.GetMany(new[] {"test1", "test2", "test3"}).ToList();
            Assert.Equal(3, settings.Count);
            Assert.False(settings.Any(it => it.Value == null));
            for (var i = 1; i <= 3; i++)
            {
                Assert.Contains($"value{i}", settings.Select(it => it.Value));
            }
        }

        [Fact]
        public void TestSetMany()
        {
            var val = DateTime.Now;
            JsonSettingsProvider.SetMany(new[]
            {
                new KeyValuePair<string, string>("to-set1", val.ToString(CultureInfo.InvariantCulture) + "1"),
                new KeyValuePair<string, string>("to-set2", val.ToString(CultureInfo.InvariantCulture) + "2"),
                new KeyValuePair<string, string>("to-set3", val.ToString(CultureInfo.InvariantCulture) + "3")
            });
            var settings =
                JsonSettingsProvider.GetMany(new[] {"test1", "test2", "test3", "to-set1", "to-set2", "to-set3"})
                    .ToList();
            Assert.Equal(6, settings.Count);
            Assert.False(settings.Any(it => it.Value == null));
            for (var i = 1; i <= 3; i++)
            {
                Assert.Contains($"value{i}", settings.Select(it => it.Value));
                Assert.Contains($"{val.ToString(CultureInfo.InvariantCulture)}{i}", settings.Select(it => it.Value));
            }
        }
    }
}