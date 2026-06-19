using CatalogueEmploi.Domaine.Entites;
using CatalogueEmploi.Domaine.Enums;
using CatalogueEmploi.Domaine.ObjetsValeur;
using CatalogueEmploi.Infrastructure.Persistance;
using CatalogueEmploi.Infrastructure.Persistance.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CatalogueEmploi.Infrastructure.Tests.Repositories;

public sealed class AnnonceEmploiRepositoryTests : IDisposable
{
    private readonly CatalogueEmploiDbContext _dbContext;
    private readonly AnnonceEmploiRepository _repository;

    public AnnonceEmploiRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<CatalogueEmploiDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new CatalogueEmploiDbContext(options);
        _repository = new AnnonceEmploiRepository(_dbContext);
    }

    [Fact]
    public async Task SauvegarderPuisRecupererUneAnnonce()
    {
        var annonce = AnnonceEmploi.Publier(
            Guid.NewGuid(), "Dev .NET", "Mission cloud.",
            TypeContrat.CDI, DomaineMetier.Creer("Cloud Azure"));

        await _repository.SauvegarderAsync(annonce);

        var trouvee = await _repository.TrouverParIdAsync(annonce.Id);
        trouvee.Should().NotBeNull();
        trouvee!.Titre.Should().Be("Dev .NET");
    }

    [Fact]
    public async Task ListerActivesNeRetournePasLesAnnoncesInactives()
    {
        var active = AnnonceEmploi.Publier(
            Guid.NewGuid(), "Active", "Description.",
            TypeContrat.CDI, DomaineMetier.Creer("DevOps"));

        var moderee = AnnonceEmploi.Publier(
            Guid.NewGuid(), "Modérée", "Description.",
            TypeContrat.CDD, DomaineMetier.Creer("Data"));
        moderee.Moderer();

        await _repository.SauvegarderAsync(active);
        await _repository.SauvegarderAsync(moderee);

        var actives = await _repository.ListerActivesAsync();
        actives.Should().HaveCount(1);
        actives[0].Titre.Should().Be("Active");
    }

    [Fact]
    public async Task MettreAJourPersisteLesModifications()
    {
        var annonce = AnnonceEmploi.Publier(
            Guid.NewGuid(), "Dev React", "Mission frontend.",
            TypeContrat.CDD, DomaineMetier.Creer("Frontend"));

        await _repository.SauvegarderAsync(annonce);

        annonce.Moderer();
        await _repository.MettreAJourAsync(annonce);

        var trouvee = await _repository.TrouverParIdAsync(annonce.Id);
        trouvee!.EstActive.Should().BeFalse();
    }

    public void Dispose() => _dbContext.Dispose();
}
