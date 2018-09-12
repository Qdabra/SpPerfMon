using Qdabra.Utility.SharePointPerfMon.RequestResults;

namespace SPPerfMon.FailureTypes
{
    class SharePointHealthFailure : ThresholdExceededFailure
    {
        internal SharePointHealthFailure(SharePointRequestSuccessResult result)
            : base(result)
        {
            Value = decimal.Parse(result.SharePointHealthScore);
        }

        public override string ThresholdDescription => "SharePoint Health Score";

        public override string Unit => "";
    }
}
