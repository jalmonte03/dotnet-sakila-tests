using Microsoft.AspNetCore.Mvc;
using Moq;
using Sakila.App.WebAPI.Controller;
using Sakila.App.WebAPI.DTOs;
using Sakila.App.WebAPI.Service;

namespace Sakila.App.Tests;

public class RentalsControllerTests
{
    [Fact]
    public async void GetRental_Returns_Ok_When_Not_Null()
    {
        // Arrange
        int id = 1;

        var rentalServiceMock = new Mock<IRentalService>();
        rentalServiceMock.Setup(x => x.GetRental(It.IsAny<int>())).ReturnsAsync(new RentalGetDTO());

        var controller = new RentalsController(rentalServiceMock.Object);

        // Act
        var actual = await controller.GetRental(id);

        // Assert
        Assert.IsType<OkObjectResult>(actual);
    }

    [Fact]
    public async void GetRental_Returns_NotFound_When_Null()
    {
        // Arrange
        int id = 1;

        var rentalServiceMock = new Mock<IRentalService>();
        rentalServiceMock.Setup(x => x.GetRental(It.IsAny<int>())).ReturnsAsync((RentalGetDTO)null!);

        var controller = new RentalsController(rentalServiceMock.Object);

        // Act
        var actual = await controller.GetRental(id);

        // Assert
        Assert.IsType<NotFoundResult>(actual);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    [InlineData(int.MinValue)]
    public async void GetRental_Returns_BadRequest_When_Page_LE_Zero(int page)
    {
        // Arrange
        int limit = 10;
        int? customerId = null;
        int? filmId = null;

        var rentalServiceMock = new Mock<IRentalService>();
        rentalServiceMock.Setup(x => x.GetRentals(page, limit, customerId, filmId));

        var controller = new RentalsController(rentalServiceMock.Object);

        // Act
        var actual = await controller.GetRentals(page, limit, customerId, filmId);

        // Assert
        Assert.IsType<BadRequestObjectResult>(actual);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    [InlineData(int.MinValue)]
    public async void GetRental_Returns_BadRequest_When_Limit_LE_Zero(int limit)
    {
        // Arrange
        int page = 1;
        int? customerId = null;
        int? filmId = null;

        var rentalServiceMock = new Mock<IRentalService>();
        rentalServiceMock.Setup(x => x.GetRentals(page, limit, customerId, filmId));

        var controller = new RentalsController(rentalServiceMock.Object);

        // Act
        var actual = await controller.GetRentals(page, limit, customerId, filmId);

        // Assert
        Assert.IsType<BadRequestObjectResult>(actual);
    }

    [Theory]
    [InlineData("2024-01-01", "Date")]
    [InlineData("Date", "2024-01-01")]
    [InlineData("Date", "Date")]
    public async void GetMonthlySummary_Returns_BadRequest_When_Date_Is_Invalid(string from, string to)
    {
        // Arrange
        var rentalServiceMock = new Mock<IRentalService>();
        rentalServiceMock.Setup(x => x.GetMonthlyRentalsSummary(It.IsAny<DateOnly>(), It.IsAny<DateOnly>()));

        var controller = new RentalsController(rentalServiceMock.Object);

        // Act
        var actual = await controller.GetMonthlySummary(from, to);

        // Assert
        Assert.IsType<BadRequestObjectResult>(actual);
    }
    
    [Theory]
    [InlineData("2024-01-01", "Date")]
    [InlineData("Date", "2024-01-01")]
    [InlineData("Date", "Date")]
    public async void GetMonthlyRentalRevenue_Returns_BadRequest_When_Date_Is_Invalid(string from, string to)
    {
        // Arrange
        var rentalServiceMock = new Mock<IRentalService>();
        rentalServiceMock.Setup(x => x.GetMonthlyRentalRevenue(It.IsAny<DateOnly>(), It.IsAny<DateOnly>()));

        var controller = new RentalsController(rentalServiceMock.Object);

        // Act
        var actual = await controller.GetMonthlyRentalRevenue(from, to);

        // Assert
        Assert.IsType<BadRequestObjectResult>(actual);
    }
}