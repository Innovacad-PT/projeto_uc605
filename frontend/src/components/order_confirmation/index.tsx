import {
  Button,
  Container,
  Divider,
  Group,
  Paper,
  Stack,
  Text,
  ThemeIcon,
  Title,
} from "@mantine/core";
import {
  IconCheck,
  IconReceipt,
  IconArrowRight,
  IconHome,
} from "@tabler/icons-react";
import { useLocation, useNavigate } from "react-router-dom";
import AppHeader from "@components/header";

export default function OrderConfirmation() {
  const location = useLocation();
  const navigate = useNavigate();
  const order = location.state?.order;
  const items = location.state?.items;

  const states: Record<string, string> = {
    pending: "Pendente",
    completed: "Completada",
    cancelled: "Cancelada",
  };

  if (!order) {
    return (
      <>
        <AppHeader />
        <Container size="sm" mt="xl" pt="xl">
          <Paper p="xl" radius="md" withBorder ta="center">
            <Stack align="center" gap="md">
              <ThemeIcon size={64} radius="xl" color="gray" variant="light">
                <IconHome size={32} />
              </ThemeIcon>
              <Text size="lg" fw={500}>
                Encomenda não encontrada
              </Text>
              <Button mt="md" onClick={() => navigate("/")} variant="light">
                Voltar à página inicial
              </Button>
            </Stack>
          </Paper>
        </Container>
      </>
    );
  }

  return (
    <>
      <AppHeader />
      <Container size="sm" mt="xl" pt="xl" pb="xl">
        <Paper p={50} radius="md" withBorder shadow="sm">
          <Stack align="center" gap="lg" mb="xl">
            <ThemeIcon size={80} radius="100%" color="green" variant="light">
              <IconCheck size={48} />
            </ThemeIcon>
            <Stack gap={0} align="center">
              <Title order={2} ta="center">
                Obrigado pela sua encomenda!
              </Title>
              <Text c="dimmed" ta="center" size="lg" mt={5}>
                A sua compra foi confirmada com sucesso.
              </Text>
            </Stack>
          </Stack>

          <Paper withBorder p="lg" radius="md" bg="gray.0">
            <Stack gap="sm">
              <Group justify="space-between">
                <Group gap="xs">
                  <IconReceipt size={20} color="gray" />
                  <Text fw={500}>Nº de encomenda:</Text>
                </Group>
                <Text fw={700} ff="monospace">
                  #{order.id}
                </Text>
              </Group>

              {order.createdAt && (
                <Group justify="space-between">
                  <Text c="dimmed" size="sm">
                    Data:
                  </Text>
                  <Text size="sm">
                    {new Date(order.createdAt).toLocaleDateString()}{" "}
                    {new Date(order.createdAt).toLocaleTimeString()}
                  </Text>
                </Group>
              )}

              <Group justify="space-between">
                <Text c="dimmed" size="sm">
                  Estado:
                </Text>
                <Text size="sm" tt="capitalize">
                  {states[order.status.toLowerCase()] || order.status}
                </Text>
              </Group>

              {items && items.length > 0 && (
                <>
                  <Divider my="xs" label="Produtos" labelPosition="center" />
                  <Stack gap="md">
                    {order.orderItems?.map((orderItem: any) => {
                      const item = items.find(
                        (i: any) => i.product.id === orderItem.productId
                      );
                      const product = item?.product;

                      if (!product) return null;

                      return (
                        <Group
                          key={orderItem.productId}
                          wrap="nowrap"
                          align="flex-start"
                        >
                          <ThemeIcon
                            size={50}
                            variant="light"
                            color="gray"
                            radius="md"
                          >
                            {product.imageUrl ? (
                              <img
                                src={product.imageUrl}
                                alt={product.name}
                                style={{
                                  width: "100%",
                                  height: "100%",
                                  objectFit: "cover",
                                  borderRadius: "4px",
                                }}
                              />
                            ) : (
                              <IconReceipt size={24} />
                            )}
                          </ThemeIcon>

                          <Stack gap={2} style={{ flex: 1 }}>
                            <Text size="sm" fw={500} lineClamp={2}>
                              {product.name}
                            </Text>
                            <Text size="xs" c="dimmed">
                              {orderItem.quantity} x €
                              {Number(orderItem.unitPrice).toFixed(2)}
                            </Text>
                          </Stack>

                          <Text size="sm" fw={600}>
                            €
                            {(
                              orderItem.quantity * Number(orderItem.unitPrice)
                            ).toFixed(2)}
                          </Text>
                        </Group>
                      );
                    })}
                  </Stack>
                </>
              )}

              <Divider my="xs" />

              <Group justify="space-between">
                <Text size="lg" fw={600}>
                  Total:
                </Text>
                <Text fw={700} size="xl" c="green.8">
                  €{Number(order.total).toFixed(2)}
                </Text>
              </Group>
            </Stack>
          </Paper>

          <Group mt={40} justify="center">
            <Button
              size="lg"
              onClick={() => navigate("/")}
              rightSection={<IconArrowRight size={18} />}
            >
              Continuar a comprar
            </Button>
          </Group>
        </Paper>
      </Container>
    </>
  );
}
