namespace ActualiteEtAbonnement.Domaine.Exceptions;

public sealed class TitreArticleInvalideException : Exception
{
    public TitreArticleInvalideException(string message) : base(message) { }
}
