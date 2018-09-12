using System.Configuration;

namespace Qdabra.Utility.SharePointPerfMon.Configuration
{
    [ConfigurationCollection(typeof(Endpoint))]
    public class EndpointCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement() => new Endpoint();

        protected override object GetElementKey(ConfigurationElement element) => ((Endpoint)element).Name;

        protected override string ElementName => "endpoint";

        public override ConfigurationElementCollectionType CollectionType => ConfigurationElementCollectionType.BasicMapAlternate;
    }
}
