using AppelOffreFreelance.Domaine.Contrats;
using AppelOffreFreelance.Domaine.Exceptions;
using GestionIdentite.Domaine.Contrats;
using GestionIdentite.Domaine.Enums;
using GestionIdentite.Domaine.Exceptions;
using MediatR;

namespace AppelOffreFreelance.Application.Features.ModererAppelOffre;

public sealed class ModererAppelOffreHandler : IRequestHandler<ModererAppelOffreCommand>
{
    private readonly IAppelOffreRepository _repository;
    private readonly IVerificateurPermission _verificateur;

    public ModererAppelOffreHandler(
        IAppelOffreRepository repository,
        IVerificateurPermission verificateur)
    {
        _repository = repository;
        _verificateur = verificateur;
    }

    public async Task Handle(ModererAppelOffreCommand command, CancellationToken cancellationToken)
    {
        if (!await _verificateur.PossedePermissionAsync(
                command.AdministrateurId, Permission.ModererAppelOffre, cancellationToken))
            throw new PermissionRefuseeException("L'utilisateur n'est pas autorisé à modérer un appel d'offre.");

        var appelOffre = await _repository.TrouverParIdAsync(command.AppelOffreId, cancellationToken)
            ?? throw new AppelOffreNonTrouveException(command.AppelOffreId);

        appelOffre.Moderer();

        await _repository.MettreAJourAsync(appelOffre, cancellationToken);
    }
}
