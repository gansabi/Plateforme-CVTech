import { Link } from 'react-router-dom'
import { useAuth } from '../../app/providers/AuthContext'

export function Dashboard() {
  const { utilisateur, role } = useAuth()

  return (
    <div className="p-8 max-w-4xl mx-auto">
      <h1 className="text-2xl font-bold mb-2">Tableau de bord</h1>
      <p className="text-gray-600 mb-8">
        Bienvenue, <span className="font-medium">{utilisateur?.email}</span> —{' '}
        <span className="inline-block bg-blue-100 text-blue-800 text-xs px-2 py-0.5 rounded">{role}</span>
      </p>

      {role === 'Candidat' && (
        <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
          <DashCard titre="Mon CV" description="Créer ou modifier votre CV" lien="/candidat/cv" />
          <DashCard titre="Annonces" description="Parcourir les offres d'emploi" lien="/annonces" />
          <DashCard titre="Appels d'offre" description="Missions freelance disponibles" lien="/appels-offre" />
          <DashCard titre="Abonnements" description="Notifications par domaine" lien="/candidat/abonnements" />
          <DashCard titre="Actualités" description="Fil éditorial tech" lien="/actualites" />
        </div>
      )}

      {role === 'Entreprise' && (
        <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
          <DashCard titre="Publier une annonce" description="Créer une offre d'emploi" lien="/entreprise/publier-annonce" />
          <DashCard titre="Publier un appel d'offre" description="Mission freelance" lien="/entreprise/publier-appel-offre" />
          <DashCard titre="Candidatures reçues" description="Consulter les CV reçus" lien="/entreprise/candidatures" />
          <DashCard titre="Propositions reçues" description="Propositions freelance" lien="/entreprise/propositions" />
          <DashCard titre="Abonnements" description="Notifications par domaine" lien="/candidat/abonnements" />
        </div>
      )}

      {role === 'Administrateur' && (
        <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
          <DashCard titre="Modération" description="Modérer annonces et appels d'offre" lien="/admin/moderation" />
          <DashCard titre="Gestion comptes" description="Bloquer / réactiver des comptes" lien="/admin/comptes" />
          <DashCard titre="Publier un article" description="Fil d'actualité éditorial" lien="/admin/articles" />
          <DashCard titre="Annonces" description="Voir toutes les annonces" lien="/annonces" />
          <DashCard titre="Appels d'offre" description="Voir les missions freelance" lien="/appels-offre" />
          <DashCard titre="Actualités" description="Consulter le fil RSS" lien="/actualites" />
        </div>
      )}
    </div>
  )
}

function DashCard({ titre, description, lien }: { titre: string; description: string; lien: string }) {
  return (
    <Link to={lien} className="block p-6 bg-white rounded-lg border hover:shadow-md transition-shadow">
      <h3 className="font-semibold text-gray-800 mb-1">{titre}</h3>
      <p className="text-sm text-gray-500">{description}</p>
    </Link>
  )
}
