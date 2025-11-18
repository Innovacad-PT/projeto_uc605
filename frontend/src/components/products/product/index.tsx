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
import { BASE_API_URL } from "@utils/api";
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
                  src={BASE_API_URL + product.imageUrl}
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

              {/* CATEGORIAS */}
              {product.categories.length > 0 && (
                <Group>
                  {product.categories.map((cat) => (
                    <Badge key={cat} variant="outline">
                      {cat}
                    </Badge>
                  ))}
                </Group>
              )}

              {/* DESCRIÇÃO */}
              {product.details && (
                <Text size="md" c="dimmed">
                  {product.details}
                </Text>
              )}

              {/* INFO TÉCNICA */}
              {product.techInfo && (
                <Text size="sm" c="dimmed">
                  <strong>Informação técnica:</strong> {product.techInfo}
                </Text>
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
