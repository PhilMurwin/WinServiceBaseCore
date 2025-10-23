using System.Text.Json;

namespace WinServiceBaseCore.Infrastructure.Extensions
{
    public static class JsonExtensions
    {
        public static string ToJson<T>(this T config, bool indented = false)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = indented,
            };

            return JsonSerializer.Serialize(config, options);
        }
    }
}
