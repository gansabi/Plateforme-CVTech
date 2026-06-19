using AppelOffreFreelance.Domaine.Exceptions;
using AppelOffreFreelance.Domaine.ObjetsValeur;
using FluentAssertions;
using Xunit;

namespace AppelOffreFreelance.Domaine.Tests.BaremeTJM;

public sealed class BaremeTJMTests
{
    [Fact]
    public void UnBaremeTJMEstCreeAvecDesValeursValides()
    {
        var bareme = ObjetsValeur.BaremeTJM.Creer(400, 700);

        bareme.Minimum.Should().Be(400);
        bareme.Maximum.Should().Be(700);
    }

    [Fact]
    public void CreerAvecMinimumNegatifLeveBaremeTJMInvalideException()
    {
        var creer = () => ObjetsValeur.BaremeTJM.Creer(-100, 500);

        creer.Should().Throw<BaremeTJMInvalideException>();
    }

    [Fact]
    public void CreerAvecMaximumInferieurAuMinimumLeveBaremeTJMInvalideException()
    {
        var creer = () => ObjetsValeur.BaremeTJM.Creer(800, 400);

        creer.Should().Throw<BaremeTJMInvalideException>();
    }
}
