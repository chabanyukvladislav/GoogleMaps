using System.Threading.Tasks;
using MapsApiLibrary.Helpers.MethodExtensions;
using MapsApiLibrary.Api.RequestSenders;

namespace MapsApiLibrary.Api
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
