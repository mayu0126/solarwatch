using System.Net;

namespace SolarWatch.Services;

public class GeocodingApi : IGeocodeDataProvider
{
    private readonly ILogger<GeocodingApi> _logger;
    private readonly IConfiguration _configuration;
    
    public GeocodingApi(ILogger<GeocodingApi> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }
    
    //This method will make the API call and get the whole string:
    public async Task<string> GetCityGeocodeAsync(string cityName)
    {

        var apiKey = _configuration["GeocodingApi:ApiKey"];
        var url = $"http://api.openweathermap.org/geo/1.0/direct?q={cityName}&limit={1}&appid={apiKey}";

        var client = new HttpClient();
        
        _logger.LogInformation("Calling Geocoding API with url: {url}", url);

        return await client.GetStringAsync(url);
    }
}