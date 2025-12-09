import { apiClient } from "@utils/api";
import type { Brand } from "@_types/brand";

export const brandService = {
  getAll: async (): Promise<Brand[]> => {
    return apiClient.get<Brand[]>("/brands");
  },

  getById: async (id: string): Promise<Brand> => {
    return apiClient.get<Brand>(`/brands/id/${id}`);
  },
  create: async (data: { id: string; name: string }): Promise<Brand> => {
    return apiClient.post<Brand>("/brands", data);
  },

  update: async (id: string, data: { name: string }): Promise<Brand> => {
    return apiClient.put<Brand>(`/brands?id=${id}`, data);
  },
  delete: async (id: string): Promise<void> => {
    await apiClient.delete<void>(`/brands?id=${id}`);
  },
};
