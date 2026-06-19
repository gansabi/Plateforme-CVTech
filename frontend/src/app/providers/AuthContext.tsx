import { createContext, useContext, useState, useCallback, type ReactNode } from 'react'
import type { Utilisateur, Role } from '../../shared/types/auth'

interface AuthContextType {
  utilisateur: Utilisateur | null
  estConnecte: boolean
  role: Role | null
  connecter: (utilisateur: Utilisateur) => void
  deconnecter: () => void
}

const AuthContext = createContext<AuthContextType | undefined>(undefined)

function chargerSession(): Utilisateur | null {
  const stored = localStorage.getItem('cvtech_session')
  if (!stored) return null
  try {
    return JSON.parse(stored) as Utilisateur
  } catch {
    return null
  }
}

export function AuthProvider({ children }: { children: ReactNode }) {
  const [utilisateur, setUtilisateur] = useState<Utilisateur | null>(chargerSession)

  const connecter = useCallback((u: Utilisateur) => {
    localStorage.setItem('cvtech_session', JSON.stringify(u))
    setUtilisateur(u)
  }, [])

  const deconnecter = useCallback(() => {
    localStorage.removeItem('cvtech_session')
    setUtilisateur(null)
  }, [])

  return (
    <AuthContext.Provider
      value={{
        utilisateur,
        estConnecte: utilisateur !== null,
        role: utilisateur?.role ?? null,
        connecter,
        deconnecter,
      }}
    >
      {children}
    </AuthContext.Provider>
  )
}

export function useAuth(): AuthContextType {
  const context = useContext(AuthContext)
  if (!context) {
    throw new Error('useAuth doit être utilisé dans un AuthProvider')
  }
  return context
}
