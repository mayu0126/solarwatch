using Microsoft.EntityFrameworkCore;
using SolarWatch.Models;

namespace SolarWatch.Services;

public class SunriseAndSunsetRepository : ISunriseAndSunsetRepository
{
    private readonly IConfiguration _configuration;
    private readonly string _databaseConnectionString;

    public SunriseAndSunsetRepository(IConfiguration configuration, string connectionString)
    {
        _configuration = configuration;
        _databaseConnectionString = connectionString;
    }
    
    public IEnumerable<SunriseAndSunset> GetAll()
    {
        throw new NotImplementedException();
    }

    public SunriseAndSunset? GetByCityAndDate(string city, string date)
    {
        using var dbContext =
            new SolarWatchContext(
                new DbContextOptionsBuilder<SolarWatchContext>().UseNpgsql(_databaseConnectionString).Options,
                _configuration);
        return dbContext.SunriseAndSunsetTimes.FirstOrDefault(c => c.City == city && c.Date == date);
    }

    public void Add(SunriseAndSunset sunriseAndSunset)
    {
        using var dbContext =
            new SolarWatchContext(
                new DbContextOptionsBuilder<SolarWatchContext>().UseNpgsql(_databaseConnectionString).Options,
                _configuration);
        dbContext.Add(sunriseAndSunset);
        dbContext.SaveChanges();
    }

    public void Delete(SunriseAndSunset sunriseAndSunset)
    {
        using var dbContext =
            new SolarWatchContext(
                new DbContextOptionsBuilder<SolarWatchContext>().UseNpgsql(_databaseConnectionString).Options,
                _configuration);
        dbContext.Remove(sunriseAndSunset);
        dbContext.SaveChanges();
    }

    public void Update(string city, string date, string sunrise, string sunset)
    {
        using var dbContext =
            new SolarWatchContext(
                new DbContextOptionsBuilder<SolarWatchContext>().UseNpgsql(_databaseConnectionString).Options,
                _configuration);
        var updated = dbContext.SunriseAndSunsetTimes.FirstOrDefault(s => s.City == city && s.Date == date);
        updated.Sunrise = sunrise;
        updated.Sunset = sunset;
        dbContext.SaveChanges();

        /*
        using var dbContext = new SolarWatchContext();
        dbContext.Update(sunriseAndSunset);
        dbContext.SaveChanges();
        */
    }
}