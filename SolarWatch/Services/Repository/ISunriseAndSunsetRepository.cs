using SolarWatch.Models;

namespace SolarWatch.Services;

public interface ISunriseAndSunsetRepository
{
    IEnumerable<SunriseAndSunset> GetAll();
    SunriseAndSunset? GetByCityAndDate(string city, string date);

    void Add(SunriseAndSunset sunriseAndSunset);
    void Delete(SunriseAndSunset sunriseAndSunset);
    void Update(string city, string date, string sunrise, string sunset);
}