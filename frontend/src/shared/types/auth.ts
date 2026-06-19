export type Role = 'Candidat' | 'Entreprise' | 'Administrateur'

export interface Utilisateur {
  utilisateurId: string
  email: string
  role: Role
}

export interface ConnexionRequest {
  email: string
  motDePasse: string
}
