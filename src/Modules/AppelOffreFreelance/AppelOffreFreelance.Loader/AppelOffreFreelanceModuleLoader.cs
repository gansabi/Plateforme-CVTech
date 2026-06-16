using AppelOffreFreelance.Application.Features.PublierAppelOffre;
using AppelOffreFreelance.Domaine.Contrats;
using AppelOffreFreelance.Infrastructure.Evenements;
using AppelOffreFreelance.Infrastructure.Persistance;
using AppelOffreFreelance.Infrastructure.Persistance.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppelOffreFreelance.Loader;

public static class AppelOffreFreelanceModuleLoader
{
    public static IServiceCollection AjouterModuleAppelOffreFreelance(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        EnregistrerInfrastructure(services, configuration);
        EnregistrerApplication(services);

        return services;
    }

    private static void EnregistrerInfrastructure(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppelOffreFreelanceDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("AppelOffreFreelance")));

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
