using System.Net.Http;
using System.Threading.Tasks;

namespace MapsApiLibrary.Api.RequestSenders
{
    internal interface ISender
    {
        Task<HttpResponseMessage> SendAsync(string url);
    }
}
