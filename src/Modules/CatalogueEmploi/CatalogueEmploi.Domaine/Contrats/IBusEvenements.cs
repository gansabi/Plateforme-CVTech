namespace CatalogueEmploi.Domaine.Contrats;

public interface IBusEvenements
{
    Task PublierAsync<T>(T evenement, CancellationToken cancellationToken = default) where T : class;
}
