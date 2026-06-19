using AppelOffreFreelance.Domaine.Contrats;
using MediatR;

namespace AppelOffreFreelance.Application.Features.ConsulterAppelsOffre;

public sealed class ConsulterAppelsOffreHandler
    : IRequestHandler<ConsulterAppelsOffreQuery, ConsulterAppelsOffreResponse>
{
    private readonly IAppelOffreRepository _repository;

    public ConsulterAppelsOffreHandler(IAppelOffreRepository repository)
        => _repository = repository;

    public async Task<ConsulterAppelsOffreResponse> Handle(
        ConsulterAppelsOffreQuery query, CancellationToken cancellationToken)
    {
        var appelsOffre = await _repository.ListerOuvertsAsync(cancellationToken);

        var resumes = appelsOffre
            .Select(a => new AppelOffreResume(
                a.Id, a.Titre, a.DomaineMetier.Valeur,
                a.BaremeTJM.Minimum, a.BaremeTJM.Maximum,
                a.DateLimite, a.DatePublication))
            .ToList()
            .AsReadOnly();

        return new ConsulterAppelsOffreResponse(resumes);
    }
}
