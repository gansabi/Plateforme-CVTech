using FluentValidation;
using GestionIdentite.Application.Comportements;
using GestionIdentite.Application.Features.CreerCompteCandidat;
using GestionIdentite.Domaine.Contrats;
using GestionIdentite.Infrastructure.Persistance.Repositories;
using GestionIdentite.Infrastructure.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GestionIdentite.Loader;

/// <summary>
/// Point unique d'enregistrement des dépendances du module GestionIdentite.
/// Aucun enregistrement ne doit avoir lieu en dehors de cette classe.
/// Cf. .agent/skills/architecture-monolithe.md — section Loader.
/// </summary>
public static class GestionIdentiteModuleLoader
{
    public static IServiceCollection AjouterModuleGestionIdentite(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        EnregistrerInfrastructure(services);
        EnregistrerApplication(services);

        return services;
    }

    private static void EnregistrerInfrastructure(IServiceCollection services)
    {
        // Le provider et la chaîne de connexion sont configurés par l'hôte avant cet appel.
        services.AddScoped<ICompteRepository, CompteRepository>();
        services.AddScoped<ICompteEntrepriseRepository, CompteEntrepriseRepository>();
        services.AddScoped<ICompteAdministrateurRepository, CompteAdministrateurRepository>();
        services.AddScoped<IRegistreUtilisateurs, RegistreUtilisateurs>();
        services.AddScoped<IVerificateurPermission, VerificateurPermission>();
        services.AddScoped<IHasheurMotDePasse, HasheurMotDePasse>();
    }

    private static void EnregistrerApplication(IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(CreerCompteCandidatHandler).Assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(CreerCompteCandidatValidator).Assembly);
    }
}
