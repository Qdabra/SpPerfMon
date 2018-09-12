using Qdabra.Utility.SharePointPerfMon.RequestResults;
using System;
using System.IO;

namespace Qdabra.Utility.SharePointPerfMon
{
    static class LogWriter
    {
        private static object ErrorLogLock = new object();
        private static object SharedLogLock = new object();

        private const string LogsFolder = "Logs";

        internal static void EnsureLogFolder()
        {
            if (!Directory.Exists(LogsFolder))
            {
                Directory.CreateDirectory(LogsFolder);
            }
        }

        private static void WriteToFile(string path, string value)
        {
            File.AppendAllText(path, value + Environment.NewLine);
        }

        private static string EnsureLogHeader(string endpointName = null)
        {
            var suffix = endpointName == null ? "" : $" {endpointName}";

            var path = Path.Combine(LogsFolder, $"{DateTime.Now.ToString("yyyyMMdd")}{suffix}.csv");

            if (!File.Exists(path))
            {
                var header = string.Join(",",
                    "Url",
                    "Request Start",
                    "Request End",
                    "Request Time Elapsed",
                    "Outcome",
                    "X-SharepointHealthScore",
                    "SPRequestDuration",
                    "SPIisLatency",
                    "SPRequestGuid"
                );

                WriteToFile(path, header);
            }

            return path;
        }

        private static void WriteLogEntryWithHeader(string line, string endpointName = null)
        {
            var filePath = EnsureLogHeader(endpointName);

            WriteToFile(filePath, line);
        }

        internal static void WriteLogEntry(string endpointName, SharePointRequestResult result)
        {
            try
            {
                var line = result.ToCsv();

                WriteLogEntryWithHeader(line, endpointName);

                lock (SharedLogLock)
                {
                    WriteLogEntryWithHeader(line);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error writing log: {e.Message}");
            }
        }

        internal static void WriteError(string error)
        {
            try
            {
                Console.WriteLine($"Error: {error}");

                var now = DateTime.Now;

                var path = Path.Combine(LogsFolder, $"{now.ToString("yyyyMMdd")} Errors.txt");
                var line = $"[{now.ToString("s")}] {error}";

                lock (ErrorLogLock)
                {
                    WriteToFile(path, line);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Encountered error when trying to write to error log: {e.Message}");
            }
        }
    }
}
