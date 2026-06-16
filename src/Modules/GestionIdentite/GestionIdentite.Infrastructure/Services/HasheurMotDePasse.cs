using System.Security.Cryptography;
using GestionIdentite.Domaine.Contrats;

namespace GestionIdentite.Infrastructure.Services;

public sealed class HasheurMotDePasse : IHasheurMotDePasse
{
    private const int TailleSelOctets = 16;
    private const int TailleHashOctets = 32;
    private const int NombreIterations = 100_000;

    public string Hacher(string motDePasse)
    {
        var sel = RandomNumberGenerator.GetBytes(TailleSelOctets);
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            motDePasse, sel, NombreIterations, HashAlgorithmName.SHA256, TailleHashOctets);

        return $"{Convert.ToBase64String(sel)}.{Convert.ToBase64String(hash)}";
    }

    public bool Verifier(string motDePasse, string hashStocke)
    {
        var parties = hashStocke.Split('.');
        if (parties.Length != 2) return false;

        byte[] sel;
        byte[] hashAttendu;

        try
        {
            sel = Convert.FromBase64String(parties[0]);
            hashAttendu = Convert.FromBase64String(parties[1]);
        }
        catch (FormatException)
        {
            return false;
        }

        var hashCalcule = Rfc2898DeriveBytes.Pbkdf2(
            motDePasse, sel, NombreIterations, HashAlgorithmName.SHA256, TailleHashOctets);

        return CryptographicOperations.FixedTimeEquals(hashCalcule, hashAttendu);
    }
}
