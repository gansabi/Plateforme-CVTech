import { useState } from 'react'
import { useNavigate, Link } from 'react-router-dom'
import { useAuth } from '../../../app/providers/AuthContext'
import { connecterUtilisateur } from '../services'

export function Connexion() {
  const [email, setEmail] = useState('')
  const [motDePasse, setMotDePasse] = useState('')
  const [erreur, setErreur] = useState('')
  const [chargement, setChargement] = useState(false)
  const { connecter } = useAuth()
  const navigate = useNavigate()

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setErreur('')
    setChargement(true)
    try {
      const reponse = await connecterUtilisateur({ email, motDePasse })
      connecter(reponse)
      navigate('/dashboard')
    } catch (err: unknown) {
      if (err && typeof err === 'object' && 'response' in err) {
        const axiosErr = err as { response?: { data?: { message?: string } } }
        setErreur(axiosErr.response?.data?.message ?? 'Erreur de connexion')
      } else {
        setErreur('Erreur de connexion')
      }
    } finally {
      setChargement(false)
    }
  }

  return (
    <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-blue-50 to-indigo-100">
      <div className="max-w-md w-full">
        <div className="text-center mb-8">
          <h1 className="text-3xl font-bold text-gray-900">💼 CVTech</h1>
          <p className="text-gray-500 mt-2">Job board · Freelance · Actualités tech</p>
        </div>
        <div className="bg-white p-8 rounded-2xl shadow-xl">
          <h2 className="text-xl font-semibold text-center mb-6 text-gray-800">Connexion</h2>
          {erreur && (
            <p className="text-red-600 text-sm mb-4 bg-red-50 border border-red-200 p-3 rounded-lg">{erreur}</p>
          )}
          <form onSubmit={handleSubmit} className="space-y-5">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Email</label>
              <input type="email" value={email} onChange={e => setEmail(e.target.value)}
                placeholder="votre@email.fr"
                className="w-full border border-gray-300 rounded-lg px-4 py-2.5 focus:ring-2 focus:ring-blue-500 focus:border-transparent outline-none transition" required />
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Mot de passe</label>
              <input type="password" value={motDePasse} onChange={e => setMotDePasse(e.target.value)}
                placeholder="••••••••"
                className="w-full border border-gray-300 rounded-lg px-4 py-2.5 focus:ring-2 focus:ring-blue-500 focus:border-transparent outline-none transition" required />
            </div>
            <button type="submit" disabled={chargement}
              className="w-full bg-gradient-to-r from-blue-600 to-indigo-600 text-white py-2.5 rounded-lg font-medium hover:from-blue-700 hover:to-indigo-700 disabled:opacity-50 transition-all">
              {chargement ? 'Connexion...' : 'Se connecter'}
            </button>
          </form>
          <div className="mt-6 text-center">
            <p className="text-sm text-gray-500">Pas encore de compte ?</p>
            <div className="flex justify-center gap-4 mt-2">
              <Link to="/inscription/candidat" className="text-sm text-blue-600 hover:text-blue-800 font-medium">
                Candidat
              </Link>
              <span className="text-gray-300">|</span>
              <Link to="/inscription/entreprise" className="text-sm text-blue-600 hover:text-blue-800 font-medium">
                Entreprise
              </Link>
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}
