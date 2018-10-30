using System.Net.Http;
using System.Threading.Tasks;

namespace MapsApiLibrary.Api.RequestSenders
{
    interface ISender
    {
        Task<HttpResponseMessage> SendAsync(string url);
    }
}
