using System.Configuration;

namespace DynamicaLabs.XrmTools.Core.Configuration
{
    public class ConfigWrapper : IConfigWrapper
    {
        public string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}