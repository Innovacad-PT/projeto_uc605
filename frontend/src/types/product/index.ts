export interface ProductPageProps {
  product: Product;
}

export interface Product {
  id: string;
  name: string;
  description?: string;
  price: string;
  stock: number;
  imageUrl?: string;
  category?: Category;
  brand?: Brand;
  technicalSpecs?: TechnicalSpec[];
  reviews?: Review[];
  createdAt?: string;
  updatedAt?: string;
  discount?: Discount;
}

export interface Category {
  id: string;
  name: string;
}

export interface Brand {
  id: string;
  name: string;
}

export interface TechnicalSpec {
  id: string;
  technicalSpecsId?: string;
  name: string;
  value?: string;
}

export interface Review {
  id: string;
  userId: string;
  productId: string;
  rating: number;
  comment?: string;
  createdAt: string;
}

export interface Discount {
  id: string;
  percentage: number;
  startTime: Date;
  endTime: Date;
}
