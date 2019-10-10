using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Configuration;
using RestSharp.Authenticators;
using System.IO;
using System.Reflection;

namespace PaymentSenseReport
{

    public class Report
    {
        private string username;
        private string password;
        private string requestId;
        private string tid;
        private string url;
        private string currency;

        private static readonly string ticketPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Report");


        AppConfiguration configFile;

        /// <summary>
        /// Constructor initialise settings
        /// </summary>
        public Report()
        {
            configFile = AppConfiguration.Instance;
            username = configFile.UserName;
            password = configFile.Password;
            url = configFile.UserAccountUrl;
            tid = configFile.Tid;
            currency = configFile.Currency;
        }


        /// <summary>
        /// Start the report 
        /// this runs at a time set by the scheduler
        /// </summary>
        public IRestResponse EndOfDayPostRequest()
        {

            Console.WriteLine("Starting Report: ");
            var config = AppConfiguration.Instance;

            RestClient client = Authenticate(url + "/pac/terminals/" + tid + "/reports");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "*/*");
            request.AddHeader("Connection", "keep-alive");
            request.AddParameter("undefined", "{\r\n  \"reportType\": \"END_OF_DAY\"\r\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            //check reponse isSuccessful
            if (response.IsSuccessful)
            {
                //deserialise response
                ReportResp reportResponse = JsonConvert.DeserializeObject<ReportResp>(response.Content);
                requestId = reportResponse.RequestId;

                //poll for result every 1 seconds block until finish
                //
                while (true)
                {
                    Thread.Sleep(1000);
                    response = GetEndOfDayData(requestId, url);

                    Console.WriteLine(response.Content + "\n\n");
                   

                    if (response.Content.Contains("REPORT COMPLETE"))
                    {
                        Console.WriteLine("\nReport Finished");
                        break;
                    }
                }

                //deserialise response
                RootObject reportDetails = JsonConvert.DeserializeObject<RootObject>(response.Content);


            }

            var outputDirectory = Path.GetFullPath(config.OutPath);
            var outputPath = Path.Combine(outputDirectory, $"{DateTime.Now:yyyyMMddHHmmss}_Report.txt");

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            //Write the new ticket
            File.WriteAllText(outputPath, response.Content.ToString());
            return response;
        }





        public IRestResponse GetEndOfDayData(string requestId, string url)
        {

            RestClient client = Authenticate(url + "/pac/terminals/" + tid + "/reports/" + requestId);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "*/*");
            request.AddHeader("Connection", "keep-alive");

            IRestResponse response = client.Execute(request);

            return response;
        }


        private RestClient Authenticate(string url)
        {
            return new RestClient(url)
            {
                Authenticator = new HttpBasicAuthenticator(username, password)
            };
        }
    }

    /// <summary>
    /// structure of the transaction response 
    /// </summary>
    class ReportResp
    {
        public string RequestId { get; set; }
        public string Location { get; set; }
    }
}
