using SPPerfMon.FailureTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qdabra.Utility.SharePointPerfMon
{
    class FailureTracker
    {
        private static readonly TimeSpan Tolerance = new TimeSpan(0, 5, 0);

        private PerfMonSettings Settings { get; }
        private TimeSpan NotificationInterval { get; }

        private MonitorFailure LastFailure { get; set; }

        private DateTime? LastNotificationSent { get; set; }

        public FailureTracker(PerfMonSettings settings)
        {
            Settings = settings;

            NotificationInterval = new TimeSpan(0, 0, settings.NotificationIntervalSeconds);
        }

        private bool IsUnderFailureTolerance(MonitorFailure failure) =>
            failure.Occurrence - LastFailure.Occurrence < Tolerance;

        private bool NotificationIntervalHasElapsed() =>
            LastNotificationSent == null || DateTime.Now - LastNotificationSent > NotificationInterval;

        private bool ShouldSendNotification(MonitorFailure failure) =>
            LastFailure != null &&
            IsUnderFailureTolerance(failure) &&
            NotificationIntervalHasElapsed();

        internal void ReportFailure(string endpointUrl, MonitorFailure failure)
        {
            if (ShouldSendNotification(failure))
            {
                EmailSender.SendAlert(endpointUrl, Settings, LastFailure, failure);
                LastNotificationSent = DateTime.Now;
            }

            LastFailure = failure;
        }
    }
}
