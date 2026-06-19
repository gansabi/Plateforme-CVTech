namespace AppelOffreFreelance.Domaine.Exceptions;

public sealed class AppelOffreNonTrouveException : Exception
{
    public AppelOffreNonTrouveException(Guid appelOffreId)
        : base($"L'appel d'offre '{appelOffreId}' est introuvable.") { }
}
