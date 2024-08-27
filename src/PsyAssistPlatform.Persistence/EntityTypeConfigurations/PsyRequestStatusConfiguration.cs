using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PsyAssistPlatform.Domain;

namespace PsyAssistPlatform.Persistence.EntityTypeConfigurations;

public class PsyRequestStatusConfiguration : IEntityTypeConfiguration<PsyRequestStatus>
{
    public void Configure(EntityTypeBuilder<PsyRequestStatus> builder)
    {
        builder.HasKey(psyRequestStatus => psyRequestStatus.Id);
        builder.HasIndex(psyRequestStatus => psyRequestStatus.Id);
        builder.Property(psyRequestStatus => psyRequestStatus.StatusUpdateDate).IsRequired();
        builder.Property(psyRequestStatus => psyRequestStatus.Comment).HasMaxLength(500);
        
        builder.HasOne(psyRequestStatus => psyRequestStatus.Status)
            .WithMany()
            .HasForeignKey(psyRequestStatus => psyRequestStatus.StatusId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}