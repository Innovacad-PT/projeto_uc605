export interface ProductPageProps {
  product: Product;
}

export interface Product {
  id: string;
  name: string;
  price: number;
  categories: string[];
  imageUrl?: string;
  details?: string;
  techInfo?: string;
}
