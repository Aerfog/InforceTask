using InforceTask.Data.Entity;
using InforceTask.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InforceTask.Controllers;

public class UrlsController : Controller
{
    private readonly ILogger<UrlsController> _logger;
    private readonly IRepository<UrlsItem> _repository;

    public UrlsController(ILogger<UrlsController> logger, IRepository<UrlsItem> repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [Authorize]
    [HttpGet("[controller]/{id}")]
    public async Task<IActionResult> UrlDetail(int id)
    {
        _logger.LogInformation("Get:Detail");
        var item = await _repository.GetOneAsync(id);
        return View(item);
    }
    
    [Route("/")]
    [HttpGet("/[controller]")]
    public IActionResult UrlsTable()
    {
        _logger.LogInformation("Get:Table");
        return View();
    }
    
}