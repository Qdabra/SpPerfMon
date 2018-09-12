using System.Configuration;

namespace Qdabra.Utility.SharePointPerfMon.Configuration
{
    public class SpPerfMonSection : ConfigurationSection
    {
        public static SpPerfMonSection GetSection()
            => ConfigurationManager.GetSection("spPerfMon") as SpPerfMonSection;

        private const string EndpointsKey = "endpoints";

        [ConfigurationProperty(EndpointsKey)]
        public EndpointCollection Endpoints
        {
            get { return base[EndpointsKey] as EndpointCollection; }
            set { base[EndpointsKey] = value; }
        }
    }
}
