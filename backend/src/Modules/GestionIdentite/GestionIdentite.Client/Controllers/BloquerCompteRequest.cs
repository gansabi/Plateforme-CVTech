namespace GestionIdentite.Client.Controllers;

public sealed record BloquerCompteRequest(Guid AdministrateurId, Guid CompteId);
