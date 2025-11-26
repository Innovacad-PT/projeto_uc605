import {
  Container,
  Text,
  Button,
  Group,
  Card,
  Divider,
  NumberInput,
  TextInput,
  Stack,
  ActionIcon,
  Image,
} from "@mantine/core";
import { IconTrash } from "@tabler/icons-react";
import { useCart } from "@services/cart";

export default function CheckoutPage() {
  const { items, clearCart, updateQuantity, removeFromCart } = useCart();
  const total = items.reduce(
    (sum, item) => sum + Number(item.product.price) * item.quantity,
    0
  );

  return (
    <Container size="md" mt="xl" pb="xl">
      <Text size="xl" fw={700} mb="lg">
        Finalizar compra
      </Text>

      {items.length === 0 ? (
        <Text c="dimmed">O carrinho está vazio.</Text>
      ) : (
        <Group align="flex-start" gap="lg">
          <Stack style={{ flex: 1 }}>
            {items.map((item) => (
              <Card
                key={item.product.id}
                shadow="sm"
                p="md"
                radius="md"
                withBorder
              >
                <Group>
                  <Image
                    src={item.product.imageUrl || null}
                    width={80}
                    height={80}
                    alt={item.product.name}
                    radius="md"
                  />

                  <Stack gap={4} style={{ flex: 1 }}>
                    <Text fw={600}>{item.product.name}</Text>
                    <Text size="sm" c="dimmed">
                      €{Number(item.product.price).toFixed(2)} / un
                    </Text>
                  </Stack>

                  <Group>
                    <NumberInput
                      value={item.quantity}
                      onChange={(val) =>
                        updateQuantity(item.product.id, Number(val))
                      }
                      min={1}
                      max={99}
                      w={80}
                      allowNegative={false}
                    />

                    <Text fw={700} w={80} ta="right">
                      €{(Number(item.product.price) * item.quantity).toFixed(2)}
                    </Text>

                    <ActionIcon
                      color="red"
                      variant="subtle"
                      onClick={() => removeFromCart(item.product.id)}
                    >
                      <IconTrash size={18} />
                    </ActionIcon>
                  </Group>
                </Group>
              </Card>
            ))}
          </Stack>

          <Card shadow="sm" p="lg" radius="md" withBorder w={350}>
            <Stack gap="md">
              <Text fw={700} size="lg">
                Resumo
              </Text>

              <Group justify="space-between">
                <Text c="dimmed">Subtotal</Text>
                <Text>€{total.toFixed(2)}</Text>
              </Group>

              <TextInput
                placeholder="Código do cupão"
                label="Cupão de desconto"
              />

              <Divider />

              <Group justify="space-between">
                <Text fw={700} size="lg">
                  Total
                </Text>
                <Text fw={700} size="lg" c="indigo">
                  €{total.toFixed(2)}
                </Text>
              </Group>

              <Button fullWidth size="md" onClick={clearCart} mt="sm">
                Confirmar compra
              </Button>
            </Stack>
          </Card>
        </Group>
      )}
    </Container>
  );
}
