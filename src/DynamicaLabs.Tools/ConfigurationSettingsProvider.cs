using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace DynamicaLabs.Tools
{
    public sealed class ConfigurationSettingsProvider : BaseSettingsProvider
    {
        private readonly string _prefix;

        public ConfigurationSettingsProvider(string prefix = "")
        {
            _prefix = prefix;
            if (!string.IsNullOrEmpty(prefix) && !_prefix.EndsWith(":"))
            {
                _prefix += ":";
            }
        }

        public override IEnumerable<KeyValuePair<string, string>> GetMany(string[] keys)
        {
            return keys.Select(it => new KeyValuePair<string, string>(it, ConfigurationManager.AppSettings[$"{_prefix}{it}"]));
        }

        public override void SetMany(IEnumerable<KeyValuePair<string, string>> values)
        {
            throw new Exception("Configuration doesn't support this option.");
        }
    }
}