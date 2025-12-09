import { Box, Title, Text, Center, Stack, ThemeIcon } from "@mantine/core";
import { IconSearchOff } from "@tabler/icons-react";
import FeaturedProducts from "@components/products/product_featured";
import type { Product } from "@_types/product";

interface ProductsSectionProps {
  products: Product[];
  loading: boolean;
  onProductClick: (id: string) => void;
  onAddToCart: (product: Product) => void;
}

export default function ProductsSection({
  products,
  loading,
  onProductClick,
  onAddToCart,
}: ProductsSectionProps) {
  return (
    <Box id="products-section">
      <Title order={2} mb="md">
        Produtos em Destaque
      </Title>

      {loading ? (
        <FeaturedProducts
          products={[]}
          loading={true}
          onClickProduct={onProductClick}
          onAddToCart={onAddToCart}
        />
      ) : products.length > 0 ? (
        <FeaturedProducts
          products={products}
          loading={false}
          onClickProduct={onProductClick}
          onAddToCart={onAddToCart}
        />
      ) : (
        <Center py={50} bg="gray.0" style={{ borderRadius: 8 }}>
          <Stack align="center" gap="xs">
            <ThemeIcon size={60} radius="xl" variant="light" color="gray">
              <IconSearchOff size={32} />
            </ThemeIcon>
            <Text size="lg" fw={600} c="dimmed">
              Nenhum produto encontrado
            </Text>
            <Text size="sm" c="dimmed" ta="center" maw={300}>
              Não encontramos produtos com as categorias selecionadas. Tenta
              limpar os filtros.
            </Text>
          </Stack>
        </Center>
      )}
    </Box>
  );
}
