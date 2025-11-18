import { useEffect, useMemo, useState } from "react";
import {
  AppShell,
  Container,
  MultiSelect,
  Title,
  Text,
  Box,
  Card,
  Button,
  Image,
} from "@mantine/core";
import axios from "axios";
import { BASE_API_URL, PRODUCTS_URL_API } from "@utils/api";
import type { Product } from "@_types/product";
import type { ApiResponse } from "@_types/general";
import { useNavigate } from "react-router-dom";

import Hero from "@components/hero";
import FeaturedProduct from "@components/products/product_featured";
import ProductGrid from "@components/products/product_grid";
import Benefits from "@components/benefits";
import AppFooter from "@components/footer";
import AppHeader from "@components/header";
import HorizontalScroll from "@components/horizontal_scroll";
import ProductCard from "@components/products/product_card";
import FeaturedProducts from "@components/products/product_featured";

export default function Landing() {
  const [cart, setCart] = useState(0);
  const [query, setQuery] = useState("");
  const [featuredProduct, setFeaturedProduct] = useState<Product>();
  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  const [categories, setCategories] = useState<Map<string, boolean>>(new Map());
  const navigate = useNavigate();
  const handleProductClick = (id: string) => navigate("/product/" + id);

  useEffect(() => {
    axios
      .get<ApiResponse<Product>>(PRODUCTS_URL_API)
      .then((res) => {
        const list = res.data.value;
        setProducts(list);

        const rnd = Math.floor(Math.random() * list.length);
        setFeaturedProduct(list[rnd]);
      })
      .catch(() => setError("Erro ao carregar produtos"))
      .finally(() => setLoading(false));
  }, []);

  const filtered = useMemo(() => {
    const activeCats = [...categories.entries()]
      .filter(([_, v]) => v)
      .map(([k]) => k);

    return products.filter((p) => {
      const matchTitle = p.name.toLowerCase().includes(query.toLowerCase());
      const matchCat =
        activeCats.length === 0 ||
        p.categories.some((c) => activeCats.includes(c));

      return matchTitle && matchCat;
    });
  }, [products, query, categories]);

  const scrollToProducts = () =>
    document.getElementById("products-section")?.scrollIntoView({
      behavior: "smooth",
    });

  return (
    <AppShell header={{ height: 70 }} footer={{ height: 200 }} padding="md">
      <AppHeader />

      <AppShell.Main>
        <Container size="lg">
          {loading && <Text>Carregando...</Text>}
          {error && <Text color="red">{error}</Text>}
          <Hero onScrollToProducts={scrollToProducts} />
          <MultiSelect
            mt="xl"
            label="Categorias"
            placeholder="Escolhe uma"
            data={["tech", "gaming", "visual", "mouse"]}
            clearable
            onChange={(e) => {
              const map = new Map<string, boolean>();
              e.forEach((cat) => map.set(cat, true));
              setCategories(map);
            }}
          />
          {/* PRODUTOS EM DESTAQUE */}
          <Title order={2} mt={40} mb="md">
            Produtos em Destaque
          </Title>

          <FeaturedProducts
            products={products}
            loading={loading}
            onClickProduct={handleProductClick}
            onAddToCart={(p: Product) => setCart(cart + 1)}
          />

          <Benefits />
        </Container>
      </AppShell.Main>

      <AppFooter />
    </AppShell>
  );
}
