import apiClient from '../../shared/api/client'
import type {
  ConnexionRequest,
  ConnexionResponse,
  InscriptionCandidatRequest,
  InscriptionEntrepriseRequest,
  CompteResponse,
} from './types'

export async function connecterUtilisateur(data: ConnexionRequest): Promise<ConnexionResponse> {
  const response = await apiClient.post<ConnexionResponse>('/api/identite/auth/connexion', data)
  return response.data
}

export async function inscrireCandidat(data: InscriptionCandidatRequest): Promise<CompteResponse> {
  const response = await apiClient.post<CompteResponse>('/api/identite/comptes-candidats', data)
  return response.data
}

export async function inscrireEntreprise(data: InscriptionEntrepriseRequest): Promise<CompteResponse> {
  const response = await apiClient.post<CompteResponse>('/api/identite/comptes-entreprises', data)
  return response.data
}
