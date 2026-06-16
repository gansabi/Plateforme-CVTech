using FluentAssertions;
using GestionIdentite.Domaine.Entites;
using GestionIdentite.Domaine.ObjetsValeur;
using GestionIdentite.Infrastructure.Persistance;
using GestionIdentite.Infrastructure.Persistance.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GestionIdentite.Infrastructure.Tests.CreerCompteCandidat;

/// <summary>
/// Tests d'intégration du repository contre une base InMemory.
/// Chaque test démarre avec une base isolée (Guid unique) pour éviter
/// toute interférence entre tests.
/// Cf. .agent/skills/regles-tdd.md — "Les tests de l'Infrastructure doivent
/// rester limités aux composants techniques."
/// </summary>
public sealed class CompteRepositoryTests
{
    // Chaque instance de la classe de test obtient sa propre base InMemory.
    private readonly string _nomBaseDeDonnees = Guid.NewGuid().ToString();

    private GestionIdentiteDbContext CreerContexte() =>
        new(new DbContextOptionsBuilder<GestionIdentiteDbContext>()
            .UseInMemoryDatabase(_nomBaseDeDonnees)
            .Options);

    private static CompteCandidat UnCompteCandidat(string email = "candidat@exemple.fr") =>
        CompteCandidat.Creer(
            Email.Creer(email),
            MotDePasse.Creer("S3cr3t!Ok"));

    // -----------------------------------------------------------------------
    // ExisteAvecEmailAsync
    // -----------------------------------------------------------------------

    [Fact]
    public async Task ExisteAvecEmailAsyncRetourneFauxSiAucunCompteExistant()
    {
        // Arrange
        await using var contexte = CreerContexte();
        var repository = new CompteRepository(contexte);

        // Act
        var existe = await repository.ExisteAvecEmailAsync("candidat@exemple.fr");

        // Assert
        existe.Should().BeFalse();
    }

    [Fact]
    public async Task ExisteAvecEmailAsyncRetourneTrueSiUnCompteAvecCetEmailExiste()
    {
        // Arrange — on sauvegarde dans un contexte, on lit dans un autre
        await using (var contexteEcriture = CreerContexte())
        {
            var repository = new CompteRepository(contexteEcriture);
            await repository.SauvegarderAsync(UnCompteCandidat("candidat@exemple.fr"));
        }

        // Act
        await using var contexteRelecture = CreerContexte();
        var repositoryLecture = new CompteRepository(contexteRelecture);
        var existe = await repositoryLecture.ExisteAvecEmailAsync("candidat@exemple.fr");

        // Assert
        existe.Should().BeTrue();
    }

    [Fact]
    public async Task ExisteAvecEmailAsyncEstInsensibleALaCasse()
    {
        // Arrange — stocké en minuscules via Email.Creer
        await using (var contexteEcriture = CreerContexte())
        {
            var repository = new CompteRepository(contexteEcriture);
            await repository.SauvegarderAsync(UnCompteCandidat("candidat@exemple.fr"));
        }

        // Act — recherche avec majuscules
        await using var contexteRelecture = CreerContexte();
        var repositoryLecture = new CompteRepository(contexteRelecture);
        var existe = await repositoryLecture.ExisteAvecEmailAsync("CANDIDAT@EXEMPLE.FR");

        // Assert
        existe.Should().BeTrue();
    }

    [Fact]
    public async Task ExisteAvecEmailAsyncNeTrouvePasUnEmailDifferent()
    {
        // Arrange
        await using (var contexteEcriture = CreerContexte())
        {
            var repository = new CompteRepository(contexteEcriture);
            await repository.SauvegarderAsync(UnCompteCandidat("alice@exemple.fr"));
        }

        // Act
        await using var contexteRelecture = CreerContexte();
        var repositoryLecture = new CompteRepository(contexteRelecture);
        var existe = await repositoryLecture.ExisteAvecEmailAsync("bob@exemple.fr");

        // Assert
        existe.Should().BeFalse();
    }

    // -----------------------------------------------------------------------
    // SauvegarderAsync
    // -----------------------------------------------------------------------

    [Fact]
    public async Task SauvegarderAsyncPersisteLeCompteEnBase()
    {
        // Arrange
        var compte = UnCompteCandidat();

        await using (var contexteEcriture = CreerContexte())
        {
            var repository = new CompteRepository(contexteEcriture);
            await repository.SauvegarderAsync(compte);
        }

        // Act
        await using var contexteRelecture = CreerContexte();
        var compteEnBase = await contexteRelecture.ComptesCandidats
            .FirstOrDefaultAsync(c => c.Id == compte.Id);

        // Assert
        compteEnBase.Should().NotBeNull();
    }

    [Fact]
    public async Task SauvegarderAsyncConserveLEmailDuCompte()
    {
        // Arrange
        var compte = UnCompteCandidat("candidat@exemple.fr");

        await using (var contexteEcriture = CreerContexte())
        {
            var repository = new CompteRepository(contexteEcriture);
            await repository.SauvegarderAsync(compte);
        }

        // Act
        await using var contexteRelecture = CreerContexte();
        var compteEnBase = await contexteRelecture.ComptesCandidats
            .FirstAsync(c => c.Id == compte.Id);

        // Assert
        compteEnBase.Email.Valeur.Should().Be("candidat@exemple.fr");
    }

    [Fact]
    public async Task SauvegarderAsyncConserveLeRoleCandidat()
    {
        // Arrange
        var compte = UnCompteCandidat();

        await using (var contexteEcriture = CreerContexte())
        {
            var repository = new CompteRepository(contexteEcriture);
            await repository.SauvegarderAsync(compte);
        }

        // Act
        await using var contexteRelecture = CreerContexte();
        var compteEnBase = await contexteRelecture.ComptesCandidats
            .FirstAsync(c => c.Id == compte.Id);

        // Assert
        compteEnBase.Role.Should().Be(GestionIdentite.Domaine.Enums.Role.Candidat);
    }

    [Fact]
    public async Task SauvegarderAsyncConserveLEtatNonBloqueParDefaut()
    {
        // Arrange
        var compte = UnCompteCandidat();

        await using (var contexteEcriture = CreerContexte())
        {
            var repository = new CompteRepository(contexteEcriture);
            await repository.SauvegarderAsync(compte);
        }

        // Act
        await using var contexteRelecture = CreerContexte();
        var compteEnBase = await contexteRelecture.ComptesCandidats
            .FirstAsync(c => c.Id == compte.Id);

        // Assert
        compteEnBase.EstBloque.Should().BeFalse();
    }

    [Fact]
    public async Task SauvegarderAsyncConserveLEtatBloqueApresModification()
    {
        // Arrange
        var compte = UnCompteCandidat();
        compte.Bloquer();

        await using (var contexteEcriture = CreerContexte())
        {
            var repository = new CompteRepository(contexteEcriture);
            await repository.SauvegarderAsync(compte);
        }

        // Act
        await using var contexteRelecture = CreerContexte();
        var compteEnBase = await contexteRelecture.ComptesCandidats
            .FirstAsync(c => c.Id == compte.Id);

        // Assert
        compteEnBase.EstBloque.Should().BeTrue();
    }
}
