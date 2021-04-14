using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TypedClientLibrary
{
    public interface ITypedClient
    {
        /// <summary>
        /// Test rate limited service
        /// </summary>
        /// <param name="cancellationToken">Stopping token</param>
        /// <returns>HTTP response</returns>
        Task<HttpResponseMessage> TestRateLimitedService(CancellationToken cancellationToken);
    }

    internal class TypedHttpClient : ITypedClient
    {
        public HttpClient Client { get; }

        public TypedHttpClient(HttpClient client)
        {
            Client = client;
        }

        public async Task<HttpResponseMessage> TestRateLimitedService(CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri("http://localhost:5000/api/status")
            };

            using HttpResponseMessage httpResponseMessage = await Client.SendAsync(request, cancellationToken);

            httpResponseMessage.EnsureSuccessStatusCode();

            return httpResponseMessage;
        }
    }
}
