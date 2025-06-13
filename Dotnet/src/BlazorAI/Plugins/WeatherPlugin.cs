using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace BlazorAI.Plugins;

public class WeatherPlugin
{
    private readonly IHttpClientFactory _httpClientFactory;


    public WeatherPlugin(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [KernelFunction("get_weather_forecast")]
    [Description("Retrieves current and hourly weather forecast data for a given location using latitude, longitude, and number of forecast days.")]
    [return: Description("Returns JSON containing weather data such as temperature, humidity, wind speed, precipitation, and more.")]
    public async Task<string> GetWeatherForecastAsync(
    [Description("Latitude of the location")] string latitude,
    [Description("Longitude of the location")] string longitude,
    [Description("Number of forecast days (1 to 16)")] int days = 1)
    {
        if (days < 1 || days > 16)
            throw new ArgumentOutOfRangeException(nameof(days), "Forecast days must be between 1 and 16.");

        var httpClient = _httpClientFactory.CreateClient();

        var url = $"https://api.open-meteo.com/v1/forecast" +
                  $"?latitude={latitude}&longitude={longitude}" +
                  $"&current=temperature_2m,relative_humidity_2m,apparent_temperature,precipitation,rain,showers,snowfall,weather_code,wind_speed_10m,wind_direction_10m,wind_gusts_10m" +
                  $"&hourly=temperature_2m,relative_humidity_2m,apparent_temperature,precipitation_probability,precipitation,rain,showers,snowfall,weather_code,cloud_cover,wind_speed_10m,uv_index" +
                  $"&forecast_days={days}";

        var response = await httpClient.GetStringAsync(url);
        return response;
    }

    [KernelFunction("get_past_weather")]
    [Description("Retrieves historical daily weather data for a given latitude and longitude over a specified number of past days.")]
    [return: Description("Returns JSON containing past weather data such as temperature, precipitation, wind speed, and more.")]
    public async Task<string> GetPastWeatherAsync(
    [Description("Latitude of the location")] string latitude,
    [Description("Longitude of the location")] string longitude,
    [Description("Number of past days to retrieve (1 to 90)")] int daysInPast = 1)
    {
        if (daysInPast < 1 || daysInPast > 90)
            throw new ArgumentOutOfRangeException(nameof(daysInPast), "Past days must be between 1 and 90.");

        var httpClient = _httpClientFactory.CreateClient();

        var url = $"https://api.open-meteo.com/v1/forecast" +
                  $"?latitude={latitude}&longitude={longitude}" +
                  $"&daily=weather_code,temperature_2m_max,temperature_2m_min,apparent_temperature_max,apparent_temperature_min,sunrise,sunset,daylight_duration,uv_index_max,precipitation_sum,rain_sum,showers_sum,snowfall_sum,precipitation_hours,wind_speed_10m_max,wind_gusts_10m_max" +
                  $"&past_days={daysInPast}";

        var response = await httpClient.GetStringAsync(url);
        return response;
    }


}

