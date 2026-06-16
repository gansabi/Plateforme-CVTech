using FluentAssertions;
using FluentValidation;
using GestionIdentite.Application.Comportements;
using MediatR;
using Xunit;

namespace GestionIdentite.Application.Tests.Comportements;

public sealed class ValidationBehaviorTests
{
    // -----------------------------------------------------------------------
    // Types internes utilisés uniquement pour les tests
    // -----------------------------------------------------------------------

    private sealed class RequeteDeTest : IRequest<ReponseDeTest>
    {
        public string Valeur { get; init; } = string.Empty;
        public string AutreValeur { get; init; } = string.Empty;
    }

    private sealed record ReponseDeTest(string Resultat);

    private sealed class ValidateurDeTest : AbstractValidator<RequeteDeTest>
    {
        public ValidateurDeTest()
        {
            RuleFor(x => x.Valeur)
                .NotEmpty()
                .WithMessage("La valeur est obligatoire.");
        }
    }

    private sealed class ValidateurSupplementaireDeTest : AbstractValidator<RequeteDeTest>
    {
        public ValidateurSupplementaireDeTest()
        {
            RuleFor(x => x.AutreValeur)
                .NotEmpty()
                .WithMessage("L'autre valeur est obligatoire.");
        }
    }

    private sealed class ValidateurMultiReglesDeTest : AbstractValidator<RequeteDeTest>
    {
        public ValidateurMultiReglesDeTest()
        {
            RuleFor(x => x.Valeur).NotEmpty().WithMessage("La valeur est obligatoire.");
            RuleFor(x => x.AutreValeur).NotEmpty().WithMessage("L'autre valeur est obligatoire.");
        }
    }

    // -----------------------------------------------------------------------
    // Helpers
    // -----------------------------------------------------------------------

    private static RequestHandlerDelegate<ReponseDeTest> CreerDelegueSucces(ref bool nextAppele)
    {
        // Capture via closure : la variable est copiée par référence implicitement.
        var capture = false;
        RequestHandlerDelegate<ReponseDeTest> delegue = () =>
        {
            capture = true;
            return Task.FromResult(new ReponseDeTest("succès"));
        };
        // On retourne le délégué et on stocke la référence de capture pour vérification.
        _ = capture; // surpasse l'avertissement CS0219
        return delegue;
    }

    // -----------------------------------------------------------------------
    // Tests
    // -----------------------------------------------------------------------

    [Fact]
    public async Task QuandAucunValidateurNestPresentLaRequetePasseAuHandlerSuivant()
    {
        // Arrange
        var behavior = new ValidationBehavior<RequeteDeTest, ReponseDeTest>(
            Array.Empty<IValidator<RequeteDeTest>>());

        var nextAppele = false;
        RequestHandlerDelegate<ReponseDeTest> next = () =>
        {
            nextAppele = true;
            return Task.FromResult(new ReponseDeTest("succès"));
        };

        // Act
        var reponse = await behavior.Handle(new RequeteDeTest(), next, CancellationToken.None);

        // Assert
        nextAppele.Should().BeTrue();
        reponse.Resultat.Should().Be("succès");
    }

    [Fact]
    public async Task QuandLaCommandeEstValideLaRequetePasseAuHandlerSuivant()
    {
        // Arrange
        var behavior = new ValidationBehavior<RequeteDeTest, ReponseDeTest>(
            [new ValidateurDeTest()]);

        var nextAppele = false;
        RequestHandlerDelegate<ReponseDeTest> next = () =>
        {
            nextAppele = true;
            return Task.FromResult(new ReponseDeTest("succès"));
        };

        var requeteValide = new RequeteDeTest { Valeur = "contenu valide" };

        // Act
        await behavior.Handle(requeteValide, next, CancellationToken.None);

        // Assert
        nextAppele.Should().BeTrue();
    }

