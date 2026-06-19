export interface AnnonceResume {
  id: string
  titre: string
  typeContrat: string
  domaineMetier: string
  datePublication: string
}

export interface ConsulterAnnoncesResponse {
  annonces: AnnonceResume[]
}

export interface PostulerAnnonceRequest {
  candidatId: string
  annonceId: string
  lettreMotivation?: string
}

export interface CreerCVRequest {
  candidatId: string
  titre: string
  resume: string
  competencesPrincipales: string[]
}

export interface ModifierCVRequest {
  candidatId: string
  titre: string
  resume: string
  competencesPrincipales: string[]
}

export interface CreerCVResponse {
  curriculumVitaeId: string
}
