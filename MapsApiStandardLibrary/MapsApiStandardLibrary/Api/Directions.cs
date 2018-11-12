using System.Net.Http;
using System.Threading.Tasks;
using MapsApiStandardLibrary.Api.Parameters.Directions;
using MapsApiStandardLibrary.Helpers;

namespace MapsApiStandardLibrary.Api
{
    internal class Directions : AbstractApi
    {
        private const string Url = ApiUrls.DirectionsUrl;

        public Directions() : base(Url) { }

        public override Task<string> GetResponseAsync(string parameters)
        {
            DirectionsParameters param = parameters;
            if (string.IsNullOrWhiteSpace(param.Key) || string.IsNullOrWhiteSpace(param.Origin) ||
                string.IsNullOrWhiteSpace(param.Destination))
            {
                throw new HttpRequestException();
            }

            return base.GetResponseAsync(parameters);
        }
    }
}
