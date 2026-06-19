import { useQuery } from '@tanstack/react-query'
import { useState } from 'react'
import { useAuth } from '../../../app/providers/AuthContext'
import { consulterAnnonces, postulerAnnonce } from '../services'
import type { AnnonceResume } from '../types'

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

  if (isLoading) return <p className="p-8">Chargement des annonces...</p>
  if (error) return <p className="p-8 text-red-600">Erreur de chargement</p>

  return (
    <div className="p-8 max-w-4xl mx-auto">
      <h1 className="text-2xl font-bold mb-6">Annonces d'emploi</h1>
      {message && <p className="mb-4 text-green-700 bg-green-50 p-2 rounded">{message}</p>}
      {data?.annonces.length === 0 && <p className="text-gray-500">Aucune annonce pour le moment.</p>}
      <div className="space-y-4">
        {data?.annonces.map((annonce: AnnonceResume) => (
          <div key={annonce.id} className="border rounded-lg p-4 bg-white shadow-sm">
            <div className="flex justify-between items-start">
              <div>
                <h2 className="text-lg font-semibold">{annonce.titre}</h2>
                <p className="text-sm text-gray-600">
                  {annonce.typeContrat} · {annonce.domaineMetier}
                </p>
                <p className="text-xs text-gray-400 mt-1">
                  Publié le {new Date(annonce.datePublication).toLocaleDateString('fr-FR')}
                </p>
              </div>
              {estConnecte && role === 'Candidat' && (
                <button
                  onClick={() => handlePostuler(annonce.id)}
                  disabled={postulationEnCours === annonce.id}
                  className="bg-blue-600 text-white px-4 py-2 rounded text-sm hover:bg-blue-700 disabled:opacity-50"
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
