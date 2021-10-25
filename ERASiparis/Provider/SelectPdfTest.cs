using System;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace ERASiparis.Provider
{
    public class SelectPdfTest
    {
        public static string apiEndpoint = "https://selectpdf.com/api2/convert/";
        public static string apiKey = "your license key here";
        public static string testUrl = "https://selectpdf.com";

    
        // POST JSON example using HttpWebRequest / HttpWebResponse (and Newtonsoft for JSON serialization)
        public static void SelectPdfPostWithHttpWebRequest()
        {
            System.Console.WriteLine("Starting conversion with HttpWebRequest ...");

            // set parameters
            SelectPdfParameters parameters = new SelectPdfParameters();
            parameters.key = apiKey;
            parameters.url = testUrl;

            // JSON serialize parameters
            string jsonData = JsonConvert.SerializeObject(parameters);
            byte[] byteData = Encoding.UTF8.GetBytes(jsonData);

            // create request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiEndpoint);

            request.ContentType = "application/json";
            request.Method = "POST";
            request.Credentials = CredentialCache.DefaultCredentials;

            // POST parameters
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteData, 0, byteData.Length);
            dataStream.Close();

            // GET response (if response code is not 200 OK, a WebException is raised)
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    // all ok - read PDF and write on disk (binary read!!!!)
                    MemoryStream ms = BinaryReadStream(responseStream);

                    // write to file
                    FileStream file = new FileStream("test1.pdf", FileMode.Create, FileAccess.Write);
                    ms.WriteTo(file);
                    file.Close();
                }
                else
                {
                    // error - get error message
                    System.Console.WriteLine("Error code: " + response.StatusCode.ToString());
                }
                responseStream.Close();
            }
            catch (WebException webEx)
            {
                // an error occurred
                System.Console.WriteLine("Error: " + webEx.Message);

                HttpWebResponse response = (HttpWebResponse)webEx.Response;
                Stream responseStream = response.GetResponseStream();

                // get details of the error message if available (text read!!!)
                StreamReader readStream = new StreamReader(responseStream);
                string message = readStream.ReadToEnd();
                responseStream.Close();

                System.Console.WriteLine("Error Message: " + message);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Error: " + ex.Message);
            }

            System.Console.WriteLine("Finished.");
        }

        // Binary read from Stream into a MemoryStream
        public static MemoryStream BinaryReadStream(Stream input)
        {
            int bytesNumber = 0;
            byte[] bytes = new byte[1025];
            MemoryStream stream = new MemoryStream();
            BinaryReader reader = new BinaryReader(input);

            do
            {
                bytesNumber = reader.Read(bytes, 0, bytes.Length);
                if (bytesNumber > 0)
                    stream.Write(bytes, 0, bytesNumber);
            } while (bytesNumber > 0);

            stream.Position = 0;
            return stream;
        }
        public class SelectPdfParameters
        {
            public string key { get; set; }
            public string url { get; set; }
            public string html { get; set; }
            public string base_url { get; set; }
        }
    }
}
