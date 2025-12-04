export type Brand = {
  id: string;
  name: string;
};

export type Category = {
  id: string;
  name: string;
};

export type TechnicalSpec = {
  key: string;
  value: string;
};

export type Review = {
  id: string;
  rating: number;
  comment: string;
  createdAt: string;
};

export type Discount = {
  id: string;
  productId: string;
  percentage: number;
};

export type Product = {
  id: string;
  name: string;
  description: string;
  price: number;
  stock: number;
  category: Category;
  brand: Brand;
  imageUrl: string;
  technicalSpecs: TechnicalSpec[];
  reviews: Review[];
  createdAt: string;
  updatedAt: string;
  discount: Discount;
};

export const products = [];
