using Microsoft.EntityFrameworkCore;
using PsyAssistPlatform.Domain;
using PsyAssistPlatform.Persistence.EntityTypeConfigurations;

namespace PsyAssistPlatform.Persistence;

public class PsyAssistContext : DbContext
{
    public PsyAssistContext(DbContextOptions<PsyAssistContext> options) : base(options)
    {
    }

    public DbSet<Approach> Approaches { get; set; }
    
    public DbSet<Contact> Contacts { get; set; }
    
    public DbSet<Psychologist> Psychologists { get; set; }
    
    public DbSet<Questionnaire> Questionnaires { get; set; }
    
    public DbSet<Role> Roles { get; set; }
    
    public DbSet<Status> Statuses { get; set; }
    
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder
            .ApplyConfiguration(new ApproachConfiguration())
            .ApplyConfiguration(new ContactConfiguration())
            .ApplyConfiguration(new PsychologistConfiguration())
            .ApplyConfiguration(new QuestionnaireConfiguration())
            .ApplyConfiguration(new RoleConfiguration())
            .ApplyConfiguration(new StatusConfiguration())
            .ApplyConfiguration(new UserConfiguration());
        
        base.OnModelCreating(builder);
    }
}