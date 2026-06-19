import { useState } from 'react'
import { useAuth } from '../../../app/providers/AuthContext'
import apiClient from '../../../shared/api/client'

export function AdminModeration() {
  const { utilisateur } = useAuth()
  const [annonceId, setAnnonceId] = useState('')
  const [appelOffreId, setAppelOffreId] = useState('')
  const [message, setMessage] = useState('')
  const [erreur, setErreur] = useState('')

  const action = async (url: string, body: Record<string, string>) => {
    setMessage('')
    setErreur('')
    try {
      await apiClient.post(url, body)
      setMessage('Action effectuée avec succès.')
    } catch {
      setErreur('Erreur lors de l\'action.')
    }
  }

  const modererAnnonce = () => {
    if (!utilisateur || !annonceId) return
    action('/api/catalogue/moderation/moderer', {
      administrateurId: utilisateur.utilisateurId,
      annonceId,
    })
  }

  const supprimerAnnonce = () => {
    if (!utilisateur || !annonceId) return
    action('/api/catalogue/moderation/supprimer', {
      administrateurId: utilisateur.utilisateurId,
      annonceId,
    })
  }

  const modererAppelOffre = () => {
    if (!utilisateur || !appelOffreId) return
    action('/api/freelance/moderation/moderer', {
      administrateurId: utilisateur.utilisateurId,
      appelOffreId,
    })
  }

  const supprimerAppelOffre = () => {
    if (!utilisateur || !appelOffreId) return
    action('/api/freelance/moderation/supprimer', {
      administrateurId: utilisateur.utilisateurId,
      appelOffreId,
    })
  }

  return (
    <div className="p-8 max-w-3xl mx-auto">
      <h1 className="text-2xl font-bold mb-6">Modération</h1>
      {message && <p className="mb-4 p-3 bg-green-50 text-green-700 rounded">{message}</p>}
      {erreur && <p className="mb-4 p-3 bg-red-50 text-red-700 rounded">{erreur}</p>}

      <section className="bg-white p-6 rounded-lg shadow mb-6">
        <h2 className="text-lg font-semibold mb-4">Annonces d'emploi</h2>
        <div className="flex gap-3 mb-3">
          <input type="text" value={annonceId} onChange={e => setAnnonceId(e.target.value)}
            placeholder="ID de l'annonce" className="flex-1 border rounded px-3 py-2" />
        </div>
        <div className="flex gap-3">
          <button onClick={modererAnnonce}
            className="bg-yellow-500 text-white px-4 py-2 rounded hover:bg-yellow-600">
            Modérer (désactiver)
          </button>
          <button onClick={supprimerAnnonce}
            className="bg-red-600 text-white px-4 py-2 rounded hover:bg-red-700">
            Supprimer
          </button>
        </div>
      </section>

      <section className="bg-white p-6 rounded-lg shadow">
        <h2 className="text-lg font-semibold mb-4">Appels d'offre</h2>
        <div className="flex gap-3 mb-3">
          <input type="text" value={appelOffreId} onChange={e => setAppelOffreId(e.target.value)}
            placeholder="ID de l'appel d'offre" className="flex-1 border rounded px-3 py-2" />
        </div>
        <div className="flex gap-3">
          <button onClick={modererAppelOffre}
            className="bg-yellow-500 text-white px-4 py-2 rounded hover:bg-yellow-600">
            Modérer (fermer)
          </button>
          <button onClick={supprimerAppelOffre}
            className="bg-red-600 text-white px-4 py-2 rounded hover:bg-red-700">
            Supprimer
          </button>
        </div>
      </section>
    </div>
  )
}
