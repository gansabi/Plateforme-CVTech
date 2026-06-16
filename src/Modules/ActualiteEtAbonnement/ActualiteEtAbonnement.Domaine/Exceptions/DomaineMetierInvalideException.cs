namespace ActualiteEtAbonnement.Domaine.Exceptions;

public sealed class DomaineMetierInvalideException : Exception
{
    public DomaineMetierInvalideException(string message) : base(message) { }
}
