using CatalogueEmploi.Domaine.Entites;
using CatalogueEmploi.Infrastructure.Persistance;
using CatalogueEmploi.Infrastructure.Persistance.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CatalogueEmploi.Infrastructure.Tests.Repositories;

public sealed class CandidatureRepositoryTests : IDisposable
{
    private readonly CatalogueEmploiDbContext _dbContext;
    private readonly CandidatureRepository _repository;

    public CandidatureRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<CatalogueEmploiDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new CatalogueEmploiDbContext(options);
        _repository = new CandidatureRepository(_dbContext);
    }

    [Fact]
    public async Task SauvegarderPuisVerifierExistence()
    {
        var candidatId = Guid.NewGuid();
        var annonceId = Guid.NewGuid();
        var candidature = Candidature.Creer(candidatId, annonceId, "Motivation.");

        await _repository.SauvegarderAsync(candidature);

        var existe = await _repository.ExisteDejaAsync(candidatId, annonceId);
        existe.Should().BeTrue();
    }

    [Fact]
    public async Task ExisteDejaRetourneFauxSiPasDeCorrespondance()
    {
        var existe = await _repository.ExisteDejaAsync(Guid.NewGuid(), Guid.NewGuid());
        existe.Should().BeFalse();
    }

    [Fact]
    public async Task ListerParAnnonceIdRetourneLesCorrespondances()
    {
        var annonceId = Guid.NewGuid();
        var c1 = Candidature.Creer(Guid.NewGuid(), annonceId, null);
        var c2 = Candidature.Creer(Guid.NewGuid(), annonceId, "Lettre.");
        var c3 = Candidature.Creer(Guid.NewGuid(), Guid.NewGuid(), null); // autre annonce

        await _repository.SauvegarderAsync(c1);
        await _repository.SauvegarderAsync(c2);
        await _repository.SauvegarderAsync(c3);

        var resultats = await _repository.ListerParAnnonceIdAsync(annonceId);
        resultats.Should().HaveCount(2);
    }

    public void Dispose() => _dbContext.Dispose();
}
