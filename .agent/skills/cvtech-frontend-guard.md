---

name: cvtech-frontend-guard
description: Force le respect de l'architecture frontend React TypeScript de la plateforme CVTech
globs: "frontend/src/**/*.{ts,tsx}"
-----------------------------------

# CONTEXTE

Le frontend de la plateforme CVTech est développé avec React, TypeScript, Vite, React Router, Axios et Tailwind CSS.

Le frontend consomme exclusivement les APIs exposées par les modules backend :

* GestionIdentite
* CatalogueEmploi
* AppelOffreFreelance
* ActualiteEtAbonnement

Le frontend doit refléter l'architecture modulaire du backend.

# ARCHITECTURE

Le frontend est organisé par modules métier.

frontend/src/

├── app/
│   ├── router/
│   ├── providers/
│   └── layouts/
│
├── shared/
│   ├── components/
│   ├── hooks/
│   ├── api/
│   └── types/
│
├── modules/
│   ├── gestion-identite/
│   ├── catalogue-emploi/
│   ├── appel-offre-freelance/
│   └── actualite-abonnement/
│
├── App.tsx
└── main.tsx

Chaque module possède ses propres :

* pages
* composants
* services API
* hooks
* types

Les composants réutilisables sont placés dans shared/.

# ROUTAGE

* Utiliser React Router.
* Toutes les routes sont centralisées dans app/router.
* Les routes protégées utilisent ProtectedRoute.
* Les routes doivent être organisées par module métier.

# APPELS API

* Utiliser Axios.
* Une seule instance Axios doit être configurée.
* Les appels API sont isolés dans les services du module concerné.
* Aucun composant React ne doit effectuer directement un appel HTTP.
* Aucun fetch dans les composants.

# AUTHENTIFICATION

* Utiliser React Context.
* Le contexte AuthContext est responsable de la session utilisateur.
* Les informations utilisateur proviennent du backend.
* Les permissions affichées dans l'interface proviennent du backend.
* Le frontend ne remplace jamais les contrôles de sécurité du backend.

# TYPESCRIPT

* Utiliser TypeScript strict.
* Le type any est interdit.
* Tous les DTO doivent refléter les contrats exposés par les APIs backend.
* Les types sont définis dans chaque module métier.

# STATE MANAGEMENT

* Utiliser React Context pour l'authentification.
* Utiliser TanStack Query pour la gestion des données serveur.
* Redux est interdit.

# INTERFACE UTILISATEUR

* Utiliser Tailwind CSS.
* Utiliser des composants fonctionnels.
* Les composants doivent être réutilisables.
* Aucun composant ne doit dépasser 300 lignes.
* La logique métier doit rester dans les hooks et services.

# PERMISSIONS

* Les écrans visibles dépendent des permissions retournées par le backend.
* Les boutons d'action doivent être masqués lorsque l'utilisateur ne possède pas la permission.
* Les permissions ne doivent jamais être codées en dur dans l'interface.

# INTERDICTIONS

* Aucun appel HTTP directement dans un composant.
* Aucun type any.
* Aucun composant de plus de 300 lignes.
* Aucun stockage de logique métier dans les pages.
* Aucun accès direct à localStorage en dehors du module d'authentification.
* Aucun contournement des permissions backend.
