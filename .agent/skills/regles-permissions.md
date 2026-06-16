--- 
name: cvtech-permission-guard 
description: Vérifie que chaque cas d'usage contrôle les permissions 
avant l'action métier 
globs: "src/Modules/**/Application/Features/**/*.cs" 
---

# CONTEXTE

Trois rôles cohabitent : Candidat, Entreprise, Administrateur. Aucune
action métier ne doit s'exécuter sans avoir interrogé en amont le
contrat `IVerificateurPermission` exposé par le module
`GestionIdentite`.

# INSTRUCTIONS

[À COMPLÉTER PAR L'ÉTUDIANT : Imposer qu'un Handler refuse de
compiler/passer la revue si la première ligne n'est pas une vérification
de permission. Décrire comment l'IA doit générer une
`PermissionRefuseeException` métier en cas d'échec, et comment elle doit
traduire la matrice de permissions du README en code.]

## Vérification obligatoire des permissions

- Chaque Handler doit vérifier les permissions avant toute action métier.
- La vérification doit être effectuée dès le début de la méthode Handle.
- Aucun accès au Domaine, à l'Infrastructure, à un Repository ou à une source de données ne doit être réalisé avant cette vérification.
- Toutes les permissions doivent être vérifiées via le contrat public `IVerificateurPermission`.
- La première opération métier d'un Handler doit être une vérification de permission.

## Gestion des refus

- Si la permission n'est pas accordée, le Handler doit immédiatement lever une `PermissionRefuseeException`.
- Aucun traitement métier ne doit être exécuté après un refus.
- Le message de l'exception doit indiquer clairement l'action refusée.
- Une permission refusée doit interrompre immédiatement l'exécution du cas d'usage.

Exemple :

```csharp
if (!await _verificateurPermission.PossedePermissionAsync(
    utilisateurId,
    Permission.PostulerAnnonce))
{
    throw new PermissionRefuseeException(
        "L'utilisateur n'est pas autorisé à postuler à une annonce.");
}
```

## Traduction de la matrice des permissions

L'IA doit respecter la matrice métier suivante :

### Candidat

- Peut créer et modifier son Curriculum Vitae.
- Peut postuler à une annonce d'emploi.
- Peut soumettre une proposition sur un appel d'offre.
- Peut s'abonner à un domaine métier.

### Entreprise

- Peut publier une annonce d'emploi.
- Peut publier un appel d'offre.
- Peut consulter uniquement les candidatures reçues sur ses propres annonces.
- Peut consulter uniquement les propositions reçues sur ses propres appels d'offre.
- Peut s'abonner à un domaine métier.

### Administrateur

- Peut publier un article d'actualité.
- Peut modérer ou supprimer une annonce.
- Peut modérer ou supprimer un appel d'offre.
- Peut bloquer ou réactiver un compte.
- Peut gérer le référentiel des domaines métier.

### Visiteur anonyme

- Peut consulter les annonces d'emploi.
- Peut consulter les appels d'offre.
- Peut consulter le flux RSS d'actualité.
- Ne peut exécuter aucune action nécessitant une authentification.

## Interdictions

- Aucun Handler ne doit contourner `IVerificateurPermission`.
- Aucun rôle ne doit être vérifié directement dans un Handler.
- Aucun module ne doit implémenter son propre système de permissions.
- Aucun traitement métier ne doit être exécuté avant la vérification des permissions.
- Aucun accès à une ressource privée ne doit être autorisé sans vérification de propriété ou de permission.
- Les permissions doivent être vérifiées côté serveur même si l'interface utilisateur masque déjà l'action.
