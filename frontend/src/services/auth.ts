import type UserType from "@_types/user";
import { apiClient } from "@utils/api";
import { logger } from "@utils/debug";
import { LogType } from "@_types/debug";

export interface AuthResponse {
  token: string;
  user?: UserType;
}

export async function login(email: string, password: string): Promise<void> {
  const data = await apiClient.post<AuthResponse>("/login", {
    identifier: email,
    password: password,
    type: "EMAIL",
  });
  logger(LogType.INFO, "Login Request Data", data);

  if (data && data.token) {
    console.log("Saving token:", data.token);
    localStorage.setItem("accessToken", data.token);
  } else {
    console.error("No token in login response:", data);
  }

  if (data.user?.role) {
    localStorage.setItem("userRole", data.user?.role);
    localStorage.setItem("userId", data.user?.id);
  }
}

export async function register(user: {
  firstName: string;
  lastName: string;
  username: string;
  email: string;
  password: string;
}): Promise<void> {
  const payload = {
    ...user,
    password: user.password,
    role: "guest",
    createdAt: new Date().toISOString(),
  };
  const data = await apiClient.post<AuthResponse>("/register", payload);

  if (data.user?.role) {
    localStorage.setItem("userRole", data.user?.role);
    localStorage.setItem("userId", data.user?.id);
  }
}

export function isLoggedIn(): boolean {
  return !!localStorage.getItem("userId");
}

export function logout(): void {
  console.log("Logout called");
  logger(LogType.INFO, "Logout called", new Error().stack);
  localStorage.removeItem("userId");
  localStorage.removeItem("userRole");
  localStorage.removeItem("accessToken");
}

export function getToken(): string | null {
  return localStorage.getItem("accessToken");
}

export function getUserRole(): "admin" | "guest" | null {
  const role = localStorage.getItem("userRole");
  if (role === "admin" || role === "guest") return role as "admin" | "guest";
  return null;
}

export function getUserId(): string | null {
  return localStorage.getItem("userId");
}
