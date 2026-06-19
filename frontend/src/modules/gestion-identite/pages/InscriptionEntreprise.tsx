import { useState } from 'react'
import { useNavigate, Link } from 'react-router-dom'
import { inscrireEntreprise } from '../services'

export function InscriptionEntreprise() {
  const [email, setEmail] = useState('')
  const [motDePasse, setMotDePasse] = useState('')
  const [nomEntreprise, setNomEntreprise] = useState('')
  const [erreur, setErreur] = useState('')
  const [succes, setSucces] = useState(false)
  const [chargement, setChargement] = useState(false)
  const navigate = useNavigate()

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setErreur('')
    setChargement(true)
    try {
      await inscrireEntreprise({ email, motDePasse, nomEntreprise })
      setSucces(true)
      setTimeout(() => navigate('/connexion'), 2000)
    } catch (err: unknown) {
      if (err && typeof err === 'object' && 'response' in err) {
        const axiosErr = err as { response?: { data?: { message?: string } } }
        setErreur(axiosErr.response?.data?.message ?? "Erreur lors de l'inscription")
      } else {
        setErreur("Erreur lors de l'inscription")
      }
    } finally {
      setChargement(false)
    }
  }

  if (succes) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-gray-50">
        <div className="bg-green-100 p-6 rounded-lg text-center">
          <p className="text-green-800 font-medium">Compte entreprise créé ! Redirection...</p>
        </div>
      </div>
    )
  }

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-50">
      <div className="max-w-md w-full bg-white p-8 rounded-lg shadow">
        <h1 className="text-2xl font-bold text-center mb-6">Inscription Entreprise</h1>
        {erreur && <p className="text-red-600 text-sm mb-4">{erreur}</p>}
        <form onSubmit={handleSubmit} className="space-y-4">
          <div>
            <label className="block text-sm font-medium text-gray-700">Nom de l'entreprise</label>
            <input type="text" value={nomEntreprise} onChange={e => setNomEntreprise(e.target.value)}
              className="mt-1 w-full border rounded px-3 py-2" required />
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700">Email</label>
            <input type="email" value={email} onChange={e => setEmail(e.target.value)}
              className="mt-1 w-full border rounded px-3 py-2" required />
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700">Mot de passe</label>
            <input type="password" value={motDePasse} onChange={e => setMotDePasse(e.target.value)}
              className="mt-1 w-full border rounded px-3 py-2" required />
          </div>
          <button type="submit" disabled={chargement}
            className="w-full bg-blue-600 text-white py-2 rounded hover:bg-blue-700 disabled:opacity-50">
            {chargement ? 'Création...' : "S'inscrire"}
          </button>
        </form>
        <p className="mt-4 text-sm text-center text-gray-600">
          Déjà un compte ? <Link to="/connexion" className="text-blue-600 hover:underline">Connexion</Link>
        </p>
      </div>
    </div>
  )
}
