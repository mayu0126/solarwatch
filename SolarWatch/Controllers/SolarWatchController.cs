using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Models;
using SolarWatch.Services;

namespace SolarWatch.Controllers;

[ApiController]
[Route("[controller]")]
public class SolarWatchController : ControllerBase
{
    
    //először be kell adni a Geocoding API-nak a városnevet string-ként
    //majd a visszaérkező lat és lon értékeket elmenteni, és ezeket átadni a Sunrise/Sunset API-nak
    
    private readonly ILogger<SolarWatchController> _logger;
    private readonly IGeocodeDataProvider _geocodeDataProvider;
    private readonly ICityJsonProcessor _cityJsonProcessor;
    private readonly ISunriseAndSunsetDataProvider _sunriseAndSunsetDataProvider;
    private readonly ISunriseAndSunsetJsonProcessor _sunriseAndSunsetJsonProcessor;
    private readonly ICityRepository _cityRepository;
    private readonly ISunriseAndSunsetRepository _sunriseAndSunsetRepository;
    
    public SolarWatchController(ILogger<SolarWatchController> logger,
        IGeocodeDataProvider geocodeDataProvider, ICityJsonProcessor cityJsonProcessor,
        ISunriseAndSunsetDataProvider sunriseAndSunsetDataProvider,
        ISunriseAndSunsetJsonProcessor sunriseAndSunsetJsonProcessor,
        ICityRepository cityRepository,
        ISunriseAndSunsetRepository sunriseAndSunsetRepository)
    {
        _logger = logger;
        _geocodeDataProvider = geocodeDataProvider;
        _cityJsonProcessor = cityJsonProcessor;
        _sunriseAndSunsetDataProvider = sunriseAndSunsetDataProvider;
        _sunriseAndSunsetJsonProcessor = sunriseAndSunsetJsonProcessor;
        _cityRepository = cityRepository;
        _sunriseAndSunsetRepository = sunriseAndSunsetRepository;
    }
    
    
    [HttpGet("GetSunriseAndSunset"), Authorize(Roles="User, Admin")]
    //public IActionResult Get(DateTime date)
    public async Task<ActionResult<SunriseAndSunset>> GetSunriseAndSunsetAsync(string cityName, string date)
    {
        try
        {
            string cityData = "";
            string newSunriseAndSunsetData = "";
            City city = new City();
            SunriseAndSunset sunriseAndSunset = new SunriseAndSunset();
            
            //if the cityName is in the database
            if (_cityRepository.GetByName(cityName) != null)
            {
                Console.WriteLine($"IF - we have the city data for {cityName}");
                city = _cityRepository.GetByName(cityName);
            }
            
            //if we DON'T HAVE the city data in the database, we ask it from the api:
            else
            {
                Console.WriteLine($"ELSE - we don't have the city data for {cityName} yet");
                cityData = await _geocodeDataProvider.GetCityGeocodeAsync(cityName);
                city = _cityJsonProcessor.Process(cityData);
                _cityRepository.Add(city);//adding the new city to the database
            }
            
            try
            {
                //if the data is in the database:
                if (_sunriseAndSunsetRepository.GetByCityAndDate(cityName, date) != null)
                {
                    Console.WriteLine($"IF - we have the sunrise and sunset data for {cityName}");
                    sunriseAndSunset = _sunriseAndSunsetRepository.GetByCityAndDate(cityName, date);
                }
                else
                {
                    Console.WriteLine($"ELSE - we don't have the sunrise and sunset data for {cityName} yet");
                    newSunriseAndSunsetData = await _sunriseAndSunsetDataProvider.GetSunriseAndSunsetAsync(city, date);
                    sunriseAndSunset = _sunriseAndSunsetJsonProcessor.Process(newSunriseAndSunsetData, cityName, date);
                    _sunriseAndSunsetRepository.Add(sunriseAndSunset);
                }
                
                return Ok(sunriseAndSunset);
                
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while getting sunrise and sunset data");
                return NotFound("Error while getting sunrise and sunset data");
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting geocode data");
            return NotFound("Error while getting geocode data");
        }
    }
    
    [HttpPost("AddCities"), Authorize(Roles="Admin")]
    public async Task<ActionResult<City>> AddCitiesAsync([Required]string cityName, [Required]double lat, [Required]double lon, string? state, string? country)
    {
        try
        {
            City city = new City
            {
                Name = cityName,
                Lat = lat,
                Lon = lon,
                State = state,
                Country = country
            };
            _cityRepository.Add(city);
            return Ok(city);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while adding new city");
            return NotFound("Error while adding new city");
        }
    }

    [HttpPut("EditCities"), Authorize(Roles="Admin")]
    public async Task<ActionResult<City>> EditCitiesAsync([Required]string cityName, double lat, double lon, string? state, string? country)
    {
        try
        {
            var city = _cityRepository.GetByName(cityName);
            City updatedCity = new City
            {
                Name = city.Name,
                Lat = lat == 0 ? city.Lat : lat,
                Lon = lon == 0 ? city.Lon : lon,
                State = state != null ? state : city.State,
                Country = country != null ? country : city.Country,
            };
            _cityRepository.Delete(city);
            _cityRepository.Update(updatedCity);
            return Ok(cityName);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while updating city");
            return NotFound("Error while updating city");
        }
    }
    
    [HttpDelete("DeleteCities"), Authorize(Roles="Admin")]
    public async Task<ActionResult<City>> DeleteCitiesAsync([Required]string cityName)
    {
        try
        {
            var city = _cityRepository.GetByName(cityName);
            _cityRepository.Delete(city);
            return Ok(cityName);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while deleting city");
            return NotFound("Error while deleting city");
        }
    }
    
    
    [HttpPost("AddSunriseAndSunset"), Authorize(Roles="Admin")]
    public async Task<ActionResult<SunriseAndSunset>> AddSunriseAndSunsetAsync([Required]string cityName, [Required]string date, [Required]string sunrise, [Required]string sunset)
    {
        try
        {
            SunriseAndSunset newSunriseAndSunset = new SunriseAndSunset
            {
                City = cityName,
                Date = date,
                Sunrise = sunrise,
                Sunset = sunset
            };
            _sunriseAndSunsetRepository.Add(newSunriseAndSunset);
            return Ok(newSunriseAndSunset);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while adding new sunrise and sunset data");
            return NotFound("Error while adding new sunrise and sunset data");
        }
    }

    [HttpPut("EditSunriseAndSunset"), Authorize(Roles="Admin")]
    public async Task<ActionResult<SunriseAndSunset>> EditSunriseAndSunsetAsync([Required]string cityName, [Required]string date, string sunrise, string sunset)
    {
        try
        {
            _sunriseAndSunsetRepository.Update(cityName, date, sunrise, sunset);
            return Ok(cityName);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while updating sunrise and sunset data");
            return NotFound("Error while updating sunrise and sunset data");
        }
    }
    
    [HttpDelete("DeleteSunriseAndSunset"), Authorize(Roles="Admin")]
    public async Task<ActionResult<SunriseAndSunset>> DeleteSunriseAndSunsetAsync([Required]string cityName, [Required]string date)
    {
        try
        {
            var sunriseAndSunsetData = _sunriseAndSunsetRepository.GetByCityAndDate(cityName, date);
            _sunriseAndSunsetRepository.Delete(sunriseAndSunsetData);
            return Ok(cityName);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while deleting sunrise and sunset data");
            return NotFound("Error while deleting sunrise and sunset data");
        }
    }
}