using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Duende.Bff;
using Duende.Bff.Yarp;
using Eiromplays.IdentityServer.Application.Common.Configurations.Account;
using Eiromplays.IdentityServer.Application.Common.Configurations.Identity;
using Eiromplays.IdentityServer.Domain.Enums;
using Eiromplays.IdentityServer.Infrastructure.Auth;
using Eiromplays.IdentityServer.Infrastructure.BackgroundJobs;
using Eiromplays.IdentityServer.Infrastructure.Caching;
using Eiromplays.IdentityServer.Infrastructure.Common;
using Eiromplays.IdentityServer.Infrastructure.Cors;
using Eiromplays.IdentityServer.Infrastructure.FileStorage;
using Eiromplays.IdentityServer.Infrastructure.Localization;
using Eiromplays.IdentityServer.Infrastructure.Mailing;
using Eiromplays.IdentityServer.Infrastructure.Mapping;
using Eiromplays.IdentityServer.Infrastructure.Middleware;
using Eiromplays.IdentityServer.Infrastructure.Notifications;
using Eiromplays.IdentityServer.Infrastructure.OpenApi;
using Eiromplays.IdentityServer.Infrastructure.Persistence;
using Eiromplays.IdentityServer.Infrastructure.Persistence.Initialization;
using Eiromplays.IdentityServer.Infrastructure.SecurityHeaders;
using Eiromplays.IdentityServer.Infrastructure.Sms;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols;

[assembly: InternalsVisibleTo("Infrastructure.Test")]

namespace Eiromplays.IdentityServer.Infrastructure;

public static class Startup
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager config, IWebHostEnvironment webHostEnvironment, ProjectType projectType)
    {
        if (projectType is ProjectType.Spa) return services.AddInfrastructureSpa(config, webHostEnvironment, projectType);

        MapsterSettings.Configure();
        return services
            .AddConfigurations(config, webHostEnvironment)
            .AddBackgroundJobs(config)
            .AddCaching(config)
            .AddCorsPolicy(config)
            .AddExceptionMiddleware()
            .AddHealthCheck()
            .AddPoLocalization(config)
            .AddMailing(config)
            .AddSms(config)
            .AddMediatR(Assembly.GetExecutingAssembly())
            .AddNotifications(config)
            .AddOpenApiDocumentation(config)
            .AddPersistence(config, projectType)
            .AddDataProtection().Services
            .AddAuth(config, projectType)
            .AddRequestLogging(config)
            .AddRouting(options => options.LowercaseUrls = true)
            .AddServices(projectType);
    }

    private static IServiceCollection AddInfrastructureSpa(this IServiceCollection services, ConfigurationManager config, IWebHostEnvironment webHostEnvironment, ProjectType projectType)
    {
        if (projectType is not ProjectType.Spa) return services;

        config.AddAwsSecretsManager(webHostEnvironment);

        var bffBuilder = services.AddBff(options => config.GetSection(nameof(BffOptions)).Bind(options));

        bffBuilder.AddRemoteApis();
        bffBuilder.AddBffPersistence(config, projectType);

        return services;
    }

    private static IServiceCollection AddHealthCheck(this IServiceCollection services) =>
        services.AddHealthChecks().Services;

    public static async Task InitializeDatabasesAsync(this IServiceProvider services, CancellationToken cancellationToken = default)
    {
        // Create a new scope to retrieve scoped services
        using var scope = services.CreateScope();

        await scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>()
            .InitializeDatabasesAsync(cancellationToken);
    }

    public static WebApplication UseInfrastructure(this WebApplication app, IConfiguration config, ProjectType projectType, Action<Config>? fastEndpointsConfigAction = null)
    {
        if (projectType is ProjectType.Spa) return app;

        app
            .UseRequestLocalization()
            .UseStaticFiles()
            .UseSecurityHeaders(config)
            .UseFileStorage()
            .UseExceptionMiddleware()
            .UseRouting()
            .UseCorsPolicy()
            .UseAuthentication()
            .UseIdentityServer(projectType)
            .UseAuthorization()
            .UseCurrentUser()
            .UseRequestLogging(config)
            .UseHangfireDashboard(config);

        app.UseFastEndpoints(fastEndpointsConfigAction);

        app.UseOpenApiDocumentation(config);

        return app;
    }

    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapControllers().RequireAuthorization();
        builder.MapHealthCheck();
        builder.MapNotifications();
        return builder;
    }

    // ReSharper disable once UnusedMethodReturnValue.Local
    private static IEndpointConventionBuilder MapHealthCheck(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapHealthChecks("/api/health").RequireAuthorization();


    // Registers AWS Secrets Manager as a source for configuration values.
    private static ConfigurationManager AddAwsSecretsManager(this ConfigurationManager configuration, IWebHostEnvironment webHostEnvironment)
    {
        string[] allowedPrefixes =
        {
            $"{webHostEnvironment.EnvironmentName}/{webHostEnvironment.ApplicationName}/",
            $"{webHostEnvironment.EnvironmentName}/EiromplaysIdentityServer/",
            "Dev/EiromplaysIdentityServer"
        };

        configuration.AddSecretsManager(configurator: config =>
        {
            config.KeyGenerator = (_, name) =>
            {
                string prefix = allowedPrefixes.First(name.StartsWith);

                name = name.Replace(prefix, string.Empty).Replace("__", ":");
                if (name.StartsWith(":"))
                    name = name[1..];

                return name;
            };

            config.SecretFilter = secret => allowedPrefixes.Any(allowed => secret.Name.StartsWith(allowed));
        });

        return configuration;
    }

    /*
        Configures custom classes for config files, so they can be retrieved from DI using IOptions<T>
        Information:
        Account Configuration:
        Profile Picture Configuration:
        Find more avatar styles here: https://avatars.dicebear.com/styles/
        You can also use a custom provider
    */
    private static IServiceCollection AddConfigurations(this IServiceCollection services, ConfigurationManager configuration, IWebHostEnvironment webHostEnvironment)
    {
        configuration.AddAwsSecretsManager(webHostEnvironment);

        return services.Configure<AccountConfiguration>(configuration.GetSection(nameof(AccountConfiguration)))
            .Configure<IdentityServerData>(configuration.GetSection(nameof(IdentityServerData)))
            .Configure<IdentityData>(configuration.GetSection(nameof(IdentityData)));
    }

    private static IApplicationBuilder UseIdentityServer(this IApplicationBuilder builder, ProjectType projectType)
    {
        return projectType is ProjectType.IdentityServer ? builder.UseIdentityServer() : builder;
    }
}