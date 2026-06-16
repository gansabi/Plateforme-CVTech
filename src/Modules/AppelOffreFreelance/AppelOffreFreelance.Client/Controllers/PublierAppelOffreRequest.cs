namespace AppelOffreFreelance.Client.Controllers;

public sealed record PublierAppelOffreRequest(
    Guid UtilisateurId,
    string Titre,
    string Description,
    string DomaineMetier,
    decimal TjmMinimum,
    decimal TjmMaximum,
    DateTime DateLimite);
