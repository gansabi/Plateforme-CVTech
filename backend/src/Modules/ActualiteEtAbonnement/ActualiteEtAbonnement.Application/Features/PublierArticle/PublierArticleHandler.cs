using ActualiteEtAbonnement.Domaine.Contrats;
using ActualiteEtAbonnement.Domaine.Entites;
using ActualiteEtAbonnement.Domaine.ObjetsValeur;
using GestionIdentite.Domaine.Contrats;
using GestionIdentite.Domaine.Enums;
using GestionIdentite.Domaine.Exceptions;
using MediatR;

namespace ActualiteEtAbonnement.Application.Features.PublierArticle;

public sealed class PublierArticleHandler
    : IRequestHandler<PublierArticleCommand, PublierArticleResponse>
{
    private readonly IArticleActualiteRepository _repository;
    private readonly IVerificateurPermission _verificateur;

    public PublierArticleHandler(
        IArticleActualiteRepository repository,
        IVerificateurPermission verificateur)
    {
        _repository = repository;
        _verificateur = verificateur;
    }

    public async Task<PublierArticleResponse> Handle(
        PublierArticleCommand command, CancellationToken cancellationToken)
    {
        if (!await _verificateur.PossedePermissionAsync(
                command.AuteurId, Permission.PublierArticleActualite, cancellationToken))
            throw new PermissionRefuseeException("L'utilisateur n'est pas autorisé à publier un article d'actualité.");

        var domaine = DomaineMetier.Creer(command.DomaineMetier);
        var article = ArticleActualite.Publier(command.AuteurId, command.Titre, command.Contenu, domaine);

        await _repository.SauvegarderAsync(article, cancellationToken);

        return new PublierArticleResponse(article.Id);
    }
}
