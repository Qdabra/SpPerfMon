using Qdabra.Utility.SharePointPerfMon.RequestResults;

namespace SPPerfMon.FailureTypes
{
    class SPRequestDurationFailure : ThresholdExceededFailure
    {
        internal SPRequestDurationFailure(SharePointRequestSuccessResult result)
            : base(result)
        {
            Value = decimal.Parse(result.SharePointRequestDuration);
        }
        
        public override string ThresholdDescription => "SPRequestDuration";

        public override string Unit => "milliseconds";
    }
}
