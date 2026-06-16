namespace ActualiteEtAbonnement.Client.Controllers;

public sealed record PublierArticleRequest(Guid AuteurId, string Titre, string Contenu, string DomaineMetier);
