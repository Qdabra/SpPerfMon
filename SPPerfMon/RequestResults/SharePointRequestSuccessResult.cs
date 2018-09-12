using System.Collections.Generic;

namespace Qdabra.Utility.SharePointPerfMon.RequestResults
{
    class SharePointRequestSuccessResult : SharePointRequestResult
    {
        internal string SharePointHealthScore { get; set; }
        internal string SharePointRequestDuration { get; set; }
        internal string SharePointIisLatency { get; set; }
        internal string SharePointRequestGuid { get; set; }

        protected override IEnumerable<string> CsvValues() => new[]
        {
            SharePointHealthScore,
            SharePointRequestDuration,
            SharePointIisLatency,
            SharePointRequestGuid
        };

        public override string GetDetails() => 
            string.Join("\n", base.GetDetails(), $"SPHealthScore: {SharePointHealthScore}", $"SPRequestDuration: {SharePointRequestDuration}");
    }
}
