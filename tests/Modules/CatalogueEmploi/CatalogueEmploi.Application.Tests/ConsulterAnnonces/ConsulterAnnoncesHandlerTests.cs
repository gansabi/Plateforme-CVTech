using CatalogueEmploi.Application.Features.ConsulterAnnonces;
using CatalogueEmploi.Domaine.Contrats;
using CatalogueEmploi.Domaine.Entites;
using CatalogueEmploi.Domaine.Enums;
using CatalogueEmploi.Domaine.ObjetsValeur;
using FluentAssertions;
using Moq;
using Xunit;

namespace CatalogueEmploi.Application.Tests.ConsulterAnnonces;

public sealed class ConsulterAnnoncesHandlerTests
{
    private readonly Mock<IAnnonceEmploiRepository> _repository = new();
    private readonly ConsulterAnnoncesHandler _handler;

    public ConsulterAnnoncesHandlerTests()
    {
        _handler = new ConsulterAnnoncesHandler(_repository.Object);
    }

    [Fact]
    public async Task LaConsultationDesAnnoncesEstPubliqueEtNecessiteAucunePermission()
    {
        _repository.Setup(r => r.ListerActivesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var reponse = await _handler.Handle(new ConsulterAnnoncesQuery(), CancellationToken.None);

        reponse.Should().NotBeNull();
    }

    [Fact]
    public async Task LeHandlerRetourneLesAnnoncesActives()
    {
        var annonces = new List<AnnonceEmploi>
        {
            AnnonceEmploi.Publier(Guid.NewGuid(), "Dev .NET", "Mission.",
                TypeContrat.CDI, DomaineMetier.Creer("Cloud Azure")),
            AnnonceEmploi.Publier(Guid.NewGuid(), "Dev React", "Mission.",
                TypeContrat.CDD, DomaineMetier.Creer("Data Science"))
        };
        _repository.Setup(r => r.ListerActivesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(annonces.AsReadOnly());

        var reponse = await _handler.Handle(new ConsulterAnnoncesQuery(), CancellationToken.None);

        reponse.Annonces.Should().HaveCount(2);
        reponse.Annonces.Should().Contain(a => a.Titre == "Dev .NET");
    }
}
