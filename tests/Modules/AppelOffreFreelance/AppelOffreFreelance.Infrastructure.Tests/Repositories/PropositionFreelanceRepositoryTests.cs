using AppelOffreFreelance.Domaine.Entites;
using AppelOffreFreelance.Infrastructure.Persistance;
using AppelOffreFreelance.Infrastructure.Persistance.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AppelOffreFreelance.Infrastructure.Tests.Repositories;

public sealed class PropositionFreelanceRepositoryTests : IDisposable
{
    private readonly AppelOffreFreelanceDbContext _dbContext;
    private readonly PropositionFreelanceRepository _repository;

    public PropositionFreelanceRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppelOffreFreelanceDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new AppelOffreFreelanceDbContext(options);
        _repository = new PropositionFreelanceRepository(_dbContext);
    }

    [Fact]
    public async Task SauvegarderPuisVerifierExistence()
    {
        var candidatId = Guid.NewGuid();
        var appelOffreId = Guid.NewGuid();
        var proposition = PropositionFreelance.Creer(candidatId, appelOffreId, 600, 20, "Scrum");

        await _repository.SauvegarderAsync(proposition);

        var existe = await _repository.ExisteDejaAsync(candidatId, appelOffreId);
        existe.Should().BeTrue();
    }

    [Fact]
    public async Task ExisteDejaRetourneFauxSiPasDeCorrespondance()
    {
        var existe = await _repository.ExisteDejaAsync(Guid.NewGuid(), Guid.NewGuid());
        existe.Should().BeFalse();
    }

    [Fact]
    public async Task ListerParAppelOffreIdRetourneLesCorrespondances()
    {
        var appelOffreId = Guid.NewGuid();
        var p1 = PropositionFreelance.Creer(Guid.NewGuid(), appelOffreId, 500, 15, "Kanban");
        var p2 = PropositionFreelance.Creer(Guid.NewGuid(), appelOffreId, 700, 30, "Scrum");
        var p3 = PropositionFreelance.Creer(Guid.NewGuid(), Guid.NewGuid(), 600, 20, "Autre");

        await _repository.SauvegarderAsync(p1);
        await _repository.SauvegarderAsync(p2);
        await _repository.SauvegarderAsync(p3);

        var resultats = await _repository.ListerParAppelOffreIdAsync(appelOffreId);
        resultats.Should().HaveCount(2);
    }

    public void Dispose() => _dbContext.Dispose();
}
