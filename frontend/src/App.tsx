import { AuthProvider } from './app/providers/AuthContext'
import { QueryProvider } from './app/providers/QueryProvider'
import { AppRouter } from './app/router/AppRouter'

function App() {
  return (
    <QueryProvider>
      <AuthProvider>
        <AppRouter />
      </AuthProvider>
    </QueryProvider>
  )
}

export default App
