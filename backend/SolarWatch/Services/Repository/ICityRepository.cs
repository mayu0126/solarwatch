using SolarWatch.Models;

namespace SolarWatch.Services;

public interface ICityRepository
{
    IEnumerable<City> GetAll();
    City? GetByName(string name);

    void Add(City city);
    void Delete(City city);
    void Update(City city);
}