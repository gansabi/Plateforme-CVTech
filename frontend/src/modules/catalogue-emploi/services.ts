import apiClient from '../../shared/api/client'
import type {
  ConsulterAnnoncesResponse,
  PostulerAnnonceRequest,
  CreerCVRequest,
  CreerCVResponse,
  ModifierCVRequest,
} from './types'

export async function consulterAnnonces(): Promise<ConsulterAnnoncesResponse> {
  const response = await apiClient.get<ConsulterAnnoncesResponse>('/api/catalogue/annonces')
  return response.data
}

export async function postulerAnnonce(data: PostulerAnnonceRequest): Promise<void> {
  await apiClient.post('/api/catalogue/candidatures', data)
}

export async function creerCV(data: CreerCVRequest): Promise<CreerCVResponse> {
  const response = await apiClient.post<CreerCVResponse>('/api/catalogue/cv', data)
  return response.data
}

export async function modifierCV(data: ModifierCVRequest): Promise<void> {
  await apiClient.put('/api/catalogue/cv', data)
}
