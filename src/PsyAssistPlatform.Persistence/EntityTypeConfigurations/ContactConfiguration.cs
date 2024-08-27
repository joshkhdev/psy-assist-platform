using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PsyAssistPlatform.Domain;

namespace PsyAssistPlatform.Persistence.EntityTypeConfigurations;

public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.HasKey(contact => contact.Id);
        builder.HasIndex(contact => contact.Id);
        builder.Property(contact => contact.Telegram).HasMaxLength(50);
        builder.Property(contact => contact.Email).HasMaxLength(250);
        builder.Property(contact => contact.Phone).HasMaxLength(12);
    }
}