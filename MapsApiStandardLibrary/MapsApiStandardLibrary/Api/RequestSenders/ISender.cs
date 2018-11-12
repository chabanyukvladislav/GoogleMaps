using System.Net.Http;
using System.Threading.Tasks;

namespace MapsApiStandardLibrary.Api.RequestSenders
{
    internal interface ISender
    {
        Task<HttpResponseMessage> SendAsync(string url);
    }
}
