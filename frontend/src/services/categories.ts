import { apiClient } from "@utils/api";
import type { Category } from "@_types/category";

export const categoryService = {
  getAll: async (): Promise<Category[]> => {
    return apiClient.get<Category[]>("/categories");
  },

  create: async (data: { id: string; name: string }): Promise<Category> => {
    return apiClient.post<Category>("/categories", data);
  },

  update: async (id: string, data: Partial<Category>): Promise<Category> => {
    return apiClient.put<Category>(`/categories/${id}`, data);
  },

  delete: async (id: string): Promise<void> => {
    return apiClient.delete(`/categories/${id}`);
  },
};
