import {
  Drawer,
  Stack,
  Group,
  Text,
  Button,
  Image,
  Divider,
  ActionIcon,
} from "@mantine/core";
import { IconTrash } from "@tabler/icons-react";
import type { Product } from "@_types/product";
import { BASE_API_URL } from "@utils/api";

export default function CartDrawer({
  opened,
  onClose,
  items,
  onRemove,
  onCheckout,
}: {
  opened: boolean;
  onClose: () => void;
  items: Product[];
  onRemove: (id: string) => void;
  onCheckout: () => void;
}) {
  const total = items.reduce((sum, p) => sum + p.price, 0);

  return (
    <Drawer opened={opened} onClose={onClose} title="O seu carrinho" size="md">
      <Stack gap="lg">
        {items.length === 0 && (
          <Text size="lg" c="dimmed" ta="center" mt="lg">
            O carrinho está vazio
          </Text>
        )}

        {items.map((prod) => (
          <Group key={prod.id} align="flex-start">
            <Image
              src={BASE_API_URL + prod.imageUrl}
              height={80}
              width={80}
              fit="cover"
              radius="md"
            />

            <Stack gap={4} style={{ flex: 1 }}>
              <Text fw={600}>{prod.name}</Text>
              <Text fw={700} c="indigo">
                €{prod.price.toFixed(2)}
              </Text>
            </Stack>

            <ActionIcon
              color="red"
              variant="subtle"
              onClick={() => onRemove(prod.id)}
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
