using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PsyAssistPlatform.Domain;

namespace PsyAssistPlatform.Persistence.EntityTypeConfigurations;

public class PsyRequestConfiguration : IEntityTypeConfiguration<PsyRequest>
{
    public void Configure(EntityTypeBuilder<PsyRequest> builder)
    {
        builder.HasKey(psyRequest => psyRequest.Id);
        builder.HasIndex(psyRequest => psyRequest.Id);

        builder.HasOne(psyRequest => psyRequest.Questionnaire)
            .WithOne()
            .HasForeignKey<PsyRequest>(psyRequest => psyRequest.QuestionnaireId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(psyRequest => psyRequest.PsychologistProfile)
            .WithMany()
            .HasForeignKey(user => user.PsychologistProfileId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(psyRequest => psyRequest.PsyRequestStatuses)
            .WithOne(psyRequestStatus => psyRequestStatus.PsyRequest)
            .HasForeignKey(psyRequestStatus => psyRequestStatus.PsyRequestId);
    }
}