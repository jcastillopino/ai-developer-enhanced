using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace BlazorAI.Plugins
{
    public class GeocodingPlugin
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiKey;

        public GeocodingPlugin(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _apiKey = configuration["GEOCODING_API_KEY"] ?? throw new MissingFieldException("GEOCODING_API_KEY");
        }

        [KernelFunction("geocode_address")]
        [Description("Takes an address search query, and returns a collection of latitude and longitude coordinates that are most likely to match the query. The more specific the query, the better the results. IE: use 27301, USA to get the address of a postal code in the US. Or '5027 Bartley Way, McLeansville NC' will get better results - than just something like '27301' or 'Springfield'.")]
        [return: Description("A JSON object with only the latitude and longitude (lat, lon) of the first matching result. Example: {\"lat\":\"37.7792588\",\"lon\":\"-122.4193286\"}")]
        public async Task<string> GeocodeAddressAsync(string address)
        {
            using HttpClient httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetStringAsync(
                $"https://geocode.maps.co/search?q={address}&api_key={_apiKey}");

            // Parse the response and extract only lat/lon for the first result
            var results = System.Text.Json.JsonDocument.Parse(response).RootElement;
            if (results.ValueKind == System.Text.Json.JsonValueKind.Array && results.GetArrayLength() > 0)
            {
                var first = results[0];
                var lat = first.GetProperty("lat").GetString();
                var lon = first.GetProperty("lon").GetString();
                return System.Text.Json.JsonSerializer.Serialize(new { lat, lon });
            }
            // Return empty object if no results
            return System.Text.Json.JsonSerializer.Serialize(new { lat = "", lon = "" });
        }
    }
}
