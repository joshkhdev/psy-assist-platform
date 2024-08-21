using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using PsyAssistPlatform.Application.Integrations.Service;
using PsyAssistPlatform.Application.Interfaces.Integration;
using PsyAssistPlatform.Application.Interfaces.Repository;
using PsyAssistPlatform.Application.Interfaces.Service;
using PsyAssistPlatform.Application.Mapping;
using PsyAssistPlatform.Application.Services;
using PsyAssistPlatform.Persistence;
using PsyAssistPlatform.Persistence.Repositories;
using PsyAssistPlatform.WebApi.Contracts;
using PsyAssistPlatform.WebApi.Mapping;
using PsyAssistPlatform.WebApi.Middlewares;
using PsyAssistPlatform.WebApi.Models.Approach;
using PsyAssistPlatform.WebApi.Models.Contact;
using PsyAssistPlatform.WebApi.Models.PsychologistProfile;
using PsyAssistPlatform.WebApi.Models.Questionnaire;
using PsyAssistPlatform.WebApi.Models.User;
using PsyAssistPlatform.WebApi.Services;

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
        services.AddAutoMapper(typeof(ApplicationMappingProfile), typeof(PresentationMappingProfile));
        services.AddRouting(options => options.LowercaseUrls = true);
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();
        services.AddValidatorsFromAssemblyContaining<CreateApproachRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateApproachRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateContactRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<CreatePsychologistProfileRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdatePsychologistProfileRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<CreateQuestionnaireRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<CreateUserRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateUserRequestValidator>();
        
        services.AddScoped(typeof(IRepository<>), typeof(EfCoreRepository<>));
        services.AddScoped<IApproachService, ApproachService>();
        services.AddScoped<IContactService, ContactService>();
        services.AddScoped<IPsychologistProfileService, PsychologistProfileService>();
        services.AddScoped<IQuestionnaireService, QuestionnaireService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IStatusService, StatusService>();
        services.AddScoped<IUserService, UserService>();

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddHttpClient<IContentService, ContentService>(x =>
        {
            x.BaseAddress = new Uri(Configuration["ContentSettings:ContentApiUrl"]);
        });

        services.AddDbContext<PsyAssistContext>(options =>
        {
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
            options.UseLazyLoadingProxies();
            options.EnableSensitiveDataLogging();
        });

        services.AddMemoryCache();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseMiddleware<LoggingHandlerMiddleware>();
        app.UseMiddleware<ExceptionHandlerMiddleware>();
        
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    
        app.UseHttpsRedirection();
        
        app.UseCors(policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}