using Microsoft.AspNetCore.Mvc;

namespace InforceTask.Controllers;

public class UrlsController : Controller
{
    private readonly ILogger<UrlsController> _logger;

    public UrlsController(ILogger<UrlsController> logger)
    {
        _logger = logger;
    }

    [Route("[controller]/")]
    [HttpGet]
    public async Task<IActionResult> GetUrls()
    {
        return await Task.Run(() => View("UrlsTable"));
    }
    
    [HttpGet("[controller]/{url}")]
    public async Task<IActionResult> GetUrl(string url)
    {
        return await Task.Run(() => View("UrlDetail"));
    }
}