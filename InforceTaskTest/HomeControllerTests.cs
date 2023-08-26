using InforceTask.Controllers;
using Microsoft.Extensions.Logging;
using InforceTask.Data.Repositories;
using InforceTask.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;

namespace InforceTaskTest;

[TestFixture]
public class HomeControllerTests
{
    private Mock<ILogger<HomeController>> _loggerMock;
    private Mock<IRepository<AboutTextAreaData>> _repositoryMock;

    [SetUp]
    public void Setup()
    {
        _loggerMock = new Mock<ILogger<HomeController>>();
        _repositoryMock = new Mock<IRepository<AboutTextAreaData>>();
    }

    [Test]
    public async Task About_Get_ReturnsViewWithAboutData()
    {
        // Arrange
        var aboutData = new AboutTextAreaData { TextContent = "Test Content" };
        _repositoryMock.Setup(repo => repo.GetOneAsync(1)).ReturnsAsync(aboutData);
        var controller = new HomeController(_loggerMock.Object, _repositoryMock.Object);

        // Act
        var result = await controller.About();

        // Assert
        Assert.IsInstanceOf<ViewResult>(result);
        var viewResult = result as ViewResult;
        Assert.AreEqual("About", viewResult.ViewName);
        Assert.AreEqual(aboutData.TextContent, viewResult.ViewData.Model);
    }
    
    [Test]
    public async Task About_Post_WithValidUser_ReturnsViewWithModel()
    {
        // Arrange
        var model = new AboutTextAreaData { TextContent = "Test Content" };
        var identity = new GenericIdentity("admin");
        identity.AddClaim(new Claim(ClaimTypes.Role, "Administrators"));
        var principal = new GenericPrincipal(identity, new string[] { "Administrators" });

        var controllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = principal
            }
        };

        _repositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<AboutTextAreaData>())).ReturnsAsync(true);

        var controller = new HomeController(_loggerMock.Object, _repositoryMock.Object)
        {
            ControllerContext = controllerContext
        };

        // Act
        var result = await controller.About(model);

        // Assert
        Assert.IsInstanceOf<ViewResult>(result);
        var viewResult = result as ViewResult;
        Assert.AreEqual("About", viewResult.ViewName);
        Assert.AreEqual(model.TextContent, viewResult.ViewData.Model);
    }

    [Test]
    public async Task About_Post_WithInvalidUser_ReturnsUnauthorized()
    {
        // Arrange
        var model = new AboutTextAreaData { TextContent = "Test Content" , Id = 1};
        var controller = new HomeController(_loggerMock.Object, _repositoryMock.Object);

        // Act
        var result = await controller.About(model);

        // Assert
        Assert.IsInstanceOf<UnauthorizedResult>(result);
    }
}