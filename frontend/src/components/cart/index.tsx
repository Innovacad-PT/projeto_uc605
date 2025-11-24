import {
  Drawer,
  Stack,
  Group,
  Text,
  Button,
  Image,
  Divider,
  ActionIcon,
  Badge,
} from "@mantine/core";
import { IconTrash } from "@tabler/icons-react";
import type { CartItem } from "@services/cart";

export default function CartDrawer({
  opened,
  onClose,
  items,
  onRemove,
  onCheckout,
}: {
  opened: boolean;
  onClose: () => void;
  items: CartItem[];
  onRemove: (id: string) => void;
  onCheckout: () => void;
}) {
  const total = items.reduce(
    (sum, item) => sum + item.product.price * item.quantity,
    0
  );

  return (
    <Drawer opened={opened} onClose={onClose} title="O seu carrinho" size="md">
      <Stack gap="lg">
        {items.length === 0 && (
          <Text size="lg" c="dimmed" ta="center" mt="lg">
            O carrinho está vazio
          </Text>
        )}

        {items.map((item) => (
          <Group key={item.product.id} align="flex-start">
            <Image
              src={item.product.imageUrl || ""}
              height={80}
              width={80}
              fit="cover"
              radius="md"
            />

            <Stack gap={4} style={{ flex: 1 }}>
              <Text fw={600}>{item.product.name}</Text>
              <Text fw={700} c="indigo">
                €{item.product.price.toFixed(2)}
              </Text>
              <Badge variant="light" color="gray">
                Qtd: {item.quantity}
              </Badge>
            </Stack>

            <ActionIcon
              color="red"
              variant="subtle"
              onClick={() => onRemove(item.product.id)}
            >
              <IconTrash size={18} />
            </ActionIcon>
          </Group>
        ))}

        {items.length > 0 && (
          <>
            <Divider />

            <Group justify="space-between">
              <Text fw={700}>Total:</Text>
              <Text fw={700}>€{total.toFixed(2)}</Text>
            </Group>

            <Button fullWidth size="md" onClick={onCheckout}>
              Finalizar compra
            </Button>
          </>
        )}
      </Stack>
    </Drawer>
  );
}
