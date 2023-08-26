using InforceTask.Data;
using InforceTask.Data.Entity;
using InforceTask.Data.Repositories;
using InforceTask.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TinyURL;

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

    [Route("UrlsTable")]
    [Route("/")]
    [HttpGet("/[controller]")]
    public async Task<IActionResult> UrlsTable()
    {
        var itemList = await _repository.GetAllAsync();
        _logger.LogInformation("Get:Table");
        return View(itemList);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(UrlModel model)
    {
        _logger.LogInformation("Post:Create");
        if (User.Identity?.Name is not null)
        {
            var shortUrl = model.FullUrl;
            if (!shortUrl.StartsWith("https://"))
            {
                if (shortUrl.StartsWith("http://"))
                {
                   shortUrl = shortUrl.Substring(7);
                }
                shortUrl = shortUrl.Insert(0, "https://");
            }

            shortUrl = shortUrl.Shrink();
            if (shortUrl.Equals("Error"))
            {
                _logger.LogError("Wrong full url!");
                return Problem();
            }

            var urlItem = new UrlsItem(model.FullUrl, shortUrl, User.Identity.Name, DateTime.Now,
                model.Description);
            await _repository.CreateAsync(urlItem);
            return Redirect("/");
        }

        return Unauthorized();
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> Remove(int id)
    {
        _logger.LogInformation("Get:Remove");
        var url = await _repository.GetOneAsync(id);
        if (User.IsInRole("Administrators") ||
            (User.Identity != null &&
             User.Identity.Name != null &&
             User.Identity.Name.Equals(url.CreatedBy)))
        {
            await _repository.RemoveAsync(id);
            return Redirect("/");
        }
        _logger.LogError("You don`t have permission");
        return Unauthorized();
    }
}