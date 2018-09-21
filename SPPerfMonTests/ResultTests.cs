using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qdabra.Utility.SharePointPerfMon.RequestResults;

namespace SPPerfMonTests
{
    [TestClass]
    public class ResultTests
    {
        [TestMethod]
        public void SanitizeErrors()
        {
            var url = "http://mycompany.sharepoint.com";
            var start = new DateTime(2018, 9, 21, 5, 0, 0);
            var end = start.AddMilliseconds(2050);


            var result = new SharePointRequestErrorResult
            {
                Url = url,
                Start = start,
                End = end,
                Message = "Everything,went\r\nall\rwrong"
            };

            Assert.AreEqual("http://mycompany.sharepoint.com,2018-09-21T05:00:00,2018-09-21T05:00:02,2.05,Everything went all wrong,,,,", result.ToCsv());
        }
    }
}
