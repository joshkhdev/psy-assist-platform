using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PsyAssistPlatform.Domain;

namespace PsyAssistPlatform.Persistence.EntityTypeConfigurations;

public class PsychologistConfiguration : IEntityTypeConfiguration<Psychologist>
{
    public void Configure(EntityTypeBuilder<Psychologist> builder)
    {
        // TODO: реализовать
    }
}