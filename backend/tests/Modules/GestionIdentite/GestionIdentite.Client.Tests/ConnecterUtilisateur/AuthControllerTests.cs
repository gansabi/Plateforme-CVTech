using FluentAssertions;
using GestionIdentite.Application.Features.ConnecterUtilisateur;
using GestionIdentite.Client.Controllers;
using GestionIdentite.Domaine.Enums;
using GestionIdentite.Domaine.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace GestionIdentite.Client.Tests.ConnecterUtilisateur;

public sealed class AuthControllerTests
{
    private readonly Mock<ISender> _sender = new();
    private readonly AuthController _controller;

    private static readonly ConnecterUtilisateurRequest RequeteValide =
        new("user@exemple.fr", "S3cr3t!Ok");

    public AuthControllerTests()
    {
        _controller = new AuthController(_sender.Object);
    }

    [Fact]
    public async Task LaConnexionAvecIdentifiantsValidesRetourneUnStatut200()
    {
        _sender.Setup(s => s.Send(It.IsAny<ConnecterUtilisateurCommand>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new ConnecterUtilisateurResponse(Guid.NewGuid(), "user@exemple.fr", Role.Candidat));

        var result = await _controller.ConnecterUtilisateur(RequeteValide, CancellationToken.None);

        result.Should().BeOfType<OkObjectResult>()
              .Which.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [Fact]
    public async Task LaConnexionAvecIdentifiantsInvalidesRetourneUnStatut401()
    {
        _sender.Setup(s => s.Send(It.IsAny<ConnecterUtilisateurCommand>(), It.IsAny<CancellationToken>()))
               .ThrowsAsync(new CredentielsInvalidesException());

        var result = await _controller.ConnecterUtilisateur(RequeteValide, CancellationToken.None);

        result.Should().BeOfType<UnauthorizedObjectResult>()
              .Which.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
    }

    [Fact]
    public async Task LaConnexionDUnCompteBlockeRetourneUnStatut403()
    {
        _sender.Setup(s => s.Send(It.IsAny<ConnecterUtilisateurCommand>(), It.IsAny<CancellationToken>()))
               .ThrowsAsync(new CompteEstBloqueException());

        var result = await _controller.ConnecterUtilisateur(RequeteValide, CancellationToken.None);

        result.Should().BeOfType<ObjectResult>()
              .Which.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
    }

    [Fact]
    public async Task LaConnexionRetourneLesInformationsUtilisateur()
    {
        var id = Guid.NewGuid();
        _sender.Setup(s => s.Send(It.IsAny<ConnecterUtilisateurCommand>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new ConnecterUtilisateurResponse(id, "user@exemple.fr", Role.Candidat));

        var result = await _controller.ConnecterUtilisateur(RequeteValide, CancellationToken.None)
            as OkObjectResult;

        result.Should().NotBeNull();
        result!.Value.Should().BeEquivalentTo(new
        {
            utilisateurId = id,
            email = "user@exemple.fr",
            role = Role.Candidat.ToString()
        });
    }
}
