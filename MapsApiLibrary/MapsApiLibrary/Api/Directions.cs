using System.Net.Http;
using System.Threading.Tasks;
using MapsApiLibrary.Api.Parametrs.Directions;
using MapsApiLibrary.Helpers;

namespace MapsApiLibrary.Api
{
    internal class Directions : AbstractApi
    {
        private const string Url = ApiUrls.DirectionsUrl;

        public Directions() : base(Url) { }

        public override Task<string> GetResponseAsync(string parametrs)
        {
            DirectionsParametrs param = parametrs;
            if (string.IsNullOrWhiteSpace(param.Key) || string.IsNullOrWhiteSpace(param.Origin) ||
                string.IsNullOrWhiteSpace(param.Destination))
            {
                throw new HttpRequestException();
            }

            return base.GetResponseAsync(parametrs);
        }
    }
}
