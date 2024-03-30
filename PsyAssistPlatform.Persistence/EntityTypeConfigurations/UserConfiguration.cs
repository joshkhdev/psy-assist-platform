using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PsyAssistPlatform.Domain;

namespace PsyAssistPlatform.Persistence.EntityTypeConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // TODO: реализовать
    }
}