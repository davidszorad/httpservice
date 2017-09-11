using System;
using System.Net.Http;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace HttpClient
{
    public interface IHttpClientHelper
    {
        HttpClient GetHttpClient(string url, string key);

        RetryPolicy CreateRetryPolicy(int retryCount, TimeSpan timeSpan);

        TResult DeserializeJsonResponse<TResult>(HttpResponseMessage response);
    }
}
