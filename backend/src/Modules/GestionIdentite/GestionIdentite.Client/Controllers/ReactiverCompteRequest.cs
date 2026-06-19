namespace GestionIdentite.Client.Controllers;

public sealed record ReactiverCompteRequest(Guid AdministrateurId, Guid CompteId);
