using InforceTask.Models;
using Microsoft.EntityFrameworkCore;

namespace InforceTask.Data;

public class ShortenerDbContext : DbContext
{
    public DbSet<AboutTextAreaData>? AboutTextAreaData { get; set; }

    public ShortenerDbContext(DbContextOptions<ShortenerDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<AboutTextAreaData>()
            .ToTable("About")
            .Property(p => p.TextContent)
            .HasColumnName("Description");
        builder.Entity<AboutTextAreaData>()
            .ToTable("About").HasKey(p => p.Id);
    }
}