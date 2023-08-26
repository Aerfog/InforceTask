using InforceTask.Data.Entity;


namespace InforceTask.Data.Repositories;

public class UrlsRepository : IRepository<UrlsItem>
{
    private readonly ShortenerDbContext _context;

    public UrlsRepository(ShortenerDbContext context)
    {
        _context = context;
    }
    
    public async Task<UrlsItem> GetOneAsync(int id)
    {
        return await Task.Run(() =>
        {
            var item = _context.Urls?.Where(u => u.Id == id).FirstOrDefault();
            if (item is not null)
            {
                return item;
            }

            throw new Exception("This item is not exist!");
        });
    }

    public async Task<IEnumerable<UrlsItem>> GetAllAsync()
    {
        return await Task.Run(() =>
        {
            var itemList = _context.Urls?.ToList();
            
            if (itemList is not null)
            {
                return itemList;
            }
            
            throw new Exception("Table is empty!");
        });
    }

    public async Task<bool> CreateAsync(UrlsItem item)
    {
        return await Task.Run(() =>
        {
            if (!(bool)_context.Urls?.Any(u => u.FullUrl.Equals(item.FullUrl)))
            {
                _context.Urls?.Add(item);
                _context.SaveChanges();
                return true;
            }

            return false;
        });
    }

    public async Task<bool> RemoveAsync(int id)
    {
        return await Task.Run(() =>
        {
            var item = _context.Urls?.Where(u =>
                u.Id == id).FirstOrDefault();
            if (item is not null)
            {
                _context.Remove(item);
                _context.SaveChanges();
                return true;
            }
            return false;
        });
    }
}