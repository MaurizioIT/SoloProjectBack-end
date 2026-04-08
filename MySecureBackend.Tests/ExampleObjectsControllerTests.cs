using Microsoft.AspNetCore.Mvc;
using Moq;
using MySecureBackend.WebApi.Controllers;
using MySecureBackend.WebApi.Models;
using MySecureBackend.WebApi.Repositories;
using MySecureBackend.WebApi.Services;

namespace MySecureBackend.Tests
{
    [TestClass]
    public sealed class ExampleObjectsControllerTests
    {
        private UserController controller;
        private Mock<IUserRepository> userRepository;
        private Mock<IAuthenticationService> authenticationService;

        [TestInitialize]
        public void Setup()
        {
            userRepository = new Mock<IUserRepository>();
            authenticationService = new Mock<IAuthenticationService>();
            controller = new UserController(userRepository.Object, authenticationService.Object);
        }

        [TestMethod]
        public async Task GetByIdAsync_UserExists_Returns200Ok()
        {
            // Arrange
            int userId = 1;
            var expectedUser = new User { UserID = userId, Username = "John Doe", Password = "HashedPassword123" };
            userRepository.Setup(x => x.SelectAsync(userId)).ReturnsAsync(expectedUser);

            // Act
            var response = await controller.GetByIdAsync(userId);

            // Assert
            Assert.IsInstanceOfType<OkObjectResult>(response.Result);
            var okResult = (OkObjectResult)response.Result;
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(expectedUser, okResult.Value);
            userRepository.Verify(x => x.SelectAsync(userId), Times.Once);
        }

        [TestMethod]
        public async Task GetByIdAsync_UserDoesNotExist_Returns404NotFound()
        {
            // Arrange
            int userId = 999;
            userRepository.Setup(x => x.SelectAsync(userId)).ReturnsAsync((User)null);

            // Act
            var response = await controller.GetByIdAsync(userId);

            // Assert
            Assert.IsInstanceOfType<NotFoundObjectResult>(response.Result);
            var notFoundResult = (NotFoundObjectResult)response.Result;
            Assert.AreEqual(404, notFoundResult.StatusCode);
            userRepository.Verify(x => x.SelectAsync(userId), Times.Once);
        }

        [TestMethod]
        public async Task AddAsync_ValidUser_Returns201Created()
        {
            // Arrange
            var newUser = new User { Username = "Jane Doe", Password = "HashedPassword456" };
            var createdUser = new User { UserID = 2, Username = "Jane Doe", Password = "HashedPassword456" };
            userRepository.Setup(x => x.InsertAsync(newUser)).ReturnsAsync(createdUser);

            // Act
            var response = await controller.AddAsync(newUser);

            // Assert
            Assert.IsInstanceOfType<CreatedAtRouteResult>(response.Result);
            var createdResult = (CreatedAtRouteResult)response.Result;
            Assert.AreEqual(201, createdResult.StatusCode);
            Assert.AreEqual("GetUserById", createdResult.RouteName);
            Assert.AreEqual(createdUser, createdResult.Value);
            userRepository.Verify(x => x.InsertAsync(newUser), Times.Once);
        }
    }
}