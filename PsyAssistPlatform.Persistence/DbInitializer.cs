using PsyAssistPlatform.Domain;
using PsyAssistPlatform.Persistence.Data;
using PsyAssistPlatform.Persistence.Repositories;

namespace PsyAssistPlatform.Persistence;

public static class DbInitializer
{
    public static void Initialize(PsyAssistContext context)
    {
        InitializeDatabase(context);

        AddFakeData(context);
    }

    public static void InitializeDatabase(PsyAssistContext context)
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }

    public static void AddFakeData(PsyAssistContext context)
    {
        context.Roles.AddRange(FakeDataFactory.Roles);
        context.Approaches.AddRange(FakeDataFactory.Approaches);
        context.Statuses.AddRange(FakeDataFactory.Statuses);
        context.Users.AddRange(FakeDataFactory.Users);
        context.Psychologists.AddRange(FakeDataFactory.Psychologists);
        context.Questionnaires.AddRange(FakeDataFactory.Questionnaires);

        context.SaveChanges();
    }
}