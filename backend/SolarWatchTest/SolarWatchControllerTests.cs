using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SolarWatch;
using SolarWatch.Controllers;
using SolarWatch.Models;
using SolarWatch.Services;

namespace SolarWatchTest;

[TestFixture]
public class SolarWatchControllerTests
{
    private Mock<ILogger<SolarWatchController>> _loggerMock;
    private Mock<IGeocodeDataProvider> _geocodeDataProviderMock;
    private Mock<ICityJsonProcessor> _cityJsonProcessorMock;
    private Mock<ISunriseAndSunsetDataProvider> _sunriseAndSunsetDataProviderMock;
    private Mock<ISunriseAndSunsetJsonProcessor> _sunriseAndSunsetJsonProcessorMock;
    private Mock<ICityRepository> _cityRepositoryMock;
    private Mock<ISunriseAndSunsetRepository> _sunriseAndSunsetRepositoryMock;

    private SolarWatchController _controller;

    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<SolarWatchController>>();
        _geocodeDataProviderMock = new Mock<IGeocodeDataProvider>();
        _cityJsonProcessorMock = new Mock<ICityJsonProcessor>();
        _sunriseAndSunsetDataProviderMock = new Mock<ISunriseAndSunsetDataProvider>();
        _sunriseAndSunsetJsonProcessorMock = new Mock<ISunriseAndSunsetJsonProcessor>();
        _cityRepositoryMock = new Mock<ICityRepository>();
        _sunriseAndSunsetRepositoryMock = new Mock<ISunriseAndSunsetRepository>();
        
        _controller = new SolarWatchController(_loggerMock.Object, _geocodeDataProviderMock.Object,
            _cityJsonProcessorMock.Object, _sunriseAndSunsetDataProviderMock.Object,
            _sunriseAndSunsetJsonProcessorMock.Object,
            _cityRepositoryMock.Object,
            _sunriseAndSunsetRepositoryMock.Object);
    }

    [Test]
    public void GetSunriseAndSunset_ReturnsNotFoundObjectResult_IfGeocodeDataProviderFails()
    {
        //arrange
        var geocodeData = "{}";
        string cityName = "Budapest";
        string date = "2023-07-29";
        _geocodeDataProviderMock.Setup(x => x.GetCityGeocodeAsync(It.IsAny<string>())).Throws(new Exception());
        
        //act
        var result = _controller.GetSunriseAndSunsetAsync(cityName, date);
        
        //assert
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result.Result);
    }
    
    [Test]
    public void GetSunriseAndSunset_ReturnsNotFoundObjectResult_IfGeocodeDataIsInvalid()
    {
        //arrange
        var geocodeData = "{}";
        string cityName = "Budapest";
        string date = "2023-07-29";
        _geocodeDataProviderMock.Setup(x => x.GetCityGeocodeAsync(It.IsAny<string>())).ReturnsAsync(geocodeData);
        _cityJsonProcessorMock.Setup(x => x.Process(geocodeData)).Throws<Exception>();
        
        //act
        var result = _controller.GetSunriseAndSunsetAsync(cityName, date);
        
        //assert
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result.Result);
    }
    
    [Test]
    public void GetSunriseAndSunset_ReturnsNotFoundObjectResult_IfSunriseAndSunsetDataProviderFails()
    {
        //arrange
        var sunriseAndSunsetData = "{}";
        string cityName = "Budapest";
        string date = "2023-07-29";
        _sunriseAndSunsetDataProviderMock.Setup(x => x.GetSunriseAndSunsetAsync(It.IsAny<City>(), It.IsAny<string>())).Throws(new Exception());
        
        //act
        var result = _controller.GetSunriseAndSunsetAsync(cityName, date);
        
        //assert
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result.Result);
    }
    
    [Test]
    public void GetSunriseAndSunset_ReturnsNotFoundObjectResult_IfSunriseAndSunsetDataIsInvalid()
    {
        //arrange
        var sunriseAndSunsetData = "{}";
        string cityName = "Budapest";
        string date = "2023-07-29";
        _sunriseAndSunsetDataProviderMock.Setup(x => x.GetSunriseAndSunsetAsync(It.IsAny<City>(), It.IsAny<string>())).ReturnsAsync(sunriseAndSunsetData);
        _sunriseAndSunsetJsonProcessorMock.Setup(x => x.Process(sunriseAndSunsetData, cityName, date)).Throws<Exception>();
        
        //act
        var result = _controller.GetSunriseAndSunsetAsync(cityName, date);
        
        //assert
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result.Result);
    }

    [Test]
    public void GetSunriseAndSunset_ReturnsOkResult_IfGeocodeDataIsValid()
    {
        //arrange
        var expectedCity = new City();
        var expectedSunriseAndSunset = new SunriseAndSunset();
        var geocodeData = "{}";
        var sunriseAndSunsetData = "{}";
        string cityName = "Budapest";
        string date = "2023-07-29";
        _geocodeDataProviderMock.Setup(x => x.GetCityGeocodeAsync(It.IsAny<string>())).ReturnsAsync(geocodeData);
        _cityJsonProcessorMock.Setup(x => x.Process(geocodeData)).Returns(expectedCity);
        _sunriseAndSunsetDataProviderMock.Setup(x => x.GetSunriseAndSunsetAsync(expectedCity, date)).ReturnsAsync(sunriseAndSunsetData);
        _sunriseAndSunsetJsonProcessorMock.Setup(x => x.Process(sunriseAndSunsetData, cityName, date)).Returns(expectedSunriseAndSunset);
        
        //act
        var result = _controller.GetSunriseAndSunsetAsync(cityName, date);

        //assert
        Assert.IsInstanceOf(typeof(OkObjectResult), result.Result.Result);
        Assert.That(((OkObjectResult)result.Result.Result).Value, Is.EqualTo(expectedSunriseAndSunset));
    }
    
}