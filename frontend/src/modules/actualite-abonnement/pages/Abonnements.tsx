import { useState } from 'react'
import { useAuth } from '../../../app/providers/AuthContext'
import apiClient from '../../../shared/api/client'

export function Abonnements() {
  const { utilisateur } = useAuth()
  const [domaineMetier, setDomaineMetier] = useState('')
  const [message, setMessage] = useState('')
  const [erreur, setErreur] = useState('')
  const [chargement, setChargement] = useState(false)

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    if (!utilisateur) return
    setChargement(true)
    setMessage('')
    setErreur('')
    try {
      await apiClient.post('/api/actualite/abonnements', {
        utilisateurId: utilisateur.utilisateurId,
        domaineMetier,
      })
      setMessage(`Abonné au domaine "${domaineMetier}" — vous recevrez des notifications.`)
      setDomaineMetier('')
    } catch {
      setErreur('Erreur — vous êtes peut-être déjà abonné à ce domaine.')
    } finally {
      setChargement(false)
    }
  }

  return (
    <div className="p-8 max-w-2xl mx-auto">
      <h1 className="text-2xl font-bold mb-2">Mes abonnements</h1>
      <p className="text-gray-600 mb-6 text-sm">
        Abonnez-vous à un domaine métier pour recevoir des notifications lors de nouvelles publications.
      </p>
      {message && <p className="mb-4 p-3 bg-green-50 text-green-700 rounded">{message}</p>}
      {erreur && <p className="mb-4 p-3 bg-red-50 text-red-700 rounded">{erreur}</p>}
      <form onSubmit={handleSubmit} className="flex gap-3">
        <input type="text" value={domaineMetier} onChange={e => setDomaineMetier(e.target.value)}
          placeholder="Ex : Cloud Azure, Data Science, DevOps..."
          className="flex-1 border rounded px-3 py-2" required />
        <button type="submit" disabled={chargement}
          className="bg-blue-600 text-white px-6 py-2 rounded hover:bg-blue-700 disabled:opacity-50">
          {chargement ? '...' : "S'abonner"}
        </button>
      </form>
    </div>
  )
}
