using CrossCutting.Api.Authentication;
using Inventory.Application.Events;
using Inventory.Infrastructure.Events;
using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;

namespace Inventory.Api;

public static class Startup
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("basic", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "basic",
                In = ParameterLocation.Header,
                Description = "Basic Authorization header using the Bearer scheme."
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "basic"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        services
            .AddAuthentication("BasicAuthentication")
            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
        services.AddAuthorization();
        // TODO: Enable hosted service (fix DI issue con scoped services)
        // services.AddHostedService<ExpiredItemsNotificatorHostedService>();
        services.AddSingleton<IEventBus, EventBus>();

        return services;
    }
}