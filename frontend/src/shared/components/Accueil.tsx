import { Link } from 'react-router-dom'
import { useAuth } from '../../app/providers/AuthContext'

export function Accueil() {
  const { estConnecte } = useAuth()

  return (
    <div>
      {/* Hero section */}
      <section className="bg-gradient-to-br from-blue-600 via-indigo-600 to-purple-700 text-white">
        <div className="max-w-5xl mx-auto px-4 py-16 text-center">
          <h1 className="text-4xl md:text-5xl font-bold mb-4">
            Un CV, trois opportunités
          </h1>
          <p className="text-lg text-blue-100 max-w-2xl mx-auto mb-8">
            Postulez à des CDI, décrochez des missions freelance et restez informé
            sur l'actualité tech — sur une seule plateforme.
          </p>
          <div className="flex justify-center gap-4 flex-wrap">
            <Link to="/annonces"
              className="bg-white text-blue-700 px-6 py-3 rounded-lg font-medium hover:bg-blue-50 transition-colors">
              Voir les annonces
            </Link>
            <Link to="/appels-offre"
              className="bg-white/10 border border-white/30 text-white px-6 py-3 rounded-lg font-medium hover:bg-white/20 transition-colors">
              Missions freelance
            </Link>
          </div>
        </div>
      </section>

      {/* Stats */}
      <section className="max-w-5xl mx-auto px-4 py-12">
        <div className="grid grid-cols-1 md:grid-cols-3 gap-6 -mt-8">
          <StatCard emoji="💼" chiffre="CDI · CDD · Stage" label="Annonces d'emploi" />
          <StatCard emoji="🤝" chiffre="TJM · Missions" label="Appels d'offre freelance" />
          <StatCard emoji="📡" chiffre="RSS 2.0" label="Fil d'actualité tech" />
        </div>
      </section>

      {/* CTA */}
      {!estConnecte && (
        <section className="max-w-5xl mx-auto px-4 pb-12">
          <div className="bg-gray-50 rounded-2xl p-8 text-center border">
            <h2 className="text-xl font-bold text-gray-900 mb-2">Prêt à commencer ?</h2>
            <p className="text-gray-500 mb-6">Créez votre compte en quelques secondes</p>
            <div className="flex justify-center gap-4 flex-wrap">
              <Link to="/inscription/candidat"
                className="bg-blue-600 text-white px-6 py-2.5 rounded-lg font-medium hover:bg-blue-700 transition-colors">
                Je suis candidat
              </Link>
              <Link to="/inscription/entreprise"
                className="bg-white border border-gray-300 text-gray-700 px-6 py-2.5 rounded-lg font-medium hover:bg-gray-50 transition-colors">
                Je suis une entreprise
              </Link>
            </div>
          </div>
        </section>
      )}
    </div>
  )
}

function StatCard({ emoji, chiffre, label }: { emoji: string; chiffre: string; label: string }) {
  return (
    <div className="bg-white rounded-xl p-6 shadow-sm border text-center">
      <span className="text-3xl">{emoji}</span>
      <p className="font-semibold text-gray-900 mt-2">{chiffre}</p>
      <p className="text-sm text-gray-500">{label}</p>
    </div>
  )
}
