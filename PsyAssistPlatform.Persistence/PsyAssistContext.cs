using Microsoft.EntityFrameworkCore;
using PsyAssistPlatform.Domain;
using PsyAssistPlatform.Persistence.EntityTypeConfigurations;
using Humanizer;

namespace PsyAssistPlatform.Persistence;

public class PsyAssistContext : DbContext
{
    public PsyAssistContext(DbContextOptions<PsyAssistContext> options) : base(options)
    {
    }

    public DbSet<Approach> Approaches { get; set; }
    
    public DbSet<Contact> Contacts { get; set; }
    
    public DbSet<PsychologistProfile> PsychologistProfiles { get; set; }
    
    public DbSet<Questionnaire> Questionnaires { get; set; }
    
    public DbSet<Role> Roles { get; set; }
    
    public DbSet<Status> Statuses { get; set; }
    
    public DbSet<User> Users { get; set; }
    
    public DbSet<PsyRequest> PsyRequests { get; set; }
    
    public DbSet<PsyRequestStatus> PsyRequestStatuses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder
            .ApplyConfiguration(new ApproachConfiguration())
            .ApplyConfiguration(new ContactConfiguration())
            .ApplyConfiguration(new PsychologistProfileConfiguration())
            .ApplyConfiguration(new QuestionnaireConfiguration())
            .ApplyConfiguration(new RoleConfiguration())
            .ApplyConfiguration(new StatusConfiguration())
            .ApplyConfiguration(new UserConfiguration())
            .ApplyConfiguration(new PsyRequestConfiguration())
            .ApplyConfiguration(new PsyRequestStatusConfiguration());
        
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var tableName = entityType.ClrType.Name.Pluralize().Underscore();
            entityType.SetTableName(tableName);

            foreach (var property in entityType.GetProperties())
            {
                var propertyName = property.Name.Underscore();
                property.SetColumnName(propertyName);
            }
        }
    }
}