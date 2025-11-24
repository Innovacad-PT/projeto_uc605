import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import ProductPage from "@components/products/product/index";
import type { Product } from "@_types/product";
import { productService } from "@services/products";

export default function ProductPageWrapper() {
  const { id } = useParams<{ id: string }>();
  const [product, setProduct] = useState<Product | null>(null);

  useEffect(() => {
    if (!id) return;

    productService
      .getById(id)
      .then((product) => setProduct(product))
      .catch((err) => console.error(err));
  }, [id]);

  if (!product) return <div>Carregando produto...</div>;

  return <ProductPage product={product} />;
}
