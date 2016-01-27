using Plugin.RestClient.Abstractions;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Plugin.RestClient
{
    /// <summary>
    /// Implementation for Feature
    /// </summary>
    public class RestClientImplementation : IRestClient
    {
        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            throw new NotImplementedException();
        }
    }
}