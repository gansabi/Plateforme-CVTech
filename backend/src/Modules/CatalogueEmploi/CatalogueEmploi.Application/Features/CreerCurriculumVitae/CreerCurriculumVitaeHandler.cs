using CatalogueEmploi.Domaine.Contrats;
using CatalogueEmploi.Domaine.Entites;
using GestionIdentite.Domaine.Contrats;
using GestionIdentite.Domaine.Enums;
using GestionIdentite.Domaine.Exceptions;
using MediatR;

namespace CatalogueEmploi.Application.Features.CreerCurriculumVitae;

public sealed class CreerCurriculumVitaeHandler
    : IRequestHandler<CreerCurriculumVitaeCommand, CreerCurriculumVitaeResponse>
{
    private readonly ICurriculumVitaeRepository _repository;
    private readonly IVerificateurPermission _verificateur;

    public CreerCurriculumVitaeHandler(
        ICurriculumVitaeRepository repository,
        IVerificateurPermission verificateur)
    {
        _repository = repository;
        _verificateur = verificateur;
    }

    public async Task<CreerCurriculumVitaeResponse> Handle(
        CreerCurriculumVitaeCommand command, CancellationToken cancellationToken)
    {
        if (!await _verificateur.PossedePermissionAsync(
                command.CandidatId, Permission.CreerCurriculumVitae, cancellationToken))
            throw new PermissionRefuseeException("L'utilisateur n'est pas autorisé à créer un curriculum vitae.");

        var cv = CurriculumVitae.Creer(
            command.CandidatId, command.Titre, command.Resume, command.CompetencesPrincipales);

        await _repository.SauvegarderAsync(cv, cancellationToken);

        return new CreerCurriculumVitaeResponse(cv.Id);
    }
}
