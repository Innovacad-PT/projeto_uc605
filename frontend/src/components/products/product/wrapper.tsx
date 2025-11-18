import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import ProductPage from "@components/products/product/index";
import axios from "axios";
import type { Product } from "@_types/product";
import { PRODUCTS_GET_BY_ID_URL } from "@utils/api";

export default function ProductPageWrapper() {
  const { id } = useParams<{ id: string }>();
  const [product, setProduct] = useState<Product | null>(null);

  useEffect(() => {
    if (!id) return;

    axios
      .get(`${PRODUCTS_GET_BY_ID_URL}${id}`)
      .then((res) => setProduct(res.data.value))
      .catch((err) => console.error(err));
  }, [id]);

  if (!product) return <div>Carregando produto...</div>;

  return <ProductPage product={product} />;
}
