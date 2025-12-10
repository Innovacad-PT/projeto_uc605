import { Card, Image, Text, Button } from "@mantine/core";
import type { Product } from "@_types/product";
import { PriceDisplay } from "../../common/PriceDisplay";
import { LogType } from "@_types/debug";
import { logger } from "@utils/debug";

export default function ProductCard({
  product,
  onClick,
  onAddToCart,
}: {
  product: Product;
  onClick: () => void;
  onAddToCart: () => void;
}) {
  logger(LogType.INFO, "Product Image Url", product.imageUrl);

  return (
    <Card
      shadow="sm"
      radius="md"
      p="md"
      style={{ minWidth: 200, cursor: "pointer" }}
      onClick={onClick}
    >
      <Image
        src={product.imageUrl || null}
        h={150}
        radius="md"
        alt={product.name}
      />

      <Text mt="sm" fw={600} size="lg">
        {product.name}{" "}
        {product.discount ? (
          <span style={{ color: "red", fontSize: "0.6em" }}>
            ({product.discount.percentage}%)
          </span>
        ) : (
          ""
        )}
      </Text>

      <PriceDisplay
        price={product.price}
        discount={product.discount}
        fw={700}
        size="xl"
      />

      <Button
        data-disabled={product.stock === 0}
        fullWidth
        mt="md"
        onClick={(e) => {
          e.stopPropagation();
          onAddToCart();
        }}
      >
        {product.stock === 0 ? "Produto Indisponível" : "Adicionar ao Carrinho"}
      </Button>
    </Card>
  );
}
