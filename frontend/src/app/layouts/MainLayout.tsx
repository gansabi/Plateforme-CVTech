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
    <div className="min-h-screen bg-gray-50">
      <nav className="bg-white shadow-sm border-b sticky top-0 z-50">
        <div className="max-w-6xl mx-auto px-4 py-3 flex justify-between items-center">
          <div className="flex items-center gap-6">
            <Link to="/" className="text-xl font-bold text-blue-600">CVTech</Link>
            <Link to="/annonces" className="text-sm text-gray-600 hover:text-blue-600">Annonces</Link>
            <Link to="/appels-offre" className="text-sm text-gray-600 hover:text-blue-600">Freelance</Link>
            <Link to="/actualites" className="text-sm text-gray-600 hover:text-blue-600">Actualités</Link>
          </div>
          <div className="flex items-center gap-3">
            {estConnecte ? (
              <>
                <Link to="/dashboard" className="text-sm text-gray-600 hover:text-blue-600 font-medium">
                  Dashboard
                </Link>

                {role === 'Candidat' && (
                  <>
                    <Link to="/candidat/cv" className="text-sm text-gray-600 hover:text-blue-600">CV</Link>
                    <Link to="/candidat/abonnements" className="text-sm text-gray-600 hover:text-blue-600">Alertes</Link>
                  </>
                )}

                {role === 'Entreprise' && (
                  <>
                    <Link to="/entreprise/publier-annonce" className="text-sm text-gray-600 hover:text-blue-600">+ Annonce</Link>
                    <Link to="/entreprise/publier-appel-offre" className="text-sm text-gray-600 hover:text-blue-600">+ Appel d'offre</Link>
                    <Link to="/entreprise/candidatures" className="text-sm text-gray-600 hover:text-blue-600">Candidatures</Link>
                    <Link to="/entreprise/propositions" className="text-sm text-gray-600 hover:text-blue-600">Propositions</Link>
                  </>
                )}

                {role === 'Administrateur' && (
                  <>
                    <Link to="/admin/moderation" className="text-sm text-gray-600 hover:text-blue-600">Modération</Link>
                    <Link to="/admin/comptes" className="text-sm text-gray-600 hover:text-blue-600">Comptes</Link>
                    <Link to="/admin/articles" className="text-sm text-gray-600 hover:text-blue-600">Articles</Link>
                  </>
                )}

                <div className="flex items-center gap-2 ml-2 pl-2 border-l">
                  <span className="text-xs bg-blue-50 text-blue-700 px-2 py-0.5 rounded">{role}</span>
                  <span className="text-xs text-gray-500 hidden md:inline">{utilisateur?.email}</span>
                  <button onClick={handleDeconnexion}
                    className="text-xs text-red-500 hover:text-red-700 font-medium">Quitter</button>
                </div>
              </>
            ) : (
              <Link to="/connexion"
                className="text-sm bg-blue-600 text-white px-4 py-1.5 rounded hover:bg-blue-700 transition-colors">
                Connexion
              </Link>
            )}
          </div>
        </div>
      </nav>
      <main className="pb-12">
        <Outlet />
      </main>
    </div>
  )
}
