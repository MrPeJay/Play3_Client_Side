using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Play3_Client_Side.API
{
    class ApiHelper
    {
        public static HttpClient ApiClient { get; set; }

        public static void InitializeClient()
        {
            ApiClient = new HttpClient {BaseAddress = new Uri("https://localhost:44333/") };
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
