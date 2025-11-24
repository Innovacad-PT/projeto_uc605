// src/services/discounts.ts

import { apiClient } from "@utils/api";
import type { Discount } from "@_types/discount";

export const discountService = {
  /** Get all discounts */
  getAll: async (): Promise<Discount[]> => {
    return apiClient.get<Discount[]>("/discounts");
  },

  /** Get a single discount by id */
  getById: async (id: string): Promise<Discount> => {
    return apiClient.get<Discount>(`/discounts/${id}`);
  },

  /** Create a new discount */
  create: async (data: {
    id: string;
    productId: string;
    percentage: number;
    startDate: string;
    endDate: string;
  }): Promise<Discount> => {
    return apiClient.post<Discount>("/discounts", data);
  },

  /** Update an existing discount */
  update: async (
    id: string,
    data: {
      productId?: string;
      percentage?: number;
      startDate?: string;
      endDate?: string;
    }
  ): Promise<Discount> => {
    return apiClient.put<Discount>(`/discounts/${id}`, data);
  },

  /** Delete a discount */
  delete: async (id: string): Promise<void> => {
    await apiClient.delete<void>(`/discounts/${id}`);
  },
};
