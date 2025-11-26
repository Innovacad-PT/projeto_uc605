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
} from "@mantine/core";
import { IconShoppingCart } from "@tabler/icons-react";
import AppHeader from "@components/header";
import { useCart } from "@services/cart";
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
                {/* IMAGEM */}
                {product.imageUrl && (
                  <Image
                    src={product.imageUrl || null}
                    alt={product.name}
                    height={350}
                    fit="cover"
                    radius="md"
                  />
                )}

                {/* TÍTULO + PREÇO */}
                <Group justify="space-between" align="flex-start">
                  <Text fw={700} size="xl" style={{ flex: 1 }}>
                    {product.name}
                  </Text>

                  <Stack align="flex-end" gap={4}>
                    {product.discount?.percentage && (
                      <Badge color="red" variant="filled">
                        -{product.discount.percentage}%
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

                {/* DESCRIÇÃO */}
                {product.description && (
                  <Text size="md" c="dimmed">
                    {product.description}
                  </Text>
                )}

                {/* INFO TÉCNICA */}
                {product.technicalSpecs &&
                  product.technicalSpecs.length > 0 && (
                    <Stack gap="xs">
                      <Text size="sm" fw={700}>
                        Especificações Técnicas:
                      </Text>
                      {product.technicalSpecs.map((spec) => (
                        <Group key={spec.id} justify="space-between">
                          <Text size="sm" c="dimmed">
                            {spec.name}
                          </Text>
                          <Text size="sm">{spec.value}</Text>
                        </Group>
                      ))}
                    </Stack>
                  )}

                {/* BOTÃO ADICIONAR */}
                <Button
                  fullWidth
                  mt="md"
                  size="md"
                  leftSection={<IconShoppingCart />}
                  onClick={() => handleAddProductToCart()}
                >
                  Adicionar ao Carrinho
                </Button>
              </Stack>
            </Card>
          </Container>
        </div>
      </AppShell>
    </>
  );
}
