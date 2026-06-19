using GestionIdentite.Domaine.Contrats;
using GestionIdentite.Domaine.Entites;
using GestionIdentite.Domaine.Exceptions;
using GestionIdentite.Domaine.ObjetsValeur;
using MediatR;

namespace GestionIdentite.Application.Features.CreerCompteCandidat;

public sealed class CreerCompteCandidatHandler
    : IRequestHandler<CreerCompteCandidatCommand, CreerCompteCandidatResponse>
{
    private readonly ICompteRepository _compteRepository;
    private readonly IHasheurMotDePasse _hasheur;

    public CreerCompteCandidatHandler(ICompteRepository compteRepository, IHasheurMotDePasse hasheur)
    {
        _compteRepository = compteRepository;
        _hasheur = hasheur;
    }

    public async Task<CreerCompteCandidatResponse> Handle(
        CreerCompteCandidatCommand command,
        CancellationToken cancellationToken)
    {
        var emailDejaUtilise = await _compteRepository.ExisteAvecEmailAsync(
            command.Email, cancellationToken);

        if (emailDejaUtilise)
            throw new EmailDejaUtiliseException(command.Email);

        var email = Email.Creer(command.Email);
        var motDePasse = MotDePasse.Creer(command.MotDePasse);
        var motDePasseHache = motDePasse.Hacher(_hasheur);

        var compte = CompteCandidat.CréerAvecMotDePasseHache(email, motDePasseHache);

        await _compteRepository.SauvegarderAsync(compte, cancellationToken);

        return new CreerCompteCandidatResponse(compte.Id);
    }
}
