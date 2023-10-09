using Microsoft.EntityFrameworkCore;
using SolarWatch.Models;

namespace SolarWatch;

public class SolarWatchContext : DbContext
{
    private readonly IConfiguration _configuration;

    public SolarWatchContext(DbContextOptions<SolarWatchContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }
    
    //The DbSet represents a table in the database
    public DbSet<City> Cities { get; set; }
    public DbSet<SunriseAndSunset> SunriseAndSunsetTimes { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connectionString = _configuration.GetConnectionString("DefaultConnection");
        optionsBuilder.UseSqlServer(connectionString);
    }
    
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        //Configure the City entity - making the 'Name' unique
        builder.Entity<City>()
            .HasIndex(u => u.Name)
            .IsUnique(); //We don't want to have entries with the same name in the database, so we apply the Unique constraint on the CityName property/column
    
        //we add the seed data to the database using the HasData method
        builder.Entity<City>()
            .HasData(
                new City { Id = 1, Name = "London", Lat = 51.509865, Lon = -0.118092 },
                new City { Id = 2, Name = "Budapest", Lat = 47.497913, Lon = 19.040236 },
                new City { Id = 3, Name = "Paris", Lat = 48.864716, Lon = 2.349014 }
            );
        
        //Configure the SunriseAndSunset entity
        builder.Entity<SunriseAndSunset>()
            .HasIndex(u => u.City);
        /*
        //we add the seed data to the database using the HasData method
        builder.Entity<SunriseAndSunset>()
            .HasData(
                new SunriseAndSunset { Id = 1, CityName = "London", Lat = 51.509865, Lon = -0.118092 },
                new SunriseAndSunset { Id = 2, CityName = "Budapest", Lat = 47.497913, Lon = 19.040236 },
                new SunriseAndSunset { Id = 3, CityName = "Paris", Lat = 48.864716, Lon = 2.349014 }
            );
            */
    }
    
    
}