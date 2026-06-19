using AppelOffreFreelance.Domaine.Exceptions;

namespace AppelOffreFreelance.Domaine.ObjetsValeur;

public sealed class BaremeTJM
{
    public decimal Minimum { get; }
    public decimal Maximum { get; }

    private BaremeTJM(decimal minimum, decimal maximum)
    {
        Minimum = minimum;
        Maximum = maximum;
    }

    public static BaremeTJM Creer(decimal minimum, decimal maximum)
    {
        if (minimum < 0)
            throw new BaremeTJMInvalideException("Le TJM minimum ne peut pas être négatif.");

        if (maximum < minimum)
            throw new BaremeTJMInvalideException("Le TJM maximum ne peut pas être inférieur au minimum.");

        return new BaremeTJM(minimum, maximum);
    }

    public override bool Equals(object? obj) =>
        obj is BaremeTJM other && Minimum == other.Minimum && Maximum == other.Maximum;

    public override int GetHashCode() => HashCode.Combine(Minimum, Maximum);

    public override string ToString() => $"{Minimum}–{Maximum} €/jour";
}
