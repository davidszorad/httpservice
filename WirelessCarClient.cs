using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Newtonsoft.Json;

namespace HttpClient
{
    public class WirelessCarClient : IWirelessCarClient
    {
        private readonly int _retryCounter = 3;
        private readonly TimeSpan _retryTimeSpan = TimeSpan.Zero;
        private readonly HttpClient _httpClient;

        public HttpClient Client => _httpClient;
        public IHttpClientHelper HttpClientHelper { get; }

        public WirelessCarClient()
        {
            HttpClientHelper = new HttpClientHelper();
            _httpClient = HttpClientHelper.GetHttpClient(
                Sitecore.Configuration.Settings.GetSetting("WirelessCarVapiServiceUrl"),
                Sitecore.Configuration.Settings.GetSetting("WirelessCarVapiAzureKey")
                );
        }

        public TResult DoRequest<TResult>(string endpoint)
        {
            var retryPolicy = HttpClientHelper.CreateRetryPolicy(_retryCounter, _retryTimeSpan);
            return retryPolicy.ExecuteAction(() =>
            {
                var message = new HttpRequestMessage(HttpMethod.Get, endpoint);
                return GetResponse<TResult>(message);
            });
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && _httpClient != null)
            {
                _httpClient.Dispose();
            }
        }

        private TResult GetResponse<TResult>(HttpRequestMessage message)
        {
            using (var response = _httpClient.SendAsync(message).Result)
            {
                if (Debugger.IsAttached && !response.IsSuccessStatusCode)
                {
                    Debugger.Break();
                }
                response.EnsureSuccessStatusCode();
                return HttpClientHelper.DeserializeJsonResponse<TResult>(response);
            }
        }
    }
}
