using AppelOffreFreelance.Domaine.Contrats;
using GestionIdentite.Domaine.Contrats;
using GestionIdentite.Domaine.Enums;
using GestionIdentite.Domaine.Exceptions;
using MediatR;

namespace AppelOffreFreelance.Application.Features.ConsulterPropositionsRecues;

public sealed class ConsulterPropositionsRecuesHandler
    : IRequestHandler<ConsulterPropositionsRecuesQuery, ConsulterPropositionsRecuesResponse>
{
    private readonly IPropositionFreelanceRepository _repository;
    private readonly IVerificateurPermission _verificateur;

    public ConsulterPropositionsRecuesHandler(
        IPropositionFreelanceRepository repository,
        IVerificateurPermission verificateur)
    {
        _repository = repository;
        _verificateur = verificateur;
    }

    public async Task<ConsulterPropositionsRecuesResponse> Handle(
        ConsulterPropositionsRecuesQuery query, CancellationToken cancellationToken)
    {
        if (!await _verificateur.PossedePermissionAsync(
                query.UtilisateurId, Permission.ConsulterPropositionsRecues, cancellationToken))
            throw new PermissionRefuseeException("L'utilisateur n'est pas autorisé à consulter les propositions reçues.");

        var propositions = await _repository.ListerParAppelOffreIdAsync(query.AppelOffreId, cancellationToken);

        var resumes = propositions
            .Select(p => new PropositionResume(
                p.Id, p.CandidatId, p.TarifJournalier,
                p.DureeJours, p.Methodologie, p.DateSoumission))
            .ToList()
            .AsReadOnly();

        return new ConsulterPropositionsRecuesResponse(resumes);
    }
}
