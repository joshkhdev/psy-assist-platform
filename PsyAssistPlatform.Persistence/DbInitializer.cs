namespace PsyAssistPlatform.Persistence;

public static class DbInitializer
{
    public static void Initialize(PsyAssistContext context)
    {
        context.Database.EnsureCreated();
    }
}