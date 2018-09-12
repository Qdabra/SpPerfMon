using System.Configuration;

namespace Qdabra.Utility.SharePointPerfMon.Configuration
{
    class Endpoint : ConfigurationElement
    {
        private const string NameKey = "name";

        [ConfigurationProperty(NameKey)]
        public string Name
        {
            get { return base[NameKey] as string; }
            set { base[NameKey] = value; }
        }

        private const string UrlKey = "url";

        [ConfigurationProperty(UrlKey)]
        public string Url
        {
            get { return base[UrlKey] as string; }
            set { base[UrlKey] = value; }
        }
    }
}
