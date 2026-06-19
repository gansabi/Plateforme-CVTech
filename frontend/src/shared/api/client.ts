import axios from 'axios'

const apiClient = axios.create({
  baseURL: '/',
  headers: {
    'Content-Type': 'application/json',
  },
})

apiClient.interceptors.request.use((config) => {
  const session = localStorage.getItem('cvtech_session')
  if (session) {
    const { utilisateurId } = JSON.parse(session)
    if (utilisateurId) {
      config.headers['X-Utilisateur-Id'] = utilisateurId
    }
  }
  return config
})

export default apiClient
