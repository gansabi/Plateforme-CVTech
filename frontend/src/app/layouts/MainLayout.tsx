import { Link, Outlet, useNavigate } from 'react-router-dom'
import { useAuth } from '../providers/AuthContext'

export function MainLayout() {
  const { utilisateur, estConnecte, role, deconnecter } = useAuth()
  const navigate = useNavigate()

  const handleDeconnexion = () => {
    deconnecter()
    navigate('/connexion')
  }

  return (
    <div className="min-h-screen bg-gray-50 flex flex-col">
      {/* Header */}
      <nav className="bg-gradient-to-r from-blue-600 to-indigo-700 shadow-lg sticky top-0 z-50">
        <div className="max-w-6xl mx-auto px-4 py-3 flex justify-between items-center">
          <div className="flex items-center gap-6">
            <Link to="/" className="text-xl font-bold text-white tracking-tight">
              💼 CVTech
            </Link>
            <Link to="/annonces" className="text-sm text-blue-100 hover:text-white transition-colors">Annonces</Link>
            <Link to="/appels-offre" className="text-sm text-blue-100 hover:text-white transition-colors">Freelance</Link>
            <Link to="/actualites" className="text-sm text-blue-100 hover:text-white transition-colors">Actualités</Link>
          </div>
          <div className="flex items-center gap-3">
            {estConnecte ? (
              <>
                <Link to="/dashboard" className="text-sm text-white font-medium hover:text-blue-200 transition-colors">
                  Dashboard
                </Link>

                {role === 'Candidat' && (
                  <>
                    <Link to="/candidat/cv" className="text-sm text-blue-100 hover:text-white">CV</Link>
                    <Link to="/candidat/abonnements" className="text-sm text-blue-100 hover:text-white">Alertes</Link>
                  </>
                )}

                {role === 'Entreprise' && (
                  <>
                    <Link to="/entreprise/publier-annonce" className="text-sm text-blue-100 hover:text-white">+ Annonce</Link>
                    <Link to="/entreprise/publier-appel-offre" className="text-sm text-blue-100 hover:text-white">+ Mission</Link>
                    <Link to="/entreprise/candidatures" className="text-sm text-blue-100 hover:text-white">Candidatures</Link>
                    <Link to="/entreprise/propositions" className="text-sm text-blue-100 hover:text-white">Propositions</Link>
                  </>
                )}

                {role === 'Administrateur' && (
                  <>
                    <Link to="/admin/moderation" className="text-sm text-blue-100 hover:text-white">Modération</Link>
                    <Link to="/admin/comptes" className="text-sm text-blue-100 hover:text-white">Comptes</Link>
                    <Link to="/admin/articles" className="text-sm text-blue-100 hover:text-white">Articles</Link>
                  </>
                )}

                <div className="flex items-center gap-2 ml-2 pl-2 border-l border-blue-400">
                  <span className="text-xs bg-white/20 text-white px-2 py-0.5 rounded">{role}</span>
                  <span className="text-xs text-blue-200 hidden md:inline">{utilisateur?.email}</span>
                  <button onClick={handleDeconnexion}
                    className="text-xs text-blue-200 hover:text-white font-medium transition-colors">
                    Quitter
                  </button>
                </div>
              </>
            ) : (
              <Link to="/connexion"
                className="text-sm bg-white text-blue-700 px-4 py-1.5 rounded-lg font-medium hover:bg-blue-50 transition-colors">
                Connexion
              </Link>
            )}
          </div>
        </div>
      </nav>

      {/* Main content */}
      <main className="flex-1 pb-12">
        <Outlet />
      </main>

      {/* Footer */}
      <footer className="bg-gray-800 text-gray-400 py-6">
        <div className="max-w-6xl mx-auto px-4 flex justify-between items-center text-xs">
          <p>© 2026 Plateforme-CVTech — Monolithe Modulaire .NET 10</p>
          <div className="flex gap-4">
            <a href="/feed/rss" className="hover:text-white transition-colors">📡 Flux RSS</a>
            <a href="/swagger" target="_blank" rel="noreferrer" className="hover:text-white transition-colors">API Swagger</a>
          </div>
        </div>
      </footer>
    </div>
  )
}
