using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PsyAssistPlatform.Domain;

namespace PsyAssistPlatform.Persistence.EntityTypeConfigurations;

public class ApproachConfiguration : IEntityTypeConfiguration<Approach>
{
    public void Configure(EntityTypeBuilder<Approach> builder)
    {
        builder.HasKey(approach => approach.Id);
        builder.HasIndex(approach => approach.Id);
        builder.HasIndex(approach => approach.Name);
        builder.Property(approach => approach.Name).HasMaxLength(50).IsRequired();
    }
}