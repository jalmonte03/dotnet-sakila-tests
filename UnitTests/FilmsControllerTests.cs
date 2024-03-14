using Microsoft.AspNetCore.Mvc;
using Moq;
using Sakila.App.WebAPI.Controller;
using Sakila.App.WebAPI.DTOs;
using Sakila.App.WebAPI.Service;

namespace Sakila.App.Tests;

public class FilmsControllerTests
{


    [Fact]
    public async void GetFilm_Returns_Ok_When_Not_Null()
    {
        // Arrange
        int id = 1;

        var filmServiceMock = new Mock<IFilmService>();
        filmServiceMock.Setup(x => x.GetFilm(It.IsAny<int>())).ReturnsAsync(new FilmGetDTO());
        
        var controller = new FilmsController(filmServiceMock.Object);

        // Act
        var actual = await controller.GetFilm(id);

        // Assert
        Assert.IsType<OkObjectResult>(actual);
    }
    
    [Fact]
    public async void GetFilm_Returns_NotFound_When_Null()
    {
        // Arrange
        int id = 1;

        var filmServiceMock = new Mock<IFilmService>();
        filmServiceMock.Setup(x => x.GetFilm(It.IsAny<int>())).ReturnsAsync((FilmGetDTO)null!);
        
        var controller = new FilmsController(filmServiceMock.Object);

        // Act
        var actual = await controller.GetFilm(id);

        // Assert
        Assert.IsType<NotFoundResult>(actual);
    }

