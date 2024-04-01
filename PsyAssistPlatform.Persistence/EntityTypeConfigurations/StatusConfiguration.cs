using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PsyAssistPlatform.Domain;

namespace PsyAssistPlatform.Persistence.EntityTypeConfigurations;

public class StatusConfiguration : IEntityTypeConfiguration<Status>
{
    public void Configure(EntityTypeBuilder<Status> builder)
    {
        builder.HasKey(status => status.Id);
        builder.HasIndex(status => status.Id);
        builder.Property(status => status.Name).HasMaxLength(50).IsRequired();
    }
}