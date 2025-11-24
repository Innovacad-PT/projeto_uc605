// src/components/ProtectedRoute.tsx

import React from "react";
import { Navigate, useLocation } from "react-router-dom";
import { useAuth } from "@contexts/AuthContext";

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
export const ProtectedRoute: React.FC<ProtectedRouteProps> = ({ children, requiredRole }) => {
  const { isAuthenticated, role } = useAuth();
  const location = useLocation();

  if (!isAuthenticated) {
    console.log("Not authenticated");
    console.log("Is authenticated: ", isAuthenticated);
    console.log("Role: ", role);
    console.log("Required role: ", requiredRole);
    // Preserve the attempted location so we can redirect back after login
    return <Navigate to="/login" state={{ from: location }} replace />;
  }

  if (requiredRole && role !== requiredRole) {
    console.log("Not authorized for this role");
    console.log("Role: ", role);
    console.log("Required role: ", requiredRole);
    // Not authorized for this role
    return <Navigate to="/" replace />;
  }

  return <>{children}</>;
};
