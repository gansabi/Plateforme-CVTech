using AppelOffreFreelance.Domaine.Contrats;
using AppelOffreFreelance.Domaine.Entites;
using AppelOffreFreelance.Domaine.Exceptions;
using GestionIdentite.Domaine.Contrats;
using GestionIdentite.Domaine.Enums;
using GestionIdentite.Domaine.Exceptions;
using MediatR;

namespace AppelOffreFreelance.Application.Features.SoumettreProposition;

public sealed class SoumettrePropositionHandler : IRequestHandler<SoumettrePropositionCommand>
{
    private readonly IPropositionFreelanceRepository _propositionRepository;
    private readonly IAppelOffreRepository _appelOffreRepository;
    private readonly IVerificateurPermission _verificateur;

    public SoumettrePropositionHandler(
        IPropositionFreelanceRepository propositionRepository,
        IAppelOffreRepository appelOffreRepository,
        IVerificateurPermission verificateur)
    {
        _propositionRepository = propositionRepository;
        _appelOffreRepository = appelOffreRepository;
        _verificateur = verificateur;
    }

    public async Task Handle(SoumettrePropositionCommand command, CancellationToken cancellationToken)
    {
        if (!await _verificateur.PossedePermissionAsync(
                command.CandidatId, Permission.SoumettrePropositon, cancellationToken))
            throw new PermissionRefuseeException("L'utilisateur n'est pas autorisé à soumettre une proposition.");

        var appelOffre = await _appelOffreRepository.TrouverParIdAsync(command.AppelOffreId, cancellationToken)
            ?? throw new AppelOffreNonTrouveException(command.AppelOffreId);

        if (await _propositionRepository.ExisteDejaAsync(command.CandidatId, command.AppelOffreId, cancellationToken))
            throw new PropositionDejaExistanteException();

        var proposition = PropositionFreelance.Creer(
            command.CandidatId, command.AppelOffreId,
            command.TarifJournalier, command.DureeJours, command.Methodologie);

        await _propositionRepository.SauvegarderAsync(proposition, cancellationToken);
    }
}
