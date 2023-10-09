using SolarWatch.Models;

namespace SolarWatch.Services;

public interface ICityJsonProcessor
{
    City Process(string cityData);
}