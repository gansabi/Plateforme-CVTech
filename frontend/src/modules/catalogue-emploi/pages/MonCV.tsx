import { useState } from 'react'
import { useAuth } from '../../../app/providers/AuthContext'
import { creerCV, modifierCV } from '../services'

export function MonCV() {
  const { utilisateur } = useAuth()
  const [titre, setTitre] = useState('')
  const [resume, setResume] = useState('')
  const [competences, setCompetences] = useState('')
  const [message, setMessage] = useState('')
  const [cvExiste, setCvExiste] = useState(false)
  const [chargement, setChargement] = useState(false)

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    if (!utilisateur) return
    setChargement(true)
    setMessage('')

    const competencesList = competences.split(',').map(c => c.trim()).filter(Boolean)
    const data = {
      candidatId: utilisateur.utilisateurId,
      titre,
      resume,
      competencesPrincipales: competencesList,
    }

    try {
      if (cvExiste) {
        await modifierCV(data)
        setMessage('CV mis à jour.')
      } else {
        await creerCV(data)
        setCvExiste(true)
        setMessage('CV créé.')
      }
    } catch {
      setMessage('Erreur lors de la sauvegarde du CV.')
    } finally {
      setChargement(false)
    }
  }

  return (
    <div className="p-8 max-w-2xl mx-auto">
      <h1 className="text-2xl font-bold mb-6">Mon Curriculum Vitae</h1>
      {message && <p className="mb-4 text-green-700 bg-green-50 p-2 rounded">{message}</p>}
      <form onSubmit={handleSubmit} className="space-y-4 bg-white p-6 rounded-lg shadow">
        <div>
          <label className="block text-sm font-medium text-gray-700">Titre</label>
          <input type="text" value={titre} onChange={e => setTitre(e.target.value)}
            placeholder="Ex : Développeur .NET Senior"
            className="mt-1 w-full border rounded px-3 py-2" required />
        </div>
        <div>
          <label className="block text-sm font-medium text-gray-700">Résumé</label>
          <textarea value={resume} onChange={e => setResume(e.target.value)}
            placeholder="Décrivez votre parcours..."
            className="mt-1 w-full border rounded px-3 py-2 h-32" />
        </div>
        <div>
          <label className="block text-sm font-medium text-gray-700">
            Compétences principales (séparées par des virgules)
          </label>
          <input type="text" value={competences} onChange={e => setCompetences(e.target.value)}
            placeholder="C#, .NET, Azure, Docker"
            className="mt-1 w-full border rounded px-3 py-2" />
        </div>
        <div className="flex gap-4">
          <button type="submit" disabled={chargement}
            className="bg-blue-600 text-white px-6 py-2 rounded hover:bg-blue-700 disabled:opacity-50">
            {chargement ? 'Sauvegarde...' : cvExiste ? 'Modifier' : 'Créer'}
          </button>
          {!cvExiste && (
            <button type="button" onClick={() => setCvExiste(true)}
              className="text-sm text-gray-500 hover:underline">
              J'ai déjà un CV (passer en mode modification)
            </button>
          )}
        </div>
      </form>
    </div>
  )
}
