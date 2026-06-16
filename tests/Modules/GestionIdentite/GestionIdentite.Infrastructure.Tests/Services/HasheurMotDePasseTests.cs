using FluentAssertions;
using GestionIdentite.Infrastructure.Services;
using Xunit;

namespace GestionIdentite.Infrastructure.Tests.Services;

public sealed class HasheurMotDePasseTests
{
    private readonly HasheurMotDePasse _hasheur = new();

    [Fact]
    public void HacherUnMotDePasseRetourneUnHashNonVide()
    {
        var hash = _hasheur.Hacher("MotDePasse1!");

        hash.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void DeuxHashagesDuMemeMotDePasseProduisentDesHashsDifferents()
    {
        var hash1 = _hasheur.Hacher("MotDePasse1!");
        var hash2 = _hasheur.Hacher("MotDePasse1!");

        hash1.Should().NotBe(hash2, "le sel aléatoire doit produire des hashs distincts");
    }

    [Fact]
    public void VerifierRetourneVraiAvecLeMotDePasseCorrect()
    {
        var hash = _hasheur.Hacher("MotDePasse1!");

        var resultat = _hasheur.Verifier("MotDePasse1!", hash);

        resultat.Should().BeTrue();
    }

    [Fact]
    public void VerifierRetourneFauxAvecUnMotDePasseIncorrect()
    {
        var hash = _hasheur.Hacher("MotDePasse1!");

        var resultat = _hasheur.Verifier("AutreMotDePasse!", hash);

        resultat.Should().BeFalse();
    }

    [Fact]
    public void VerifierRetourneFauxAvecUnHashManipule()
    {
        var resultat = _hasheur.Verifier("MotDePasse1!", "hash_invalide");

        resultat.Should().BeFalse();
    }
}
