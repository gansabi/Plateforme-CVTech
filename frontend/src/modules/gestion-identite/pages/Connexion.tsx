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
    <div className="min-h-screen flex items-center justify-center bg-gray-50">
      <div className="max-w-md w-full bg-white p-8 rounded-lg shadow">
        <h1 className="text-2xl font-bold text-center mb-6">Connexion</h1>
        {erreur && <p className="text-red-600 text-sm mb-4">{erreur}</p>}
        <form onSubmit={handleSubmit} className="space-y-4">
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
            {chargement ? 'Connexion...' : 'Se connecter'}
          </button>
        </form>
        <p className="mt-4 text-sm text-center text-gray-600">
          Pas encore de compte ?{' '}
          <Link to="/inscription/candidat" className="text-blue-600 hover:underline">Candidat</Link>
          {' | '}
          <Link to="/inscription/entreprise" className="text-blue-600 hover:underline">Entreprise</Link>
        </p>
      </div>
    </div>
  )
}
