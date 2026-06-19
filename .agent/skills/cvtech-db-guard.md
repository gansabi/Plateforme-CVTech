---
name: cvtech-db-guard
description: Force le respect des règles de persistance et de gestion de base de données du projet Plateforme-CVTech
globs: "backend/src/**/*.cs"
---

# CONTEXTE

Le projet Plateforme-CVTech utilise Entity Framework Core comme ORM principal. Chaque module possède son propre DbContext isolé. La base de données est séparée logiquement par module conformément au principe du monolithe modulaire.

# STRATÉGIE DE BASE DE DONNÉES

## Environnements

| Environnement | Provider | Justification |
|---|---|---|
| Tests unitaires | InMemory | Rapide, isolé, pas de dépendance externe |
| Développement local | SQLite | Zéro infrastructure, fonctionne sur Linux/Mac/Windows |
| Production | SQL Server (Azure SQL) | Imposé par le README, scalable |

## Configuration

- Les chaînes de connexion sont définies dans `appsettings.{Environment}.json`
- Le provider EF Core est sélectionné dans le ModuleLoader ou le Program.cs selon l'environnement
- Les fichiers .db SQLite sont créés dans le dossier `backend/data/` (ignoré par git)

## Migrations EF Core

- Une migration par module, nommée `Initial`
- Les migrations sont stockées dans le projet Infrastructure de chaque module
- Commande de génération : `dotnet ef migrations add Initial --project <Infrastructure> --startup-project <Host>`
- Commande d'application : `dotnet ef database update --project <Infrastructure> --startup-project <Host>`
- Les migrations doivent être appliquées automatiquement au démarrage en développement

# RÈGLES

## Isolation des modules

- Chaque module possède son propre DbContext
- Chaque module possède sa propre base de données (fichier SQLite séparé ou schéma SQL Server séparé)
- Aucun DbContext ne doit référencer des entités d'un autre module
- Aucun accès cross-database entre modules
- La communication entre modules passe exclusivement par le bus d'événements ou les contrats publics

## DbContext

- Un seul DbContext par module
- Le DbContext est défini dans la couche Infrastructure
- Le DbContext est enregistré dans le ModuleLoader
- Le DbContext applique les configurations depuis son propre assembly via `ApplyConfigurationsFromAssembly`
- Le DbContext ne doit jamais être injecté dans la couche Application ou Domaine

## Configurations EF Core

- Une classe de configuration par entité (IEntityTypeConfiguration<T>)
- Les configurations sont dans le dossier `Persistance/Configurations/`
- Les Value Objects utilisent `OwnsOne` pour le mapping
- Les enums utilisent `HasConversion<string>()` pour le stockage lisible
- Les index uniques sont définis sur les contraintes métier (ex: un CV par candidat)

## Repositories

- Les repositories sont dans `Persistance/Repositories/`
- Les repositories implémentent les contrats définis dans le Domaine
- Les repositories utilisent uniquement le DbContext de leur module
- Les repositories sont enregistrés en Scoped dans le ModuleLoader

## Conventions de nommage

- Tables au pluriel en français : `AnnoncesEmploi`, `Candidatures`, `Abonnements`
- Colonnes des Value Objects aplaties : `DomaineMetier` (pas de table séparée)
- Clés primaires : `Id` (Guid)
- Index composites nommés par convention EF Core

## Données de seed (optionnel)

- Les données de démonstration peuvent être ajoutées via `HasData` dans les configurations
- Ou via un script de seed appelé au démarrage en développement
- Les données de seed ne doivent jamais être appliquées en production

# INTERDICTIONS

- Aucun accès direct à la base de données d'un autre module
- Aucun DbContext dans le Domaine, l'Application ou la couche Client
- Aucun SQL brut sauf cas exceptionnel documenté
- Aucun Entity Framework dans la couche Domaine
- Aucune migration générée sans vérification de l'impact sur les données existantes
- Aucun fichier .db commité dans le dépôt git (ajouter `*.db` au .gitignore)
- Aucune chaîne de connexion en dur dans le code

# STRUCTURE DES FICHIERS

```
backend/
├── data/                          (ignoré par git — fichiers SQLite)
│   ├── GestionIdentite.db
│   ├── CatalogueEmploi.db
│   ├── AppelOffreFreelance.db
│   └── ActualiteEtAbonnement.db
└── src/
    ├── Host/
    │   ├── appsettings.json              (connexions prod SQL Server)
    │   └── appsettings.Development.json  (connexions dev SQLite)
    └── Modules/
        └── <Module>/
            └── <Module>.Infrastructure/
                ├── Persistance/
                │   ├── <Module>DbContext.cs
                │   ├── Configurations/
                │   │   └── <Entite>Configuration.cs
                │   └── Repositories/
                │       └── <Entite>Repository.cs
                └── Migrations/           (générées par EF Core)
```

# APPSETTINGS DÉVELOPPEMENT (SQLite)

```json
{
  "ConnectionStrings": {
    "GestionIdentite": "Data Source=../../data/GestionIdentite.db",
    "CatalogueEmploi": "Data Source=../../data/CatalogueEmploi.db",
    "AppelOffreFreelance": "Data Source=../../data/AppelOffreFreelance.db",
    "ActualiteEtAbonnement": "Data Source=../../data/ActualiteEtAbonnement.db"
  }
}
```

# APPSETTINGS PRODUCTION (Azure SQL)

```json
{
  "ConnectionStrings": {
    "GestionIdentite": "Server=tcp:<serveur>.database.windows.net;Database=CVTech_GestionIdentite;...",
    "CatalogueEmploi": "Server=tcp:<serveur>.database.windows.net;Database=CVTech_CatalogueEmploi;...",
    "AppelOffreFreelance": "Server=tcp:<serveur>.database.windows.net;Database=CVTech_AppelOffreFreelance;...",
    "ActualiteEtAbonnement": "Server=tcp:<serveur>.database.windows.net;Database=CVTech_ActualiteEtAbonnement;..."
  }
}
```
