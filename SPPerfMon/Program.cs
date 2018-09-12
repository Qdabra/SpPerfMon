using Qdabra.Utility.SharePointPerfMon.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Qdabra.Utility.SharePointPerfMon
{
    class Program
    {
        static void RunMonitor(PerfMonSettings settings, Endpoint endpoint) => new PerfMonitor(settings, endpoint).RunMonitor();

        static void Main(string[] args)
        {
            try
            {
                var settings = PerfMonSettings.LoadSettings();

                if (args.Any(a => a.Equals("--testemail", StringComparison.InvariantCultureIgnoreCase)))
                {
                    TestEmail(settings);
                    return;
                }

                Parallel.ForEach(settings.Endpoints, (endpoint) => RunMonitor(settings, endpoint));
            }
            catch (Exception e)
            {
                LogWriter.WriteError($"Catastrophic failure: {e.Message} {e.StackTrace}");
            }
        }

        private static void TestEmail(PerfMonSettings settings)
        {
            Console.WriteLine("Testing e-mail settings...");

            EmailSender.SendEmail(settings, "Email Test", "It Works!\nIt Works!");
        }
    }
}
