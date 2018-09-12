using Qdabra.Utility.SharePointPerfMon.RequestResults;

namespace SPPerfMon.FailureTypes
{
    class RequestFailure : MonitorFailure
    {
        internal RequestFailure(SharePointRequestErrorResult result)
            : base(result)
        {
            Message = result.Message;
        }

        public string Message { get; }

        public override string ToString() => $"Request failed with an error: {Message}";
    }
}
