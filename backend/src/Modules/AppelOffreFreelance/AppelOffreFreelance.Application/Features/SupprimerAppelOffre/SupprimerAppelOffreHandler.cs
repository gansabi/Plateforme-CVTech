using AppelOffreFreelance.Domaine.Contrats;
using AppelOffreFreelance.Domaine.Exceptions;
using GestionIdentite.Domaine.Contrats;
using GestionIdentite.Domaine.Enums;
using GestionIdentite.Domaine.Exceptions;
using MediatR;

namespace AppelOffreFreelance.Application.Features.SupprimerAppelOffre;

public sealed class SupprimerAppelOffreHandler : IRequestHandler<SupprimerAppelOffreCommand>
{
    private readonly IAppelOffreRepository _repository;
    private readonly IVerificateurPermission _verificateur;

    public SupprimerAppelOffreHandler(
        IAppelOffreRepository repository,
        IVerificateurPermission verificateur)
    {
        _repository = repository;
        _verificateur = verificateur;
    }

    public async Task Handle(SupprimerAppelOffreCommand command, CancellationToken cancellationToken)
    {
        if (!await _verificateur.PossedePermissionAsync(
                command.AdministrateurId, Permission.SupprimerAppelOffre, cancellationToken))
            throw new PermissionRefuseeException("L'utilisateur n'est pas autorisé à supprimer un appel d'offre.");

        var appelOffre = await _repository.TrouverParIdAsync(command.AppelOffreId, cancellationToken)
            ?? throw new AppelOffreNonTrouveException(command.AppelOffreId);

        appelOffre.Supprimer();

        await _repository.MettreAJourAsync(appelOffre, cancellationToken);
    }
}
