using System.Text.Json;
using SolarWatch.Models;

namespace SolarWatch.Services;

public class SunriseAndSunsetJsonProcessor : ISunriseAndSunsetJsonProcessor
{
    public SunriseAndSunset Process(string data, string city, string date)
    {
        JsonDocument json = JsonDocument.Parse(data);
        JsonElement results = json.RootElement.GetProperty("results");
        //JsonElement sunrise = json.RootElement.GetProperty("sunrise");
        //JsonElement sunset = json.RootElement.GetProperty("sunset");

        SunriseAndSunset sunriseAndSunset = new SunriseAndSunset
        {
            City = city,
            Date = date,
            Sunrise = results.GetProperty("sunrise").GetString(),
            Sunset = results.GetProperty("sunset").GetString(),
        };
        
        return sunriseAndSunset;
    }
}