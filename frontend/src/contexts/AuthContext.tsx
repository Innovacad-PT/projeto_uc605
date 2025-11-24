// src/contexts/AuthContext.tsx

import { createContext, useContext, useState, type ReactNode, useEffect } from "react";
import { login as loginApi, logout as logoutApi, getUserRole } from "@services/auth";

interface AuthContextProps {
  isAuthenticated: boolean;
  role: "admin" | "guest" | null;
  login: (email: string, password: string) => Promise<void>;
  logout: () => void;
}

const AuthContext = createContext<AuthContextProps | undefined>(undefined);

export const AuthProvider = ({ children }: { children: ReactNode }) => {
  const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);
  const [role, setRole] = useState<"admin" | "guest" | null>(null);

  useEffect(() => {
    const token = localStorage.getItem("accessToken");
    const storedRole = getUserRole();
    if (token) {
      setIsAuthenticated(true);
      setRole(storedRole);
    }
  }, []);

  const login = async (email: string, password: string) => {
    await loginApi(email, password);
    setIsAuthenticated(true);
    setRole(getUserRole());
  };

  const logout = () => {
    logoutApi();
    setIsAuthenticated(false);
    setRole(null);
  };

  return (
    <AuthContext.Provider value={{ isAuthenticated, role, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = (): AuthContextProps => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return context;
};
