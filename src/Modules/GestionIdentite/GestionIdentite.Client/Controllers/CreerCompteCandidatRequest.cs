namespace GestionIdentite.Client.Controllers;

/// <summary>
/// DTO HTTP entrant — traduit par le Controller en CreerCompteCandidatCommand.
/// N'appartient qu'à la couche Client : l'Application ne le connaît pas.
/// </summary>
public sealed record CreerCompteCandidatRequest(string Email, string MotDePasse);
