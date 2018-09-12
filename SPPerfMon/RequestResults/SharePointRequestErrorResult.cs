using System.Collections.Generic;
using System.Linq;

namespace Qdabra.Utility.SharePointPerfMon.RequestResults
{
    class SharePointRequestErrorResult : SharePointRequestResult
    {
        protected override IEnumerable<string> CsvValues() => Enumerable.Empty<string>();
    }
}
