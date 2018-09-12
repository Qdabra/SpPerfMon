using System;
using System.Collections.Generic;
using System.Linq;

namespace Qdabra.Utility.SharePointPerfMon.RequestResults
{
    abstract class SharePointRequestResult
    {
        private const int CsvColumnCount = 9;

        public string Url { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Message { get; set; }

        internal double TotalSeconds => (End - Start).TotalSeconds;

        protected abstract IEnumerable<string> CsvValues();

        internal string ToCsv()
        {
            var sharedValues = new[] { Url, Start.ToString("s"), End.ToString("s"), TotalSeconds.ToString(), Message };
            var values = sharedValues.Concat(CsvValues()).ToList();

            var padded = values.Concat(Enumerable.Repeat("", CsvColumnCount - values.Count));

            return string.Join(",", padded);
        }

        public virtual string GetDetails() =>
            string.Join("\n", $"Time: {Start}", $"Total Duration: {TotalSeconds}");
    }
}
