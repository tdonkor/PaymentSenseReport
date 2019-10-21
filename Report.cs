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
using Acrelec.Library.Logger;
namespace PaymentSenseReport
{

    public class Report
    {
        private string username;
        private string password;
        private string requestId;
        private string tid;
        private string url;
        private string installerId;
        private string softwareHouseId;
        private string mediaType;
        private string reportPath;
        

       // private static readonly string ReportPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Report");
        
       // AppConfiguration configFile;
    

        /// <summary>
        /// Constructor initialise settings
        /// </summary>
        public Report()
        {


            //// configFile = AppConfiguration.Instance;
            // username = configFile.UserName;
            // password = configFile.Password;
            // url = configFile.UserAccountUrl;
            // tid = configFile.Tid;
            // installerId = configFile.InstallerId;
            // softwareHouseId = configFile.SoftwareHouseId;
            // mediaType = configFile.MediaType;
            // reportPath = configFile.ReportPath;
            // iniPath = configFile.IniPath;

             username = ConfigurationManager.AppSettings["username"];
             password = ConfigurationManager.AppSettings["password"];
             url = ConfigurationManager.AppSettings["url"];
             tid = ConfigurationManager.AppSettings["tid"];
             installerId = ConfigurationManager.AppSettings["installerId"];
             softwareHouseId = ConfigurationManager.AppSettings["softwareHouseId"];
             mediaType = ConfigurationManager.AppSettings["mediaType"];
             reportPath = ConfigurationManager.AppSettings["reportPath"];

        }


        /// <summary>
        /// Start the report 
        /// this runs at a time set by the scheduler
        /// </summary>
        public IRestResponse EndOfDayPostRequest()
        {

            try
            {


                //if (File.Exists(reportPath))
                //{
                //    File.Delete(reportPath);
                //}

                Log.Info("Starting Report: ");
               // var config = AppConfiguration.Instance;

                RestClient client = Authenticate(url + "/pac/terminals/" + tid + "/reports");
                var request = new RestRequest(Method.POST);
                request = RequestParams(request);
                request.AddParameter("EndOfDay", "{\r\n  \"reportType\": \"END_OF_DAY\"\r\n}", ParameterType.RequestBody);
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

                        if (response.Content.Contains("REPORT COMPLETE"))
                        {
                            Log.Info("\nReport Complete....\n");
                            break;
                        }
                    }

                }

                //Save json report details.

                var outputDirectory = reportPath;
                  
                    var outputPath = Path.Combine(outputDirectory, $"{DateTime.Now:yyyyMMddHHmmss}_End_Of_Day_Report.json");


                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                    //Write the new ticket
                    File.WriteAllText(outputPath, response.Content);

                Log.Info($"\nEnd of Day Report complete  is at: {outputPath}\n");

                return response;


            }
            catch (Exception ex)
            {
                Log.Error(ex);

                return null;
             
            }
           
        }

        /// <summary>
        /// Get end of day request ID
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public IRestResponse GetEndOfDayData(string requestId, string url)
        {

            RestClient client = Authenticate(url + "/pac/terminals/" + tid + "/reports/" + requestId);
            var request = new RestRequest(Method.GET);
            request = RequestParams(request);

            IRestResponse response = client.Execute(request);

            return response;
        }


        /// <summary>
        /// The Request Parameters for the REST API calls 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private RestRequest RequestParams(RestRequest request)
        {
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", mediaType);
            request.AddHeader("Software-House-Id", softwareHouseId);
            request.AddHeader("Installer-Id", installerId);
            request.AddHeader("Connection", "keep-alive");

            return request;
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
