using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Newtonsoft.Json;

namespace HttpClient
{
    public class HttpClientHelper : IHttpClientHelper
    {
        public RetryPolicy CreateRetryPolicy(int retryCount, TimeSpan timeSpan)
        {
            var strategy = new HttpTransientErrorDetectionStrategy();
            var fixedinterval = new FixedInterval(retryCount, timeSpan);
            var retryPolicy = new RetryPolicy(strategy, fixedinterval);
            return retryPolicy;
        }

        public HttpClient GetHttpClient(string url, string key)
        {
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(key))
            {
                var message = string.Format("Empty or null argument provided to GetHttplClient: url = {0} ; key = {1}",
                    url, key);
                throw new ArgumentException(message);
            }

            var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(35);
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", key);
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        public TResult DeserializeJsonResponse<TResult>(HttpResponseMessage response)
        {
            using (var responseStream = response.Content.ReadAsStreamAsync().Result)
            {
                using (StreamReader sr = new StreamReader(responseStream))
                {
                    using (JsonReader reader = new JsonTextReader(sr))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        return serializer.Deserialize<TResult>(reader);
                    }
                }

            }
        }
    }
}
