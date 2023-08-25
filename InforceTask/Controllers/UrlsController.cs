using InforceTask.Data;
using InforceTask.Data.Entity;
using InforceTask.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TinyURL;

namespace InforceTask.Controllers;

public class UrlsController : Controller
{
    private readonly ILogger<UrlsController> _logger;
    private readonly ShortenerDbContext _context;

    public UrlsController(ILogger<UrlsController> logger, ShortenerDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    [Authorize]
    [HttpGet("[controller]/{id}")]
    public async Task<IActionResult> UrlDetail(int id)
    {
        return await Task.Run(() =>
        {
            var item = _context.Urls?.Where(u => u.Id == id).FirstOrDefault();
            if (item is not null)
            {
                return View(item);
            }

            return (IActionResult)NotFound();
        });
    }
    
    [Route("UrlsTable")]
    [Route("/")]
    [HttpGet("/[controller]")]
    public async Task<IActionResult> UrlsTable()
    {
        return await Task.Run(() =>
        {
            var itemList = _context.Urls?.ToList();
            _logger.LogInformation("Get:Table");
            if (itemList is not null)
            {
                _logger.LogInformation("Success");
                return View(itemList);
            }
            _logger.LogError("Table not found");
            return (IActionResult)NotFound();
        });
    }
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(UrlModel model)
    {
        return await Task.Run(() =>
        {
            if (User.Identity?.Name is not null)
            {
                var shortUrl = model.FullUrl;
                if (!shortUrl.StartsWith("https://"))
                {
                    shortUrl = shortUrl.Insert(0, "https://").Shrink();
                }

                if (shortUrl.Equals("Error"))
                {
                    return Problem();
                }
                var urlItem = new UrlsItem(model.FullUrl, shortUrl,User.Identity.Name, DateTime.Now, model.Description);
                if (!(bool)_context.Urls?.Any(u => u.FullUrl.Equals(model.FullUrl)))
                {
                    _context.Urls?.Add(urlItem);
                    _context.SaveChanges();
                    return Redirect("/");
                }

                return Problem();
            }

            return (IActionResult)Unauthorized();
        });
    }
    
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> Remove(int id)
    {
        return await Task.Run(() =>
        {
            var item = _context.Urls?.Where(u =>
                u.Id == id && 
                (User.IsInRole("Administrators") || 
                 (User.Identity != null && 
                  User.Identity.Name != null && 
                  User.Identity.Name.Equals(u.CreatedBy)))).FirstOrDefault();
            if (item is not null)
            {
                _context.Remove(item);
                _context.SaveChanges();
                return Redirect("/");
            }

            return NotFound() as IActionResult;
        });
    }
}