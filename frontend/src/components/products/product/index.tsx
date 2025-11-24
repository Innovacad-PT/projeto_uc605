import type { ProductPageProps } from "@_types/product";
import {
  AppShell,
  Badge,
  Button,
  Card,
  Container,
  Divider,
  Group,
  Image,
  Stack,
  Text,
} from "@mantine/core";
import { IconShoppingCart } from "@tabler/icons-react";
import { BASE_URL } from "@utils/api";
import AppHeader from "@components/header";
import { useCart } from "@services/cart";

export default function ProductPage({ product }: ProductPageProps) {
  const { addToCart } = useCart();

  const handleAddProductToCart = () => {
    addToCart(product);
  };

  return (
    <>
      <AppShell>
        <AppHeader />

        <Container size="lg" mt="xl">
          <Divider mb="xl" />

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
                  src={BASE_URL + product.imageUrl}
                  alt={product.name}
                  height={350}
                  fit="contain"
                  radius="md"
                />
              )}

              {/* TÍTULO + PREÇO */}
              <Group justify="space-between">
                <Text fw={700} size="xl">
                  {product.name}
                </Text>

                <Badge size="lg" color="green" variant="light">
                  {product.price.toFixed(2)} €
                </Badge>
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
              {product.technicalSpecs && product.technicalSpecs.length > 0 && (
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
      </AppShell>
    </>
  );
}
