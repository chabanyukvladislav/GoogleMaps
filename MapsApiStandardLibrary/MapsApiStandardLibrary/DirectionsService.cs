using System.Threading.Tasks;
using MapsApiStandardLibrary.Api;
using MapsApiStandardLibrary.Api.Parameters.Directions;
using MapsApiStandardLibrary.Helpers;
using MapsApiStandardLibrary.Models.Directions;

namespace MapsApiStandardLibrary
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
            Parameters.Clear();
            return JsonDeserializer<DirectionsResult>.Deserialize(response);
        }
    }
}
