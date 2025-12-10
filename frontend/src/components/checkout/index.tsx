import {
  Container,
  Text,
  Button,
  Group,
  Card,
  Divider,
  NumberInput,
  Stack,
  ActionIcon,
  Image,
} from "@mantine/core";
import { IconTrash } from "@tabler/icons-react";
import { useCart } from "@contexts/CartContext";
import { useNavigate } from "react-router-dom";
import { createOrder, type OrderAdd } from "@services/order";
import { getUserId, isLoggedIn } from "@services/auth";
import { notifications } from "@mantine/notifications";
import AppHeader from "@components/header";

export default function CheckoutPage() {
  const navigate = useNavigate();
  const { items, clearCart, updateQuantity, removeFromCart } = useCart();
  const total = items.reduce(
    (sum, item) => sum + Number(item.product.price) * item.quantity,
    0
  );

  const finishPurchase = async () => {
    if (!isLoggedIn()) {
      navigate("/login", {
        state: {
          from: "/checkout",
        },
      });
      return;
    }

    const order: OrderAdd = {
      userId: getUserId()!,
      total,
      status: "pending",
      products: items.reduce((acc, item) => {
        acc[item.product.id] = item.quantity;
        return acc;
      }, {} as Record<string, number>),
    };

    const result = await createOrder(order);

    if (!result) {
      notifications.show({
        title: "Erro",
        message: "Não foi possível realizar a compra.",
        color: "red",
      });
      return;
    }

    clearCart();
    notifications.show({
      title: "Successo",
      message: "Compra realizada com sucesso.",
      color: "green",
    });

    navigate("/order-confirmation", {
      state: {
        order: result,
        items,
      },
    });
  };

  return (
    <>
      <AppHeader />
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
                        max={item.product.stock!}
                        w={80}
                        allowNegative={false}
                      />

                      <Text fw={700} w={80} ta="right">
                        €
                        {(Number(item.product.price) * item.quantity).toFixed(
                          2
                        )}
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

                <Divider />

                <Group justify="space-between">
                  <Text fw={700} size="lg">
                    Total
                  </Text>
                  <Text fw={700} size="lg" c="indigo">
                    €{total.toFixed(2)}
                  </Text>
                </Group>

                <Button fullWidth size="md" onClick={finishPurchase} mt="sm">
                  Confirmar compra
                </Button>
              </Stack>
            </Card>
          </Group>
        )}
      </Container>
    </>
  );
}
