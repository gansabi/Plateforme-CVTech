using GestionIdentite.Domaine.Contrats;
using GestionIdentite.Domaine.Entites;
using GestionIdentite.Domaine.Exceptions;
using GestionIdentite.Domaine.ObjetsValeur;
using MediatR;

namespace GestionIdentite.Application.Features.CreerCompteAdministrateur;

public sealed class CreerCompteAdministrateurHandler
    : IRequestHandler<CreerCompteAdministrateurCommand, CreerCompteAdministrateurResponse>
{
    private readonly ICompteAdministrateurRepository _repository;
    private readonly IHasheurMotDePasse _hasheur;

    public CreerCompteAdministrateurHandler(
        ICompteAdministrateurRepository repository,
        IHasheurMotDePasse hasheur)
    {
        _repository = repository;
        _hasheur = hasheur;
    }

    public async Task<CreerCompteAdministrateurResponse> Handle(
        CreerCompteAdministrateurCommand command,
        CancellationToken cancellationToken)
    {
        var emailDejaUtilise = await _repository.ExisteAvecEmailAsync(command.Email, cancellationToken);

        if (emailDejaUtilise)
            throw new EmailDejaUtiliseException(command.Email);

        var email = Email.Creer(command.Email);
        var motDePasse = MotDePasse.Creer(command.MotDePasse);
        var motDePasseHache = motDePasse.Hacher(_hasheur);

        var compte = CompteAdministrateur.Creer(email, command.NomComplet, motDePasseHache);

        await _repository.SauvegarderAsync(compte, cancellationToken);

        return new CreerCompteAdministrateurResponse(compte.Id);
    }
}
