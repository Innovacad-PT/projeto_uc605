import { Container, Text, Button, Group, Card, Divider } from "@mantine/core";
import { useCart } from "@services/cart";
import type { Product } from "@_types/product";

export default function CheckoutPage() {
  const { items, clearCart } = useCart();
  const total = items.reduce((sum: number, p: Product) => sum + p.price, 0);

  return (
    <Container size="sm" mt="xl">
      <Text size="xl" fw={700} mb="lg">
        Finalizar compra
      </Text>

      {items.length === 0 ? (
        <Text c="dimmed">O carrinho está vazio.</Text>
      ) : (
        <Card shadow="sm" p="lg" radius="md">
          {items.map((p: Product) => (
            <Group key={p.id} justify="space-between" mb="sm">
              <Text>{p.name}</Text>
              <Text fw={600}>€{p.price.toFixed(2)}</Text>
            </Group>
          ))}

          <Divider my="md" />

          <Group justify="space-between">
            <Text fw={700}>Total:</Text>
            <Text fw={700}>€{total.toFixed(2)}</Text>
          </Group>

          <Button fullWidth mt="lg" size="md" onClick={clearCart}>
            Confirmar compra
          </Button>
        </Card>
      )}
    </Container>
  );
}
