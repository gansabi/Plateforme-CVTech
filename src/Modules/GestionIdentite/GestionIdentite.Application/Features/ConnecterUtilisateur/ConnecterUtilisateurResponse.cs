using GestionIdentite.Domaine.Enums;

namespace GestionIdentite.Application.Features.ConnecterUtilisateur;

public sealed record ConnecterUtilisateurResponse(
    Guid UtilisateurId,
    string Email,
    Role Role);
