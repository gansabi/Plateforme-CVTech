namespace ActualiteEtAbonnement.Application.Features.ConsulterFilRss;

public sealed record ArticleRssResume(
    Guid Id,
    string Titre,
    string Contenu,
    string DomaineMetier,
    DateTime DatePublication);

public sealed record ConsulterFilRssResponse(IReadOnlyList<ArticleRssResume> Articles);
