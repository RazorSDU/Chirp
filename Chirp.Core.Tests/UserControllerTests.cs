using Chirp.API.Controllers;
using Chirp.Core.DTOs;
using Chirp.Tests.Backend.Mocks;
using Microsoft.AspNetCore.Mvc;

namespace Chirp.Core.Tests;

public class UserControllerTests
{
    [Fact]
    public async Task GetUsers_ReturnsOkAndUsers()
    {
        // Arrange
        var mockUserService = new MockUserService();
        mockUserService.CreateUserAsync(new CreateUserDto(){Username = "testuser1", Password = "<PASSWORD1>"});
        mockUserService.CreateUserAsync(new CreateUserDto(){Username = "testuser2", Password = "<PASSWORD2>"});
        mockUserService.CreateUserAsync(new CreateUserDto(){Username = "testuser3", Password = "<PASSWORD3>"});
        var controller = new UsersController(mockUserService);

        // Act
        var result = await controller.GetUsers();



        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var users = Assert.IsAssignableFrom<IEnumerable<UserDto>>(okResult.Value);

        var userList = users.ToList();

        Assert.Equal(3, userList.Count());
        Assert.Equal("testuser1", userList[0].Username);
        Assert.Equal("testuser2", userList[1].Username);
        Assert.Equal("testuser3", userList[2].Username);
    }

    [Fact]
    public async Task GetUserById_ShouldReturnOkAndUser()
    {
        // Arrange
        var mockUserService = new MockUserService();
        var user = await mockUserService.CreateUserAsync(new CreateUserDto(){Username = "testuser", Password = "<PASSWORD>"});
        var controller = new UsersController(mockUserService);

        // Act
        var result = await controller.GetUserById(user.Id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var userResult = Assert.IsAssignableFrom<UserDto>(okResult.Value);
        Assert.Equal("testuser", userResult.Username);
    }

}