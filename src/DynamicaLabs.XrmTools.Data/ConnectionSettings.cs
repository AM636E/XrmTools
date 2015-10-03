using System.Configuration;

namespace DynamicaLabs.XrmTools.Data
{
    public class ConnectionSettings : ConfigurationSection
    {
        [ConfigurationProperty("username", IsRequired = false)]
        public string Username
        {
            get { return this["username"].ToString(); }
            set { this["username"] = value; }
        }

        [ConfigurationProperty("password", IsRequired = false)]
        public string Password
        {
            get { return this["password"].ToString(); }
            set { this["password"] = value; }
        }

        [ConfigurationProperty("url", IsRequired = false)]
        public string Url
        {
            get { return this["url"].ToString(); }
            set { this["url"] = value; }
        }


        [ConfigurationProperty("debug", IsRequired = false)]
        public bool Debug
        {
            get
            {
                return bool.Parse(this["debug"].ToString());
            }
            set { this["debug"] = value; }
        }

        public string ToConnectionString()
        {
            return $"Url={Url}; Username={Username}; Password={Password}";
        }
    }
}