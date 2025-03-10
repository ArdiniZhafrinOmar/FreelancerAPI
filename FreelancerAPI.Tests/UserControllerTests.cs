
using FreelancerAPI.Models;       // User Model
using Microsoft.AspNetCore.Mvc;   // API Responses
using Microsoft.EntityFrameworkCore; // EF Core for testing
using Microsoft.Extensions.Logging;
using Moq;

public class UserControllerTests
{

    private async Task<AppDbContext> GetDatabaseContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
        var databaseContext = new AppDbContext(options);
        databaseContext.Database.EnsureCreated();

        if (await databaseContext.Users.CountAsync() <= 0)
        {
            databaseContext.Users.Add(new User
            {
                Username = "john_doe",
                Email = "john@example.com",
                Phone = "60123456789", 
                Skillsets = "C#, .NET",
                Hobby = "Reading"
            });
            databaseContext.Users.Add(new User
            {
                Username = "jane_smith",
                Email = "jane@example.com",
                Phone = "60129876543",
                Skillsets = "React, Node.js",
                Hobby = "Traveling"
            });

            await databaseContext.SaveChangesAsync();
        }
        return databaseContext;
    }
    
    [Fact]
    public async Task GetUsers_ReturnsUsersList()
    {
        // Arrange
        var dbContext = await GetDatabaseContext();
        var mockLogger = new Mock<ILogger<UserController>>();
        var controller = new UserController(dbContext, mockLogger.Object);

        // Act
        var result = await controller.GetUsers();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = okResult.Value;

        // Use dynamic typing to access anonymous type properties
        var usersList = response.GetType().GetProperty("Data")?.GetValue(response, null);

        Assert.NotNull(usersList);
        Assert.IsType<List<User>>(usersList);
    }

    [Fact]
    public async Task CreateUser_ReturnsCreatedUser()
    {
        // Arrange
        var dbContext = await GetDatabaseContext();
        var mockLogger = new Mock<ILogger<UserController>>();
        var controller = new UserController(dbContext, mockLogger.Object);
        var newUser = new User
        {
            Username = "test_user",
            Email = "test@example.com",
            Phone = "60123456789",
            Skillsets = "C#, .NET",
            Hobby = "Gaming"
        };

        // Act
        var result = await controller.CreateUser(newUser);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result); 
        var createdUser = Assert.IsType<User>(createdResult.Value);
        Assert.Equal("test_user", createdUser.Username);
    }

    [Fact]
    public async Task UpdateUser_ReturnsNoContent()
    {
        // Arrange
        var dbContext = await GetDatabaseContext();
        var mockLogger = new Mock<ILogger<UserController>>();
        var controller = new UserController(dbContext, mockLogger.Object);
        var existingUser = await dbContext.Users.FirstAsync();
        existingUser.Username = "updated_user";

        // Act
        var result = await controller.UpdateUser(existingUser.Id, existingUser);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteUser_RemovesUser()
    {
        // Arrange
        var dbContext = await GetDatabaseContext();
        var mockLogger = new Mock<ILogger<UserController>>();
        var controller = new UserController(dbContext, mockLogger.Object);
        var existingUser = await dbContext.Users.FirstAsync();

        // Act
        var result = await controller.DeleteUser(existingUser.Id);
        var deletedUser = await dbContext.Users.FindAsync(existingUser.Id);

        // Assert
        Assert.IsType<NoContentResult>(result);
        Assert.Null(deletedUser); // Ensure user is deleted
    }

}
