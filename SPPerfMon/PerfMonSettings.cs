using Microsoft.SharePoint.Client;
using Qdabra.Utility.SharePointPerfMon.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security;

namespace Qdabra.Utility.SharePointPerfMon
{
    class PerfMonSettings
    {
        internal string SpSite { get; private set; }
        internal string Username { get; private set; }
        internal SecureString Password { get; private set; }
        internal int IntervalSeconds { get; private set; }

        internal string NotificationRecipients { get; private set; }

        internal int NotificationIntervalSeconds { get; private set; }

        internal decimal MaxResponseSeconds { get; private set; }
        internal decimal MaxRequestDurationMilliseconds { get; private set; }
        internal int MaxSharePointHealth { get; private set; }

        internal ICredentials Credentials => MakeCredentials(Username, Password);

        internal IList<Endpoint> Endpoints { get; private set; }

        private static string GetSetting(string key) =>
            ConfigurationManager.AppSettings.Get(key);

        private static int GetIntSetting(string key, int defaultValue)
            => int.TryParse(GetSetting(key), out int value) ? value : defaultValue;

        private static decimal GetDecimalSetting(string key, decimal defaultValue)
            => decimal.TryParse(GetSetting(key), out decimal value) ? value : defaultValue;

        private static SecureString MakeSecureString(string value) =>
            new NetworkCredential("", value).SecurePassword;

        private static ICredentials MakeCredentials(string username, SecureString password) =>
            new SharePointOnlineCredentials(username, password);


        internal static PerfMonSettings LoadSettings()
        {
            var settingsSection = SpPerfMonSection.GetSection();

            if(settingsSection == null)
            {
                throw new ApplicationException("Unable to load spPerfMon config section.");
            }

            return new PerfMonSettings
            {
                Username = GetSetting("username"),
                Password = MakeSecureString(GetSetting("password")),
                IntervalSeconds = GetIntSetting("intervalSeconds", 30),
                NotificationRecipients = GetSetting("notificationRecipients"),
                NotificationIntervalSeconds = GetIntSetting("notificationIntervalSeconds", 600),

                MaxResponseSeconds = GetDecimalSetting("maxResponseSeconds", 3),
                MaxRequestDurationMilliseconds = GetDecimalSetting("maxRequestDurationMilliseconds", 1000),
                MaxSharePointHealth = GetIntSetting("maxSharePointHealth", 6),
                Endpoints = settingsSection.Endpoints.OfType<Endpoint>().Where(e => !string.IsNullOrWhiteSpace(e.Url)).ToList(),
            };
        }
    }
}
