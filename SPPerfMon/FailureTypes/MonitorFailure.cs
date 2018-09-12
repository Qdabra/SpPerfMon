using Qdabra.Utility.SharePointPerfMon.RequestResults;
using System;

namespace SPPerfMon.FailureTypes
{
    class MonitorFailure
    {
        public DateTime Occurrence { get; }
        public SharePointRequestResult SPResult { get; }

        public MonitorFailure(SharePointRequestResult result)
        {
            Occurrence = DateTime.Now;
            SPResult = result;
        }
    }
}
