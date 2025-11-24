// src/services/products.ts

import { apiClient } from "@utils/api";
import type { Product } from "@_types/product";

export const productService = {
  /** Get all products */
  getAll: async (): Promise<Product[]> => {
    return apiClient.get<Product[]>("/products");
  },

  /** Get a single product by id */
  getById: async (id: string): Promise<Product> => {
    return apiClient.get<Product>(`/products/${id}`);
  },

  /** Create a new product (multipart/form-data) */
  create: async (data: FormData): Promise<Product> => {
    // For multipart we need to use fetch directly because apiClient uses JSON.
    const token = localStorage.getItem("accessToken");
    const baseUrl = import.meta.env.VITE_API_URL || "";
    console.log(data);
    const response = await fetch(`${baseUrl}/products`, {
      method: "POST",
      headers: token ? { Authorization: `Bearer ${token}` } : undefined,
      body: data,
    });
    if (!response.ok) {
      const txt = await response.text();
      throw new Error(`Failed to create product: ${response.status} ${txt}`);
    }
    const apiResponse = await response.json();
    if (apiResponse.hasError) {
      throw new Error(`API Error [${apiResponse.code}]: ${apiResponse.message}`);
    }
    return apiResponse.value;
  },

  /** Update an existing product */
  update: async (id: string, payload: Partial<Product>): Promise<Product> => {
    return apiClient.put<Product>(`/products/${id}`, payload);
  },

  /** Delete a product */
  delete: async (id: string): Promise<void> => {
    await apiClient.delete<void>(`/products/${id}`);
  },
};
