import { useQuery } from '@tanstack/react-query'
import { useState } from 'react'
import { useAuth } from '../../../app/providers/AuthContext'
import { consulterAnnonces, postulerAnnonce } from '../services'
import type { AnnonceResume } from '../types'

const BADGE_CONTRAT: Record<string, string> = {
  CDI: 'bg-green-100 text-green-800',
  CDD: 'bg-orange-100 text-orange-800',
  Stage: 'bg-blue-100 text-blue-800',
  Alternance: 'bg-purple-100 text-purple-800',
  Apprentissage: 'bg-teal-100 text-teal-800',
}

export function Annonces() {
  const { utilisateur, estConnecte, role } = useAuth()
  const [postulationEnCours, setPostulationEnCours] = useState<string | null>(null)
  const [message, setMessage] = useState('')

  const { data, isLoading, error } = useQuery({
    queryKey: ['annonces'],
    queryFn: consulterAnnonces,
  })

  const handlePostuler = async (annonceId: string) => {
    if (!utilisateur) return
    setPostulationEnCours(annonceId)
    setMessage('')
    try {
      await postulerAnnonce({ candidatId: utilisateur.utilisateurId, annonceId })
      setMessage('Candidature envoyée !')
    } catch {
      setMessage('Erreur lors de la candidature.')
    } finally {
      setPostulationEnCours(null)
    }
  }

  if (isLoading) return <p className="p-8 text-gray-500">Chargement des annonces...</p>
  if (error) return <p className="p-8 text-red-600">Erreur de chargement</p>

  return (
    <div className="p-8 max-w-4xl mx-auto">
      <div className="mb-8">
        <h1 className="text-2xl font-bold text-gray-900">Annonces d'emploi</h1>
        <p className="text-sm text-gray-500 mt-1">{data?.annonces.length ?? 0} offre(s) disponible(s)</p>
      </div>
      {message && <p className="mb-4 text-green-700 bg-green-50 border border-green-200 p-3 rounded-lg">{message}</p>}
      {data?.annonces.length === 0 && (
        <div className="text-center py-12">
          <p className="text-gray-400 text-lg">Aucune annonce pour le moment</p>
          <p className="text-gray-300 text-sm mt-1">Revenez bientôt</p>
        </div>
      )}
      <div className="space-y-4">
        {data?.annonces.map((annonce: AnnonceResume) => (
          <div key={annonce.id} className="border border-gray-200 rounded-xl p-5 bg-white hover:shadow-md transition-shadow">
            <div className="flex justify-between items-start">
              <div className="space-y-2">
                <h2 className="text-lg font-semibold text-gray-900">{annonce.titre}</h2>
                <div className="flex items-center gap-2 flex-wrap">
                  <span className={`text-xs font-medium px-2.5 py-0.5 rounded-full ${BADGE_CONTRAT[annonce.typeContrat] ?? 'bg-gray-100 text-gray-800'}`}>
                    {annonce.typeContrat}
                  </span>
                  <span className="text-xs font-medium px-2.5 py-0.5 rounded-full bg-indigo-50 text-indigo-700">
                    {annonce.domaineMetier}
                  </span>
                </div>
                <p className="text-xs text-gray-400">
                  Publié le {new Date(annonce.datePublication).toLocaleDateString('fr-FR')}
                </p>
              </div>
              {estConnecte && role === 'Candidat' && (
                <button
                  onClick={() => handlePostuler(annonce.id)}
                  disabled={postulationEnCours === annonce.id}
                  className="bg-blue-600 text-white px-5 py-2 rounded-lg text-sm font-medium hover:bg-blue-700 disabled:opacity-50 transition-colors"
                >
                  {postulationEnCours === annonce.id ? '...' : 'Postuler'}
                </button>
              )}
            </div>
          </div>
        ))}
      </div>
    </div>
  )
}
