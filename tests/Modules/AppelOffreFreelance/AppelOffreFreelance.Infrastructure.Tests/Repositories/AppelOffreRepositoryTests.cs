using AppelOffreFreelance.Domaine.Entites;
using AppelOffreFreelance.Domaine.Enums;
using AppelOffreFreelance.Domaine.ObjetsValeur;
using AppelOffreFreelance.Infrastructure.Persistance;
using AppelOffreFreelance.Infrastructure.Persistance.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AppelOffreFreelance.Infrastructure.Tests.Repositories;

public sealed class AppelOffreRepositoryTests : IDisposable
{
    private readonly AppelOffreFreelanceDbContext _dbContext;
    private readonly AppelOffreRepository _repository;

    public AppelOffreRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppelOffreFreelanceDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new AppelOffreFreelanceDbContext(options);
        _repository = new AppelOffreRepository(_dbContext);
    }

    [Fact]
    public async Task SauvegarderPuisRecupererUnAppelOffre()
    {
        var appelOffre = AppelOffre.Publier(
            Guid.NewGuid(), "Mission Cloud", "Description.",
            DomaineMetier.Creer("Cloud Azure"), BaremeTJM.Creer(500, 800), DateTime.UtcNow.AddDays(30));

        await _repository.SauvegarderAsync(appelOffre);

        var trouve = await _repository.TrouverParIdAsync(appelOffre.Id);
        trouve.Should().NotBeNull();
        trouve!.Titre.Should().Be("Mission Cloud");
    }

    [Fact]
    public async Task ListerOuvertsNeRetournePasLesAppelsFermes()
    {
        var ouvert = AppelOffre.Publier(
            Guid.NewGuid(), "Ouvert", "Desc.",
            DomaineMetier.Creer("DevOps"), BaremeTJM.Creer(400, 700), DateTime.UtcNow.AddDays(30));

        var ferme = AppelOffre.Publier(
            Guid.NewGuid(), "Fermé", "Desc.",
            DomaineMetier.Creer("Data"), BaremeTJM.Creer(300, 600), DateTime.UtcNow.AddDays(30));
        ferme.Moderer();

        await _repository.SauvegarderAsync(ouvert);
        await _repository.SauvegarderAsync(ferme);

        var ouverts = await _repository.ListerOuvertsAsync();
        ouverts.Should().HaveCount(1);
        ouverts[0].Titre.Should().Be("Ouvert");
    }

    [Fact]
    public async Task MettreAJourPersisteLesModifications()
    {
        var appelOffre = AppelOffre.Publier(
            Guid.NewGuid(), "Mission", "Desc.",
            DomaineMetier.Creer("Cloud"), BaremeTJM.Creer(400, 700), DateTime.UtcNow.AddDays(30));

        await _repository.SauvegarderAsync(appelOffre);

        appelOffre.Moderer();
        await _repository.MettreAJourAsync(appelOffre);

        var trouve = await _repository.TrouverParIdAsync(appelOffre.Id);
        trouve!.Statut.Should().Be(StatutAppelOffre.Ferme);
    }

    public void Dispose() => _dbContext.Dispose();
}
