import { Navigate } from 'react-router-dom'
import { useAuth } from '../providers/AuthContext'
import type { Role } from '../../shared/types/auth'

interface ProtectedRouteProps {
  children: React.ReactNode
  rolesAutorises?: Role[]
}

export function ProtectedRoute({ children, rolesAutorises }: ProtectedRouteProps) {
  const { estConnecte, role } = useAuth()

  if (!estConnecte) {
    return <Navigate to="/connexion" replace />
  }

  if (rolesAutorises && role && !rolesAutorises.includes(role)) {
    return <Navigate to="/" replace />
  }

  return <>{children}</>
}
