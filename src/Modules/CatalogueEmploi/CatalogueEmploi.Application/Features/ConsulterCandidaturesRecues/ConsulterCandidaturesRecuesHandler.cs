using CatalogueEmploi.Domaine.Contrats;
using GestionIdentite.Domaine.Contrats;
using GestionIdentite.Domaine.Enums;
using GestionIdentite.Domaine.Exceptions;
using MediatR;

namespace CatalogueEmploi.Application.Features.ConsulterCandidaturesRecues;

public sealed class ConsulterCandidaturesRecuesHandler
    : IRequestHandler<ConsulterCandidaturesRecuesQuery, ConsulterCandidaturesRecuesResponse>
{
    private readonly ICandidatureRepository _repository;
    private readonly IVerificateurPermission _verificateur;

    public ConsulterCandidaturesRecuesHandler(
        ICandidatureRepository repository,
        IVerificateurPermission verificateur)
    {
        _repository = repository;
        _verificateur = verificateur;
    }

    public async Task<ConsulterCandidaturesRecuesResponse> Handle(
        ConsulterCandidaturesRecuesQuery query, CancellationToken cancellationToken)
    {
        if (!await _verificateur.PossedePermissionAsync(
                query.UtilisateurId, Permission.ConsulterCandidaturesRecues, cancellationToken))
            throw new PermissionRefuseeException("L'utilisateur n'est pas autorisé à consulter les candidatures reçues.");

        var candidatures = await _repository.ListerParAnnonceIdAsync(query.AnnonceId, cancellationToken);

        var resumes = candidatures
            .Select(c => new CandidatureResume(c.Id, c.CandidatId, c.DatePostulation, c.LettreMotivation))
            .ToList()
            .AsReadOnly();

        return new ConsulterCandidaturesRecuesResponse(resumes);
    }
}
