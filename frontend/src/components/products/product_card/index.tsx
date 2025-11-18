import { Card, Image, Text, Button } from "@mantine/core";
import { BASE_API_URL } from "@utils/api";
import type { Product } from "@_types/product";

export default function ProductCard({
  product,
  onClick,
  onAddToCart,
}: {
  product: Product;
  onClick: () => void;
  onAddToCart: () => void;
}) {
  return (
    <Card
      shadow="sm"
      radius="md"
      p="md"
      style={{ minWidth: 200, cursor: "pointer" }}
      onClick={onClick}
    >
      <Image
        src={BASE_API_URL + product.imageUrl}
        h={150}
        radius="md"
        alt={product.name}
      />

      <Text mt="sm" fw={600} size="lg">
        {product.name}
      </Text>

      <Text fw={700} c="indigo" size="xl">
        €{product.price}
      </Text>

      <Button
        fullWidth
        mt="md"
        onClick={(e) => {
          e.stopPropagation();
          onAddToCart();
        }}
      >
        Adicionar
      </Button>
    </Card>
  );
}
