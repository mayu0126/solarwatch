using SolarWatch.Models;

namespace SolarWatch.Services;

public interface ISunriseAndSunsetJsonProcessor
{
    SunriseAndSunset Process(string data, string city, string date);
}