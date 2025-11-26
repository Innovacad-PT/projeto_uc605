// src/services/products.ts

import { apiClient, BASE_URL } from "@utils/api";
import type { Discount, Product } from "@_types/product";
import { logger } from "@utils/debug";
import { LogType } from "@_types/debug";

export const productService = {
  /** Get all products */
  getAll: async (): Promise<Product[]> => {
    let products: Product[] = await apiClient.get<Product[]>("/products");

    // Fetch discounts for all products in parallel and wait for all to complete
    await Promise.all(
      products.map(async (product) => {
        try {
          product.discount = await apiClient.get<Discount>(
            `/products/${product.id}/discount`
          );
          logger(LogType.INFO, "Discount", product.discount);
        } catch (error) {
          // If discount fetch fails, just leave it undefined
          logger(LogType.ERROR, "Failed to fetch discount", String(error));
        }
        
        // Prepend BASE_URL to imageUrl if it's a relative path
        if (product.imageUrl && !product.imageUrl.startsWith('https')) {
          product.imageUrl = `${BASE_URL}${product.imageUrl}`;
        }
      })
    );

    return products;
  },


  /** Get a single product by id */
  getById: async (id: string): Promise<Product> => {
    const product = await apiClient.get<Product>(`/products/${id}`);
    
    // Fetch discount
    try {
      product.discount = await apiClient.get<Discount>(`/products/${id}/discount`);
    } catch (error) {
      // If discount fetch fails, just leave it undefined
      logger(LogType.ERROR, "Failed to fetch discount", String(error));
    }
    
    // Prepend BASE_URL to imageUrl if it's a relative path
    if (product.imageUrl && !product.imageUrl.startsWith('https')) {
      product.imageUrl = `${BASE_URL}${product.imageUrl}`;
    }
    
    return product;
  },

  /** Create a new product (multipart/form-data) */
  create: async (data: FormData): Promise<Product> => {
    // For multipart we need to use fetch directly because apiClient uses JSON.
    const token = localStorage.getItem("accessToken");
    logger(LogType.INFO, "Product Data", data);
    const baseUrl = import.meta.env.VITE_API_URL || "";

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
      throw new Error(
        `API Error [${apiResponse.code}]: ${apiResponse.message}`
      );
    }
    return apiResponse.value;
  },

  /** Update an existing product */
  update: async (id: string, payload: Partial<Product>): Promise<Product> => {
    //payload.price = Number(payload.price?.replaceAll(".", ",").replaceAll("€", "")).toString();
    
    console.log(payload);
    return apiClient.put<Product>(`/products/${id}`, payload);
  },

  /** Delete a product */
  delete: async (id: string): Promise<void> => {
    await apiClient.delete<void>(`/products/${id}`);
  },
};
