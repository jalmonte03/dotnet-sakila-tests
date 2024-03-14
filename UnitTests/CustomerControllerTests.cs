using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Sakila.App.WebAPI.Controller;
using Sakila.App.WebAPI.DTOs;
using Sakila.App.WebAPI.Model;
using Sakila.App.WebAPI.Service;

namespace Sakila.App.Tests;

public class CustomerControllerTests
{
    IEnumerable<CustomerDTO> customers;

    public CustomerControllerTests()
    {
        customers = new List<CustomerDTO>()
        {
            new() {
                Id = 1,
                First_Name = "John",
                Last_Name = "Doe",
                Email = "john.doe@email.com",
                StreetAddress = "123 Main Street",
                StreetAddress2 = "Apt #1",
                City = "Orlando",
                Country = "USA",
                Created = DateTime.Now,
                Active = '1'    
            },
            new() {
                Id = 2,
                First_Name = "Carl",
                Last_Name = "Mitch",
                Email = "carl.mitch@email.com",
                StreetAddress = "123 Main Street",
                StreetAddress2 = "Apt #2",
                City = "Orlando",
                Country = "USA",
                Created = DateTime.Now,
                Active = '1'    
            },
            new() {
                Id = 3,
                First_Name = "Peter",
                Last_Name = "Blake",
                Email = "peter.Blake@email.com",
                StreetAddress = "123 Main Street",
                StreetAddress2 = "Apt #3",
                City = "Orlando",
                Country = "USA",
                Created = DateTime.Now,
                Active = '1'    
            },

        };
    }

    [Fact]
    public async void GetCustomer_Returns_Ok_When_Find_A_Customer()
    {
        // Arrange
        int customerId = 1;

        var mock = new Mock<ICustomerService>();
        mock.Setup(x => x.GetCustomer(It.IsAny<int>())).ReturnsAsync(new CustomerDTO { Id = customerId });
        var controller = new CustomersController(mock.Object);

        // Act
        var actual = await controller.GetCustomer(customerId) as OkObjectResult;

        // Assert
        Assert.IsType<OkObjectResult>(actual);
        Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
    }

    [Fact]
    public async void GetCustomer_Returns_NotFound_When_Customer_Is_Null()
    {
        // Arrange
        int customerId = 1;

        var mock = new Mock<ICustomerService>();
        mock.Setup(x => x.GetCustomer(It.IsAny<int>())).ReturnsAsync(value: null);
        var controller = new CustomersController(mock.Object);

        // Act
        var actual = await controller.GetCustomer(customerId) as NotFoundResult;

        // Assert
        Assert.IsType<NotFoundResult>(actual);
        Assert.Equal(StatusCodes.Status404NotFound, actual.StatusCode);
    }

    [Fact]
    public async void GetCustomers_Returns_Ok ()
    {
        // Arrange
        int page = 1;
        int limit = 10;
        string name = "";

        var customerServiceMock = new Mock<ICustomerService>();
        customerServiceMock.Setup(x => x.GetCustomers(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(new CustomersResponseDTO{
            CurrentPage = 1,
            Total = customers.Count(),
            Customers = customers
        });
        
        var controller = new CustomersController(customerServiceMock.Object);

        // Act
        var actual = await controller.GetCustomers(page, limit, name);
        var getCustomerResponse = (actual as OkObjectResult)!.Value as CustomersResponseDTO;
        
        // Assert
        Assert.IsType<OkObjectResult>(actual);
        Assert.Equal(StatusCodes.Status200OK, (actual as OkObjectResult)!.StatusCode);
        Assert.Same(customers, getCustomerResponse!.Customers);
    }

    [Theory]
    [InlineData(0,0,"")]
    [InlineData(0,10,"")]
    [InlineData(10,0,"")]
    [InlineData(-100,20,"")]
    [InlineData(20,-100,"")]
    public async void GetCustomers_Returns_BadRequest_When_PAGE_And_LIMIT_LE_Zero(int page, int limit, string name)
    {
        // Arrange
        var customerServiceMock = new Mock<ICustomerService>();
    
        var controller = new CustomersController(customerServiceMock.Object);

        // Act
        var actual = await controller.GetCustomers(page, limit, name);

        // Assert
        Assert.IsType<BadRequestObjectResult>(actual);
        Assert.Equal(StatusCodes.Status400BadRequest, (actual as BadRequestObjectResult)!.StatusCode);
    }

    [Fact]
    public async void GetCustomersRentals_Returns_Ok()
    {
        // Arrange
        int id = 1;
        int page = 1;
        int limit = 10;

        var customerServiceMock = new Mock<ICustomerService>();

        customerServiceMock.Setup(x => x.GetCustomerRentals(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new CustomerRentalsDTO
        {
            Page = 1,
            Total = 0,
            Rentals = new List<CustomerRentalGetDTO>()
        });

        var controller = new CustomersController(customerServiceMock.Object);

        // Act
        var actual = await controller.GetCustomersRentals(id, page, limit);

        // Assert
        Assert.IsType<OkObjectResult>(actual);
    }

    [Theory]
    [InlineData(0,10)]
    [InlineData(10,0)]
    [InlineData(0,0)]
    [InlineData(int.MinValue,10)]
    [InlineData(10,int.MinValue)]
    [InlineData(int.MinValue,int.MinValue)]
    public async void GetCustomerRentals_Returns_BadRequest_When_PAGE_and_LIMIT_LE_Zero(int page, int limit)
    {
        // Arrange
        var customerServiceMock = new Mock<ICustomerService>();

        var controller = new CustomersController(customerServiceMock.Object);

        // Act
        var actual = await controller.GetCustomersRentals(1, page, limit);

        // Assert
        Assert.IsType<BadRequestObjectResult>(actual);
    }

    [Fact]
    public async void GetCustomerWatchedCategories_Returns_Ok()
    {
        // Arrange
        var customerServiceMock = new Mock<ICustomerService>();
        customerServiceMock.Setup(x => x.GetCustomerWatchedCategories(It.IsAny<int>())).ReturnsAsync(new List<CustomerWatchedCategoryDTO>());
        
        var controller = new CustomersController(customerServiceMock.Object);

        // Act
        var actual = await controller.GetCustomerWatchedCategories(1);
        
        // Assert
        Assert.IsType<OkObjectResult>(actual);
    }

    [Fact]
    public async void GetCustomerSummary_Returns_Ok_When_Not_Null()
    {
        // Arrange
        int id = 1;

        var customerServiceMock = new Mock<ICustomerService>();
        customerServiceMock.Setup(x => x.GetCustomerSummary(It.IsAny<int>())).ReturnsAsync(new CustomerSummaryDTO());

        var controller = new CustomersController(customerServiceMock.Object);
    
        // Act
        var actual = await controller.GetCustomerSummary(id);

        // Assert
        Assert.IsType<OkObjectResult>(actual);
    }
    
    [Fact]
    public async void GetCustomerSummary_Returns_NotFound_When_Null()
    {
        // Arrange
        int id = 1;

        var customerServiceMock = new Mock<ICustomerService>();
        customerServiceMock.Setup(x => x.GetCustomerSummary(It.IsAny<int>())).ReturnsAsync((CustomerSummaryDTO)null!);

        var controller = new CustomersController(customerServiceMock.Object);
    
        // Act
        var actual = await controller.GetCustomerSummary(id);

        // Assert
        Assert.IsType<NotFoundResult>(actual);
    }
}