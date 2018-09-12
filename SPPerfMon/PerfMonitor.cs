using Qdabra.Utility.SharePointPerfMon.Configuration;
using Qdabra.Utility.SharePointPerfMon.RequestResults;
using SPPerfMon.FailureTypes;
using System;
using System.Net;
using System.Threading;

namespace Qdabra.Utility.SharePointPerfMon
{
    class PerfMonitor
    {
        private static object RequestLock = new object();

        public FailureTracker Tracker { get; }
        public PerfMonSettings Settings { get; }
        public Endpoint Endpoint { get; }

        public PerfMonitor(PerfMonSettings settings, Endpoint endpoint)
        {
            Settings = settings;
            Tracker = new FailureTracker(settings);
            Endpoint = endpoint;
        }

        private static string GetHeader(WebResponse response, string key) => response.Headers[key];

        private bool ExceedsMaxResponseTime(SharePointRequestSuccessResult result) =>
            (decimal)result.TotalSeconds > Settings.MaxResponseSeconds;

        private void ReportFailure(MonitorFailure failure) =>
            Tracker.ReportFailure(Endpoint.Url, failure);
            

        private void ProcessRequestResult(SharePointRequestSuccessResult result)
        {
            if (ExceedsMaxResponseTime(result))
            {
                ReportFailure(new TotalRequestTimeFailure(result));
            }
            else if (ExceedsMaxRequestDuration(result))
            {
                ReportFailure(new SPRequestDurationFailure(result));
            }
            else if (ExceedsMaxHealthScore(result))
            {
                ReportFailure(new SharePointHealthFailure(result));
            }

            LogWriter.WriteLogEntry(Endpoint.Name, result);
        }

        private bool ExceedsMaxRequestDuration(SharePointRequestSuccessResult result) =>
            decimal.TryParse(result.SharePointRequestDuration, out var value) && value > Settings.MaxRequestDurationMilliseconds;

        private bool ExceedsMaxHealthScore(SharePointRequestSuccessResult result) =>
            int.TryParse(result.SharePointHealthScore, out var value) && value > Settings.MaxSharePointHealth;

        private void ProcessRequestFailure(SharePointRequestErrorResult result)
        {
            ReportFailure(new RequestFailure(result));

            LogWriter.WriteLogEntry(Endpoint.Name, result);
        }

        private void MakeRequest()
        {
            var start = DateTime.Now;
            var queryUrl = Endpoint.Url;

            try
            {
                var request = WebRequest.Create(queryUrl);

                request.Headers.Add("X-FORMS_BASED_AUTH_ACCEPTED", "f");
                request.Credentials = Settings.Credentials;

                using (var response = request.GetResponse())
                {
                    ProcessRequestResult(new SharePointRequestSuccessResult
                    {
                        Url = queryUrl,
                        Start = start,
                        End = DateTime.Now,
                        Message = "success",
                        SharePointHealthScore = GetHeader(response, "X-SharePointHealthScore"),
                        SharePointRequestDuration = GetHeader(response, "SPRequestDuration"),
                        SharePointIisLatency = GetHeader(response, "SPIisLatency"),
                        SharePointRequestGuid = GetHeader(response, "SPRequestGuid"),
                    });
                }
            }
            catch (Exception e)
            {
                ProcessRequestFailure(new SharePointRequestErrorResult
                {
                    Url = queryUrl,
                    Start = start,
                    End = DateTime.Now,
                    Message = e.Message,
                });
            }
        }

        internal void RunMonitor()
        {
            LogWriter.EnsureLogFolder();

            while (true)
            {
                // Multiple concurrent requests from the same worker process can cause WebRequest
                // to become deadlocked and time out. So only allow one request at a time
                lock (RequestLock)
                {
                    MakeRequest();
                }

                Thread.Sleep(Settings.IntervalSeconds * 1000);
            }
        }
    }
}
