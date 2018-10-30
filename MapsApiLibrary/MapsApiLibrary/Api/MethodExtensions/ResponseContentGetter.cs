using System.Net.Http;
using System.Threading.Tasks;

namespace MapsApiLibrary.Api.MethodExtensions
{
    static class ResponseContentGetter
    {
        public static async Task<string> GetContentAsync(this Task<HttpResponseMessage> response)
        {
            var message = await response;
            if (!message.IsSuccessStatusCode)
            {
                throw new HttpRequestException();
            }

            return await message.Content.ReadAsStringAsync();
        }
    }
}
