using ActualiteEtAbonnement.Domaine.Contrats;
using MediatR;

namespace ActualiteEtAbonnement.Application.Features.ConsulterFilRss;

public sealed class ConsulterFilRssHandler
    : IRequestHandler<ConsulterFilRssQuery, ConsulterFilRssResponse>
{
    private readonly IArticleActualiteRepository _repository;

    public ConsulterFilRssHandler(IArticleActualiteRepository repository)
        => _repository = repository;

    public async Task<ConsulterFilRssResponse> Handle(
        ConsulterFilRssQuery query, CancellationToken cancellationToken)
    {
        var articles = await _repository.ListerAsync(query.DomaineMetier, cancellationToken);

        var resumes = articles
            .Select(a => new ArticleRssResume(
                a.Id, a.Titre, a.Contenu,
                a.DomaineMetier.Valeur, a.DatePublication))
            .ToList()
            .AsReadOnly();

        return new ConsulterFilRssResponse(resumes);
    }
}
