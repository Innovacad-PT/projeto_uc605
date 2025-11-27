import {
  createContext,
  useContext,
  useEffect,
  useState,
  PropsWithChildren,
} from "react";
import * as SecureStore from "expo-secure-store";

type AuthContextType = {
  isAuthenticated: boolean;
  token: string | null;
  signIn: (email: string, password: string) => Promise<void>;
  signOut: () => Promise<void>;
  isLoading: boolean;
};

const AuthContext = createContext<AuthContextType>({
  isAuthenticated: false,
  token: null,
  signIn: async (email: string, password: string) => {},
  signOut: async () => {},
  isLoading: true,
});

export function AuthProvider({ children }: PropsWithChildren) {
  const [token, setToken] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    async function loadToken() {
      try {
        const storedToken = await SecureStore.getItemAsync("auth_token");
        if (storedToken) {
          setToken(storedToken);
        }
      } catch (error) {
        console.error("Failed to load token", error);
      } finally {
        setIsLoading(false);
      }
    }

    loadToken();
  }, []);

  const signIn = async (newToken: string) => {
    try {
      await SecureStore.setItemAsync("auth_token", newToken);
      setToken(newToken);
    } catch (error) {
      console.error("Failed to save token", error);
    }
  };

  const signOut = async () => {
    try {
      await SecureStore.deleteItemAsync("auth_token");
      setToken(null);
    } catch (error) {
      console.error("Failed to delete token", error);
    }
  };

  return (
    <AuthContext.Provider
      value={{
        isAuthenticated: !!token,
        token,
        signIn,
        signOut,
        isLoading,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
}

export const useAuth = () => useContext(AuthContext);
