using GestionIdentite.Domaine.Contrats;
using GestionIdentite.Domaine.Exceptions;
using MediatR;

namespace GestionIdentite.Application.Features.ConnecterUtilisateur;

public sealed class ConnecterUtilisateurHandler
    : IRequestHandler<ConnecterUtilisateurCommand, ConnecterUtilisateurResponse>
{
    private readonly IRegistreUtilisateurs _registre;
    private readonly IHasheurMotDePasse _hasheur;

    public ConnecterUtilisateurHandler(IRegistreUtilisateurs registre, IHasheurMotDePasse hasheur)
    {
        _registre = registre;
        _hasheur = hasheur;
    }

    public async Task<ConnecterUtilisateurResponse> Handle(
        ConnecterUtilisateurCommand command,
        CancellationToken cancellationToken)
    {
        var utilisateur = await _registre.TrouverParEmailAsync(command.Email, cancellationToken);

        if (utilisateur is null || !_hasheur.Verifier(command.MotDePasse, utilisateur.MotDePasseHache))
            throw new CredentielsInvalidesException();

        if (utilisateur.EstBloque)
            throw new CompteEstBloqueException();

        return new ConnecterUtilisateurResponse(utilisateur.Id, utilisateur.Email, utilisateur.Role);
    }
}
