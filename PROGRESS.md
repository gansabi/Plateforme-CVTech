# État du projet — Plateforme-CVTech

## Récapitulatif global

- 4 modules back-end implémentés
- 324 tests passent (0 échec)
- Architecture monolithe modulaire .NET 10 respectée
- Communication inter-modules via événements MediatR et contrats publics
- TDD appliqué sur toutes les couches

---

## Module A — GestionIdentite ✅ TERMINÉ

### Couches

| Couche | Statut |
|---|---|
| Domaine | ✅ |
| Application | ✅ |
| Infrastructure | ✅ |
| Client | ✅ |
| Loader | ✅ |

### Concepts métier

- ProfilCandidat (CompteCandidat)
- ProfilEntreprise (CompteEntreprise)
- Administrateur (CompteAdministrateur)
- RoleUtilisateur (enum Role)
- MatricePermissions (MatricePermissions.cs)
- RevendicationPermission (RevendicationPermission.cs)
- Email, MotDePasse (Value Objects)

### Cas d'usage

- CreerCompteCandidat
- CreerCompteEntreprise
- CreerCompteAdministrateur
- ConnecterUtilisateur
- BloquerCompte
- ReactiverCompte

### Contrats publics exposés

- IVerificateurPermission (consommé par les 3 autres modules)
- Permission (enum — 18 permissions)

### Endpoints

- POST api/identite/comptes-candidats
- POST api/identite/comptes-entreprises
- POST api/identite/administrateurs
- POST api/identite/auth/connexion
- POST api/identite/admin/bloquer
- POST api/identite/admin/reactiver

### Tests

- Domaine : 86
- Application : 65
- Infrastructure : 19
- Client : 21
- **Total : 191**

---

## Module B — CatalogueEmploi ✅ TERMINÉ

### Couches

| Couche | Statut |
|---|---|
| Domaine | ✅ |
| Application | ✅ |
| Infrastructure | ✅ |
| Client | ✅ |
| Loader | ✅ |

### Concepts métier

- AnnonceEmploi (entité)
- CurriculumVitae (entité)
- Candidature (entité)
- TypeContrat (enum : CDI, CDD, Stage, Alternance, Apprentissage)
- DomaineMetier (Value Object)

### Événement émis

- AnnoncePubliee (consommé par ActualiteEtAbonnement)

### Cas d'usage

- PublierAnnonce (Entreprise, permission PublierAnnonce)
- ConsulterAnnonces (public, anonyme)
- PostulerAnnonce (Candidat, permission PostulerAnnonce)
- CreerCurriculumVitae (Candidat, permission CreerCurriculumVitae)
- ModifierCurriculumVitae (Candidat, permission ModifierCurriculumVitae)
- ModererAnnonce (Admin, permission ModererAnnonce)
- SupprimerAnnonce (Admin, permission SupprimerAnnonce)
- ConsulterCandidaturesRecues (Entreprise, permission ConsulterCandidaturesRecues)

### Endpoints

- GET  api/catalogue/annonces (publique)
- POST api/catalogue/annonces (entreprise)
- POST api/catalogue/candidatures (candidat)
- GET  api/catalogue/candidatures?utilisateurId=&annonceId= (entreprise)
- POST api/catalogue/cv (candidat)
- PUT  api/catalogue/cv (candidat)
- POST api/catalogue/moderation/moderer (admin)
- POST api/catalogue/moderation/supprimer (admin)

### Infrastructure

- CatalogueEmploiDbContext (3 DbSets)
- AnnonceEmploiRepository
- CandidatureRepository
- CurriculumVitaeRepository
- BusEvenements (MediatR)
- Configurations EF Core : AnnonceEmploiConfiguration, CandidatureConfiguration, CurriculumVitaeConfiguration
- CatalogueEmploiModuleLoader

### Tests

- Domaine : 21
- Application : 31
- Infrastructure : 9
- Client : 0 (projet existant mais vide — pas de régression)
- **Total : 61**

---

## Module C — AppelOffreFreelance ✅ TERMINÉ

### Couches

| Couche | Statut |
|---|---|
| Domaine | ✅ |
| Application | ✅ |
| Infrastructure | ✅ |
| Client | ✅ |
| Loader | ✅ |

### Concepts métier

- AppelOffre (entité)
- PropositionFreelance (entité)
- DomaineMetier (Value Object)
- BaremeTJM (Value Object — minimum/maximum)
- StatutAppelOffre (enum : Ouvert, Ferme, Attribue)

### Événement émis

- AppelOffrePublie (consommé par ActualiteEtAbonnement)

### Cas d'usage

- PublierAppelOffre (Entreprise, permission PublierAppelOffre)
- ConsulterAppelsOffre (public, anonyme)
- SoumettreProposition (Candidat, permission SoumettrePropositon)
- ConsulterPropositionsRecues (Entreprise, permission ConsulterPropositionsRecues)
- ModererAppelOffre (Admin, permission ModererAppelOffre)
- SupprimerAppelOffre (Admin, permission SupprimerAppelOffre)

### Endpoints

- GET  api/freelance/appels-offre (publique)
- POST api/freelance/appels-offre (entreprise)
- POST api/freelance/propositions (candidat)
- GET  api/freelance/propositions?utilisateurId=&appelOffreId= (entreprise)
- POST api/freelance/moderation/moderer (admin)
- POST api/freelance/moderation/supprimer (admin)

### Infrastructure

- AppelOffreFreelanceDbContext (2 DbSets)
- AppelOffreRepository
- PropositionFreelanceRepository
- BusEvenements (MediatR)
- Configurations EF Core : AppelOffreConfiguration, PropositionFreelanceConfiguration
- AppelOffreFreelanceModuleLoader

