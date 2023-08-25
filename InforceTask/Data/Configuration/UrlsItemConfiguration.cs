using InforceTask.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InforceTask.Data.Configuration;

public class UrlsItemConfiguration : IEntityTypeConfiguration<UrlsItem>
{
    public void Configure(EntityTypeBuilder<UrlsItem> builder)
    {
        builder.ToTable("Urls");
        builder.Property(p => p.Id).HasColumnName("Id");
        builder.Property(p => p.FullUrl).HasColumnName("FullUrl");
        builder.Property(p => p.ShortUrl).HasColumnName("ShortUrl");
        builder.Property(p => p.CreatedBy).HasColumnName("CreatedBy");
        builder.Property(p => p.CreatedDate).HasColumnName("CreatedDate");
        builder.Property(p => p.Descriptions).HasColumnName("Descriptions");
        builder.HasIndex(p => p.Id);
        builder.HasKey(p => p.Id);
    }
}