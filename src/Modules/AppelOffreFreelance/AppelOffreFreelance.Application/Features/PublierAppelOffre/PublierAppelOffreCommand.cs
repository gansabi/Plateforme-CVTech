using MediatR;

namespace AppelOffreFreelance.Application.Features.PublierAppelOffre;

public sealed class PublierAppelOffreCommand : IRequest<PublierAppelOffreResponse>
{
    public Guid UtilisateurId { get; init; }
    public string Titre { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string DomaineMetier { get; init; } = string.Empty;
    public decimal TjmMinimum { get; init; }
    public decimal TjmMaximum { get; init; }
    public DateTime DateLimite { get; init; }
}
