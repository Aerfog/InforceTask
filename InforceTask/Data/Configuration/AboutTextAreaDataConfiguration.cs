using InforceTask.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InforceTask.Data.Configuration;

public class AboutTextAreaDataConfiguration : IEntityTypeConfiguration<AboutTextAreaData>
{
    public void Configure(EntityTypeBuilder<AboutTextAreaData> builder)
    {
        builder.ToTable("About");
        builder.Property(p => p.TextContent)
            .HasColumnName("Description");
        builder.HasKey(p => p.Id);
    }
}