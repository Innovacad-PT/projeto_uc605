// src/services/categories.ts

import { apiClient } from "@utils/api";
import type { Category } from "@_types/category";

export const categoryService = {
  /** Get all categories */
  getAll: async (): Promise<Category[]> => {
    return apiClient.get<Category[]>("/categories");
  },

  /** Create a new category */
  create: async (data: { id: string; name: string }): Promise<Category> => {
    return apiClient.post<Category>("/categories", data);
  },
};
