using ActualiteEtAbonnement.Domaine.Entites;
using ActualiteEtAbonnement.Domaine.ObjetsValeur;
using ActualiteEtAbonnement.Infrastructure.Persistance;
using ActualiteEtAbonnement.Infrastructure.Persistance.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ActualiteEtAbonnement.Infrastructure.Tests.Repositories;

public sealed class ArticleActualiteRepositoryTests : IDisposable
{
    private readonly ActualiteEtAbonnementDbContext _dbContext;
    private readonly ArticleActualiteRepository _repository;

    public ArticleActualiteRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ActualiteEtAbonnementDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new ActualiteEtAbonnementDbContext(options);
        _repository = new ArticleActualiteRepository(_dbContext);
    }

    [Fact]
    public async Task SauvegarderPuisListerRetourneLArticle()
    {
        var article = ArticleActualite.Publier(
            Guid.NewGuid(), "Tendances Cloud", "Contenu.", DomaineMetier.Creer("Cloud Azure"));

        await _repository.SauvegarderAsync(article);

        var articles = await _repository.ListerAsync();
        articles.Should().HaveCount(1);
        articles[0].Titre.Should().Be("Tendances Cloud");
    }

    [Fact]
    public async Task ListerAvecFiltreDomaineRetourneUniquementLesBonsArticles()
    {
        var a1 = ArticleActualite.Publier(Guid.NewGuid(), "Cloud", "C.", DomaineMetier.Creer("Cloud Azure"));
        var a2 = ArticleActualite.Publier(Guid.NewGuid(), "Data", "D.", DomaineMetier.Creer("Data Science"));

        await _repository.SauvegarderAsync(a1);
        await _repository.SauvegarderAsync(a2);

        var articles = await _repository.ListerAsync("Cloud Azure");
        articles.Should().HaveCount(1);
        articles[0].Titre.Should().Be("Cloud");
    }

    public void Dispose() => _dbContext.Dispose();
}
