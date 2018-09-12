using Qdabra.Utility.SharePointPerfMon.RequestResults;

namespace SPPerfMon.FailureTypes
{
    abstract class ThresholdExceededFailure : MonitorFailure
    {
        internal ThresholdExceededFailure(SharePointRequestResult result) : base(result) { }

        public abstract string ThresholdDescription { get; }

        public decimal Value { get; protected set; }

        public abstract string Unit { get; }

        public override string ToString() => $"{ThresholdDescription} was {Value} {Unit}";
    }
}
