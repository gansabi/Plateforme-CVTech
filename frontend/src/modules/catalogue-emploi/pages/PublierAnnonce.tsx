import { useState } from 'react'
import { useAuth } from '../../../app/providers/AuthContext'
import apiClient from '../../../shared/api/client'

const TYPES_CONTRAT = ['CDI', 'CDD', 'Stage', 'Alternance', 'Apprentissage']

export function PublierAnnonce() {
  const { utilisateur } = useAuth()
  const [titre, setTitre] = useState('')
  const [description, setDescription] = useState('')
  const [typeContrat, setTypeContrat] = useState('CDI')
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
      await apiClient.post('/api/catalogue/annonces', {
        utilisateurId: utilisateur.utilisateurId,
        titre,
        description,
        typeContrat,
        domaineMetier,
      })
      setMessage('Annonce publiée avec succès.')
      setTitre('')
      setDescription('')
      setDomaineMetier('')
    } catch {
      setErreur('Erreur lors de la publication.')
    } finally {
      setChargement(false)
    }
  }

  return (
    <div className="p-8 max-w-2xl mx-auto">
      <h1 className="text-2xl font-bold mb-6">Publier une annonce d'emploi</h1>
      {message && <p className="mb-4 p-3 bg-green-50 text-green-700 rounded">{message}</p>}
      {erreur && <p className="mb-4 p-3 bg-red-50 text-red-700 rounded">{erreur}</p>}
      <form onSubmit={handleSubmit} className="space-y-4 bg-white p-6 rounded-lg shadow">
        <div>
          <label className="block text-sm font-medium text-gray-700">Titre du poste</label>
          <input type="text" value={titre} onChange={e => setTitre(e.target.value)}
            placeholder="Ex : Développeur .NET Senior"
            className="mt-1 w-full border rounded px-3 py-2" required />
        </div>
        <div>
          <label className="block text-sm font-medium text-gray-700">Description</label>
          <textarea value={description} onChange={e => setDescription(e.target.value)}
            placeholder="Décrivez le poste, les responsabilités..."
            className="mt-1 w-full border rounded px-3 py-2 h-32" required />
        </div>
        <div className="grid grid-cols-2 gap-4">
          <div>
            <label className="block text-sm font-medium text-gray-700">Type de contrat</label>
            <select value={typeContrat} onChange={e => setTypeContrat(e.target.value)}
              className="mt-1 w-full border rounded px-3 py-2">
              {TYPES_CONTRAT.map(t => <option key={t} value={t}>{t}</option>)}
            </select>
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700">Domaine métier</label>
            <input type="text" value={domaineMetier} onChange={e => setDomaineMetier(e.target.value)}
              placeholder="Ex : Cloud Azure"
              className="mt-1 w-full border rounded px-3 py-2" required />
          </div>
        </div>
        <button type="submit" disabled={chargement}
          className="w-full bg-blue-600 text-white py-2 rounded hover:bg-blue-700 disabled:opacity-50">
          {chargement ? 'Publication...' : 'Publier l\'annonce'}
        </button>
      </form>
    </div>
  )
}