    [Fact]
    public async Task QuandLaCommandeEstInvalideLeBehaviorLeveUneValidationException()
    {
        // Arrange
        var behavior = new ValidationBehavior<RequeteDeTest, ReponseDeTest>(
            [new ValidateurDeTest()]);

        RequestHandlerDelegate<ReponseDeTest> next =
            () => Task.FromResult(new ReponseDeTest("ne doit pas être appelé"));

        var requeteInvalide = new RequeteDeTest { Valeur = string.Empty };

        // Act
        var creer = () => behavior.Handle(requeteInvalide, next, CancellationToken.None);

        // Assert
        await creer.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task QuandLaCommandeEstInvalideLaBehaviorNAppellePasLeHandlerSuivant()
    {
        // Arrange
        var behavior = new ValidationBehavior<RequeteDeTest, ReponseDeTest>(
            [new ValidateurDeTest()]);

        var nextAppele = false;
        RequestHandlerDelegate<ReponseDeTest> next = () =>
        {
            nextAppele = true;
            return Task.FromResult(new ReponseDeTest("ne doit pas être appelé"));
        };

        var requeteInvalide = new RequeteDeTest { Valeur = string.Empty };

        // Act
        try { await behavior.Handle(requeteInvalide, next, CancellationToken.None); }
        catch (ValidationException) { /* attendu */ }

        // Assert
        nextAppele.Should().BeFalse();
    }

    [Fact]
    public async Task LExceptionDeValidationContientLErreurDeValidation()
    {
        // Arrange
        var behavior = new ValidationBehavior<RequeteDeTest, ReponseDeTest>(
            [new ValidateurDeTest()]);

        RequestHandlerDelegate<ReponseDeTest> next =
            () => Task.FromResult(new ReponseDeTest(""));

        var requeteInvalide = new RequeteDeTest { Valeur = string.Empty };

        // Act
        var exception = await Record.ExceptionAsync(
            () => behavior.Handle(requeteInvalide, next, CancellationToken.None));

        // Assert
        exception.Should().BeOfType<ValidationException>();
        var validationEx = (ValidationException)exception!;
        validationEx.Errors.Should().ContainSingle(e =>
            e.PropertyName == nameof(RequeteDeTest.Valeur) &&
            e.ErrorMessage == "La valeur est obligatoire.");
    }

    [Fact]
    public async Task ToutesLesErreursDeValidationSontPresentesDansLException()
    {
        // Arrange — un seul validateur avec plusieurs règles toutes en échec
        var behavior = new ValidationBehavior<RequeteDeTest, ReponseDeTest>(
            [new ValidateurMultiReglesDeTest()]);

        RequestHandlerDelegate<ReponseDeTest> next =
            () => Task.FromResult(new ReponseDeTest(""));

        var requeteInvalide = new RequeteDeTest
        {
            Valeur = string.Empty,
            AutreValeur = string.Empty
        };

        // Act
        var exception = await Record.ExceptionAsync(
            () => behavior.Handle(requeteInvalide, next, CancellationToken.None));

        // Assert
        exception.Should().BeOfType<ValidationException>();
        var validationEx = (ValidationException)exception!;
        validationEx.Errors.Should().HaveCount(2);
    }

    [Fact]
    public async Task LesErreursDeMultiplesValidateursSontToutesAggregeesDansLException()
    {
        // Arrange — deux validateurs distincts, chacun échouant sur un champ différent
        var behavior = new ValidationBehavior<RequeteDeTest, ReponseDeTest>(
        [
            new ValidateurDeTest(),
            new ValidateurSupplementaireDeTest()
        ]);

        RequestHandlerDelegate<ReponseDeTest> next =
            () => Task.FromResult(new ReponseDeTest(""));

        var requeteInvalide = new RequeteDeTest
        {
            Valeur = string.Empty,
            AutreValeur = string.Empty
        };

        // Act
        var exception = await Record.ExceptionAsync(
            () => behavior.Handle(requeteInvalide, next, CancellationToken.None));

        // Assert
        exception.Should().BeOfType<ValidationException>();
        var validationEx = (ValidationException)exception!;
        validationEx.Errors.Should().HaveCount(2);
        validationEx.Errors.Should().Contain(e => e.PropertyName == nameof(RequeteDeTest.Valeur));
        validationEx.Errors.Should().Contain(e => e.PropertyName == nameof(RequeteDeTest.AutreValeur));
    }
}
