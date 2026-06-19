namespace ActualiteEtAbonnement.Domaine.Exceptions;

public sealed class AbonnementDejaExistantException : Exception
{
    public AbonnementDejaExistantException()
        : base("L'utilisateur est déjà abonné à ce domaine métier.") { }
}
