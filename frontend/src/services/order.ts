import { apiClient } from "@utils/api";

export interface OrderAdd {
  userId: string;
  total: number;
  status: string;
  products: Record<string, number>;
}

export interface OrderItemEntity {
  productId: string;
  quantity: number;
  unitPrice: number;
}

export interface OrderResult {
  id: number;
  userId: string;
  createdAt: string;
  total: number;
  status: string;
  orderItems: OrderItemEntity[];
}

export async function createOrder(order: OrderAdd) {
  const response = await apiClient.post<OrderResult>("/orders", order);

  if (!response) return null;

  return response;
}

export async function getOrdersByUserId(userId: string) {
  const response = await apiClient.get<OrderResult[]>(`/orders/user/${userId}`);
  return response || [];
}
