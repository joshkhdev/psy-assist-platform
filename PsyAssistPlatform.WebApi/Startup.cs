using FluentValidation;
using FluentValidation.AspNetCore;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
using PsyAssistPlatform.WebApi.Extensions;
using PsyAssistPlatform.WebApi.Mapping;
using PsyAssistPlatform.WebApi.Middlewares;
using PsyAssistPlatform.WebApi.Models.Approach;
using PsyAssistPlatform.WebApi.Models.Contact;
using PsyAssistPlatform.WebApi.Models.PsychologistProfile;
using PsyAssistPlatform.WebApi.Models.Questionnaire;
using PsyAssistPlatform.WebApi.Models.User;
using PsyAssistPlatform.WebApi.Services;
using PsyAssistPlatform.WebApi.TokenValidation;

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
        services.AddSwaggerGenWithJWTSupport();

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

        services.AddTransient<ICustomTokenRequestValidator, CustomTokenRequestValidator>();

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

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = Configuration["Jwt:Authority"];
                options.Audience = Configuration["Jwt:Audience"];
                options.RequireHttpsMetadata = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Startup>>();
                        logger.LogError("Token validation failed: {Exception}", context.Exception);
                        return Task.CompletedTask;
                    },
                };
            });

        services.AddAuthorization();
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

        app.UseStaticFiles();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}