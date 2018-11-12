using System.Threading.Tasks;
using MapsApiStandardLibrary.Helpers.MethodExtensions;
using MapsApiStandardLibrary.Api.RequestSenders;

namespace MapsApiStandardLibrary.Api
{
    internal abstract class AbstractApi
    {
        private readonly ISender _sender;
        private readonly string _url;

        protected AbstractApi(string url)
        {
            _sender = new HttpSender();
            _url = url;
        }

        public virtual Task<string> GetResponseAsync(string parameters)
        {
            var url = $"{_url}?{parameters}";
            var response =  _sender.SendAsync(url).GetContentAsync();
            return response;
        }
    }
}
