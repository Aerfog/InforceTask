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

        [Test]
        public async Task UrlsTable_ReturnsCorrectView()
        {
            // Arrange
            var itemList = new List<UrlsItem> { new UrlsItem("http://example.com", "shorturl1", "user1", DateTime.Now, "desc1"), 
                new UrlsItem("http://example.org", "shorturl2", "user2", DateTime.Now, "desc2") };
            _repositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(itemList);
            var controller = new UrlsController(_loggerMock.Object, _repositoryMock.Object);

            // Act
            var result = await controller.UrlsTable();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            var model = viewResult.Model as List<UrlsItem>;
            Assert.AreSame(itemList, model);
        }

        [Test]
        public async Task Create_ValidUser_RedirectsToRoot()
        {
            // Arrange
            var userModel = new UrlModel { FullUrl = "http://example.com", Description = "Test" };
            var identity = new GenericIdentity("admin");
            identity.AddClaim(new Claim(ClaimTypes.Name, "admin"));
            var principal = new GenericPrincipal(identity, new string[] { "Administrators" });

            var controllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = principal
                }
            };

            _repositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<UrlsItem>())).ReturnsAsync(true);
            var controller = new UrlsController(_loggerMock.Object, _repositoryMock.Object)
            {
                ControllerContext = controllerContext
            };

            // Act
            var result = await controller.Create(userModel);

            // Assert
            Assert.IsInstanceOf<RedirectResult>(result);
            var redirectResult = result as RedirectResult;
            Assert.AreEqual("/", redirectResult.Url);
            _repositoryMock.Verify(repo => repo.CreateAsync(It.Is<UrlsItem>(item =>
                item.FullUrl == userModel.FullUrl &&
                item.CreatedBy == "admin" &&
                item.Descriptions == userModel.Description
            )), Times.Once);
        }

        [Test]
        public async Task Remove_ValidUser_Admin_RedirectsToRoot()
        {
            // Arrange
            var itemId = 1;
            var urlItem = new UrlsItem("http://example.com", "shorturl", "admin", DateTime.Now, "description");
            var identity = new GenericIdentity("admin");
            identity.AddClaim(new Claim(ClaimTypes.Name, "admin"));
            var principal = new GenericPrincipal(identity, new string[] { "Administrators" });

            var controllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = principal
                }
            };

            _repositoryMock.Setup(repo => repo.GetOneAsync(itemId)).ReturnsAsync(urlItem);
            _repositoryMock.Setup(repo => repo.RemoveAsync(itemId)).ReturnsAsync(true);
            var controller = new UrlsController(_loggerMock.Object, _repositoryMock.Object)
            {
                ControllerContext = controllerContext
            };

            // Act
            var result = await controller.Remove(itemId);

            // Assert
            Assert.IsInstanceOf<RedirectResult>(result);
            var redirectResult = result as RedirectResult;
            Assert.AreEqual("/", redirectResult.Url);
        }

        [Test]
        public async Task Remove_ValidUser_SameUser_RedirectsToRoot()
        {
            // Arrange
            var itemId = 1;
            var urlItem = new UrlsItem("http://example.com", "shorturl", "user1", DateTime.Now, "description");
            var identity = new GenericIdentity("user1");
            identity.AddClaim(new Claim(ClaimTypes.Name, "user1"));
            var principal = new GenericPrincipal(identity, null);

            var controllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = principal
                }
            };

            _repositoryMock.Setup(repo => repo.GetOneAsync(itemId)).ReturnsAsync(urlItem);
            _repositoryMock.Setup(repo => repo.RemoveAsync(itemId)).ReturnsAsync(true);
            var controller = new UrlsController(_loggerMock.Object, _repositoryMock.Object)
            {
                ControllerContext = controllerContext
            };

            // Act
            var result = await controller.Remove(itemId);

            // Assert
            Assert.IsInstanceOf<RedirectResult>(result);
            var redirectResult = result as RedirectResult;
            Assert.AreEqual("/", redirectResult.Url);
        }
    }
}
