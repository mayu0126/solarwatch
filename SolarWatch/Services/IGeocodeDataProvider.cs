namespace SolarWatch.Services;

public interface IGeocodeDataProvider
{
    Task<string> GetCityGeocodeAsync(string cityName);
}