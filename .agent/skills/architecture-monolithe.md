--- 
name: cvtech-architecture-guard 
description: Force le respect de la structure des couches du projet 
Plateforme-CVTech 
globs: "src/Modules/**/*.cs" 
---

# CONTEXTE

Nous sommes en 2026. Le projet utilise .NET 10. Le code doit refléter le langage métier en français pour les couches Domain et Application, tout en gardant les standards industriels (anglais) pour l'infrastructure et la technique (Controllers, Handlers, Infrastructure, ModuleLoader).

Le projet est un monolithe modulaire composé de quatre modules métier :

- GestionIdentite
- CatalogueEmploi
- AppelOffreFreelance
- ActualiteEtAbonnement

Chaque module est une boîte noire indépendante. La communication entre modules s'effectue uniquement via des contrats publics ou un bus d'événements interne.

# INSTRUCTIONS

## Architecture générale

- Chaque module doit respecter la structure Client / Application / Domaine / Infrastructure / Loader.
- Chaque module doit être considéré comme autonome.
- Les modules ne doivent jamais accéder directement aux classes internes d'un autre module.
- Les modules communiquent uniquement via des contrats publics ou des événements métier.
- Les références directes entre dossiers de modules différents sont interdites.
- Toute nouvelle fonctionnalité doit être intégrée dans le module métier concerné.

## Dépendances autorisées

- La couche Client dépend uniquement de la couche Application.
- La couche Application dépend uniquement du Domaine et des contrats publics nécessaires.
- La couche Domaine ne dépend d'aucune autre couche.
- La couche Infrastructure dépend du Domaine et de l'Application pour implémenter leurs contrats.
- La couche Loader dépend de toutes les couches du module afin de configurer l'injection de dépendances.
- Les dépendances inverses sont interdites.
- Une couche ne doit jamais dépendre d'une couche plus externe.

## Couche Client

- La couche Client est la seule porte d'entrée du module.
- Les Controllers exposent les endpoints API et effectuent uniquement le mapping des données.
- Aucun traitement métier ne doit être implémenté dans les Controllers.
- Les Controllers délèguent systématiquement l'exécution à la couche Application via MediatR.

## Couche Application

- La couche Application contient les cas d'usage sous forme de Vertical Slices.
- Chaque fonctionnalité possède son propre dossier.
- Chaque fonctionnalité doit contenir au minimum Command ou Query, Handler et Validator.
- Les Handlers orchestrent le traitement mais ne portent pas les règles métier complexes.
- Les Handlers utilisent MediatR.
- Les Handlers ne doivent jamais accéder directement à la base de données.

## Couche Domaine

- La couche Domaine contient les entités, agrégats, objets de valeur et exceptions métier.
- Les concepts métier doivent être nommés en français.
- Les règles métier doivent être implémentées dans les entités et agrégats.
- Les Value Objects doivent être privilégiés pour représenter les concepts métier sans identité.
- Les invariants métier doivent être protégés par le Domaine.
- Les exceptions métier doivent être définies dans le Domaine.
- Le Domaine ne dépend d'aucune technologie externe.

### Interdictions dans le Domaine

- DbContext interdit.
- Entity Framework interdit.
- Dapper interdit.
- HttpClient interdit.
- ILogger interdit.
- Toute dépendance d'infrastructure est interdite.

## Couche Infrastructure

- La couche Infrastructure contient les détails techniques.
- Elle contient la persistance, les accès aux services externes et les implémentations techniques.
- Entity Framework Core et Dapper sont autorisés uniquement dans cette couche.
- L'Infrastructure implémente les contrats définis dans les couches supérieures.

## Loader

- Chaque module possède un ModuleLoader.
- Toute configuration d'injection de dépendances doit être centralisée dans le ModuleLoader.
- Aucun enregistrement de dépendance ne doit être effectué ailleurs.

## Gestion des permissions

- Le module GestionIdentite est le seul propriétaire des rôles et permissions.
- Les autres modules ne doivent jamais accéder directement aux données d'identité.
- Les autres modules utilisent uniquement les contrats publics exposés par GestionIdentite.
- Toute vérification d'autorisation doit passer par ces contrats publics.

## Langage métier

- Les concepts métier du Domaine et de l'Application doivent être nommés en français.
- Les entités, agrégats, objets de valeur, commandes, requêtes et exceptions métier utilisent le vocabulaire métier du projet.
- Les éléments techniques peuvent rester en anglais : Controller, Handler, Repository, ModuleLoader, DbContext, EventBus.

## Événements métier

- Les événements doivent être immuables.
- Les événements doivent représenter un fait métier déjà réalisé.
- Les événements transportent uniquement les données nécessaires à leur traitement.
- Les événements ne doivent jamais contenir de DbContext, Repository ou dépendance technique.
- Les consommateurs d'événements ne doivent jamais modifier directement l'état interne d'un autre module.

### Événements connus

- CatalogueEmploi émet AnnoncePubliee.
- AppelOffreFreelance émet AppelOffrePublie.
- ActualiteEtAbonnement consomme ces événements afin de générer les notifications.

## Structure d'une Vertical Slice

Chaque fonctionnalité doit être organisée dans son propre dossier :

Features/
└── PublierAnnonce/
├── PublierAnnonceCommand.cs
├── PublierAnnonceHandler.cs
├── PublierAnnonceValidator.cs
└── PublierAnnonceResponse.cs

- Les fonctionnalités ne doivent jamais être regroupées par type technique.
- Chaque Vertical Slice doit contenir tous les éléments nécessaires à son cas d'usage.
- Une Vertical Slice doit être autonome et limiter les dépendances avec les autres fonctionnalités.

## Règles d'interdiction globales

- Aucun accès direct à la base de données d'un autre module.
- Aucun DbContext dans le Domaine ou la couche Client.
- Aucun Handler ne doit appeler directement l'Infrastructure d'un autre module.
- Aucun Controller ne doit contenir de logique métier.
- Aucun module ne doit contourner les contrats publics ou le bus d'événements.
- Aucune communication directe entre modules en dehors des contrats publics et des événements métier.
