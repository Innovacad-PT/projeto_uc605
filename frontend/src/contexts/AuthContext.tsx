import {
  createContext,
  useContext,
  useState,
  type ReactNode,
  useEffect,
} from "react";
import {
  login as loginApi,
  logout as logoutApi,
  getUserRole,
  getUserId,
} from "@services/auth";
import { apiClient } from "@utils/api";
import type UserType from "@_types/user";

interface AuthContextProps {
  isAuthenticated: boolean;
  role: "admin" | "guest" | null;
  login: (email: string, password: string) => Promise<void>;
  logout: () => void;
  exists: () => Promise<boolean>;
}

const AuthContext = createContext<AuthContextProps | undefined>(undefined);

export const AuthProvider = ({ children }: { children: ReactNode }) => {
  const [isAuthenticated, setIsAuthenticated] = useState<boolean>(() => {
    return !!localStorage.getItem("accessToken");
  });
  const [role, setRole] = useState<"admin" | "guest" | null>(() => {
    return getUserRole();
  });

  useEffect(() => {}, []);

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

  const exists = async (): Promise<boolean> => {
    try {
      const userId = getUserId();
      if (!userId) {
        return false;
      }

      const result = await apiClient.get<UserType>("/users/" + userId);

      if (!result) {
        return true;
      }
      return true;
    } catch (e) {
      return true;
    }
  };

  return (
    <AuthContext.Provider
      value={{ isAuthenticated, role, login, logout, exists }}
    >
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
