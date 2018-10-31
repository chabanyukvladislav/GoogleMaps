using System.Threading.Tasks;
using MapsApiLibrary.Api;
using MapsApiLibrary.Api.Parametrs.Directions;
using MapsApiLibrary.Helpers;
using MapsApiLibrary.Models.Directions;

namespace MapsApiLibrary
{
    public class DirectionsController
    {
        private readonly AbstractApi _directions;

        public DirectionsParametrs Parametrs { get; }

        public DirectionsController()
        {
            _directions = new Directions();
            Parametrs = new DirectionsParametrs();
        }

        public async Task<DirectionsResult> GetResult()
        {
            var response = await _directions.GetResponseAsync(Parametrs);
            return JsonDeserializer<DirectionsResult>.Deserialize(response);
        }
    }
}
