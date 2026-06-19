import { useQuery } from '@tanstack/react-query'
import apiClient from '../../../shared/api/client'

interface Article {
  id: string
  titre: string
  contenu: string
  domaineMetier: string
  datePublication: string
}

// Fallback : on récupère le contenu RSS et on l'affiche simplement
export function Actualites() {
  const { data, isLoading, error } = useQuery({
    queryKey: ['actualites'],
    queryFn: async () => {
      try {
        // Tentative via un parsing simple du flux RSS
        const response = await apiClient.get('/feed/rss', { responseType: 'text' })
        const parser = new DOMParser()
        const xml = parser.parseFromString(response.data as string, 'application/xml')
        const items = xml.querySelectorAll('item')
        const articles: Article[] = []
        items.forEach(item => {
          articles.push({
            id: item.querySelector('guid')?.textContent ?? '',
            titre: item.querySelector('title')?.textContent ?? '',
            contenu: item.querySelector('description')?.textContent ?? '',
            domaineMetier: item.querySelector('category')?.textContent ?? '',
            datePublication: item.querySelector('pubDate')?.textContent ?? '',
          })
        })
        return articles
      } catch {
        return []
      }
    },
  })

  if (isLoading) return <p className="p-8">Chargement des actualités...</p>
  if (error) return <p className="p-8 text-red-600">Erreur de chargement</p>

  return (
    <div className="p-8 max-w-4xl mx-auto">
      <h1 className="text-2xl font-bold mb-2">Actualités Tech</h1>
      <p className="text-sm text-gray-500 mb-6">Fil éditorial — disponible en RSS : <a href="/feed/rss" className="text-blue-600 underline">/feed/rss</a></p>
      {(!data || data.length === 0) && <p className="text-gray-500">Aucun article publié pour le moment.</p>}
      <div className="space-y-4">
        {data?.map(article => (
          <article key={article.id} className="bg-white border rounded-lg p-5">
            <h2 className="text-lg font-semibold mb-1">{article.titre}</h2>
            <p className="text-xs text-gray-400 mb-2">{article.domaineMetier} · {article.datePublication}</p>
            <p className="text-gray-700 text-sm">{article.contenu}</p>
          </article>
        ))}
      </div>
    </div>
  )
}
