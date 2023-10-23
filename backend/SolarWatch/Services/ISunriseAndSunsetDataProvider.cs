using SolarWatch.Models;

namespace SolarWatch.Services;

public interface ISunriseAndSunsetDataProvider
{
    Task<string> GetSunriseAndSunsetAsync(City data, string date);
}