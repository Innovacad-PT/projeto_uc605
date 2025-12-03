import {
  createContext,
  useContext,
  useEffect,
  useState,
  PropsWithChildren,
} from "react";
import * as SecureStore from "expo-secure-store";
import Toast from "react-native-toast-message";

type AuthContextType = {
  isAuthenticated: boolean;
  token: string | null;
  signIn: (email: string, password: string) => Promise<boolean>;
  signOut: () => Promise<void>;
  isLoading: boolean;
};

const AuthContext = createContext<AuthContextType>({
  isAuthenticated: false,
  token: null,
  signIn: async (email: string, password: string): Promise<boolean> => {
    return false;
  },
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

  const signIn = async (email: string, password: string) => {
    try {
      const response = await fetch("http://148.63.1.66:7000/login", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          identifier: email,
          passwordHash: password,
          type: "email",
        }),
      });

      if (response.status === 400) {
        Toast.show({
          type: "error",
          text1: "Erro",
          text2: "Credenciais inválidas",
        });
        console.log(await response.json());
        return false;
      }

      if (response.status === 500) {
        throw new Error("Error no servidor");
      }

      const data = await response.json();

      if (data.hasError) {
        Toast.show({
          type: "error",
          text1: "Erro",
          text2: data.message,
        });
        return false;
      }

      const newToken = data.value.token;

      if (!newToken) {
        Toast.show({
          type: "error",
          text1: "Erro",
          text2: "Erro ao fazer login, tente novamente.",
        });
        return false;
      }

      await SecureStore.setItemAsync("auth_token", newToken);
      setToken(newToken);
      return true;
    } catch (error) {
      console.error("Failed to save token", error);
      return false;
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
