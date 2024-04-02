using PsyAssistPlatform.Domain;
using PsyAssistPlatform.Persistence.Data;
using PsyAssistPlatform.Persistence.Repositories;

namespace PsyAssistPlatform.Persistence;

public static class DbInitializer
{
    private const bool IsTest = false;

    public static void Initialize(PsyAssistContext context)
    {
        InitializeDatabase(context, IsTest);

        AddFakeData(context, IsTest);
        TestFakeData(context, IsTest);
    }

    public static void InitializeDatabase(PsyAssistContext context, bool isTest)
    {
        if (isTest)
            context.Database.EnsureDeleted();

        context.Database.EnsureCreated();
    }

    public static void AddFakeData(PsyAssistContext context, bool isTest)
    {
        if (!isTest) 
            return;

        context.Roles.AddRange(FakeDataFactory.Roles);
        context.Users.AddRange(FakeDataFactory.Users);
        context.Psychologists.AddRange(FakeDataFactory.Psychologists);

        context.SaveChanges();
    }

    public static void TestFakeData(PsyAssistContext context, bool isTest)
    {
        if (!isTest) 
            return;

        var rolesRepository = new EfCoreRepository<Role>(context);
        var usersRepository = new EfCoreRepository<User>(context);
        var psychologistRepository = new EfCoreRepository<Psychologist>(context);

        Task.Run(async () =>
        {
            Task[] tasks = 
            [
                rolesRepository.GetAllAsync(CancellationToken.None),
                usersRepository.GetAllAsync(CancellationToken.None),
                psychologistRepository.GetAllAsync(CancellationToken.None)
            ];

            await Task.WhenAll(tasks);
        });
    }

}