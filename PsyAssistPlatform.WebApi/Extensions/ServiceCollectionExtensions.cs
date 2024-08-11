using MassTransit;
using PsyAssistPlatform.Messages;

namespace PsyAssistFeedback.WebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRabbitMqServices(this IServiceCollection services, IConfiguration configuration)
            => services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    var rabbitUrl = configuration["IntegrationSettings:RabbitUrl"];

                    cfg.Host(rabbitUrl, h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.ConfigureEndpoints(context);
                });

                var serviceAddress = configuration["IntegrationSettings:RabbitFeedbackServiceUrl"];
                if (serviceAddress != null)
                {
                    var timeout = TimeSpan.FromSeconds(10);
                    x.AddRequestClient<FeedbacksMessage>(new Uri(serviceAddress), timeout);
                }
            });
    }
}
