import { useQuery } from '@tanstack/react-query'
import { useState } from 'react'
import { useAuth } from '../../../app/providers/AuthContext'
import apiClient from '../../../shared/api/client'

interface AppelOffreResume {
  id: string
  titre: string
  domaineMetier: string
  tjmMinimum: number
  tjmMaximum: number
  dateLimite: string
  datePublication: string
}

async function consulterAppelsOffre() {
  const response = await apiClient.get<{ appelsOffre: AppelOffreResume[] }>('/api/freelance/appels-offre')
  return response.data
}

export function AppelsOffre() {
  const { utilisateur, estConnecte, role } = useAuth()
  const [soumissionEnCours, setSoumissionEnCours] = useState<string | null>(null)
  const [message, setMessage] = useState('')

  const { data, isLoading } = useQuery({
    queryKey: ['appels-offre'],
    queryFn: consulterAppelsOffre,
  })

  const handleSoumettre = async (appelOffreId: string) => {
    if (!utilisateur) return
    setSoumissionEnCours(appelOffreId)
    setMessage('')
    try {
      await apiClient.post('/api/freelance/propositions', {
        candidatId: utilisateur.utilisateurId,
        appelOffreId,
        tarifJournalier: 600,
        dureeJours: 20,
        methodologie: 'Agile Scrum',
      })
      setMessage('Proposition soumise !')
    } catch {
      setMessage('Erreur lors de la soumission.')
    } finally {
      setSoumissionEnCours(null)
    }
  }

  if (isLoading) return <p className="p-8">Chargement...</p>

  return (
    <div className="p-8 max-w-4xl mx-auto">
      <h1 className="text-2xl font-bold mb-6">Appels d'offre freelance</h1>
      {message && <p className="mb-4 p-3 bg-green-50 text-green-700 rounded">{message}</p>}
      {data?.appelsOffre.length === 0 && <p className="text-gray-500">Aucun appel d'offre pour le moment.</p>}
      <div className="space-y-4">
        {data?.appelsOffre.map(ao => (
          <div key={ao.id} className="border rounded-lg p-4 bg-white shadow-sm">
            <div className="flex justify-between items-start">
              <div>
                <h2 className="text-lg font-semibold">{ao.titre}</h2>
                <p className="text-sm text-gray-600">{ao.domaineMetier} · {ao.tjmMinimum}–{ao.tjmMaximum} €/jour</p>
                <p className="text-xs text-gray-400 mt-1">
                  Limite : {new Date(ao.dateLimite).toLocaleDateString('fr-FR')}
                </p>
              </div>
              {estConnecte && role === 'Candidat' && (
                <button onClick={() => handleSoumettre(ao.id)}
                  disabled={soumissionEnCours === ao.id}
                  className="bg-green-600 text-white px-4 py-2 rounded text-sm hover:bg-green-700 disabled:opacity-50">
                  {soumissionEnCours === ao.id ? '...' : 'Proposer'}
                </button>
              )}
            </div>
          </div>
        ))}
      </div>
    </div>
  )
}
