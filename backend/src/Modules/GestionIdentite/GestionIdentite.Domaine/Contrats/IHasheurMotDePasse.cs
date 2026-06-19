namespace GestionIdentite.Domaine.Contrats;

public interface IHasheurMotDePasse
{
    string Hacher(string motDePasse);
    bool Verifier(string motDePasse, string hashStocke);
}
