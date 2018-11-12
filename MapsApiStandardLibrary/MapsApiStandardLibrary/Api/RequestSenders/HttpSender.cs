using System.Net.Http;
using System.Threading.Tasks;

namespace MapsApiStandardLibrary.Api.RequestSenders
{
    internal class HttpSender : ISender
    {
        private readonly HttpClient _client;

        public HttpSender()
        {
            _client = new HttpClient();
        }

        public Task<HttpResponseMessage> SendAsync(string url)
        {
            return _client.GetAsync(url);
        }
    }
}
