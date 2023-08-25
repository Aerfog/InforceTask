using InforceTask.Data.Entity;
using InforceTask.Models;
using Microsoft.EntityFrameworkCore;

namespace InforceTask.Data;

public class ShortenerDbContext : DbContext
{
    public DbSet<AboutTextAreaData>? AboutTextAreaData { get; set; }
    public DbSet<UrlsItem>? Urls { get; set; }
    public ShortenerDbContext(DbContextOptions<ShortenerDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(ShortenerDbContext).Assembly);
    }
}