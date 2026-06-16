using FluentAssertions;
using GestionIdentite.Domaine.Enums;
using GestionIdentite.Domaine.ObjetsValeur;
using GestionIdentite.Infrastructure.Persistance;
using GestionIdentite.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GestionIdentite.Infrastructure.Tests.Services;

public sealed class VerificateurPermissionTests
{
    private static GestionIdentiteDbContext CreerContexte(string nom)
        => new(new DbContextOptionsBuilder<GestionIdentiteDbContext>()
            .UseInMemoryDatabase(nom)
            .Options);

    [Fact]
    public async Task UnCandidatPossedePermissionPostulerAnnonce()
    {
        var nom = Guid.NewGuid().ToString();
        var ctxEcriture = CreerContexte(nom);
        var email = Email.Creer("candidat@exemple.fr");
        var compte = Domaine.Entites.CompteCandidat.CréerAvecMotDePasseHache(email, "hash");
        ctxEcriture.ComptesCandidats.Add(compte);
        await ctxEcriture.SaveChangesAsync();

        var ctxLecture = CreerContexte(nom);
        var verificateur = new VerificateurPermission(ctxLecture);

        var resultat = await verificateur.PossedePermissionAsync(compte.Id, Permission.PostulerAnnonce);

        resultat.Should().BeTrue();
    }

    [Fact]
    public async Task UnCandidatNePossedePasBloquerCompte()
    {
        var nom = Guid.NewGuid().ToString();
        var ctxEcriture = CreerContexte(nom);
        var email = Email.Creer("candidat@exemple.fr");
        var compte = Domaine.Entites.CompteCandidat.CréerAvecMotDePasseHache(email, "hash");
        ctxEcriture.ComptesCandidats.Add(compte);
        await ctxEcriture.SaveChangesAsync();

        var ctxLecture = CreerContexte(nom);
        var verificateur = new VerificateurPermission(ctxLecture);

        var resultat = await verificateur.PossedePermissionAsync(compte.Id, Permission.BloquerCompte);

        resultat.Should().BeFalse();
    }

    [Fact]
    public async Task UneEntreprisePossedePermissionPublierAnnonce()
    {
        var nom = Guid.NewGuid().ToString();
        var ctxEcriture = CreerContexte(nom);
        var email = Email.Creer("entreprise@exemple.fr");
        var compte = Domaine.Entites.CompteEntreprise.Creer(email, "TechCorp", "hash");
        ctxEcriture.ComptesEntreprises.Add(compte);
        await ctxEcriture.SaveChangesAsync();

        var ctxLecture = CreerContexte(nom);
        var verificateur = new VerificateurPermission(ctxLecture);

        var resultat = await verificateur.PossedePermissionAsync(compte.Id, Permission.PublierAnnonce);

        resultat.Should().BeTrue();
    }

    [Fact]
    public async Task UnAdministrateurPossedePermissionBloquerCompte()
    {
        var nom = Guid.NewGuid().ToString();
        var ctxEcriture = CreerContexte(nom);
        var email = Email.Creer("admin@exemple.fr");
        var compte = Domaine.Entites.CompteAdministrateur.Creer(email, "Alice Martin", "hash");
        ctxEcriture.ComptesAdministrateurs.Add(compte);
        await ctxEcriture.SaveChangesAsync();

        var ctxLecture = CreerContexte(nom);
        var verificateur = new VerificateurPermission(ctxLecture);

        var resultat = await verificateur.PossedePermissionAsync(compte.Id, Permission.BloquerCompte);

        resultat.Should().BeTrue();
    }

    [Fact]
    public async Task UnUtilisateurInexistantNePossedeAucunePermission()
    {
        var ctx = CreerContexte(Guid.NewGuid().ToString());
        var verificateur = new VerificateurPermission(ctx);

        var resultat = await verificateur.PossedePermissionAsync(Guid.NewGuid(), Permission.PostulerAnnonce);

        resultat.Should().BeFalse();
    }
}
