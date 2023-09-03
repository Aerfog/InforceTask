using InforceTask.Data.Entity;
using InforceTask.Data.Repositories;
using InforceTask.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TinyURL;

namespace InforceTask.Controllers;

[ApiController]
[Route("/Urls/Api")]
public class UrlsApiController : Controller
{
    private readonly ILogger<UrlsApiController> _logger;
    private readonly IRepository<UrlsItem> _repository;

    public UrlsApiController(ILogger<UrlsApiController> logger, IRepository<UrlsItem> repository)
    {
        _logger = logger;
        _repository = repository;
    }
    
    [HttpGet]
    public async Task<IActionResult> UrlsTable()
    {
        var itemList = await _repository.GetAllAsync();
        _logger.LogInformation("Get:Table");
        return Ok(itemList);
    }

    [Authorize]
    [HttpPost()]
    public async Task<IActionResult> Create(UrlModel model)
    {
        _logger.LogInformation("Post:Create");
        if (User.Identity?.Name is null) return Unauthorized();
        var shortUrl = model.FullUrl;
        if (!shortUrl.StartsWith("https://"))
        {
            if (shortUrl.StartsWith("ht" + "tp://"))
            {
                shortUrl = shortUrl.Substring(7);
            }
            shortUrl = shortUrl.Insert(0, "https://");
        }

        shortUrl = await shortUrl.ShrinkAsync();
        if (shortUrl.Equals("Error"))
        {
            _logger.LogError("Wrong full url!");
            return Problem();
        }

        var urlItem = new UrlsItem(model.FullUrl, shortUrl, User.Identity.Name, DateTime.Now,
            model.Description);
        await _repository.CreateAsync(urlItem);
        return Ok();

    }

    [Authorize]
    [HttpPost("{id}")]
    public async Task<IActionResult> Remove(int id)
    {
        _logger.LogInformation("Get:Remove");
        var url = await _repository.GetOneAsync(id);
        if (User.IsInRole("Administrators") ||
            (User.Identity is { Name: not null } &&
             User.Identity.Name.Equals(url.CreatedBy)))
        {
            await _repository.RemoveAsync(id);
            return Ok();
        }
        _logger.LogError("You don`t have permission");
        return Unauthorized();
    }
}