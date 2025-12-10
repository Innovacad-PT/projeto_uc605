import { apiClient } from "@utils/api";
import type UserType from "@_types/user";

export async function getUsers(): Promise<UserType[]> {
  const users = await apiClient.get<UserType[]>("/users");
  return users || [];
}

export async function getUser(id: string): Promise<UserType | undefined> {
  return await apiClient.get<UserType>(`/users/${id}`);
}

export async function createUser(
  user: Omit<UserType, "createdAt" | "role"> & {
    role: string;
    password?: string;
  }
): Promise<UserType | undefined> {
  return await apiClient.post<UserType>("/users", user);
}

export async function updateUser(
  id: string,
  user: Partial<UserType>
): Promise<UserType | undefined> {
  return await apiClient.put<UserType>(`/users/${id}`, user);
}

export async function deleteUser(id: string): Promise<void> {
  await apiClient.delete(`/users/${id}`);
}
