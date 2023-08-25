using InforceTask.Controllers;
using Microsoft.Extensions.Logging;
using InforceTask.Data;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace InforceTaskTest;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task AboutGetTest()
    {
        var mockDbContext = new Mock<ShortenerDbContext>();
        var mockLogger = new Mock<ILogger<HomeController>>();

        var controller = new HomeController(mockLogger.Object, mockDbContext.Object);

        var result = await controller.About();
        
        Assert.IsInstanceOf<IActionResult>(result);
        Mock.VerifyAll();
    }
    
    public void AboutPostTest()
    {
        var mockDbContext = new Mock<ShortenerDbContext>();
        var mockLogger = new Mock<ILogger>();
        mockDbContext.Setup(context => context.AboutTextAreaData!.FirstOrDefault());
        mockLogger.Setup(logger => logger.LogInformation("About:Get"));
        Assert.Pass();
    }
}