using GestionIdentite.Domaine.Contrats;
using GestionIdentite.Domaine.Entites;
using GestionIdentite.Domaine.Exceptions;
using GestionIdentite.Domaine.ObjetsValeur;
using MediatR;

namespace GestionIdentite.Application.Features.CreerCompteEntreprise;

public sealed class CreerCompteEntrepriseHandler
    : IRequestHandler<CreerCompteEntrepriseCommand, CreerCompteEntrepriseResponse>
{
    private readonly ICompteEntrepriseRepository _compteRepository;
    private readonly IHasheurMotDePasse _hasheur;

    public CreerCompteEntrepriseHandler(
        ICompteEntrepriseRepository compteRepository,
        IHasheurMotDePasse hasheur)
    {
        _compteRepository = compteRepository;
        _hasheur = hasheur;
    }

    public async Task<CreerCompteEntrepriseResponse> Handle(
        CreerCompteEntrepriseCommand command,
        CancellationToken cancellationToken)
    {
        var emailDejaUtilise = await _compteRepository.ExisteAvecEmailAsync(
            command.Email, cancellationToken);

        if (emailDejaUtilise)
            throw new EmailDejaUtiliseException(command.Email);

        var email = Email.Creer(command.Email);
        var motDePasse = MotDePasse.Creer(command.MotDePasse);
        var motDePasseHache = motDePasse.Hacher(_hasheur);

        var compte = CompteEntreprise.Creer(email, command.NomEntreprise, motDePasseHache);
        await _compteRepository.SauvegarderAsync(compte, cancellationToken);

        return new CreerCompteEntrepriseResponse(compte.Id);
    }
}
