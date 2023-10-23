using System.Net;
using SolarWatch.Models;

namespace SolarWatch.Services;

public class SunriseAndSunsetApi : ISunriseAndSunsetDataProvider
{
    
    private readonly ILogger<SunriseAndSunsetApi> _logger;
    
    public SunriseAndSunsetApi(ILogger<SunriseAndSunsetApi> logger)
    {
        _logger = logger;
    }
    
    //This method will make the API call and get the whole string:
    public async Task<string> GetSunriseAndSunsetAsync(City cityData, string date)
    {
        var url = $"https://api.sunrise-sunset.org/json?lat={cityData.Lat}&lng={cityData.Lon}&date={date}";
        var client = new HttpClient();
        
        _logger.LogInformation("Calling SunriseAndSunset API with url: {url}", url);

        return await client.GetStringAsync(url);
    }
}