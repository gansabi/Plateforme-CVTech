import { useState } from 'react'
import { useAuth } from '../../../app/providers/AuthContext'
import apiClient from '../../../shared/api/client'

export function AdminComptes() {
  const { utilisateur } = useAuth()
  const [compteId, setCompteId] = useState('')
  const [message, setMessage] = useState('')
  const [erreur, setErreur] = useState('')

  const bloquerCompte = async () => {
    if (!utilisateur || !compteId) return
    setMessage('')
    setErreur('')
    try {
      await apiClient.post(`/api/identite/admin/comptes/${compteId}/bloquer`, {
        administrateurId: utilisateur.utilisateurId,
        compteId,
      })
      setMessage('Compte bloqué.')
    } catch {
      setErreur('Erreur lors du blocage.')
    }
  }

  const reactiverCompte = async () => {
    if (!utilisateur || !compteId) return
    setMessage('')
    setErreur('')
    try {
      await apiClient.post(`/api/identite/admin/comptes/${compteId}/reactiver`, {
        administrateurId: utilisateur.utilisateurId,
        compteId,
      })
      setMessage('Compte réactivé.')
    } catch {
      setErreur('Erreur lors de la réactivation.')
    }
  }

  return (
    <div className="p-8 max-w-3xl mx-auto">
      <h1 className="text-2xl font-bold mb-6">Gestion des comptes</h1>
      {message && <p className="mb-4 p-3 bg-green-50 text-green-700 rounded">{message}</p>}
      {erreur && <p className="mb-4 p-3 bg-red-50 text-red-700 rounded">{erreur}</p>}

      <div className="bg-white p-6 rounded-lg shadow">
        <div className="mb-4">
          <label className="block text-sm font-medium text-gray-700 mb-1">ID du compte</label>
          <input type="text" value={compteId} onChange={e => setCompteId(e.target.value)}
            placeholder="ID du compte à gérer" className="w-full border rounded px-3 py-2" />
        </div>
        <div className="flex gap-3">
          <button onClick={bloquerCompte}
            className="bg-red-600 text-white px-6 py-2 rounded hover:bg-red-700">
            Bloquer
          </button>
          <button onClick={reactiverCompte}
            className="bg-green-600 text-white px-6 py-2 rounded hover:bg-green-700">
            Réactiver
          </button>
        </div>
      </div>
    </div>
  )
}
