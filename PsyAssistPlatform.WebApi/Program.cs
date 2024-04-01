using PsyAssistPlatform.Persistence;

namespace PsyAssistPlatform.WebApi;

public class Program
{
    public static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        
        using var scope = host.Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        try
        {
            var context = serviceProvider.GetRequiredService<PsyAssistContext>();
            await DbInitializer.InitializeAsync(context);
        }
        catch (Exception ex)
        {
            // TODO: Заменить в будущем на Logger
            Console.WriteLine(ex);
        }
        
        host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
}