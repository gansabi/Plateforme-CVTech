import { Link } from 'react-router-dom'
import { useAuth } from '../../app/providers/AuthContext'

export function Dashboard() {
  const { utilisateur, role } = useAuth()

  return (
    <div className="p-8 max-w-5xl mx-auto">
      <div className="mb-8">
        <h1 className="text-2xl font-bold text-gray-900">Tableau de bord</h1>
        <p className="text-gray-500 mt-1">
          Bienvenue, <span className="font-medium text-gray-700">{utilisateur?.email}</span>
          <span className="ml-2 inline-block bg-blue-100 text-blue-800 text-xs font-medium px-2.5 py-0.5 rounded-full">{role}</span>
        </p>
      </div>

      {role === 'Candidat' && (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-5">
          <DashCard emoji="📄" titre="Mon CV" description="Créer ou modifier votre CV" lien="/candidat/cv" couleur="blue" />
          <DashCard emoji="💼" titre="Annonces" description="Parcourir les offres d'emploi" lien="/annonces" couleur="green" />
          <DashCard emoji="🤝" titre="Appels d'offre" description="Missions freelance disponibles" lien="/appels-offre" couleur="emerald" />
          <DashCard emoji="🔔" titre="Abonnements" description="Notifications par domaine métier" lien="/candidat/abonnements" couleur="orange" />
          <DashCard emoji="📰" titre="Actualités" description="Fil éditorial tech" lien="/actualites" couleur="purple" />
        </div>
      )}

      {role === 'Entreprise' && (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-5">
          <DashCard emoji="📝" titre="Publier une annonce" description="Créer une offre d'emploi" lien="/entreprise/publier-annonce" couleur="blue" />
          <DashCard emoji="🤝" titre="Publier un appel d'offre" description="Mission freelance" lien="/entreprise/publier-appel-offre" couleur="emerald" />
          <DashCard emoji="📩" titre="Candidatures reçues" description="Consulter les CV reçus" lien="/entreprise/candidatures" couleur="orange" />
          <DashCard emoji="📊" titre="Propositions reçues" description="Propositions freelance" lien="/entreprise/propositions" couleur="purple" />
          <DashCard emoji="🔔" titre="Abonnements" description="Notifications par domaine" lien="/candidat/abonnements" couleur="teal" />
        </div>
      )}

      {role === 'Administrateur' && (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-5">
          <DashCard emoji="🛡️" titre="Modération" description="Modérer annonces et appels d'offre" lien="/admin/moderation" couleur="red" />
          <DashCard emoji="👥" titre="Gestion comptes" description="Bloquer / réactiver des comptes" lien="/admin/comptes" couleur="orange" />
          <DashCard emoji="✍️" titre="Publier un article" description="Fil d'actualité éditorial" lien="/admin/articles" couleur="purple" />
          <DashCard emoji="💼" titre="Annonces" description="Voir toutes les annonces" lien="/annonces" couleur="blue" />
          <DashCard emoji="🤝" titre="Appels d'offre" description="Voir les missions freelance" lien="/appels-offre" couleur="emerald" />
          <DashCard emoji="📰" titre="Actualités" description="Consulter le fil RSS" lien="/actualites" couleur="teal" />
        </div>
      )}
    </div>
  )
}

const COULEURS: Record<string, string> = {
  blue: 'border-l-blue-500 hover:bg-blue-50',
  green: 'border-l-green-500 hover:bg-green-50',
  emerald: 'border-l-emerald-500 hover:bg-emerald-50',
  orange: 'border-l-orange-500 hover:bg-orange-50',
  purple: 'border-l-purple-500 hover:bg-purple-50',
  red: 'border-l-red-500 hover:bg-red-50',
  teal: 'border-l-teal-500 hover:bg-teal-50',
}

function DashCard({ emoji, titre, description, lien, couleur }: {
  emoji: string; titre: string; description: string; lien: string; couleur: string
}) {
  return (
    <Link to={lien}
      className={`block p-5 bg-white rounded-xl border border-gray-200 border-l-4 ${COULEURS[couleur] ?? ''} hover:shadow-md transition-all`}>
      <div className="flex items-start gap-3">
        <span className="text-2xl">{emoji}</span>
        <div>
          <h3 className="font-semibold text-gray-800">{titre}</h3>
          <p className="text-sm text-gray-500 mt-0.5">{description}</p>
        </div>
      </div>
    </Link>
  )
}
