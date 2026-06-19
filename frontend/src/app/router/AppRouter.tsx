import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'
import { MainLayout } from '../layouts/MainLayout'
import { ProtectedRoute } from './ProtectedRoute'
import { useAuth } from '../providers/AuthContext'

// Identité
import { Connexion } from '../../modules/gestion-identite/pages/Connexion'
import { InscriptionCandidat } from '../../modules/gestion-identite/pages/InscriptionCandidat'
import { InscriptionEntreprise } from '../../modules/gestion-identite/pages/InscriptionEntreprise'
import { AdminComptes } from '../../modules/gestion-identite/pages/AdminComptes'
import { AdminModeration } from '../../modules/gestion-identite/pages/AdminModeration'
import { AdminArticles } from '../../modules/gestion-identite/pages/AdminArticles'

// Catalogue Emploi
import { Annonces } from '../../modules/catalogue-emploi/pages/Annonces'
import { MonCV } from '../../modules/catalogue-emploi/pages/MonCV'
import { PublierAnnonce } from '../../modules/catalogue-emploi/pages/PublierAnnonce'
import { CandidaturesRecues } from '../../modules/catalogue-emploi/pages/CandidaturesRecues'

// Appel Offre Freelance
import { AppelsOffre } from '../../modules/appel-offre-freelance/pages/AppelsOffre'
import { PublierAppelOffre } from '../../modules/appel-offre-freelance/pages/PublierAppelOffre'
import { PropositionsRecues } from '../../modules/appel-offre-freelance/pages/PropositionsRecues'

// Actualité
import { Actualites } from '../../modules/actualite-abonnement/pages/Actualites'
import { Abonnements } from '../../modules/actualite-abonnement/pages/Abonnements'

// Shared
import { Dashboard } from '../../shared/components/Dashboard'

function RedirectSiConnecte({ children }: { children: React.ReactNode }) {
  const { estConnecte } = useAuth()
  if (estConnecte) return <Navigate to="/dashboard" replace />
  return <>{children}</>
}

function NotFound() {
  return (
    <div className="p-12 text-center">
      <h1 className="text-4xl font-bold text-gray-800 mb-2">404</h1>
      <p className="text-gray-500">Page introuvable</p>
    </div>
  )
}

export function AppRouter() {
  return (
    <BrowserRouter>
      <Routes>
        <Route element={<MainLayout />}>
          {/* Public */}
          <Route path="/" element={<Annonces />} />
          <Route path="/annonces" element={<Annonces />} />
          <Route path="/appels-offre" element={<AppelsOffre />} />
          <Route path="/actualites" element={<Actualites />} />

          {/* Dashboard */}
          <Route path="/dashboard" element={
            <ProtectedRoute><Dashboard /></ProtectedRoute>
          } />

          {/* Candidat */}
          <Route path="/candidat/cv" element={
            <ProtectedRoute rolesAutorises={['Candidat']}><MonCV /></ProtectedRoute>
          } />
          <Route path="/candidat/abonnements" element={
            <ProtectedRoute rolesAutorises={['Candidat', 'Entreprise', 'Administrateur']}>
              <Abonnements />
            </ProtectedRoute>
          } />

          {/* Entreprise */}
          <Route path="/entreprise/publier-annonce" element={
            <ProtectedRoute rolesAutorises={['Entreprise']}><PublierAnnonce /></ProtectedRoute>
          } />
          <Route path="/entreprise/publier-appel-offre" element={
            <ProtectedRoute rolesAutorises={['Entreprise']}><PublierAppelOffre /></ProtectedRoute>
          } />
          <Route path="/entreprise/candidatures" element={
            <ProtectedRoute rolesAutorises={['Entreprise']}><CandidaturesRecues /></ProtectedRoute>
          } />
          <Route path="/entreprise/propositions" element={
            <ProtectedRoute rolesAutorises={['Entreprise']}><PropositionsRecues /></ProtectedRoute>
          } />

          {/* Administrateur */}
          <Route path="/admin/moderation" element={
            <ProtectedRoute rolesAutorises={['Administrateur']}><AdminModeration /></ProtectedRoute>
          } />
          <Route path="/admin/comptes" element={
            <ProtectedRoute rolesAutorises={['Administrateur']}><AdminComptes /></ProtectedRoute>
          } />
          <Route path="/admin/articles" element={
            <ProtectedRoute rolesAutorises={['Administrateur']}><AdminArticles /></ProtectedRoute>
          } />

          {/* 404 */}
          <Route path="*" element={<NotFound />} />
        </Route>

        {/* Auth (pleine page) */}
        <Route path="/connexion" element={
          <RedirectSiConnecte><Connexion /></RedirectSiConnecte>
        } />
        <Route path="/inscription/candidat" element={
          <RedirectSiConnecte><InscriptionCandidat /></RedirectSiConnecte>
        } />
        <Route path="/inscription/entreprise" element={
          <RedirectSiConnecte><InscriptionEntreprise /></RedirectSiConnecte>
        } />
      </Routes>
    </BrowserRouter>
  )
}
