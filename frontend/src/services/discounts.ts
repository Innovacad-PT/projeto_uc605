import { apiClient } from "@utils/api";
import type { Discount } from "@_types/discount";

export const discountService = {
  getAll: async (): Promise<Discount[]> => {
    return apiClient.get<Discount[]>("/discounts");
  },

  getById: async (id: string): Promise<Discount> => {
    return apiClient.get<Discount>(`/discounts/${id}`);
  },

  create: async (data: {
    id: string;
    productId: string;
    percentage: number;
    startDate: number;
    endDate: number;
  }): Promise<Discount> => {
    return apiClient.post<Discount>("/discounts", data);
  },

  update: async (
    id: string,
    data: {
      productId: string;
      percentage: number;
      startDate: number;
      endDate: number;
    }
  ): Promise<Discount> => {
    return apiClient.put<Discount>(`/discounts/${id}`, data);
  },

  delete: async (id: string): Promise<void> => {
    await apiClient.delete<void>(`/discounts/${id}`);
  },
};
