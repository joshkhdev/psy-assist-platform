using PsyAssistPlatform.Domain;
using PsyAssistPlatform.Persistence.Repositories;

namespace PsyAssistPlatform.Persistence;

public static class DbInitializer
{
    public static async Task InitializeAsync(PsyAssistContext context)
    {
        var isTest = false;

        await DeleteDatabaseAsync(context, isTest);
        await context.Database.EnsureCreatedAsync();

        await AddFakeDataAsync(context, isTest);
        await TestFakeDataAsync(context, isTest);
    }

    #region Fake Data Tests

    public static async Task DeleteDatabaseAsync(PsyAssistContext context, bool isTest)
    {
        if (!isTest) return;

        await context.Database.EnsureDeletedAsync();
    }

    public static async Task AddFakeDataAsync(PsyAssistContext context, bool isTest)
    {
        if (!isTest) return;

        await context.Psychologists.AddRangeAsync(FakeDataFactory.Psychologists);
        await context.SaveChangesAsync();
    }

    public static async Task TestFakeDataAsync(PsyAssistContext context, bool isTest)
    {
        if (!isTest) return;

        var psychologistRepository = new EfCoreRepository<Psychologist>(context);
        var testPsychologist = await psychologistRepository.GetAllAsync(CancellationToken.None);

        return;
    }

    #endregion

}