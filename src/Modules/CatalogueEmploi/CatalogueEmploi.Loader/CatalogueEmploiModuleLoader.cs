using CatalogueEmploi.Application.Features.PublierAnnonce;
using CatalogueEmploi.Domaine.Contrats;
using CatalogueEmploi.Infrastructure.Evenements;
using CatalogueEmploi.Infrastructure.Persistance;
using CatalogueEmploi.Infrastructure.Persistance.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogueEmploi.Loader;

/// <summary>
/// Point unique d'enregistrement des dépendances du module CatalogueEmploi.
/// Aucun enregistrement ne doit avoir lieu en dehors de cette classe.
/// Cf. .agent/skills/architecture-monolithe.md — section Loader.
/// </summary>
public static class CatalogueEmploiModuleLoader
{
    public static IServiceCollection AjouterModuleCatalogueEmploi(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        EnregistrerInfrastructure(services, configuration);
        EnregistrerApplication(services);

        return services;
    }

    private static void EnregistrerInfrastructure(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CatalogueEmploiDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("CatalogueEmploi")));

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
