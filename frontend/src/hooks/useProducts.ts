import { useState, useEffect } from 'react';
import type { Product } from '@_types/product';
import { productService } from '@services/products';

export function useProducts() {
  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [featuredProduct, setFeaturedProduct] = useState<Product>();

  useEffect(() => {
    productService
      .getAll()
      .then((list) => {
        setProducts(list);

        if (list.length > 0) {
          const rnd = Math.floor(Math.random() * list.length);
          setFeaturedProduct(list[rnd]);
        }
      })
      .catch((err) => {
        console.error(err);
        setError('Erro ao carregar produtos');
      })
      .finally(() => setLoading(false));
  }, []);

  return { products, loading, error, featuredProduct };
}
