import { useState } from 'react'
import { useAuth } from '../../../app/providers/AuthContext'
import apiClient from '../../../shared/api/client'

export function AdminArticles() {
  const { utilisateur } = useAuth()
  const [titre, setTitre] = useState('')
  const [contenu, setContenu] = useState('')
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
      await apiClient.post('/api/actualite/articles', {
        auteurId: utilisateur.utilisateurId,
        titre,
        contenu,
        domaineMetier,
      })
      setMessage('Article publié avec succès.')
      setTitre('')
      setContenu('')
      setDomaineMetier('')
    } catch {
      setErreur('Erreur lors de la publication.')
    } finally {
      setChargement(false)
    }
  }

  return (
    <div className="p-8 max-w-2xl mx-auto">
      <h1 className="text-2xl font-bold mb-6">Publier un article d'actualité</h1>
      {message && <p className="mb-4 p-3 bg-green-50 text-green-700 rounded">{message}</p>}
      {erreur && <p className="mb-4 p-3 bg-red-50 text-red-700 rounded">{erreur}</p>}
      <form onSubmit={handleSubmit} className="space-y-4 bg-white p-6 rounded-lg shadow">
        <div>
          <label className="block text-sm font-medium text-gray-700">Titre</label>
          <input type="text" value={titre} onChange={e => setTitre(e.target.value)}
            placeholder="Titre de l'article" className="mt-1 w-full border rounded px-3 py-2" required />
        </div>
        <div>
          <label className="block text-sm font-medium text-gray-700">Contenu</label>
          <textarea value={contenu} onChange={e => setContenu(e.target.value)}
            placeholder="Rédigez l'article..."
            className="mt-1 w-full border rounded px-3 py-2 h-40" required />
        </div>
        <div>
          <label className="block text-sm font-medium text-gray-700">Domaine métier</label>
          <input type="text" value={domaineMetier} onChange={e => setDomaineMetier(e.target.value)}
            placeholder="Ex : Cloud Azure, DevOps, Data Science"
            className="mt-1 w-full border rounded px-3 py-2" required />
        </div>
        <button type="submit" disabled={chargement}
          className="w-full bg-blue-600 text-white py-2 rounded hover:bg-blue-700 disabled:opacity-50">
          {chargement ? 'Publication...' : 'Publier l\'article'}
        </button>
      </form>
    </div>
  )
}
