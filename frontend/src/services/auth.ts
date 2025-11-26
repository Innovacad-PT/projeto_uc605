// src/services/auth.ts

import type UserType from "@_types/user";
import { apiClient } from "@utils/api";
import { logger } from "@utils/debug";
import { LogType } from "@_types/debug";

export interface AuthResponse {
  token: string;
  user?: UserType;
}

/**
 * Perform login with email and password.
 * Stores the JWT token in localStorage.
 */
export async function login(email: string, password: string): Promise<void> {
  const data = await apiClient.post<AuthResponse>("/login", {
    identifier: email,
    passwordHash: password,
    type: "EMAIL",
  });
  logger(LogType.INFO, "Login Request Data", data);

  localStorage.setItem("accessToken", data.token);
  if (data.user?.role) {
    localStorage.setItem("userRole", data.user?.role);
  }
}

/**
 * Register a new user.
 */
export async function register(user: {
  firstName: string;
  lastName: string;
  username: string;
  email: string;
  password: string;
}): Promise<void> {
  const payload = {
    ...user,
    passwordHash: user.password,
    role: "guest",
    createdAt: new Date().toISOString(),
  };
  const data = await apiClient.post<AuthResponse>("/register", payload);

  if (data.user?.role) {
    localStorage.setItem("userRole", data.user?.role);
  }
}

/**
 * Logout the current user.
 */
export function logout(): void {
  localStorage.removeItem("accessToken");
  localStorage.removeItem("userRole");
}

/**
 * Retrieve stored JWT token.
 */
export function getToken(): string | null {
  return localStorage.getItem("accessToken");
}

/**
 * Retrieve current user role (admin or guest).
 */
export function getUserRole(): "admin" | "guest" | null {
  const role = localStorage.getItem("userRole");
  if (role === "admin" || role === "guest") return role as "admin" | "guest";
  return null;
}
