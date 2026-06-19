export interface ConnexionRequest {
  email: string
  motDePasse: string
}

export interface ConnexionResponse {
  utilisateurId: string
  email: string
  role: 'Candidat' | 'Entreprise' | 'Administrateur'
}

export interface InscriptionCandidatRequest {
  email: string
  motDePasse: string
}

export interface InscriptionEntrepriseRequest {
  email: string
  motDePasse: string
  nomEntreprise: string
}

export interface CompteResponse {
  compteId: string
}
