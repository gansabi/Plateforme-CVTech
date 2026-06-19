namespace AppelOffreFreelance.Domaine.Exceptions;

public sealed class PropositionDejaExistanteException : Exception
{
    public PropositionDejaExistanteException()
        : base("Le candidat a déjà soumis une proposition pour cet appel d'offre.") { }
}
