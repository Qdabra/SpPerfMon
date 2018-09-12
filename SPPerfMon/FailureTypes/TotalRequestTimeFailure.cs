using Qdabra.Utility.SharePointPerfMon.RequestResults;

namespace SPPerfMon.FailureTypes
{
    class TotalRequestTimeFailure : ThresholdExceededFailure
    {
        internal TotalRequestTimeFailure(SharePointRequestSuccessResult result)
            : base(result)
        {
            Value = (decimal)result.TotalSeconds;
        }

        public override string ThresholdDescription => "Total request time";

        public override string Unit => "second(s)";
    }
}
