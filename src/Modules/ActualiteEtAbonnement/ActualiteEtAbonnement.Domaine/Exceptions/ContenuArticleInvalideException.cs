namespace ActualiteEtAbonnement.Domaine.Exceptions;

public sealed class ContenuArticleInvalideException : Exception
{
    public ContenuArticleInvalideException(string message) : base(message) { }
}
