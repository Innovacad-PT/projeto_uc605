import { AppShell, Container, Text, Stack, Divider } from "@mantine/core";
import { useNavigate } from "react-router-dom";

import Hero from "@components/hero";
import Benefits from "@components/benefits";
import AppFooter from "@components/footer";
import AppHeader from "@components/header";
import ProductsSection from "./ProductsSection";
import Newsletter from "@components/newsletter";
import Contact from "@components/contact";

import { useProducts } from "../../hooks/useProducts";
import { useCart } from "@contexts/CartContext";

export default function Landing() {
  const { products, loading } = useProducts();
  const { addToCart } = useCart();
  const navigate = useNavigate();

  const handleProductClick = (id: string) => navigate("/product/" + id);

  const scrollToProducts = () =>
    document.getElementById("products-section")?.scrollIntoView({
      behavior: "smooth",
    });

  return (
    <AppShell header={{ height: 70 }} footer={{ height: 200 }} padding="md">
      <AppHeader />

      <AppShell.Main>
        <Container size="lg">
          {loading && products.length === 0 && <Text>Carregando...</Text>}

          <Stack gap={80}>
            <Hero onScrollToProducts={scrollToProducts} />

            <ProductsSection
              products={products}
              loading={loading}
              onProductClick={handleProductClick}
              onAddToCart={addToCart}
            />

            <Benefits />

            <Divider />

            <Newsletter />

            <Divider />

            <Contact />
          </Stack>
        </Container>
      </AppShell.Main>

      <AppFooter />
    </AppShell>
  );
}
