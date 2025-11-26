import { useState, useEffect, useMemo } from "react";
import {
  Container,
  Title,
  TextInput,
  Select,
  NumberInput,
  Button,
  Paper,
  Grid,
  SimpleGrid,
  Text,
  Stack,
  Group,
  Pagination,
  Loader,
  Center,
} from "@mantine/core";
import { IconSearch, IconX } from "@tabler/icons-react";
import { productService } from "@services/products";
import { brandService } from "@services/brands";
import { categoryService } from "@services/categories";
import type { Product } from "@_types/product";
import type { Brand } from "@_types/brand";
import type { Category } from "@_types/category";
import ProductCard from "@components/products/product_card";
import { useCart } from "@services/cart";
import { useNavigate } from "react-router-dom";
import AppHeader from "@components/header";
import { getFinalPrice } from "@utils/price";

export default function ProductsListPage() {
  const navigate = useNavigate();
  const { addToCart } = useCart();

  // Data states
  const [products, setProducts] = useState<Product[]>([]);
  const [brands, setBrands] = useState<Brand[]>([]);
  const [categories, setCategories] = useState<Category[]>([]);
  const [loading, setLoading] = useState(true);

  // Filter states
  const [searchQuery, setSearchQuery] = useState("");
  const [selectedCategory, setSelectedCategory] = useState<string | null>(null);
  const [selectedBrand, setSelectedBrand] = useState<string | null>(null);
  const [minPrice, setMinPrice] = useState<number | string>("");
  const [maxPrice, setMaxPrice] = useState<number | string>("");

  // Pagination state
  const [currentPage, setCurrentPage] = useState(1);
  const itemsPerPage = 12;

  // Load data on mount
  useEffect(() => {
    const loadData = async () => {
      try {
        const [productsData, brandsData, categoriesData] = await Promise.all([
          productService.getAll(),
          brandService.getAll(),
          categoryService.getAll(),
        ]);
        setProducts(productsData);
        setBrands(brandsData);
        setCategories(categoriesData);
      } catch (error) {
        console.error("Failed to load data:", error);
      } finally {
        setLoading(false);
      }
    };
    loadData();
  }, []);

  // Filter and paginate products
  const { filteredProducts, totalPages } = useMemo(() => {
    let filtered = [...products];

    // Search filter
    if (searchQuery.trim()) {
      const query = searchQuery.toLowerCase();
      filtered = filtered.filter(
        (p) =>
          p.name.toLowerCase().includes(query) ||
          p.description?.toLowerCase().includes(query)
      );
    }

    // Category filter
    if (selectedCategory) {
      filtered = filtered.filter((p) => p.category?.id === selectedCategory);
    }

    // Brand filter
    if (selectedBrand) {
      filtered = filtered.filter((p) => p.brand?.id === selectedBrand);
    }

    // Price filter
    const min = typeof minPrice === "number" ? minPrice : parseFloat(minPrice as string);
    const max = typeof maxPrice === "number" ? maxPrice : parseFloat(maxPrice as string);

    if (!isNaN(min)) {
      filtered = filtered.filter((p) => {
        const finalPrice = getFinalPrice(p.price, p.discount?.percentage);
        return finalPrice >= min;
      });
    }

    if (!isNaN(max)) {
      filtered = filtered.filter((p) => {
        const finalPrice = getFinalPrice(p.price, p.discount?.percentage);
        return finalPrice <= max;
      });
    }

    // Calculate pagination
    const total = Math.ceil(filtered.length / itemsPerPage);
    const startIndex = (currentPage - 1) * itemsPerPage;
    const paginated = filtered.slice(startIndex, startIndex + itemsPerPage);

    return { filteredProducts: paginated, totalPages: total, totalCount: filtered.length };
  }, [products, searchQuery, selectedCategory, selectedBrand, minPrice, maxPrice, currentPage]);

  // Reset to page 1 when filters change
  useEffect(() => {
    setCurrentPage(1);
  }, [searchQuery, selectedCategory, selectedBrand, minPrice, maxPrice]);

  // Clear all filters
  const handleClearFilters = () => {
    setSearchQuery("");
    setSelectedCategory(null);
    setSelectedBrand(null);
    setMinPrice("");
    setMaxPrice("");
    setCurrentPage(1);
  };

  // Check if any filters are active
  const hasActiveFilters =
    searchQuery.trim() ||
    selectedCategory ||
    selectedBrand ||
    minPrice !== "" ||
    maxPrice !== "";

  if (loading) {
    return (
      <>
        <AppHeader />
        <Center style={{ height: "80vh" }}>
          <Loader size="xl" />
        </Center>
      </>
    );
  }

  return (
    <>
      <AppHeader />
      <Container size="xl" py="xl">
        <Stack gap="xl">
          {/* Page Title */}
          <Title order={1}>All Products</Title>

          {/* Filters Section */}
          <Paper shadow="sm" p="md" radius="md" withBorder>
            <Stack gap="md">
              <Grid gutter="md">
                {/* Search */}
                <Grid.Col span={{ base: 12, md: 6 }}>
                  <TextInput
                    label="Procurar"
                    placeholder="Procurar produtos..."
                    leftSection={<IconSearch size={16} />}
                    value={searchQuery}
                    onChange={(e) => setSearchQuery(e.target.value)}
                  />
                </Grid.Col>

                {/* Category Filter */}
                <Grid.Col span={{ base: 12, sm: 6, md: 3 }}>
                  <Select
                    placeholder="Todas as Categorias"
                    label="Categoria"
                    data={categories.map((c) => ({ value: c.id, label: c.name }))}
                    value={selectedCategory}
                    onChange={setSelectedCategory}
                    clearable
                  />
                </Grid.Col>

                {/* Brand Filter */}
                <Grid.Col span={{ base: 12, sm: 6, md: 3 }}>
                  <Select
                    placeholder="Todas as Marcas"
                    label="Marca"
                    data={brands.map((b) => ({ value: b.id, label: b.name }))}
                    value={selectedBrand}
                    onChange={setSelectedBrand}
                    clearable
                  />
                </Grid.Col>

                {/* Min Price */}
                <Grid.Col span={{ base: 12, sm: 6, md: 3 }}>
                  <NumberInput
                    label="Preço Minimo (€)"
                    placeholder="0.00"
                    min={0}
                    decimalScale={2}
                    value={minPrice}
                    onChange={setMinPrice}
                  />
                </Grid.Col>

                {/* Max Price */}
                <Grid.Col span={{ base: 12, sm: 6, md: 3 }}>
                  <NumberInput
                    label="Preço Maximo (€)"
                    placeholder="999.99"
                    min={0}
                    decimalScale={2}
                    value={maxPrice}
                    onChange={setMaxPrice}
                  />
                </Grid.Col>

                {/* Clear Filters Button */}
                <Grid.Col span={{ base: 12, md: 6}} style={{alignContent: "end" }}>
                  <Button
                    variant="light"
                    color="gray"
                    leftSection={<IconX size={16} />}
                    onClick={handleClearFilters}
                    disabled={!hasActiveFilters}
                    fullWidth
                  >
                    Limpar Filtros
                  </Button>
                </Grid.Col>
              </Grid>
            </Stack>
          </Paper>

          {/* Results Info */}
          <Group justify="space-between">
            <Text size="sm" c="dimmed">
              Showing {filteredProducts.length} of{" "}
              {products.filter((p) => {
                let filtered = true;
                if (searchQuery.trim()) {
                  const query = searchQuery.toLowerCase();
                  filtered = filtered && (p.name.toLowerCase().includes(query) || (p.description?.toLowerCase().includes(query) ?? false));
                }
                if (selectedCategory) filtered = filtered && p.category?.id === selectedCategory;
                if (selectedBrand) filtered = filtered && p.brand?.id === selectedBrand;
                const min = typeof minPrice === "number" ? minPrice : parseFloat(minPrice as string);
                const max = typeof maxPrice === "number" ? maxPrice : parseFloat(maxPrice as string);
                if (!isNaN(min)) filtered = filtered && getFinalPrice(p.price, p.discount?.percentage) >= min;
                if (!isNaN(max)) filtered = filtered && getFinalPrice(p.price, p.discount?.percentage) <= max;
                return filtered;
              }).length}{" "}
              products
            </Text>
          </Group>

          {/* Products Grid */}
          {filteredProducts.length === 0 ? (
            <Center py="xl">
              <Text size="lg" c="dimmed">
                No products found. Try adjusting your filters.
              </Text>
            </Center>
          ) : (
            <SimpleGrid
              cols={{ base: 1, sm: 2, md: 3, lg: 4 }}
              spacing="lg"
            >
              {filteredProducts.map((product) => (
                <ProductCard
                  key={product.id}
                  product={product}
                  onClick={() => navigate(`/product/${product.id}`)}
                  onAddToCart={() => addToCart(product)}
                />
              ))}
            </SimpleGrid>
          )}

          {/* Pagination */}
          {totalPages > 1 && (
            <Center mt="xl">
              <Pagination
                total={totalPages}
                value={currentPage}
                onChange={setCurrentPage}
                size="lg"
              />
            </Center>
          )}
        </Stack>
      </Container>
    </>
  );
}
