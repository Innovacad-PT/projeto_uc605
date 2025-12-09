import { useRef, useEffect, useCallback } from "react";
import {
  Box,
  Card,
  Image,
  Text,
  Button,
  Skeleton,
  ActionIcon,
} from "@mantine/core";
import { IconChevronLeft, IconChevronRight } from "@tabler/icons-react";
import type { Product } from "@_types/product";
import { PriceDisplay } from "../../common/PriceDisplay";

interface FeaturedProductsProps {
  products: Product[];
  onClickProduct: (id: string) => void;
  onAddToCart: (product: Product) => void;
  loading?: boolean;
}

export default function FeaturedProducts({
  products,
  onClickProduct,
  onAddToCart,
  loading = false,
}: FeaturedProductsProps) {
  const scrollRef = useRef<HTMLDivElement>(null);
  const cardWidth = 240;

  const scroll = useCallback((direction: "left" | "right") => {
    if (!scrollRef.current) return;
    const container = scrollRef.current;
    const scrollAmount = container.clientWidth * 0.8;
    container.scrollBy({
      left: direction === "right" ? scrollAmount : -scrollAmount,
      behavior: "smooth",
    });
  }, []);

  useEffect(() => {
    if (!products.length) return;
    const interval = setInterval(() => scroll("right"), 4000);
    return () => clearInterval(interval);
  }, [products, scroll]);

  if (loading) {
    return (
      <Box
        style={{
          display: "flex",
          gap: "16px",
          overflowX: "auto",
          paddingBottom: "10px",
        }}
      >
        {Array.from({ length: 5 }).map((_, i) => (
          <Skeleton key={i} height={250} width={220} radius="md" />
        ))}
      </Box>
    );
  }

  return (
    <Box style={{ position: "relative" }}>
      <ActionIcon
        style={{ position: "absolute", left: 0, top: "40%", zIndex: 10 }}
        size="lg"
        variant="filled"
        onClick={() => scroll("left")}
      >
        <IconChevronLeft size={24} />
      </ActionIcon>

      <ActionIcon
        style={{ position: "absolute", right: 0, top: "40%", zIndex: 10 }}
        size="lg"
        variant="filled"
        onClick={() => scroll("right")}
      >
        <IconChevronRight size={24} />
      </ActionIcon>

      <Box
        ref={scrollRef}
        style={{
          display: "flex",
          gap: "16px",
          overflowX: "auto",
          padding: "10px 40px",
          scrollSnapType: "x mandatory",
          cursor: "grab",
        }}
      >
        {products.slice(0, 10).map((p) => (
          <Card
            key={p.id}
            shadow="sm"
            radius="md"
            p="md"
            style={{
              minWidth: `${cardWidth}px`,
              scrollSnapAlign: "start",
              cursor: "pointer",
              transition: "0.3s",
            }}
            onClick={() => onClickProduct(p.id)}
            onMouseEnter={(e) =>
              (e.currentTarget.style.transform = "scale(1.03)")
            }
            onMouseLeave={(e) => (e.currentTarget.style.transform = "scale(1)")}
            draggable
            onDragStart={(e) => e.preventDefault()}
          >
            <Image
              src={p.imageUrl || null}
              height={140}
              radius="md"
              alt={p.name}
            />
            <Text mt="sm" fw={500} lineClamp={1}>
              {p.name}{" "}
              {p.discount != undefined ? (
                <span style={{ color: "red", fontSize: "0.7em" }}>
                  (-{p.discount.percentage}%)
                </span>
              ) : (
                ""
              )}
            </Text>
            <PriceDisplay
              price={p.price}
              discount={p.discount}
              fw={700}
              c="indigo"
            />
            <Button
              fullWidth
              mt="md"
              onClick={(e) => {
                e.stopPropagation();
                onAddToCart(p);
              }}
            >
              Adicionar
            </Button>
          </Card>
        ))}
      </Box>
    </Box>
  );
}
