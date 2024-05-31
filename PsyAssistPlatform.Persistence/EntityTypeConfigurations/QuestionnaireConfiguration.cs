using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PsyAssistPlatform.Domain;

namespace PsyAssistPlatform.Persistence.EntityTypeConfigurations;

public class QuestionnaireConfiguration : IEntityTypeConfiguration<Questionnaire>
{
    public void Configure(EntityTypeBuilder<Questionnaire> builder)
    {
        builder.HasKey(questionnaire => questionnaire.Id);
        builder.HasIndex(questionnaire => questionnaire.Id);
        builder.Property(questionnaire => questionnaire.Name).HasMaxLength(100).IsRequired();
        builder.Property(questionnaire => questionnaire.Pronouns).HasMaxLength(50).IsRequired();
        builder.Property(questionnaire => questionnaire.Age).IsRequired();
        builder.Property(questionnaire => questionnaire.TimeZone).HasMaxLength(50).IsRequired();
        builder.Property(questionnaire => questionnaire.ContactId).IsRequired();
        builder.Property(questionnaire => questionnaire.NeuroDifferences).HasMaxLength(500).IsRequired();
        builder.Property(questionnaire => questionnaire.MentalSpecifics).HasMaxLength(500).IsRequired();
        builder.Property(questionnaire => questionnaire.PsyWishes).HasMaxLength(500).IsRequired();
        builder.Property(questionnaire => questionnaire.PsyQuery).HasMaxLength(500).IsRequired();
        builder.Property(questionnaire => questionnaire.TherapyExperience).HasMaxLength(500).IsRequired();
        builder.Property(questionnaire => questionnaire.IsForPay).IsRequired();
        builder.Property(questionnaire => questionnaire.RegistrationDate).IsRequired();

        builder.HasOne(questionnaire => questionnaire.Contact)
            .WithOne(contact => contact.Questionnaire)
            .HasForeignKey<Questionnaire>(questionnaire => questionnaire.ContactId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}