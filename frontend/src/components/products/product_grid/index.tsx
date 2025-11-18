import { SimpleGrid } from "@mantine/core";
import ProductCard from "../product_card";
import type { Product } from "@_types/product";

export default function ProductGrid({
  products,
  onClickProduct,
  onAddToCart,
}: {
  products: Product[];
  onClickProduct: (id: string) => void;
  onAddToCart: () => void;
}) {
  return (
    <SimpleGrid cols={{ base: 2, sm: 3, md: 4 }} spacing="lg">
      {products.map((p) => (
        <ProductCard
          key={p.id}
          product={p}
          onClick={() => onClickProduct(p.id)}
          onAddToCart={onAddToCart}
        />
      ))}
    </SimpleGrid>
  );
}
