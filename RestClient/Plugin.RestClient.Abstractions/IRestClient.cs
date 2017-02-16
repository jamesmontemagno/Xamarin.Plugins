using System.Net.Http;
using System.Threading.Tasks;

namespace Plugin.RestClient.Abstractions
{
    /// <summary>
    /// Interface for RestClient
    /// </summary>
    public interface IRestClient
    {
        /// <summary>
        /// Sends an http request asynchronously
        /// </summary>
        /// <param name="request">HTTP message containing all the parameters needed for the request</param>
        /// <returns>The task object representing the asynchronous operation</returns>
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
    }
}
