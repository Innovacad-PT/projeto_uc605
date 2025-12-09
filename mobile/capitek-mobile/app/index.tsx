import { Redirect } from "expo-router";
import { useAuth } from "../context/use_auth_context";

export default function Index() {
  const { hasSeenSplashScreen, isLoading } = useAuth();

  if (isLoading) {
    return null;
  }

  return <Redirect href={hasSeenSplashScreen ? "/home" : "/splash"} />;
}
