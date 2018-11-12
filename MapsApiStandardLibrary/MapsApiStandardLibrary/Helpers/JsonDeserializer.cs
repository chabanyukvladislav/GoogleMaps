using Newtonsoft.Json;

namespace MapsApiStandardLibrary.Helpers
{
    internal static class JsonDeserializer<T>
    {
        public static T Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
