import type { ProductPageProps } from "@_types/product";
import {
  AppShell,
  Badge,
  Button,
  Card,
  Container,
  Group,
  Image,
  Stack,
  Text,
  Rating,
  Divider,
} from "@mantine/core";
import { IconShoppingCart } from "@tabler/icons-react";
import AppHeader from "@components/header";
import { useCart } from "@contexts/CartContext";
import { LogType } from "@_types/debug";
import { logger } from "@utils/debug";

import { PriceDisplay } from "@components/common/PriceDisplay";

export default function ProductPage({ product }: ProductPageProps) {
  const { addToCart } = useCart();

  const handleAddProductToCart = () => {
    addToCart(product);
  };

  logger(LogType.INFO, "Image URL", product.imageUrl);

  return (
    <>
      <AppShell>
        <AppHeader />
        <div
          style={{
            minWidth: "100%",
            minHeight: "100%",
            padding: "60px",
          }}
        >
          <Container size="lg" mt="xl">
            <Card
              shadow="sm"
              radius="md"
              p="xl"
              withBorder
              style={{
                maxWidth: 780,
                margin: "0 auto",
              }}
            >
              <Stack gap="lg">
                {product.imageUrl && (
                  <Image
                    src={product.imageUrl || null}
                    alt={product.name}
                    height={350}
                    fit="cover"
                    radius="md"
                  />
                )}

                <Group justify="space-between" align="flex-start">
                  <Text fw={700} size="xl" style={{ flex: 1 }}>
                    {product.name}
                  </Text>

                  <Stack align="flex-end" gap={4}>
                    {product.discount?.percentage && (
                      <Badge color="red" variant="filled">
                        {product.discount.percentage}% de Desconto
                      </Badge>
                    )}
                    <PriceDisplay
                      price={product.price}
                      discount={product.discount}
                      size="xl"
                      fw={700}
                      c="green"
                    />
                  </Stack>
                </Group>

                <Group>
                  {product.category && (
                    <Badge variant="outline">{product.category.name}</Badge>
                  )}
                  {product.brand && (
                    <Badge variant="outline" color="gray">
                      {product.brand.name}
                    </Badge>
                  )}
                </Group>

                {product.description && (
                  <Text size="md" c="dimmed">
                    {product.description}
                  </Text>
                )}

                {product.technicalSpecs &&
                  product.technicalSpecs.length > 0 && (
                    <Stack gap="xs">
                      <Text size="sm" fw={700}>
                        Especificações Técnicas:
                      </Text>
                      <Group justify="space-between">
                        <Text size="sm" fw={600}>
                          Especificação
                        </Text>
                        <Text size="sm" fw={600}>
                          Valor
                        </Text>
                      </Group>
                      {product.technicalSpecs.map((spec) => (
                        <Group key={spec.id} justify="space-between">
                          <Text size="sm" c="dimmed">
                            {spec.key}
                          </Text>
                          <Text size="sm">{spec.value}</Text>
                        </Group>
                      ))}
                    </Stack>
                  )}

                <Divider my="sm" />

                <Stack gap="xs">
                  <Text size="lg" fw={700}>
                    Avaliações ({product.reviews?.length || 0})
                  </Text>
                  {product.reviews && product.reviews.length > 0 ? (
                    product.reviews.map((review) => (
                      <Card key={review.id} withBorder padding="sm" radius="md">
                        <Group justify="space-between" mb="xs">
                          <Rating value={review.rating} readOnly size="sm" />
                          <Text size="xs" c="dimmed">
                            {new Date(review.createdAt).toLocaleDateString()}
                          </Text>
                        </Group>
                        {review.comment && (
                          <Text size="sm">{review.comment}</Text>
                        )}
                      </Card>
                    ))
                  ) : (
                    <Text c="dimmed" size="sm">
                      Este produto ainda não tem avaliações.
                    </Text>
                  )}
                </Stack>

                <Button
                  data-disabled={product.stock === 0}
                  fullWidth
                  mt="md"
                  size="md"
                  leftSection={<IconShoppingCart />}
                  onClick={() => handleAddProductToCart()}
                >
                  {product.stock === 0
                    ? "Produto Indisponível"
                    : "Adicionar ao Carrinho"}
                </Button>
              </Stack>
            </Card>
          </Container>
        </div>
      </AppShell>
    </>
  );
}
