using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using PsyAssistFeedback.WebApi.Extensions;
using PsyAssistPlatform.Application.Interfaces.Repository;
using PsyAssistPlatform.Application.Interfaces.Service;
using PsyAssistPlatform.Application.Mapping;
using PsyAssistPlatform.Application.Services;
using PsyAssistPlatform.Messages;
using PsyAssistPlatform.Domain;
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
        
        services.AddScoped<IRepository<Approach>, EfCoreRepository<Approach>>();
        services.AddScoped<IRepository<Contact>, EfCoreRepository<Contact>>();
        services.AddScoped<IRepository<PsychologistProfile>, EfCoreRepository<PsychologistProfile>>();
        services.AddScoped<IRepository<PsyRequest>, PsyRequestRepository>();
        services.AddScoped<IPsyRequestStatusRepository, PsyRequestStatusRepository>();
        services.AddScoped<IRepository<Questionnaire>, EfCoreRepository<Questionnaire>>();
        services.AddScoped<IRepository<Role>, RoleRepository>();
        services.AddScoped<IRepository<User>, EfCoreRepository<User>>();
        services.AddScoped<IRepository<Status>, StatusRepository>();
        services.AddScoped<IPsyRequestInfoRepository, PsyRequestInfoRepository>();
        
        services.AddScoped<IApproachService, ApproachService>();
        services.AddScoped<IContactService, ContactService>();
        services.AddScoped<IPsychologistProfileService, PsychologistProfileService>();
        services.AddScoped<IQuestionnaireService, QuestionnaireService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IStatusService, StatusService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPsyRequestInfoService, PsyRequestInfoService>();

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddDbContext<PsyAssistContext>(options =>
        {
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
            options.UseLazyLoadingProxies();
            options.EnableSensitiveDataLogging();
        });

        services.AddMemoryCache();
        services.AddRabbitMqServices(Configuration);
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