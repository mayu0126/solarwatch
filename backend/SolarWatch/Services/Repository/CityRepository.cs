using Microsoft.EntityFrameworkCore;
using SolarWatch.Models;

namespace SolarWatch.Services;

public class CityRepository : ICityRepository
{
    private readonly IConfiguration _configuration;

    public CityRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public IEnumerable<City> GetAll()
    {
        //We create a new SolarWatchContext instance every time the database is accessed,
        //and then dispose of it when the work is done by adding the using statement
        //This ensures that database connections are properly closed and resources are released
        //The Dispose() method will be called automatically when the methods are finished executing
        using var dbContext = new SolarWatchContext(new DbContextOptionsBuilder<SolarWatchContext>().UseSqlServer(_configuration.GetConnectionString("DefaultConnection")).Options, _configuration);
        return dbContext.Cities.ToList();
    }

    public City? GetByName(string name)
    {
        using var dbContext = new SolarWatchContext(new DbContextOptionsBuilder<SolarWatchContext>().UseSqlServer(_configuration.GetConnectionString("DefaultConnection")).Options, _configuration);
        return dbContext.Cities.FirstOrDefault(c => c.Name == name);
    }

    public void Add(City city)
    {
        using var dbContext = new SolarWatchContext(new DbContextOptionsBuilder<SolarWatchContext>().UseSqlServer(_configuration.GetConnectionString("DefaultConnection")).Options, _configuration);
        dbContext.Add(city);
        dbContext.SaveChanges();
    }

    public void Delete(City city)
    {
        using var dbContext = new SolarWatchContext(new DbContextOptionsBuilder<SolarWatchContext>().UseSqlServer(_configuration.GetConnectionString("DefaultConnection")).Options, _configuration);
        dbContext.Remove(city);
        dbContext.SaveChanges();
    }

    public void Update(City city)
    {  
        using var dbContext = new SolarWatchContext(new DbContextOptionsBuilder<SolarWatchContext>().UseSqlServer(_configuration.GetConnectionString("DefaultConnection")).Options, _configuration);
        dbContext.Update(city);
        dbContext.SaveChanges();
    }
}