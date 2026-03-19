using Microsoft.AspNetCore.Mvc;
using Moq;
using MySecureBackend.WebApi.Models;
using MySecureBackend.WebApi.Repositories;
using MySecureBackend.WebApi.Services;

namespace MySecureBackend.Tests
{
    [TestClass]
    public sealed class ExampleObjectsControllerTests
    {
        private Mock<IAuthenticationService> authenticationService;

        [TestInitialize]
        public void Setup()
        {
        }

        [TestMethod]
        public async Task Get_ExampleObjectThatDoesNotExist_Returns404NotFound()
        {
            // Arrange
            Guid id = Guid.NewGuid();

        }
    }
}