import { useState } from 'react'
import { useAuth } from '../../../app/providers/AuthContext'
import apiClient from '../../../shared/api/client'

export function PublierAppelOffre() {
  const { utilisateur } = useAuth()
  const [titre, setTitre] = useState('')
  const [description, setDescription] = useState('')
  const [domaineMetier, setDomaineMetier] = useState('')
  const [tjmMin, setTjmMin] = useState('')
  const [tjmMax, setTjmMax] = useState('')
  const [dateLimite, setDateLimite] = useState('')
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
      await apiClient.post('/api/freelance/appels-offre', {
        utilisateurId: utilisateur.utilisateurId,
        titre,
        description,
        domaineMetier,
        tjmMinimum: Number(tjmMin),
        tjmMaximum: Number(tjmMax),
        dateLimite: new Date(dateLimite).toISOString(),
      })
      setMessage('Appel d\'offre publié.')
      setTitre('')
      setDescription('')
      setDomaineMetier('')
      setTjmMin('')
      setTjmMax('')
      setDateLimite('')
    } catch {
      setErreur('Erreur lors de la publication.')
    } finally {
      setChargement(false)
    }
  }

  return (
    <div className="p-8 max-w-2xl mx-auto">
      <h1 className="text-2xl font-bold mb-6">Publier un appel d'offre</h1>
      {message && <p className="mb-4 p-3 bg-green-50 text-green-700 rounded">{message}</p>}
      {erreur && <p className="mb-4 p-3 bg-red-50 text-red-700 rounded">{erreur}</p>}
      <form onSubmit={handleSubmit} className="space-y-4 bg-white p-6 rounded-lg shadow">
        <div>
          <label className="block text-sm font-medium text-gray-700">Titre de la mission</label>
          <input type="text" value={titre} onChange={e => setTitre(e.target.value)}
            placeholder="Ex : Architecture Cloud Azure"
            className="mt-1 w-full border rounded px-3 py-2" required />
        </div>
        <div>
          <label className="block text-sm font-medium text-gray-700">Description / Cahier des charges</label>
          <textarea value={description} onChange={e => setDescription(e.target.value)}
            placeholder="Décrivez le contexte, les livrables attendus..."
            className="mt-1 w-full border rounded px-3 py-2 h-32" required />
        </div>
        <div>
          <label className="block text-sm font-medium text-gray-700">Domaine métier</label>
          <input type="text" value={domaineMetier} onChange={e => setDomaineMetier(e.target.value)}
            placeholder="Ex : Cloud Azure" className="mt-1 w-full border rounded px-3 py-2" required />
        </div>
        <div className="grid grid-cols-3 gap-4">
          <div>
            <label className="block text-sm font-medium text-gray-700">TJM Min (€)</label>
            <input type="number" value={tjmMin} onChange={e => setTjmMin(e.target.value)}
              placeholder="400" className="mt-1 w-full border rounded px-3 py-2" required />
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700">TJM Max (€)</label>
            <input type="number" value={tjmMax} onChange={e => setTjmMax(e.target.value)}
              placeholder="800" className="mt-1 w-full border rounded px-3 py-2" required />
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700">Date limite</label>
            <input type="date" value={dateLimite} onChange={e => setDateLimite(e.target.value)}
              className="mt-1 w-full border rounded px-3 py-2" required />
          </div>
        </div>
        <button type="submit" disabled={chargement}
          className="w-full bg-blue-600 text-white py-2 rounded hover:bg-blue-700 disabled:opacity-50">
          {chargement ? 'Publication...' : 'Publier l\'appel d\'offre'}
        </button>
      </form>
    </div>
  )
}
