using ActualiteEtAbonnement.Application.Features.PublierArticle;
using ActualiteEtAbonnement.Domaine.Contrats;
using ActualiteEtAbonnement.Infrastructure.Notifications;
using ActualiteEtAbonnement.Infrastructure.Persistance.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ActualiteEtAbonnement.Loader;

/// <summary>
/// Point unique d'enregistrement des dépendances du module ActualiteEtAbonnement.
/// Le DbContext est configuré par le Host (choix du provider selon l'environnement).
/// </summary>
public static class ActualiteEtAbonnementModuleLoader
{
    public static IServiceCollection AjouterModuleActualiteEtAbonnement(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        EnregistrerInfrastructure(services);
        EnregistrerApplication(services);

        return services;
    }

    private static void EnregistrerInfrastructure(IServiceCollection services)
    {
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
