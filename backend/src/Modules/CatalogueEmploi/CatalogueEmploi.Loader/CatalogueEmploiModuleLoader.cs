using CatalogueEmploi.Application.Features.PublierAnnonce;
using CatalogueEmploi.Domaine.Contrats;
using CatalogueEmploi.Infrastructure.Evenements;
using CatalogueEmploi.Infrastructure.Persistance.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogueEmploi.Loader;

/// <summary>
/// Point unique d'enregistrement des dépendances du module CatalogueEmploi.
/// Le DbContext est configuré par le Host (choix du provider selon l'environnement).
/// Cf. .agent/skills/architecture-monolithe.md — section Loader.
/// </summary>
public static class CatalogueEmploiModuleLoader
{
    public static IServiceCollection AjouterModuleCatalogueEmploi(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        EnregistrerInfrastructure(services);
        EnregistrerApplication(services);

        return services;
    }

    private static void EnregistrerInfrastructure(IServiceCollection services)
    {
        services.AddScoped<IAnnonceEmploiRepository, AnnonceEmploiRepository>();
        services.AddScoped<ICandidatureRepository, CandidatureRepository>();
        services.AddScoped<ICurriculumVitaeRepository, CurriculumVitaeRepository>();
        services.AddScoped<IBusEvenements, BusEvenements>();
    }

    private static void EnregistrerApplication(IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(PublierAnnonceHandler).Assembly));

        services.AddValidatorsFromAssembly(typeof(PublierAnnonceValidator).Assembly);
    }
}
