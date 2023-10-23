using System.Text.Json;
using SolarWatch.Models;

namespace SolarWatch.Services;

public class CityJsonProcessor : ICityJsonProcessor
{
    public City Process(string cityData)
    {

        JsonDocument json = JsonDocument.Parse(cityData);
        JsonElement name = json.RootElement[0].GetProperty("name");
        JsonElement lat = json.RootElement[0].GetProperty("lat");
        JsonElement lon = json.RootElement[0].GetProperty("lon");

        City city = new City
        {
            Name = name.GetString(),
            Lat = (float)lat.GetDouble(),
            Lon = (float)lon.GetDouble()
        };
        
        return city;
    }
}