import * as SecureStore from "expo-secure-store";
import {
  createContext,
  PropsWithChildren,
  useContext,
  useEffect,
  useState,
} from "react";
import Toast from "react-native-toast-message";

type AuthContextType = {
  isAuthenticated: boolean;
  token: string | null;
  isLoading: boolean;
  hasSeenSplashScreen: boolean;
  signIn: (email: string, password: string) => Promise<boolean>;
  signOut: () => Promise<void>;
  setHasSeenSplashScreen: (value: boolean) => Promise<void>;
};

const AuthContext = createContext<AuthContextType>({
  isAuthenticated: false,
  token: null,
  isLoading: true,
  hasSeenSplashScreen: false,
  signIn: async (email: string, password: string): Promise<boolean> => {
    return false;
  },
  signOut: async () => {},
  setHasSeenSplashScreen: async (value: boolean) => {},
});

export function AuthProvider({ children }: PropsWithChildren) {
  const [token, setToken] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [hasSeenSplashScreen, setSeenSplashScreen] = useState(false);

  useEffect(() => {
    async function loadAuthData() {
      try {
        const [storedToken, hasSeenSplash] = await Promise.all([
          SecureStore.getItemAsync("auth_token"),
          SecureStore.getItemAsync("hasSeenSplashScreen"),
        ]);

        if (storedToken) {
          setToken(storedToken);
        }

        console.log("Raw hasSeenSplash from store:", hasSeenSplash);

        if (hasSeenSplash === "true") {
          setSeenSplashScreen(true);
        }
      } catch (error) {
        console.error("Failed to load auth data", error);
      } finally {
        setIsLoading(false);
      }
    }

    loadAuthData();
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

  const setHasSeenSplashScreen = async (value: boolean) => {
    try {
      await SecureStore.setItemAsync("hasSeenSplashScreen", value.toString());
      setSeenSplashScreen(value);
      console.log("hasSeenSplashScreen", value);
    } catch (error) {
      console.error("Failed to save hasSeenSplashScreen", error);
    }
  };

  return (
    <AuthContext.Provider
      value={{
        isAuthenticated: !!token,
        token,
        isLoading,
        hasSeenSplashScreen,
        signIn,
        signOut,
        setHasSeenSplashScreen,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
}

export const useAuth = () => useContext(AuthContext);
