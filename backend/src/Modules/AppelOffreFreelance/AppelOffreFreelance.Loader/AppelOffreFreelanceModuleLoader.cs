using AppelOffreFreelance.Application.Features.PublierAppelOffre;
using AppelOffreFreelance.Domaine.Contrats;
using AppelOffreFreelance.Infrastructure.Evenements;
using AppelOffreFreelance.Infrastructure.Persistance.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppelOffreFreelance.Loader;

/// <summary>
/// Point unique d'enregistrement des dépendances du module AppelOffreFreelance.
/// Le DbContext est configuré par le Host (choix du provider selon l'environnement).
/// </summary>
public static class AppelOffreFreelanceModuleLoader
{
    public static IServiceCollection AjouterModuleAppelOffreFreelance(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        EnregistrerInfrastructure(services);
        EnregistrerApplication(services);

        return services;
    }

    private static void EnregistrerInfrastructure(IServiceCollection services)
    {
        services.AddScoped<IAppelOffreRepository, AppelOffreRepository>();
        services.AddScoped<IPropositionFreelanceRepository, PropositionFreelanceRepository>();
        services.AddScoped<IBusEvenements, BusEvenements>();
    }

    private static void EnregistrerApplication(IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(PublierAppelOffreHandler).Assembly));

        services.AddValidatorsFromAssembly(typeof(PublierAppelOffreValidator).Assembly);
    }
}
