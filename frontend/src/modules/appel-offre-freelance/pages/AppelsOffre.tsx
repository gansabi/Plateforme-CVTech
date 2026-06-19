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

  if (isLoading) return <p className="p-8 text-gray-500">Chargement...</p>

  return (
    <div className="p-8 max-w-4xl mx-auto">
      <div className="mb-8">
        <h1 className="text-2xl font-bold text-gray-900">Appels d'offre freelance</h1>
        <p className="text-sm text-gray-500 mt-1">{data?.appelsOffre.length ?? 0} mission(s) disponible(s)</p>
      </div>
      {message && <p className="mb-4 p-3 bg-green-50 border border-green-200 text-green-700 rounded-lg">{message}</p>}
      {data?.appelsOffre.length === 0 && (
        <div className="text-center py-12">
          <p className="text-gray-400 text-lg">Aucun appel d'offre pour le moment</p>
        </div>
      )}
      <div className="space-y-4">
        {data?.appelsOffre.map(ao => (
          <div key={ao.id} className="border border-gray-200 rounded-xl p-5 bg-white hover:shadow-md transition-shadow">
            <div className="flex justify-between items-start">
              <div className="space-y-2">
                <h2 className="text-lg font-semibold text-gray-900">{ao.titre}</h2>
                <div className="flex items-center gap-2 flex-wrap">
                  <span className="text-xs font-medium px-2.5 py-0.5 rounded-full bg-indigo-50 text-indigo-700">
                    {ao.domaineMetier}
                  </span>
                  <span className="text-xs font-medium px-2.5 py-0.5 rounded-full bg-emerald-50 text-emerald-700">
                    {ao.tjmMinimum}–{ao.tjmMaximum} €/jour
                  </span>
                </div>
                <p className="text-xs text-gray-400">
                  Date limite : {new Date(ao.dateLimite).toLocaleDateString('fr-FR')}
                </p>
              </div>
              {estConnecte && role === 'Candidat' && (
                <button onClick={() => handleSoumettre(ao.id)}
                  disabled={soumissionEnCours === ao.id}
                  className="bg-emerald-600 text-white px-5 py-2 rounded-lg text-sm font-medium hover:bg-emerald-700 disabled:opacity-50 transition-colors">
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
