using ActualiteEtAbonnement.Application.Features.PublierArticle;
using ActualiteEtAbonnement.Domaine.Contrats;
using ActualiteEtAbonnement.Infrastructure.Notifications;
using ActualiteEtAbonnement.Infrastructure.Persistance;
using ActualiteEtAbonnement.Infrastructure.Persistance.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ActualiteEtAbonnement.Loader;

public static class ActualiteEtAbonnementModuleLoader
{
    public static IServiceCollection AjouterModuleActualiteEtAbonnement(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        EnregistrerInfrastructure(services, configuration);
        EnregistrerApplication(services);

        return services;
    }

    private static void EnregistrerInfrastructure(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ActualiteEtAbonnementDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ActualiteEtAbonnement")));

        services.AddScoped<IArticleActualiteRepository, ArticleActualiteRepository>();
        services.AddScoped<IAbonnementRepository, AbonnementRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IServiceNotification, ServiceNotificationConsole>();
    }

    private static void EnregistrerApplication(IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(PublierArticleHandler).Assembly));

        services.AddValidatorsFromAssembly(typeof(PublierArticleHandler).Assembly);
    }
}
