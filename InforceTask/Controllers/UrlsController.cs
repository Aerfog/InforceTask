using InforceTask.Data;
using InforceTask.Data.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

    [Route("[controller]/")]
    [Route("/")]
    [HttpGet]
    public async Task<IActionResult> UrlsTable()
    {
        var urlList = new List<UrlsItem>()
        {
            new UrlsItem(
                "https://hyperhost.ua/tools/uk/surli?gclid=Cj0KCQjw_5unBhCMARIsACZyzS2M4n4vkG1FRJekAbOPOfGDSG-UpeFB_iDdeN8iOBFFoJtbU8d-p7caArZyEALw_wcB",
                "http://surl.li/kkqap", "test@example.com", DateTime.Now, "SomeText"),
            new UrlsItem(
                "https://hyperhost.ua/tools/uk/surli?gclid=Cj0KCQjw_5unBhCMARIsACZyzS2M4n4vkG1FRJekAbOPOfGDSG-UpeFB_iDdeN8iOBFFoJtbU8d-p7caArZyEALw_wcB",
                "http://surl.li/kkqap", "test@example.com", DateTime.Now, "SomeText"),
            new UrlsItem(
                "https://hyperhost.ua/tools/uk/surli?gclid=Cj0KCQjw_5unBhCMARIsACZyzS2M4n4vkG1FRJekAbOPOfGDSG-UpeFB_iDdeN8iOBFFoJtbU8d-p7caArZyEALw_wcB",
                "http://surl.li/kkqap", "daniil@example.com", DateTime.Now, "SomeText"),
            new UrlsItem(
                "https://hyperhost.ua/tools/uk/surli?gclid=Cj0KCQjw_5unBhCMARIsACZyzS2M4n4vkG1FRJekAbOPOfGDSG-UpeFB_iDdeN8iOBFFoJtbU8d-p7caArZyEALw_wcB",
                "http://surl.li/kkqap", "test@example.com", DateTime.Now, "SomeText"),
        };
        return await Task.Run(() => View(urlList));
    }
    
    [Authorize]
    [HttpGet("[controller]/{id}")]
    public async Task<IActionResult> UrlDetail(int id)
    {
        var url = new UrlsItem("https://hyperhost.ua/tools/uk/surli?gclid=Cj0KCQjw_5unBhCMARIsACZyzS2M4n4vkG1FRJekAbOPOfGDSG-UpeFB_iDdeN8iOBFFoJtbU8d-p7caArZyEALw_wcB",
            "http://surl.li/kkqap", "test@example.com", DateTime.Now, "SomeText");
        return await Task.Run(() => View(url));
    }

    [Authorize]
    [HttpDelete("[controller]/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        return await Task.Run(() =>View("UrlsTable"));
    }
    
    [Authorize]
    [HttpPost("[controller]/{id}")]
    public async Task<IActionResult> Create(int id)
    {
        return await Task.Run(() => View("UrlsTable"));
    }
}