export interface Discount {
  id: string;
  productId: string;
  percentage: number;
  startDate: string; // ISO date-time
  endDate: string; // ISO date-time
}