### Tests

- Domaine : 19
- Application : 13
- Infrastructure : 6
- Client : 3
- **Total : 41**

---

## Module D — ActualiteEtAbonnement ✅ TERMINÉ

### Couches

| Couche | Statut |
|---|---|
| Domaine | ✅ |
| Application | ✅ |
| Infrastructure | ✅ |
| Client | ✅ |
| Loader | ✅ |

### Concepts métier

- ArticleActualite (entité)
- Abonnement (entité)
- Notification (entité)
- DomaineMetier (Value Object)
- CanalDiffusion (enum : Email, InApp)

### Événements consommés

- AnnoncePubliee (depuis CatalogueEmploi) → génère notifications
- AppelOffrePublie (depuis AppelOffreFreelance) → génère notifications

### Cas d'usage

- PublierArticle (Admin, permission PublierArticleActualite)
- ConsulterFilRss (public, anonyme — RSS 2.0)
- SAbonnerDomaine (Candidat/Entreprise, permission SAbonnerDomaine)
- NotifierAbonnesAnnonce (consommateur d'événement interne, pas de permission)
- NotifierAbonnesAppelOffre (consommateur d'événement interne, pas de permission)

### Endpoints

- GET  /feed/rss?domaine=... (RSS 2.0 public, articles éditoriaux uniquement)
- POST api/actualite/articles (admin)
- POST api/actualite/abonnements (candidat/entreprise)

### Infrastructure

- ActualiteEtAbonnementDbContext (3 DbSets)
- ArticleActualiteRepository
- AbonnementRepository
- NotificationRepository
- ServiceNotificationConsole (simule envoi email en console)
- Configurations EF Core : ArticleActualiteConfiguration, AbonnementConfiguration, NotificationConfiguration
- ActualiteEtAbonnementModuleLoader

### Tests

- Domaine : 15
- Application : 7
- Infrastructure : 5
- Client : 4
- **Total : 31**

---

## Audit de conformité README3.md

### Fonctionnalités back-end

| Fonctionnalité README | Statut | Module |
|---|---|---|
| Inscription candidat | ✅ | A |
| Inscription entreprise | ✅ | A |
| Connexion utilisateur | ✅ | A |
| Blocage/réactivation compte | ✅ | A |
| Matrice permissions (18 permissions) | ✅ | A |
| IVerificateurPermission contrat public | ✅ | A |
| Publication annonce emploi | ✅ | B |
| Consultation annonces (publique) | ✅ | B |
| Création/modification CV | ✅ | B |
| Candidature à une annonce | ✅ | B |
| Consultation candidatures reçues | ✅ | B |
| Modération/suppression annonce | ✅ | B |
| Événement AnnoncePubliee | ✅ | B |
| Publication appel d'offre | ✅ | C |
| Consultation appels d'offre (publique) | ✅ | C |
| Soumission proposition freelance | ✅ | C |
| Consultation propositions reçues | ✅ | C |
| Modération/suppression appel d'offre | ✅ | C |
| Événement AppelOffrePublie | ✅ | C |
| Publication article actualité | ✅ | D |
| Endpoint RSS 2.0 public (/feed/rss) | ✅ | D |
| Filtrage RSS par domaine | ✅ | D |
| RSS contient UNIQUEMENT articles éditoriaux | ✅ | D |
| Abonnement à un domaine métier | ✅ | D |
| Notification à la publication (AnnoncePubliee) | ✅ | D |
| Notification à la publication (AppelOffrePublie) | ✅ | D |
| Au moins 1 canal de notification fonctionnel | ✅ | D (console/email) |

### Éléments manquants (non back-end ou non critiques)

| Élément README | Statut | Commentaire |
|---|---|---|
| Front-end (React ou Blazor) | ❌ Non commencé | Livrable attendu |
| Scripts migration EF Core / Azure SQL | ❌ À générer | `dotnet ef migrations add` à exécuter |
| Fichier README.md avec diagramme Mermaid | ❌ À rédiger | Guide de démarrage |
| Sélection lauréat (AppelOffre) | ⚠️ Partiel | Statut Attribue existe mais pas de cas d'usage dédié |
| Gestion référentiel domaines métier (Admin) | ⚠️ Non implémenté | Permission existe, pas de CRUD dédié |
| Tests Client CatalogueEmploi | ⚠️ Vide | Projet existe mais pas de tests |

### Architecture et Skills

| Règle | Respectée |
|---|---|
| 5 couches par module | ✅ |
| Vertical Slices (1 dossier/feature) | ✅ |
| Domaine 100% français | ✅ |
| Technique en anglais (Controller, Handler, Repository) | ✅ |
| Permission vérifiée en 1ère ligne du Handle | ✅ |
| PermissionRefuseeException levée si refus | ✅ |
| Communication inter-modules via événements/contrats | ✅ |
| Aucun accès direct à la DB d'un autre module | ✅ |
| Aucun DbContext dans Domaine ou Client | ✅ |
| Tests TDD nommés en français | ✅ |
| xUnit + FluentAssertions + Moq | ✅ |
| ModuleLoader centralise le DI | ✅ |

---

## Prochaines actions prioritaires

1. Générer les migrations EF Core (`dotnet ef migrations add Initial`)
2. Implémenter le front-end (React ou Blazor)
3. Rédiger le README.md final avec diagramme Mermaid
4. Ajouter le cas d'usage GererReferentielDomaineMetier (Admin)
5. Ajouter les tests Client pour CatalogueEmploi
6. (Optionnel) Ajouter le cas d'usage SelectionnerLaureat pour AppelOffre
