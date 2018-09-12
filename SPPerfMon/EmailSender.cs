using SPPerfMon.FailureTypes;
using System;
using System.Linq;
using System.Net.Mail;

namespace Qdabra.Utility.SharePointPerfMon
{
    class EmailSender
    {
        internal static void SendEmail(PerfMonSettings settings, string title, string message)
        {
            try
            {
                var client = new SmtpClient();

                var msg = new MailMessage()
                {
                    IsBodyHtml = false,
                    Subject = title,
                    Body = message,
                    To = { settings.NotificationRecipients },
                };

                client.Send(msg);
            }
            catch (Exception e)
            {
                LogWriter.WriteError($"Encountered error while sending e-mail: {e.Message}");
            }
        }

        internal static string FormatFailure(MonitorFailure failure, int num) =>
            $"Issue {num + 1}: {failure}\n\nDetails:\n\n{failure.SPResult.GetDetails()}";

        internal static void SendAlert(string url, PerfMonSettings settings, params MonitorFailure[] failures)
        {
            var body = $"SharePoint Performance Monitor detected performance issues\n\n{string.Join("\n\n\n", failures.Select(FormatFailure))}\n\nSee log output for details.";

            SendEmail(
                settings,
                $"[Alert] SharePoint performance issues detected on {url}",
                body
            );
        }
    }
}
