---
name: cvtech-tdd-expert
description: Force l'écriture des tests de spécification avant le code
métier
globs: "tests/*_/_.cs"

---

# PROTOCOLE TDD

[À COMPLÉTER PAR L'ÉTUDIANT : Définir comment l'IA doit structurer les
tests avec xUnit et FluentAssertions, imposer le nommage des méthodes de
test décrivant une règle métier en français (ex :
`UnCandidatBloquéNePeutPasPostuler`), et interdire la génération du code
de production avant la validation du test (Red → Green → Refactor).]

## Principe général

- Toute nouvelle fonctionnalité doit commencer par l'écriture d'un test.
- Aucun code de production ne doit être généré avant l'existence d'un test décrivant le comportement attendu.
- Le cycle TDD doit toujours suivre l'ordre : Red → Green → Refactor.
- Les tests constituent la spécification fonctionnelle du système.
- Le comportement métier doit être décrit par les tests avant d'être implémenté.

## Phase Red

- Écrire un test décrivant une règle métier.
- Vérifier que le test échoue.
- Le test doit échouer pour la bonne raison.
- Aucun code métier ne doit être généré durant cette phase.

## Phase Green

- Écrire uniquement le minimum de code nécessaire pour faire passer le test.
- Ne pas anticiper des fonctionnalités futures.
- Ne pas ajouter de comportement non couvert par un test.

## Phase Refactor

- Refactoriser uniquement lorsque tous les tests sont au vert.
- Préserver le comportement métier validé.
- Supprimer les duplications.
- Améliorer la lisibilité et la maintenabilité du code.

## Structure des tests

- Utiliser xUnit.
- Utiliser FluentAssertions pour les assertions.
- Respecter le modèle Arrange / Act / Assert.
- Un test doit vérifier une seule règle métier.
- Les tests doivent être lisibles et explicites.
- Les dépendances externes doivent être simulées à l'aide de mocks ou de doubles de test.

## Convention de nommage

- Les noms de tests doivent être rédigés en français.
- Les noms doivent décrire explicitement une règle métier.
- Les noms doivent être compréhensibles par un expert métier.

Exemples :

- UnCandidatBloqueNePeutPasPostuler
- UneEntreprisePeutPublierUneAnnonce
- UnAdministrateurPeutBloquerUnCompte
- UneAnnoncePublieeGenereUneNotification
- UnUtilisateurNonAutoriseRecoitUnePermissionRefuseeException

## Priorité des tests

- Les tests du Domaine doivent être écrits avant les tests de l'Application.
- Les tests de l'Application doivent être écrits avant les tests des Controllers.
- Les tests de l'Infrastructure doivent rester limités aux composants techniques.
- Les règles métier doivent être validées au niveau du Domaine avant toute implémentation technique.

## Tests métier

- Les tests doivent être centrés sur le comportement métier et non sur les détails techniques.
- Les règles du Domaine doivent être couvertes avant les Controllers ou l'Infrastructure.
- Toute nouvelle règle métier doit être accompagnée d'au moins un test.

## Couverture des permissions

Toute fonctionnalité protégée doit posséder au minimum :

- un test validant l'autorisation de l'action ;
- un test validant le refus de l'action ;
- un test validant la levée de PermissionRefuseeException.

Exemples :

- UnCandidatPeutPostulerAUneAnnonce
- UneEntrepriseNePeutPasPostulerAUneAnnonce
- UnePermissionRefuseeEstLeveeLorsqueLUtilisateurNestPasAutorise

## Couverture des événements métier

Toute fonctionnalité publiant un événement métier doit posséder un test vérifiant :

- que l'événement est publié ;
- que l'événement contient les données attendues ;
- que le comportement métier est exécuté avant la publication de l'événement.

Exemples :

- UneAnnoncePublieeGenereLEvenementAnnoncePubliee
- UnAppelOffrePublieGenereLEvenementAppelOffrePublie
- UneAnnoncePublieeDeclencheUneNotification

## Interdictions

- Ne jamais générer une fonctionnalité sans générer son test associé.
- Ne jamais considérer une fonctionnalité terminée si le test n'existe pas.
- Ne jamais écrire un test dont le nom ne décrit pas clairement une règle métier.
- Ne jamais ignorer un test en échec.
- Ne jamais désactiver un test pour faire passer la build.
- Ne jamais implémenter plusieurs règles métier dans un seul test.
- Ne jamais écrire de logique métier sans couverture de test.
