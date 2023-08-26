using System.Diagnostics;
using InforceTask.Data;
using InforceTask.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using InforceTask.Models;
using Microsoft.AspNetCore.Authorization;

namespace InforceTask.Controllers;

[Route("[controller]/[action]")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRepository<AboutTextAreaData> _repository;

    public HomeController(ILogger<HomeController> logger, IRepository<AboutTextAreaData> repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> About()
    {
        _logger.LogInformation("Get:About");
        var aboutData = await _repository.GetOneAsync(1);
        return View("About", aboutData.TextContent);
    }

    [Authorize(Roles = "Administrators")]
    [HttpPost]
    public async Task<IActionResult> About(AboutTextAreaData model)
    {
        _logger.LogInformation("Post:About");
        if (User is null || User.Identity is null || User.Identity.Name is null || !User.IsInRole("Administrators"))
        {
            return Unauthorized();
        }
        await _repository.CreateAsync(model);
        return View("About", model.TextContent);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> Error()
    {
        var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        return await Task.Run(() => View(new ErrorViewModel { RequestId = requestId }));
    }
}