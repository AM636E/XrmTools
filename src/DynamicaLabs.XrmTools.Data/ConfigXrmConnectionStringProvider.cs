namespace DynamicaLabs.XrmTools.Data
{
    public class ConfigXrmConnectionStringProvider : IXrmConnectionStringProvider
    {
        private readonly ConnectionSettings _connectionSettings;

        public ConfigXrmConnectionStringProvider(ConnectionSettings connectionSettings)
        {
            _connectionSettings = connectionSettings;
        }

        public string GetUsername()
        {
            return _connectionSettings.Username;
        }

        public string GetPassword()
        {
            return _connectionSettings.Password;
        }

        public string GetUrl()
        {
            return _connectionSettings.Url;
        }
    }
}