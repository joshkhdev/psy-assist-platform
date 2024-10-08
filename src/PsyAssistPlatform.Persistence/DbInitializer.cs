using PsyAssistPlatform.Persistence.Data;

namespace PsyAssistPlatform.Persistence;

public static class DbInitializer
{
    public static void Initialize(PsyAssistContext context)
    {
        InitializeDatabase(context);
        AddFakeData(context);
    }

    private static void InitializeDatabase(PsyAssistContext context)
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }

    private static void AddFakeData(PsyAssistContext context)
    {
        context.Roles.AddRange(FakeDataFactory.Roles);
        context.Approaches.AddRange(FakeDataFactory.Approaches);
        context.Statuses.AddRange(FakeDataFactory.Statuses);
        context.Contacts.AddRange(FakeDataFactory.Contacts);
        context.Users.AddRange(FakeDataFactory.Users);
        context.Questionnaires.AddRange(FakeDataFactory.Questionnaires);
        context.SaveChanges();
        
        context.PsychologistProfiles.AddRange(FakeDataFactory.PsychologistProfiles);
        context.SaveChanges();
        
        context.PsyRequests.AddRange(FakeDataFactory.PsyRequests);
        context.SaveChanges();
        
        context.PsyRequestStatuses.AddRange(FakeDataFactory.PsyRequestStatuses);
        context.SaveChanges();
    }
}