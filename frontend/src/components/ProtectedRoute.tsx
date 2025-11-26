// src/components/ProtectedRoute.tsx

import React from "react";
import { Navigate, useLocation } from "react-router-dom";
import { useAuth } from "@contexts/AuthContext";
import { logger } from "@utils/debug";
import { LogType } from "@_types/debug";

interface ProtectedRouteProps {
  /**
   * The component (or element) to render when access is allowed.
   */
  children: React.ReactNode;
  /**
   * Required role for this route. Currently only "admin" is used.
   */
  requiredRole?: "admin";
}

/**
 * Wrapper that checks authentication and role before rendering the child component.
 * If the user is not authenticated, they are redirected to the login page.
 * If a requiredRole is specified and the user does not have it, they are redirected to the home page.
 */
export const ProtectedRoute: React.FC<ProtectedRouteProps> = ({
  children,
  requiredRole,
}) => {
  const { isAuthenticated, role } = useAuth();
  const location = useLocation();

  if (!isAuthenticated) {
    logger(LogType.WARN, "Not authenticated");
    logger(LogType.INFO, "Is authenticated", isAuthenticated);
    logger(LogType.INFO, "Role", role);
    logger(LogType.INFO, "Required Role", requiredRole);
    // Preserve the attempted location so we can redirect back after login
    return <Navigate to="/login" state={{ from: location }} replace />;
  }

  if (requiredRole && role !== requiredRole) {
    logger(LogType.WARN, "Not authorized for this role");
    logger(LogType.INFO, "Role: ", role);
    logger(LogType.INFO, "Required role: ", requiredRole);
    // Not authorized for this role
    return <Navigate to="/" replace />;
  }

  return <>{children}</>;
};
