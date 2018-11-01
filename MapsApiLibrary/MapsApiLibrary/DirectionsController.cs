using System.Threading.Tasks;
using MapsApiLibrary.Api;
using MapsApiLibrary.Api.Parameters.Directions;
using MapsApiLibrary.Helpers;
using MapsApiLibrary.Models.Directions;

namespace MapsApiLibrary
{
    public class DirectionsController
    {
        private readonly AbstractApi _directions;

        public DirectionsParameters Parameters { get; }

        public DirectionsController()
        {
            _directions = new Directions();
            Parameters = new DirectionsParameters();
        }

        public async Task<DirectionsResult> GetResult()
        {
            var response = await _directions.GetResponseAsync(Parameters);
            return JsonDeserializer<DirectionsResult>.Deserialize(response);
        }
    }
}
