using ActualiteEtAbonnement.Domaine.Entites;
using ActualiteEtAbonnement.Domaine.ObjetsValeur;
using ActualiteEtAbonnement.Infrastructure.Persistance;
using AppelOffreFreelance.Domaine.Entites;
using AppelOffreFreelance.Infrastructure.Persistance;
using CatalogueEmploi.Domaine.Entites;
using CatalogueEmploi.Domaine.Enums;
using CatalogueEmploi.Infrastructure.Persistance;
using GestionIdentite.Domaine.Entites;
using GestionIdentite.Domaine.ObjetsValeur;
using GestionIdentite.Infrastructure.Persistance;
using GestionIdentite.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace PlateformeCVTech.Api;

/// <summary>
/// Données de démonstration créées uniquement en environnement Development.
/// Ne doit JAMAIS être appelé en Production.
/// </summary>
public static class DonneesDemonstration
{
    // IDs fixes pour permettre les références croisées
    public static readonly Guid AdminId = Guid.Parse("a0000000-0000-0000-0000-000000000001");
    public static readonly Guid CandidatId = Guid.Parse("c0000000-0000-0000-0000-000000000001");
    public static readonly Guid EntrepriseId = Guid.Parse("e0000000-0000-0000-0000-000000000001");

    public static void Initialiser(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var hasheur = new HasheurMotDePasse();
        var mdpHash = hasheur.Hacher("Demo2026!");

        InitialiserIdentite(scope, mdpHash);
        InitialiserCatalogue(scope);
        InitialiserFreelance(scope);
        InitialiserActualite(scope);
    }

    private static void InitialiserIdentite(IServiceScope scope, string mdpHash)
    {
        var db = scope.ServiceProvider.GetRequiredService<GestionIdentiteDbContext>();
        if (db.ComptesAdministrateurs.Any()) return;

        var admin = CompteAdministrateur.Creer(
            Email.Creer("admin@cvtech.fr"), "Admin CVTech", mdpHash);

        var candidat = CompteCandidat.CréerAvecMotDePasseHache(
            Email.Creer("candidat@demo.fr"), mdpHash);

        var entreprise = CompteEntreprise.Creer(
            Email.Creer("entreprise@techcorp.fr"), "TechCorp", mdpHash);

        // On force les IDs pour le seed
        SetId(admin, AdminId);
        SetId(candidat, CandidatId);
        SetId(entreprise, EntrepriseId);

        db.ComptesAdministrateurs.Add(admin);
        db.ComptesCandidats.Add(candidat);
        db.ComptesEntreprises.Add(entreprise);
        db.SaveChanges();
    }

    private static void InitialiserCatalogue(IServiceScope scope)
    {
        var db = scope.ServiceProvider.GetRequiredService<CatalogueEmploiDbContext>();
        if (db.AnnoncesEmploi.Any()) return;

        var annonce1 = AnnonceEmploi.Publier(EntrepriseId,
            "Développeur .NET Senior", "Mission d'architecture modulaire .NET 10 sur Azure.",
            TypeContrat.CDI, CatalogueEmploi.Domaine.ObjetsValeur.DomaineMetier.Creer("Cloud Azure"));

        var annonce2 = AnnonceEmploi.Publier(EntrepriseId,
            "Data Engineer", "Pipeline de données en temps réel avec Kafka et Spark.",
            TypeContrat.CDD, CatalogueEmploi.Domaine.ObjetsValeur.DomaineMetier.Creer("Data Science"));

        var annonce3 = AnnonceEmploi.Publier(EntrepriseId,
            "DevOps Engineer", "Infrastructure as Code, CI/CD, Kubernetes.",
            TypeContrat.CDI, CatalogueEmploi.Domaine.ObjetsValeur.DomaineMetier.Creer("DevOps"));

        db.AnnoncesEmploi.AddRange(annonce1, annonce2, annonce3);
        db.SaveChanges();

        // Candidatures
        var candidature1 = Candidature.Creer(CandidatId, annonce1.Id, "Très motivé par cette mission cloud.");
        var candidature2 = Candidature.Creer(CandidatId, annonce3.Id, null);
        db.Candidatures.AddRange(candidature1, candidature2);

        // CV
        var cv = CurriculumVitae.Creer(CandidatId,
            "Développeur Full Stack .NET",
            "5 ans d'expérience en architecture modulaire, Azure, React.",
            ["C#", ".NET 10", "Azure", "React", "Docker", "Kubernetes"]);
        db.CurriculumsVitae.Add(cv);

        db.SaveChanges();
    }

    private static void InitialiserFreelance(IServiceScope scope)
    {
        var db = scope.ServiceProvider.GetRequiredService<AppelOffreFreelanceDbContext>();
        if (db.AppelsOffre.Any()) return;

        var ao1 = AppelOffre.Publier(EntrepriseId,
            "Migration Cloud Azure", "Migrer l'infrastructure on-premise vers Azure.",
            AppelOffreFreelance.Domaine.ObjetsValeur.DomaineMetier.Creer("Cloud Azure"),
            AppelOffreFreelance.Domaine.ObjetsValeur.BaremeTJM.Creer(550, 850),
            DateTime.UtcNow.AddDays(45));

        var ao2 = AppelOffre.Publier(EntrepriseId,
            "Audit Cybersécurité", "Audit complet de la posture sécurité de l'entreprise.",
            AppelOffreFreelance.Domaine.ObjetsValeur.DomaineMetier.Creer("Cybersécurité"),
            AppelOffreFreelance.Domaine.ObjetsValeur.BaremeTJM.Creer(700, 1200),
            DateTime.UtcNow.AddDays(30));

        db.AppelsOffre.AddRange(ao1, ao2);
        db.SaveChanges();

        // Propositions
        var prop1 = PropositionFreelance.Creer(CandidatId, ao1.Id, 650, 30, "Agile Scrum avec sprints de 2 semaines");
        db.Propositions.Add(prop1);
        db.SaveChanges();
    }

    private static void InitialiserActualite(IServiceScope scope)
    {
        var db = scope.ServiceProvider.GetRequiredService<ActualiteEtAbonnementDbContext>();
        if (db.Articles.Any()) return;

        var domCloud = ActualiteEtAbonnement.Domaine.ObjetsValeur.DomaineMetier.Creer("Cloud Azure");
        var domDevOps = ActualiteEtAbonnement.Domaine.ObjetsValeur.DomaineMetier.Creer("DevOps");
        var domData = ActualiteEtAbonnement.Domaine.ObjetsValeur.DomaineMetier.Creer("Data Science");

        var a1 = ArticleActualite.Publier(AdminId,
            "Les tendances Cloud en 2026",
            "Le serverless et les architectures event-driven dominent les nouvelles migrations.",
            domCloud);
        var a2 = ArticleActualite.Publier(AdminId,
            "DevOps : l'essor de Platform Engineering",
            "Les équipes platform engineering remplacent les DevOps traditionnels dans les grandes organisations.",
            domDevOps);
        var a3 = ArticleActualite.Publier(AdminId,
            "Data Science et IA générative",
            "Les LLM transforment le métier de data scientist : de l'entraînement au fine-tuning.",
            domData);

        db.Articles.AddRange(a1, a2, a3);

        // Abonnement du candidat au domaine Cloud Azure
        var abonnement = Abonnement.Creer(CandidatId, domCloud);
        db.Abonnements.Add(abonnement);

        db.SaveChanges();
    }

    private static void SetId<T>(T entity, Guid id) where T : class
    {
        var prop = typeof(T).GetProperty("Id");
        prop?.SetValue(entity, id);
    }
}
