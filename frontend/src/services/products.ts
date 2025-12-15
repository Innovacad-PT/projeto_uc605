import { apiClient, BASE_URL } from "@utils/api";
import type { Discount, Product } from "@_types/product";
import { logger } from "@utils/debug";
import { LogType } from "@_types/debug";

export const productService = {
  getAll: async (): Promise<Product[]> => {
    const products: Product[] = await apiClient.get<Product[]>("/products");

    if (!products || !Array.isArray(products)) {
      return [];
    }

    await Promise.all(
      products.map(async (product) => {
        try {
          product.discount = await apiClient.get<Discount>(
            `/products/${product.id}/discount`
          );
          logger(LogType.INFO, "Discount", product.discount);
        } catch (error) {
          logger(LogType.ERROR, "Failed to fetch discount", String(error));
        }

        if (product.imageUrl && !product.imageUrl.startsWith("https")) {
          product.imageUrl = `${BASE_URL}${product.imageUrl}`;
        }
      })
    );

    return products;
  },

  getById: async (id: string): Promise<Product> => {
    const product = await apiClient.get<Product>(`/products/${id}`);

    try {
      product.discount = await apiClient.get<Discount>(
        `/products/${id}/discount`
      );
    } catch (error) {
      logger(LogType.ERROR, "Failed to fetch discount", String(error));
    }

    if (product.imageUrl && !product.imageUrl.startsWith("https")) {
      product.imageUrl = `${BASE_URL}${product.imageUrl}`;
    }

    return product;
  },

  create: async (data: FormData): Promise<Product> => {
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
  update: async (
    id: string,
    payload: FormData | Partial<Product>
  ): Promise<Product> => {
    if (payload instanceof FormData) {
      const token = localStorage.getItem("accessToken");
      const baseUrl = import.meta.env.VITE_API_URL || "";

      const requestHeaders: HeadersInit = {};
      if (token) {
        requestHeaders["Authorization"] = `Bearer ${token}`;
      }

      console.log(payload);

      const response = await fetch(`${baseUrl}/products/${id}`, {
        method: "PUT",
        headers: requestHeaders,
        body: payload,
      });

      if (!response.ok) {
        const txt = await response.text();
        throw new Error(`Failed to update product: ${response.status} ${txt}`);
      }

      if (response.status === 204) {
        return {} as Product;
      }

      const apiResponse = await response.json();
      if (apiResponse.hasError) {
        throw new Error(
          `API Error [${apiResponse.code}]: ${apiResponse.message}`
        );
      }
      return apiResponse.value;
    }

    return apiClient.put<Product>(`/products/${id}`, payload);
  },
  delete: async (id: string): Promise<void> => {
    await apiClient.delete<void>(`/products/${id}`);
  },
};
