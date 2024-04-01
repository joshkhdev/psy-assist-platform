using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PsyAssistPlatform.Domain;

namespace PsyAssistPlatform.Persistence.EntityTypeConfigurations;

public class PsychologistConfiguration : IEntityTypeConfiguration<Psychologist>
{
    public void Configure(EntityTypeBuilder<Psychologist> builder)
    {
        builder.HasKey(psychologist => psychologist.Id);
        builder.HasIndex(psychologist => psychologist.Id);
        builder.Property(psychologist => psychologist.Name).HasMaxLength(50).IsRequired();
        builder.Property(psychologist => psychologist.TimeZone).HasMaxLength(250).IsRequired();
        builder.Property(psychologist => psychologist.Description).HasMaxLength(500).IsRequired();
        builder.Property(psychologist => psychologist.RequestsInclude).HasMaxLength(500).IsRequired();
        builder.Property(psychologist => psychologist.RequestsExclude).HasMaxLength(500).IsRequired();
        builder.Property(psychologist => psychologist.IsActive).IsRequired();

        builder.HasOne(psychologist => psychologist.User)
            .WithMany()
            .HasForeignKey(psychologist => psychologist.UserId);
    }
}