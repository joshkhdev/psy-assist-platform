using Microsoft.EntityFrameworkCore;
using PsyAssistPlatform.Application.Interfaces;
using PsyAssistPlatform.Persistence;
using PsyAssistPlatform.Persistence.Repositories;
using PsyAssistPlatform.WebApi.Mapping;

namespace PsyAssistPlatform.WebApi;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private IConfiguration Configuration { get; }
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddRouting(options => options.LowercaseUrls = true);
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        
        services.AddScoped(typeof(IRepository<>), typeof(EfCoreRepository<>));
        
        services.AddDbContext<PsyAssistContext>(options =>
        {
            options.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
            options.UseLazyLoadingProxies();
            options.EnableSensitiveDataLogging();
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    
        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}