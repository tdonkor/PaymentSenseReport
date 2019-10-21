using Acrelec.Library.Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PaymentSenseReport
{
    public class AppConfiguration
    {
        private class ConfigurationEntry
        {
            public string Key { get; set; }

            public string Section { get; set; }

            public string Value { get; set; }
        }

        static AppConfiguration()
        {
            Instance = new AppConfiguration();
        }

        private IList<ConfigurationEntry> _entries;

        private AppConfiguration(string iniPath = null)
        {
            var executingAssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            var path = new FileInfo(iniPath ?? executingAssemblyName + ".ini").FullName;
            ParseFile(path);
        }

        private void ParseFile(string path)
        {
            _entries = new List<ConfigurationEntry>();

            if (!File.Exists(path))
            {
                Console.WriteLine($"Configuration file {path} not found. Using default values.");
                Log.Info($"Configuration file {path} not found. Using default values.");
                return;
            }
            else
            {
                Console.WriteLine($"Configuration file {path} found\n.Using Config values....");
                Log.Info($"Configuration file {path} found\n.Using Config values....");
            }

            var section = string.Empty;
            foreach (var line in File.ReadAllLines(path))
            {
                var sectionMatch = Regex.Match(line, @"^\[(\w+)\]\s*$");
                if (sectionMatch.Success)
                {
                    section = sectionMatch.Groups[1].Value;
                    continue;
                }

                var entryMatch = Regex.Match(line, @"^([^;]+?)=([\s\S]+?)$");
                if (entryMatch.Success)
                {
                    _entries.Add(new ConfigurationEntry()
                    {
                        Section = section,
                        Key = entryMatch.Groups[1].Value,
                        Value = entryMatch.Groups[2].Value
                    });
                }
            }
        }

        public string ReportPath
        {
            get
            {
                return _entries.FirstOrDefault(_ => _.Key == "REPORT_PATH")?.Value ?? @"C:\acrelec\PaymentSenseEODReports\EOD_Reports\";
            }
        }

        public string IniPath
        {
            get
            {
                return _entries.FirstOrDefault(_ => _.Key == "INIT_PATH")?.Value ?? @"C:\acrelec\PaymentSenseEODReports\EO";
            }
        }

        public string UserName
        {
            get
            {
                return _entries.FirstOrDefault(_ => _.Key == "USERNAME")?.Value ?? "Acrelec";
            }
        }
        public string Password
        {
            get
            {
                return _entries.FirstOrDefault(_ => _.Key == "PASSWORD")?.Value ?? "8ffb32c4-8b29-428d-b5e8-896f7ca7890d";
            }
        }

        public string UserAccountUrl
        {
            get
            {
                return _entries.FirstOrDefault(_ => _.Key == "USER_ACCOUNT_URL")?.Value ?? "https://st185l090000.test.connect.paymentsense.cloud";
            }
        }
        public string Tid
        {
            get
            {
                return _entries.FirstOrDefault(_ => _.Key == "TID")?.Value ?? "22163665";
            }
        }
        public string MediaType
        {
            get
            {
                return _entries.FirstOrDefault(_ => _.Key == "MEDIATYPE")?.Value ?? "application/connect.v2+json";
            }
        }
        public string InstallerId
        {
            get
            {
                return _entries.FirstOrDefault(_ => _.Key == "INSTALLERID")?.Value ?? "ST185L09";
            }
        }
        public string SoftwareHouseId
        {
            get
            {
                return _entries.FirstOrDefault(_ => _.Key == "SOFTWAREHOUSEID")?.Value ?? "ST185L09";
            }
        }

        public static AppConfiguration Instance { get; }
    }
}
