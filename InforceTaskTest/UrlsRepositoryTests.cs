using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InforceTask.Controllers;
using InforceTask.Models;
using InforceTask.Data.Entity;
using InforceTask.Data.Repositories;
using NUnit.Framework;

namespace InforceTaskTest
{
    [TestFixture]
    public class UrlsControllerTests
    {
        private Mock<ILogger<UrlsController>> _loggerMock;
        private Mock<IRepository<UrlsItem>> _repositoryMock;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<UrlsController>>();
            _repositoryMock = new Mock<IRepository<UrlsItem>>();
        }

        [Test]
        public async Task UrlDetail_ReturnsCorrectView()
        {
            // Arrange
            var itemId = 1;
            var urlItem = new UrlsItem("http://example.com", "shorturl", "admin", DateTime.Now, "description");
            _repositoryMock.Setup(repo => repo.GetOneAsync(itemId)).ReturnsAsync(urlItem);
            var controller = new UrlsController(_loggerMock.Object, _repositoryMock.Object);

            // Act
            var result = await controller.UrlDetail(itemId);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreSame(urlItem, viewResult.Model);
        }
        
    }
}
