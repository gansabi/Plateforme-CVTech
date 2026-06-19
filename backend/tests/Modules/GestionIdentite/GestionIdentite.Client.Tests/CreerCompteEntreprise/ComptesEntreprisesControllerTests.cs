using FluentAssertions;
using GestionIdentite.Application.Features.CreerCompteEntreprise;
using GestionIdentite.Client.Controllers;
using GestionIdentite.Domaine.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace GestionIdentite.Client.Tests.CreerCompteEntreprise;

public sealed class ComptesEntreprisesControllerTests
{
    private readonly Mock<ISender> _sender = new();
    private readonly ComptesEntreprisesController _controller;

    private static readonly CreerCompteEntrepriseRequest RequeteValide =
        new("entreprise@exemple.fr", "S3cr3t!Ok", "TechCorp SAS");

    public ComptesEntreprisesControllerTests()
    {
        _controller = new ComptesEntreprisesController(_sender.Object);
    }

    [Fact]
    public async Task LaCreationDUnCompteEntrepriseRetourneUnStatut201()
    {
        _sender.Setup(s => s.Send(It.IsAny<CreerCompteEntrepriseCommand>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new CreerCompteEntrepriseResponse(Guid.NewGuid()));

        var result = await _controller.CreerCompteEntreprise(RequeteValide, CancellationToken.None);

        result.Should().BeOfType<CreatedAtActionResult>()
              .Which.StatusCode.Should().Be(StatusCodes.Status201Created);
    }

    [Fact]
    public async Task LaCreationAvecEmailDejaUtiliseRetourneUnStatut409()
    {
        _sender.Setup(s => s.Send(It.IsAny<CreerCompteEntrepriseCommand>(), It.IsAny<CancellationToken>()))
               .ThrowsAsync(new EmailDejaUtiliseException("existant@exemple.fr"));

        var result = await _controller.CreerCompteEntreprise(
            new CreerCompteEntrepriseRequest("existant@exemple.fr", "S3cr3t!Ok", "Corp"),
            CancellationToken.None);

        result.Should().BeOfType<ConflictObjectResult>()
              .Which.StatusCode.Should().Be(StatusCodes.Status409Conflict);
    }

    [Fact]
    public async Task LaCreationMappeCorrectementLaRequeteVersLaCommand()
    {
        CreerCompteEntrepriseCommand? commandeCapturee = null;
        _sender.Setup(s => s.Send(It.IsAny<CreerCompteEntrepriseCommand>(), It.IsAny<CancellationToken>()))
               .Callback<IRequest<CreerCompteEntrepriseResponse>, CancellationToken>(
                   (cmd, _) => commandeCapturee = cmd as CreerCompteEntrepriseCommand)
               .ReturnsAsync(new CreerCompteEntrepriseResponse(Guid.NewGuid()));

        await _controller.CreerCompteEntreprise(RequeteValide, CancellationToken.None);

        commandeCapturee!.Email.Should().Be(RequeteValide.Email);
        commandeCapturee.NomEntreprise.Should().Be(RequeteValide.NomEntreprise);
    }
}
