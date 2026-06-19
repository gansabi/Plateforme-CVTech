import { useState } from 'react'
import { useAuth } from '../../../app/providers/AuthContext'
import apiClient from '../../../shared/api/client'

interface Candidature {
  id: string
  candidatId: string
  datePostulation: string
  lettreMotivation: string | null
}

export function CandidaturesRecues() {
  const { utilisateur } = useAuth()
  const [annonceId, setAnnonceId] = useState('')
  const [candidatures, setCandidatures] = useState<Candidature[]>([])
  const [chargement, setChargement] = useState(false)
  const [erreur, setErreur] = useState('')

  const handleRecherche = async (e: React.FormEvent) => {
    e.preventDefault()
    if (!utilisateur || !annonceId) return
    setChargement(true)
    setErreur('')
    try {
      const response = await apiClient.get('/api/catalogue/candidatures', {
        params: { utilisateurId: utilisateur.utilisateurId, annonceId },
      })
      setCandidatures(response.data.candidatures)
    } catch {
      setErreur('Erreur lors du chargement des candidatures.')
    } finally {
      setChargement(false)
    }
  }

  return (
    <div className="p-8 max-w-3xl mx-auto">
      <h1 className="text-2xl font-bold mb-6">Candidatures reçues</h1>
      <form onSubmit={handleRecherche} className="flex gap-3 mb-6">
        <input type="text" value={annonceId} onChange={e => setAnnonceId(e.target.value)}
          placeholder="ID de l'annonce"
          className="flex-1 border rounded px-3 py-2" required />
        <button type="submit" disabled={chargement}
          className="bg-blue-600 text-white px-6 py-2 rounded hover:bg-blue-700 disabled:opacity-50">
          {chargement ? '...' : 'Rechercher'}
        </button>
      </form>
      {erreur && <p className="text-red-600 mb-4">{erreur}</p>}
      {candidatures.length === 0 && !chargement && (
        <p className="text-gray-500">Aucune candidature trouvée.</p>
      )}
      <div className="space-y-3">
        {candidatures.map(c => (
          <div key={c.id} className="bg-white border rounded-lg p-4">
            <p className="text-sm font-medium">Candidat : <span className="font-mono text-xs">{c.candidatId}</span></p>
            <p className="text-sm text-gray-500">Postulé le {new Date(c.datePostulation).toLocaleDateString('fr-FR')}</p>
            {c.lettreMotivation && (
              <p className="mt-2 text-sm text-gray-700 italic">"{c.lettreMotivation}"</p>
            )}
          </div>
        ))}
      </div>
    </div>
  )
}
