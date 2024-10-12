using System.Text.Json;

namespace Common.Extensions
{
    public static class DeserializeExtensions
    {
        private static JsonSerializerOptions defaultSerializerSettings =
            new JsonSerializerOptions();


        public static T Deserialize<T>(this string json)
        {
            return JsonSerializer.Deserialize<T>(json, defaultSerializerSettings);
        }

        public static T DeserializeCustom<T>(this string json)
        {
            var defaultSerializerSettings = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            return JsonSerializer.Deserialize<T>(json, defaultSerializerSettings);
        }
    }
}

