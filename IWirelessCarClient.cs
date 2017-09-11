using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpClient
{
    /// <summary>
    /// Http client for accessories.
    /// </summary>
    public interface IWirelessCarClient : IDisposable
    {
        /// <summary>
        /// Http client.
        /// </summary>
        HttpClient Client { get; }

        /// <summary>
        /// Http client helper.
        /// </summary>
        IHttpClientHelper HttpClientHelper { get; }

        /// <summary>
        /// Does the request and maps it to T model.
        /// </summary>
        /// <typeparam name="T">Type to map with.</typeparam>
        /// <param name="endpoint">The endpoint.</param>
        /// <returns>Mapped response.</returns>
        T DoRequest<T>(string endpoint);
    }
}
