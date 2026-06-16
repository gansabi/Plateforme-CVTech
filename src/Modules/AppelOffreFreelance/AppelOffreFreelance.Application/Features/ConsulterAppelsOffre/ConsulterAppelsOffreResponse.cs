namespace AppelOffreFreelance.Application.Features.ConsulterAppelsOffre;

public sealed record AppelOffreResume(
    Guid Id,
    string Titre,
    string DomaineMetier,
    decimal TjmMinimum,
    decimal TjmMaximum,
    DateTime DateLimite,
    DateTime DatePublication);

public sealed record ConsulterAppelsOffreResponse(IReadOnlyList<AppelOffreResume> AppelsOffre);
