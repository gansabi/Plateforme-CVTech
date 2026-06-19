import { useState } from 'react'
import { useAuth } from '../../../app/providers/AuthContext'
import apiClient from '../../../shared/api/client'

interface Proposition {
  id: string
  candidatId: string
  tarifJournalier: number
  dureeJours: number
  methodologie: string
  dateSoumission: string
}

export function PropositionsRecues() {
  const { utilisateur } = useAuth()
  const [appelOffreId, setAppelOffreId] = useState('')
  const [propositions, setPropositions] = useState<Proposition[]>([])
  const [chargement, setChargement] = useState(false)
  const [erreur, setErreur] = useState('')

  const handleRecherche = async (e: React.FormEvent) => {
    e.preventDefault()
    if (!utilisateur || !appelOffreId) return
    setChargement(true)
    setErreur('')
    try {
      const response = await apiClient.get('/api/freelance/propositions', {
        params: { utilisateurId: utilisateur.utilisateurId, appelOffreId },
      })
      setPropositions(response.data.propositions)
    } catch {
      setErreur('Erreur lors du chargement.')
    } finally {
      setChargement(false)
    }
  }

  return (
    <div className="p-8 max-w-3xl mx-auto">
      <h1 className="text-2xl font-bold mb-6">Propositions reçues</h1>
      <form onSubmit={handleRecherche} className="flex gap-3 mb-6">
        <input type="text" value={appelOffreId} onChange={e => setAppelOffreId(e.target.value)}
          placeholder="ID de l'appel d'offre" className="flex-1 border rounded px-3 py-2" required />
        <button type="submit" disabled={chargement}
          className="bg-blue-600 text-white px-6 py-2 rounded hover:bg-blue-700 disabled:opacity-50">
          {chargement ? '...' : 'Rechercher'}
        </button>
      </form>
      {erreur && <p className="text-red-600 mb-4">{erreur}</p>}
      {propositions.length === 0 && !chargement && (
        <p className="text-gray-500">Aucune proposition trouvée.</p>
      )}
      <div className="space-y-3">
        {propositions.map(p => (
          <div key={p.id} className="bg-white border rounded-lg p-4">
            <div className="flex justify-between items-start">
              <div>
                <p className="text-sm font-medium">Candidat : <span className="font-mono text-xs">{p.candidatId}</span></p>
                <p className="text-sm text-gray-600">{p.tarifJournalier} €/jour · {p.dureeJours} jours</p>
                <p className="text-sm text-gray-500">Méthodologie : {p.methodologie}</p>
              </div>
              <p className="text-xs text-gray-400">{new Date(p.dateSoumission).toLocaleDateString('fr-FR')}</p>
            </div>
          </div>
        ))}
      </div>
    </div>
  )
}
