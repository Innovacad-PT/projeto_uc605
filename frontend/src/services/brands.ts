// src/services/brands.ts

import { apiClient } from "@utils/api";
import type { Brand } from "@_types/brand";

export const brandService = {
  /** Get all brands */
  getAll: async (): Promise<Brand[]> => {
    return apiClient.get<Brand[]>("/brands");
  },

  /** Get a single brand by id */
  getById: async (id: string): Promise<Brand> => {
    return apiClient.get<Brand>(`/brands/id/${id}`);
  },

  /** Create a new brand */
  create: async (data: { id: string; name: string }): Promise<Brand> => {
    return apiClient.post<Brand>("/brands", data);
  },

  /** Update an existing brand */
  update: async (id: string, data: { name: string }): Promise<Brand> => {
    return apiClient.put<Brand>(`/brands?id=${id}`, data);
  },

  /** Delete a brand */
  delete: async (id: string): Promise<void> => {
    await apiClient.delete<void>(`/brands?id=${id}`);
  },
};
