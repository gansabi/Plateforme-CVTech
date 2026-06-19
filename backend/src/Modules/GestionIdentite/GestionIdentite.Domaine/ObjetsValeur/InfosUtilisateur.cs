using GestionIdentite.Domaine.Enums;

namespace GestionIdentite.Domaine.ObjetsValeur;

/// <summary>
/// Read-model léger retourné par IRegistreUtilisateurs.
/// Centralise les données nécessaires à l'authentification et à la vérification des permissions
/// sans exposer l'agrégat complet.
/// </summary>
public sealed record InfosUtilisateur(
    Guid Id,
    string Email,
    Role Role,
    string MotDePasseHache,
    bool EstBloque);
