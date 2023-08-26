using InforceTask.Models;

namespace InforceTask.Data.Repositories;

public class AboutRepository : IRepository<AboutTextAreaData>
{
    private readonly ShortenerDbContext _context;

    public AboutRepository(ShortenerDbContext context)
    {
        _context = context;
    }
    
    public async Task<AboutTextAreaData> GetOneAsync(int id)
    {
        return await Task.Run(() =>
        {
            var aboutData = _context.AboutTextAreaData?.FirstOrDefault();
            if (aboutData is null)
            {
                aboutData = new AboutTextAreaData() {Id = 1, TextContent = string.Empty};
            }
            return aboutData;
        });
    }

    public async Task<IEnumerable<AboutTextAreaData>> GetAllAsync()
    {
        return new List<AboutTextAreaData>()
        {
            await GetOneAsync(1)
        };
    }

    public async Task<bool> CreateAsync(AboutTextAreaData item)
    {
        return await Task.Run(() =>
        {
            var aboutData = _context.AboutTextAreaData?.FirstOrDefault();
            
            if (aboutData is not null)
            {
                aboutData.TextContent = item.TextContent;
            }
            else
            {
                _context.Add(new AboutTextAreaData() {Id = 1, TextContent = item.TextContent});
            }

            _context.SaveChanges();
            return true;
        });
    }

    public async Task<bool> RemoveAsync(int id)
    {
        return await Task.Run(() =>
        {
            var aboutData = _context.AboutTextAreaData?.FirstOrDefault();
            if(aboutData is not null)
            {
                _context.Remove(aboutData);
                _context.SaveChanges();
                return true;
            }
            return false;
        });
    }
}