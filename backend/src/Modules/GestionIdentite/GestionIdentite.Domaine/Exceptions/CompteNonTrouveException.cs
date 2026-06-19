namespace GestionIdentite.Domaine.Exceptions;

public sealed class CompteNonTrouveException(Guid compteId)
    : Exception($"Le compte '{compteId}' est introuvable.");
