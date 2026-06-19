using FluentValidation;
using MediatR;

namespace GestionIdentite.Application.Comportements;

/// <summary>
/// Comportement MediatR qui exécute tous les validateurs FluentValidation
/// enregistrés pour la requête avant d'appeler le Handler.
/// Lève une ValidationException agrégée si au moins une règle échoue.
/// </summary>
public sealed class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var erreurs = _validators
            .Select(v => v.Validate(new ValidationContext<TRequest>(request)))
            .SelectMany(r => r.Errors)
            .Where(f => f is not null)
            .ToList();

        if (erreurs.Count > 0)
            throw new ValidationException(erreurs);

        return await next();
    }
}
