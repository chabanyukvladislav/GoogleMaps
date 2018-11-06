using System.Threading.Tasks;
using MapsApiLibrary.Api;
using MapsApiLibrary.Api.Parameters.Directions;
using MapsApiLibrary.Helpers;
using MapsApiLibrary.Models.Directions;

namespace MapsApiLibrary
{
    public class DirectionsService : IService<DirectionsParameters, DirectionsResult>
    {
        private readonly AbstractApi _directions;

        public DirectionsParameters Parameters { get; }

        public DirectionsService()
        {
            _directions = new Directions();
            Parameters = new DirectionsParameters();
        }

        public async Task<DirectionsResult> GetResultAsync()
        {
            var response = await _directions.GetResponseAsync(Parameters);
            return JsonDeserializer<DirectionsResult>.Deserialize(response);
        }
    }
}
