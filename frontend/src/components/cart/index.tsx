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
import type { CartItem } from "@contexts/CartContext";
import { BASE_URL } from "@utils/api";
import { getFinalPrice } from "@utils/price";
import { PriceDisplay } from "@components/common/PriceDisplay";

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
  const total = items.reduce((sum, item) => {
    const price = getFinalPrice(
      item.product.price,
      item.product.discount?.percentage
    );
    return sum + price * item.quantity;
  }, 0);

  return (
    <Drawer opened={opened} onClose={onClose} title="O seu carrinho" size="md">
      <Stack gap="lg">
        {items.length === 0 ? (
          <Text size="lg" c="dimmed" ta="center" mt="lg">
            O carrinho está vazio
          </Text>
        ) : (
          <>
            {items.map((item) => (
              <Group key={item.product.id} align="flex-start" wrap="nowrap">
                <Image
                  src={new URL(
                    item.product.imageUrl || "",
                    BASE_URL
                  ).toString()}
                  h={80}
                  w={80}
                  fit="cover"
                  radius="md"
                />

                <Stack gap={4} style={{ flex: 1 }}>
                  <Group align="center" gap="xs">
                    <Text fw={600} lineClamp={1}>
                      {item.product.name}
                    </Text>
                    {item.product.discount?.percentage && (
                      <Badge color="red" variant="filled" size="sm">
                        -{item.product.discount.percentage}%
                      </Badge>
                    )}
                  </Group>

                  <PriceDisplay
                    price={item.product.price}
                    discount={item.product.discount}
                    fw={700}
                    c="indigo"
                  />

                  <Group gap="xs">
                    <Badge variant="light" color="gray">
                      Qtd: {item.quantity}
                    </Badge>
                  </Group>

                  <Text fw={500} size="sm">
                    Subtotal: €
                    {(
                      getFinalPrice(
                        item.product.price,
                        item.product.discount?.percentage
                      ) * item.quantity
                    ).toFixed(2)}
                  </Text>
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
