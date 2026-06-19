using GestionIdentite.Domaine.Contrats;
using GestionIdentite.Domaine.Enums;
using GestionIdentite.Domaine.Exceptions;
using MediatR;

namespace GestionIdentite.Application.Features.ReactiverCompte;

public sealed class ReactiverCompteHandler : IRequestHandler<ReactiverCompteCommand, ReactiverCompteResponse>
{
    private readonly IVerificateurPermission _verificateur;
    private readonly ICompteRepository _repoCandidat;
    private readonly ICompteEntrepriseRepository _repoEntreprise;

    public ReactiverCompteHandler(
        IVerificateurPermission verificateur,
        ICompteRepository repoCandidat,
        ICompteEntrepriseRepository repoEntreprise)
    {
        _verificateur = verificateur;
        _repoCandidat = repoCandidat;
        _repoEntreprise = repoEntreprise;
    }

    public async Task<ReactiverCompteResponse> Handle(
        ReactiverCompteCommand command,
        CancellationToken cancellationToken)
    {
        // Vérification de permission — obligatoire avant tout accès métier (regles-permissions.md)
        var autorise = await _verificateur.PossedePermissionAsync(
            command.AdministrateurId, Permission.ReactiverCompte, cancellationToken);

        if (!autorise)
            throw new PermissionRefuseeException(
                "L'utilisateur n'est pas autorisé à réactiver un compte.");

        var candidat = await _repoCandidat.TrouverParIdAsync(command.CompteId, cancellationToken);
        if (candidat is not null)
        {
            candidat.Reactiver();
            return new ReactiverCompteResponse(candidat.Id);
        }

        var entreprise = await _repoEntreprise.TrouverParIdAsync(command.CompteId, cancellationToken);
        if (entreprise is not null)
        {
            entreprise.Reactiver();
            return new ReactiverCompteResponse(entreprise.Id);
        }

        throw new CompteNonTrouveException(command.CompteId);
    }
}
