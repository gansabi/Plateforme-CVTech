using CatalogueEmploi.Domaine.Contrats;
using CatalogueEmploi.Domaine.Entites;
using CatalogueEmploi.Domaine.Exceptions;
using GestionIdentite.Domaine.Contrats;
using GestionIdentite.Domaine.Enums;
using GestionIdentite.Domaine.Exceptions;
using MediatR;

namespace CatalogueEmploi.Application.Features.PostulerAnnonce;

public sealed class PostulerAnnonceHandler : IRequestHandler<PostulerAnnonceCommand>
{
    private readonly ICandidatureRepository _candidatureRepository;
    private readonly IAnnonceEmploiRepository _annonceRepository;
    private readonly IVerificateurPermission _verificateur;

    public PostulerAnnonceHandler(
        ICandidatureRepository candidatureRepository,
        IAnnonceEmploiRepository annonceRepository,
        IVerificateurPermission verificateur)
    {
        _candidatureRepository = candidatureRepository;
        _annonceRepository = annonceRepository;
        _verificateur = verificateur;
    }

    public async Task Handle(PostulerAnnonceCommand command, CancellationToken cancellationToken)
    {
        if (!await _verificateur.PossedePermissionAsync(
                command.CandidatId, Permission.PostulerAnnonce, cancellationToken))
            throw new PermissionRefuseeException("L'utilisateur n'est pas autorisé à postuler à une annonce.");

        var annonce = await _annonceRepository.TrouverParIdAsync(command.AnnonceId, cancellationToken)
            ?? throw new AnnonceNonTrouveeException(command.AnnonceId);

        if (await _candidatureRepository.ExisteDejaAsync(command.CandidatId, command.AnnonceId, cancellationToken))
            throw new CandidatureDejaExistanteException();

        var candidature = Candidature.Creer(command.CandidatId, command.AnnonceId, command.LettreMotivation);

        await _candidatureRepository.SauvegarderAsync(candidature, cancellationToken);
    }
}
