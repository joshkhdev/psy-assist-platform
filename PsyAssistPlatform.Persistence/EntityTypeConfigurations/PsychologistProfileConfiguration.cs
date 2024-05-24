using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PsyAssistPlatform.Domain;

namespace PsyAssistPlatform.Persistence.EntityTypeConfigurations;

public class PsychologistProfileConfiguration : IEntityTypeConfiguration<PsychologistProfile>
{
    public void Configure(EntityTypeBuilder<PsychologistProfile> builder)
    {
        builder.HasKey(psychologistProfile => psychologistProfile.Id);
        builder.HasIndex(psychologistProfile => psychologistProfile.Id);
        builder.Property(psychologistProfile => psychologistProfile.Name).HasMaxLength(50).IsRequired();
        builder.Property(psychologistProfile => psychologistProfile.TimeZone).HasMaxLength(50).IsRequired();
        builder.Property(psychologistProfile => psychologistProfile.Description).HasMaxLength(500).IsRequired();
        builder.Property(psychologistProfile => psychologistProfile.RequestsInclude).HasMaxLength(500).IsRequired();
        builder.Property(psychologistProfile => psychologistProfile.RequestsExclude).HasMaxLength(500).IsRequired();
        builder.Property(psychologistProfile => psychologistProfile.IsActive).IsRequired();

        builder.HasOne(psychologistProfile => psychologistProfile.User)
            .WithOne(user => user.PsychologistProfile)
            .HasForeignKey<PsychologistProfile>(psychologistProfile => psychologistProfile.UserId)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}