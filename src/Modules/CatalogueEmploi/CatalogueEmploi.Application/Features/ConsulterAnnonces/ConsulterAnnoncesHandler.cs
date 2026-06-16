using CatalogueEmploi.Domaine.Contrats;
using MediatR;

namespace CatalogueEmploi.Application.Features.ConsulterAnnonces;

public sealed class ConsulterAnnoncesHandler
    : IRequestHandler<ConsulterAnnoncesQuery, ConsulterAnnoncesResponse>
{
    private readonly IAnnonceEmploiRepository _repository;

    public ConsulterAnnoncesHandler(IAnnonceEmploiRepository repository)
        => _repository = repository;

    public async Task<ConsulterAnnoncesResponse> Handle(
        ConsulterAnnoncesQuery query, CancellationToken cancellationToken)
    {
        var annonces = await _repository.ListerActivesAsync(cancellationToken);

        var resumes = annonces
            .Select(a => new AnnonceResume(
                a.Id, a.Titre, a.TypeContrat.ToString(),
                a.DomaineMetier.Valeur, a.DatePublication))
            .ToList()
            .AsReadOnly();

        return new ConsulterAnnoncesResponse(resumes);
    }
}
