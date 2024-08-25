using PsyAssistPlatform.Persistence;
using Serilog;
using Serilog.Events;

namespace PsyAssistPlatform.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .WriteTo.Console()
            .CreateLogger();

        var host = CreateHostBuilder(args).Build();
        
        using var scope = host.Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        
        Log.Information("Starting web host");
        
        try
        {
            var context = serviceProvider.GetRequiredService<PsyAssistContext>();
            DbInitializer.Initialize(context);
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "An error ocurred while app initialization");
        }
        finally
        {
            Log.CloseAndFlush();
        }
        
        host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseSerilog()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .ConfigureLogging((context, logging) =>
            {
                if (context.HostingEnvironment.IsProduction())
                {
                    Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                        .WriteTo.File(
                            $"{Environment.CurrentDirectory}/Logs/PsyAssistPlatformWebApiLog-.txt",
                            rollingInterval: RollingInterval.Day,
                            retainedFileCountLimit: 30)
                        .CreateLogger();
                }
                else
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                    logging.AddDebug();
                }
            });
}