using System.Diagnostics;
using InforceTask.Data;
using Microsoft.AspNetCore.Mvc;
using InforceTask.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace InforceTask.Controllers;

[Route("[controller]/[action]")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ShortenerDbContext _dbContext;
    
    public HomeController(ILogger<HomeController> logger, ShortenerDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    [Route("/")]
    public async Task<IActionResult> Index()
    {
        return await Task.Run(View);
    }
    
    [HttpGet]
    public async Task<IActionResult> About()
    {
        return await Task.Run(() =>
        {
            var aboutData = _dbContext.AboutTextAreaData?.FirstOrDefault();
            var description = string.Empty;
            if (aboutData is not null)
            {
                description = aboutData.TextContent;
            }
            
            return View("About", description);
        });
    }

    [Authorize(Roles = "Administrators")]
    [HttpPost]
    public async Task<IActionResult> About(AboutTextAreaData model)
    {
        return await Task.Run(() =>
        {
            var aboutData = _dbContext.AboutTextAreaData?.FirstOrDefault();
        
            if (aboutData is not null)
            {
                aboutData.TextContent = model.TextContent;
            }
            else
            {
                _dbContext.Add(model);
            }

            _dbContext.SaveChanges();
            return View("About", model.TextContent);
        });
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> Error()
    {
        return await Task.Run(() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }));
    }
}