    [Fact]
    public async void GetAllFilms_Returns_Ok()
    {
        // Arrange
        int page = 1;
        int limit = 10;
        string name = "";

        var filmServiceMock = new Mock<IFilmService>();
        filmServiceMock.Setup(x => x.GetFilms(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(new FilmsGetDTO());
        
        var controller = new FilmsController(filmServiceMock.Object);
        
        // Act
        var actual = await controller.GetAllFilms(page, limit, name);

        // Assert
        Assert.IsType<OkObjectResult>(actual);
    }
    
    [Theory]
    [InlineData(0,10)]
    [InlineData(10,0)]
    [InlineData(0,0)]
    [InlineData(int.MinValue,10)]
    [InlineData(10,int.MinValue)]
    [InlineData(int.MinValue, int.MinValue)]
    public async void GetAllFilms_Returns_BadRequest_When_PAGE_and_LIMIT_Is_LE_Zero(int page, int limit)
    {
        // Arrange
        string name = "";

        var filmServiceMock = new Mock<IFilmService>();
        filmServiceMock.Setup(x => x.GetFilms(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(new FilmsGetDTO());
        
        var controller = new FilmsController(filmServiceMock.Object);
        
        // Act
        var actual = await controller.GetAllFilms(page, limit, name);

        // Assert
        Assert.IsType<BadRequestObjectResult>(actual);
    }

    [Fact]
    public async void GetMostRentedFilm_Returns_Ok()
    {
        // Arrange
        int limit = 10;
        string from = "2024-01-01";
        string to = "2024-01-01";

        var filmServiceMock = new Mock<IFilmService>();
        filmServiceMock.Setup(x => x.GetMostRentedFilms(It.IsAny<int>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>()));

        var controller = new FilmsController(filmServiceMock.Object);

        // Act
        var actual = await controller.GetMostRentedFilms(limit, from, to);

        // Assert
        Assert.IsType<OkObjectResult>(actual);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    [InlineData(101)]
    [InlineData(int.MinValue)]
    [InlineData(int.MaxValue)]
    public async void GetMostRentedFilm_Returns_BadRequest_When_Limit_Not_Between_1_And_100(int limit)
    {
        // Arrange
        string from = "2024-01-01";
        string to = "2024-01-01";

        var filmServiceMock = new Mock<IFilmService>();
        filmServiceMock.Setup(x => x.GetMostRentedFilms(It.IsAny<int>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>()));

        var controller = new FilmsController(filmServiceMock.Object);

        // Act
        var actual = await controller.GetMostRentedFilms(limit, from, to);

        // Assert
        Assert.IsType<BadRequestObjectResult>(actual);
    }
    
    [Theory]
    [InlineData("2024-01-01", "Date")]
    [InlineData("Date", "2024-01-01")]
    [InlineData("Date", "Date")]
    public async void GetMostRentedFilm_Returns_BadRequest_When_FROM_and_TO_Have_Wrong_Format(string from, string to)
    {
        // Arrange
        int limit = 10;

        var filmServiceMock = new Mock<IFilmService>();
        filmServiceMock.Setup(x => x.GetMostRentedFilms(It.IsAny<int>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>()));

        var controller = new FilmsController(filmServiceMock.Object);

        // Act
        var actual = await controller.GetMostRentedFilms(limit, from, to);

        // Assert
        Assert.IsType<BadRequestObjectResult>(actual);
    }
    
    [Fact]
    public async void GetMostWatchedCategories_Returns_Ok()
    {
        // Arrange
        int limit = 10;
        string from = "2024-01-01";
        string to = "2024-01-01";

        var filmServiceMock = new Mock<IFilmService>();
        filmServiceMock.Setup(x => x.GetMostWatchedCategories(It.IsAny<int>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>()));

        var controller = new FilmsController(filmServiceMock.Object);

        // Act
        var actual = await controller.GetMostWatchedCategories(limit, from, to);

        // Assert
        Assert.IsType<OkObjectResult>(actual);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    [InlineData(int.MinValue)]
    public async void GetMostWatchedCategories_Returns_BadRequest_When_Limit_Is_LE_Zero(int limit)
    {
        // Arrange
        string from = "2024-01-01";
        string to = "2024-01-01";

        var filmServiceMock = new Mock<IFilmService>();
        filmServiceMock.Setup(x => x.GetMostWatchedCategories(It.IsAny<int>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>()));

        var controller = new FilmsController(filmServiceMock.Object);

        // Act
        var actual = await controller.GetMostWatchedCategories(limit, from, to);

        // Assert
        Assert.IsType<BadRequestObjectResult>(actual);
    }
    
    [Theory]
    [InlineData("2024-01-01", "Date")]
    [InlineData("Date", "2024-01-01")]
    [InlineData("Date", "Date")]
    public async void GetMostWatchedCategories_Returns_BadRequest_When_FROM_and_TO_Have_Wrong_Format(string from, string to)
    {
        // Arrange
        int limit = 10;

        var filmServiceMock = new Mock<IFilmService>();
        filmServiceMock.Setup(x => x.GetMostWatchedCategories(It.IsAny<int>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>()));

        var controller = new FilmsController(filmServiceMock.Object);

        // Act
        var actual = await controller.GetMostWatchedCategories(limit, from, to);

        // Assert
        Assert.IsType<BadRequestObjectResult>(actual);
    }

    [Fact]
    public async void GetFilmSummary_Returns_Ok_When_Not_Null()
    {
        // Arrange
        int id = 1;

        var filmServiceMock = new Mock<IFilmService>();
        filmServiceMock.Setup(x => x.GetFilmSummary(It.IsAny<int>())).ReturnsAsync(new FilmSummaryDTO());

        var controller = new FilmsController(filmServiceMock.Object);

        // Act
        var actual = await controller.GetFilmSummary(id);

        // Assert
        Assert.IsType<OkObjectResult>(actual);
    }
    
    [Fact]
    public async void GetFilmSummary_Returns_NotFound_When_Null()
    {
        // Arrange
        int id = 1;

        var filmServiceMock = new Mock<IFilmService>();
        filmServiceMock.Setup(x => x.GetFilmSummary(It.IsAny<int>())).ReturnsAsync((FilmSummaryDTO)null!);

        var controller = new FilmsController(filmServiceMock.Object);

        // Act
        var actual = await controller.GetFilmSummary(id);

        // Assert
        Assert.IsType<NotFoundResult>(actual);
    }
}