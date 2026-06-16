using GestionIdentite.Domaine.Contrats;
using GestionIdentite.Domaine.Enums;
using GestionIdentite.Domaine.Exceptions;
using MediatR;

namespace GestionIdentite.Application.Features.BloquerCompte;

public sealed class BloquerCompteHandler : IRequestHandler<BloquerCompteCommand, BloquerCompteResponse>
{
    private readonly IVerificateurPermission _verificateur;
    private readonly ICompteRepository _repoCandidat;
    private readonly ICompteEntrepriseRepository _repoEntreprise;

    public BloquerCompteHandler(
        IVerificateurPermission verificateur,
        ICompteRepository repoCandidat,
        ICompteEntrepriseRepository repoEntreprise)
    {
        _verificateur = verificateur;
        _repoCandidat = repoCandidat;
        _repoEntreprise = repoEntreprise;
    }

    public async Task<BloquerCompteResponse> Handle(
        BloquerCompteCommand command,
        CancellationToken cancellationToken)
    {
        // Vérification de permission — obligatoire avant tout accès métier (regles-permissions.md)
        var autorise = await _verificateur.PossedePermissionAsync(
            command.AdministrateurId, Permission.BloquerCompte, cancellationToken);

        if (!autorise)
            throw new PermissionRefuseeException(
                "L'utilisateur n'est pas autorisé à bloquer un compte.");

        var candidat = await _repoCandidat.TrouverParIdAsync(command.CompteId, cancellationToken);
        if (candidat is not null)
        {
            candidat.Bloquer();
            return new BloquerCompteResponse(candidat.Id);
        }

        var entreprise = await _repoEntreprise.TrouverParIdAsync(command.CompteId, cancellationToken);
        if (entreprise is not null)
        {
            entreprise.Bloquer();
            return new BloquerCompteResponse(entreprise.Id);
        }

        throw new CompteNonTrouveException(command.CompteId);
    }
}
