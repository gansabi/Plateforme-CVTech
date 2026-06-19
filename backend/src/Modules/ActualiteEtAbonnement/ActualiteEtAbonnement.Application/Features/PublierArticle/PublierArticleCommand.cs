using MediatR;

namespace ActualiteEtAbonnement.Application.Features.PublierArticle;

public sealed class PublierArticleCommand : IRequest<PublierArticleResponse>
{
    public Guid AuteurId { get; init; }
    public string Titre { get; init; } = string.Empty;
    public string Contenu { get; init; } = string.Empty;
    public string DomaineMetier { get; init; } = string.Empty;
}
