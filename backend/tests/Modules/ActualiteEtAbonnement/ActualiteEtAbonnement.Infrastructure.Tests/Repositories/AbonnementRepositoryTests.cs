using ActualiteEtAbonnement.Domaine.Entites;
using ActualiteEtAbonnement.Domaine.ObjetsValeur;
using ActualiteEtAbonnement.Infrastructure.Persistance;
using ActualiteEtAbonnement.Infrastructure.Persistance.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ActualiteEtAbonnement.Infrastructure.Tests.Repositories;

public sealed class AbonnementRepositoryTests : IDisposable
{
    private readonly ActualiteEtAbonnementDbContext _dbContext;
    private readonly AbonnementRepository _repository;

    public AbonnementRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ActualiteEtAbonnementDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new ActualiteEtAbonnementDbContext(options);
        _repository = new AbonnementRepository(_dbContext);
    }

    [Fact]
    public async Task SauvegarderPuisVerifierExistence()
    {
        var utilisateurId = Guid.NewGuid();
        var abonnement = Abonnement.Creer(utilisateurId, DomaineMetier.Creer("Cloud Azure"));

        await _repository.SauvegarderAsync(abonnement);

        var existe = await _repository.ExisteDejaAsync(utilisateurId, "Cloud Azure");
        existe.Should().BeTrue();
    }

    [Fact]
    public async Task ExisteDejaRetourneFauxSiPasDAbonnement()
    {
        var existe = await _repository.ExisteDejaAsync(Guid.NewGuid(), "Inexistant");
        existe.Should().BeFalse();
    }

    [Fact]
    public async Task ListerParDomaineRetourneLesAbonnesConcernes()
    {
        var a1 = Abonnement.Creer(Guid.NewGuid(), DomaineMetier.Creer("Cloud Azure"));
        var a2 = Abonnement.Creer(Guid.NewGuid(), DomaineMetier.Creer("Cloud Azure"));
        var a3 = Abonnement.Creer(Guid.NewGuid(), DomaineMetier.Creer("Data Science"));

        await _repository.SauvegarderAsync(a1);
        await _repository.SauvegarderAsync(a2);
        await _repository.SauvegarderAsync(a3);

        var abonnes = await _repository.ListerParDomaineAsync("Cloud Azure");
        abonnes.Should().HaveCount(2);
    }

    public void Dispose() => _dbContext.Dispose();
}
