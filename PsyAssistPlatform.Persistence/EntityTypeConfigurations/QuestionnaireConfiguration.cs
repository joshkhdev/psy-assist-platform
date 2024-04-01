using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PsyAssistPlatform.Domain;

namespace PsyAssistPlatform.Persistence.EntityTypeConfigurations;

public class QuestionnaireConfiguration : IEntityTypeConfiguration<Questionnaire>
{
    public void Configure(EntityTypeBuilder<Questionnaire> builder)
    {
        // TODO: реализовать
    }
}