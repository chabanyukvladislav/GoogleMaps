using System.Threading.Tasks;
using MapsApiLibrary.Api.MethodExtensions;
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

        public virtual Task<string> GetResponseAsync(string parametrs)
        {
            var url = string.Format($"{0}?{1}", _url, parametrs);
            var response =  _sender.SendAsync(url).GetContentAsync();
            return response;
        }
    }
}
