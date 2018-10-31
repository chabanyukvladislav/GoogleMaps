using Newtonsoft.Json;

namespace MapsApiLibrary.Helpers
{
    internal static class JsonDeserializer<T>
    {
        public static T Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
