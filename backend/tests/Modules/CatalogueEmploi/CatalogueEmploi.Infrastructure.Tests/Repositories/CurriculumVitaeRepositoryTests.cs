using CatalogueEmploi.Domaine.Entites;
using CatalogueEmploi.Infrastructure.Persistance;
using CatalogueEmploi.Infrastructure.Persistance.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CatalogueEmploi.Infrastructure.Tests.Repositories;

public sealed class CurriculumVitaeRepositoryTests : IDisposable
{
    private readonly CatalogueEmploiDbContext _dbContext;
    private readonly CurriculumVitaeRepository _repository;

    public CurriculumVitaeRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<CatalogueEmploiDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new CatalogueEmploiDbContext(options);
        _repository = new CurriculumVitaeRepository(_dbContext);
    }

    [Fact]
    public async Task SauvegarderPuisRecupererParCandidatId()
    {
        var candidatId = Guid.NewGuid();
        var cv = CurriculumVitae.Creer(candidatId, "Dev Senior", "Résumé.", ["C#", "Azure"]);

        await _repository.SauvegarderAsync(cv);

        var trouve = await _repository.TrouverParCandidatIdAsync(candidatId);
        trouve.Should().NotBeNull();
        trouve!.Titre.Should().Be("Dev Senior");
        trouve.CompetencesPrincipales.Should().Contain("C#");
    }

    [Fact]
    public async Task TrouverParCandidatIdRetourneNullSiInexistant()
    {
        var resultat = await _repository.TrouverParCandidatIdAsync(Guid.NewGuid());
        resultat.Should().BeNull();
    }

    [Fact]
    public async Task MettreAJourPersisteLesModifications()
    {
        var candidatId = Guid.NewGuid();
        var cv = CurriculumVitae.Creer(candidatId, "Ancien titre", "Ancien résumé.", []);

        await _repository.SauvegarderAsync(cv);

        cv.Modifier("Nouveau titre", "Nouveau résumé.", ["Terraform"]);
        await _repository.MettreAJourAsync(cv);

        var trouve = await _repository.TrouverParCandidatIdAsync(candidatId);
        trouve!.Titre.Should().Be("Nouveau titre");
        trouve.CompetencesPrincipales.Should().Contain("Terraform");
    }

    public void Dispose() => _dbContext.Dispose();
}
