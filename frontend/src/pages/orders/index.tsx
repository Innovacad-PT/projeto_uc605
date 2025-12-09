import {
  Container,
  Title,
  Stack,
  Paper,
  Group,
  Text,
  Badge,
  Loader,
  Collapse,
  Box,
  Divider,
} from "@mantine/core";
import { useEffect, useState } from "react";
import { getOrdersByUserId, type OrderResult } from "@services/order";
import { getUserId } from "@services/auth";
import {
  IconPackage,
  IconChevronDown,
  IconChevronUp,
} from "@tabler/icons-react";
import { productService } from "@services/products";
import type { Product } from "@_types/product";
import AppHeader from "@components/header";

export default function OrdersPage() {
  const [orders, setOrders] = useState<OrderResult[]>([]);
  const [loading, setLoading] = useState(true);
  const [productsMap, setProductsMap] = useState<Record<string, Product>>({});

  const [expandedOrders, setExpandedOrders] = useState<Record<number, boolean>>(
    {}
  );

  useEffect(() => {
    const fetchOrders = async () => {
      const userId = getUserId();
      if (!userId) return;

      try {
        setLoading(true);
        const data = await getOrdersByUserId(userId);

        const sortedData = data.sort((a, b) =>
          b.createdAt.localeCompare(a.createdAt)
        );
        setOrders(sortedData);

        const productIds = new Set<string>();
        sortedData.forEach((order) => {
          order.orderItems.forEach((item) => productIds.add(item.productId));
        });

        const products: Record<string, Product> = {};
        await Promise.all(
          Array.from(productIds).map(async (id) => {
            try {
              const p = await productService.getById(id);
              products[id] = p;
            } catch (e) {
              console.error("Failed to load product", id);
            }
          })
        );
        setProductsMap(products);
      } catch (error) {
        console.error("Failed to fetch orders", error);
      } finally {
        setLoading(false);
      }
    };

    fetchOrders();
  }, []);

  const toggleExpand = (orderId: number) => {
    setExpandedOrders((prev) => ({
      ...prev,
      [orderId]: !prev[orderId],
    }));
  };

  const statusColors: Record<string, string> = {
    pending: "yellow",
    completed: "green",
    cancelled: "red",
    shipped: "blue",
  };

  const statusLabels: Record<string, string> = {
    pending: "Pendente",
    completed: "Concluída",
    cancelled: "Cancelada",
    shipped: "Enviada",
  };

  if (loading) {
    return (
      <Container
        size="md"
        mt="xl"
        style={{ display: "flex", justifyContent: "center" }}
      >
        <Loader />
      </Container>
    );
  }

  const formatDate = (dateString: string) => {
    return (
      new Date(dateString).toLocaleDateString() +
      " " +
      new Date(dateString).toLocaleTimeString([], {
        hour: "2-digit",
        minute: "2-digit",
      })
    );
  };

  return (
    <>
      <AppHeader />
      <Container size="md" mt="xl" pb="xl">
        <Title order={2} mb="lg">
          As minhas encomendas
        </Title>

        {orders.length === 0 ? (
          <Paper p="xl" withBorder ta="center">
            <Text c="dimmed">Ainda não fez nenhuma encomenda.</Text>
          </Paper>
        ) : (
          <Stack>
            {orders.map((order) => (
              <Paper
                key={order.id}
                withBorder
                radius="md"
                style={{ overflow: "hidden" }}
              >
                <Group
                  justify="space-between"
                  p="md"
                  bg="gray.0"
                  onClick={() => toggleExpand(order.id)}
                  style={{ cursor: "pointer" }}
                >
                  <Group gap="md">
                    <Box>
                      <Text fw={700}>#{order.id}</Text>
                      <Text size="xs" c="dimmed">
                        {formatDate(order.createdAt)}
                      </Text>
                    </Box>
                    <Badge
                      color={statusColors[order.status.toLowerCase()] || "gray"}
                    >
                      {statusLabels[order.status.toLowerCase()] || order.status}
                    </Badge>
                  </Group>
                  <Group>
                    <Text fw={700}>€{Number(order.total).toFixed(2)}</Text>
                    {expandedOrders[order.id] ? (
                      <IconChevronUp size={18} />
                    ) : (
                      <IconChevronDown size={18} />
                    )}
                  </Group>
                </Group>

                <Collapse in={expandedOrders[order.id]}>
                  <Stack gap={0}>
                    <Divider />
                    {order.orderItems.map((item) => {
                      const product = productsMap[item.productId];
                      return (
                        <Group
                          key={item.productId}
                          p="md"
                          align="center"
                          justify="space-between"
                          style={{ borderBottom: "1px solid #eee" }}
                        >
                          <Group>
                            <Box
                              w={40}
                              h={40}
                              bg="gray.1"
                              style={{
                                borderRadius: 4,
                                overflow: "hidden",
                                display: "flex",
                                alignItems: "center",
                                justifyContent: "center",
                              }}
                            >
                              {product?.imageUrl ? (
                                <img
                                  src={product.imageUrl}
                                  alt={product.name}
                                  style={{
                                    width: "100%",
                                    height: "100%",
                                    objectFit: "cover",
                                  }}
                                />
                              ) : (
                                <IconPackage size={20} color="gray" />
                              )}
                            </Box>
                            <Box>
                              <Text size="sm" fw={500}>
                                {product?.name || "Produto desconhecido"}
                              </Text>
                              <Text size="xs" c="dimmed">
                                {item.quantity} x €
                                {Number(item.unitPrice).toFixed(2)}
                              </Text>
                            </Box>
                          </Group>
                          <Text size="sm" fw={600}>
                            €
                            {(item.quantity * Number(item.unitPrice)).toFixed(
                              2
                            )}
                          </Text>
                        </Group>
                      );
                    })}
                  </Stack>
                </Collapse>
              </Paper>
            ))}
          </Stack>
        )}
      </Container>
    </>
  );
}
