import { useState } from "react";
import {
  AppShell,
  AppShellHeader,
  Container,
  Text,
  Group,
  Image,
  Input,
  Button,
  Card,
  SimpleGrid,
  Badge,
  Grid,
  Stack,
  Title,
} from "@mantine/core";
import { IconSearch, IconShoppingCart } from "@tabler/icons-react";
import { hideNotification } from "@mantine/notifications";

export default function LojaLanding() {
  const [cart, setCart] = useState(0);
  const [query, setQuery] = useState("");

  const produtos = [
    {
      id: 1,
      title: "Portátil Gamer X-Pro 15",
      price: 1099,
      img: "https://images.unsplash.com/photo-1517336714731-489689fd1ca8",
    },
    {
      id: 2,
      title: "SSD NVMe 1TB",
      price: 79.99,
      img: "https://images.unsplash.com/photo-1580910051071-9de5d6b42b6b",
    },
    {
      id: 3,
      title: 'Monitor 27" 144Hz',
      price: 259,
      img: "https://images.unsplash.com/photo-1580894908361-1b78b4b5f2a6",
    },
    {
      id: 4,
      title: "Mouse Gaming RGB",
      price: 39.9,
      img: "https://images.unsplash.com/photo-1519923047626-3f6a3a5a7f6d",
    },
  ];

  const filtered = produtos.filter((p) =>
    p.title.toLowerCase().includes(query.toLowerCase())
  );

  return (
    <AppShell header={{ height: 70 }} padding="md">
      <header style={{ height: "70px", paddingInline: "24px" }}>
        <Group h="100%" justify="space-between">
          <Group>
            <Badge size="lg" color="indigo">
              CT
            </Badge>
            <Text fw={600}>CAPITEK</Text>
          </Group>

          <Input
            leftSection={<IconSearch size={18} />}
            placeholder="Procurar produtos..."
            w={350}
            value={query}
            onChange={(e) => setQuery(e.currentTarget.value)}
          />

          <Button variant="subtle" leftSection={<IconShoppingCart />}>
            Carrinho ({cart})
          </Button>
        </Group>
      </header>

      <Container size="lg">
        {/* HERO */}
        <Grid mt="xl">
          <Grid.Col span={{ base: 12, md: 6 }}>
            <Stack>
              <Title order={1}>Tecnologia ao melhor preço</Title>
              <Text c="dimmed">
                Descobre ofertas em informática, gaming, smartphones e muito
                mais.
              </Text>
              <Group>
                <Button size="md">Ver Produtos</Button>
                <Button size="md" variant="outline">
                  Ofertas do Dia
                </Button>
              </Group>
            </Stack>
          </Grid.Col>

          <Grid.Col span={{ base: 12, md: 6 }}>
            <Card shadow="lg" radius="lg" p="md">
              <Image
                src="https://images.unsplash.com/photo-1517336714731-489689fd1ca8?auto=format&fit=crop&w=800&q=60"
                h={200}
                radius="md"
                alt="Laptop"
              />
              <Text fw={600} mt="md">
                Portátil Gamer X-Pro 15"
              </Text>
              <Text c="indigo" fw={700}>
                €1099
              </Text>
            </Card>
          </Grid.Col>
        </Grid>

        {/* PRODUTOS */}
        <Title order={2} mt={40} mb="md">
          Produtos em Destaque
        </Title>

        <SimpleGrid cols={{ base: 2, sm: 3, md: 4 }} spacing="md">
          {filtered.map((p) => (
            <Card key={p.id} shadow="sm" radius="md" p="md">
              <Image
                src={`${p.img}?auto=format&fit=crop&w=600&q=60`}
                h={140}
                radius="md"
                alt={p.title}
              />
              <Text mt="sm" fw={500}>
                {p.title}
              </Text>
              <Text fw={700} c="indigo">
                €{p.price}
              </Text>
              <Button fullWidth mt="md" onClick={() => setCart(cart + 1)}>
                Adicionar
              </Button>
            </Card>
          ))}
        </SimpleGrid>

        {/* BENEFÍCIOS */}
        <Grid mt={50}>
          <Grid.Col span={4}>
            <Text fw={700}>Entrega Rápida</Text>
            <Text c="dimmed">24-48h em Portugal Continental</Text>
          </Grid.Col>
          <Grid.Col span={4}>
            <Text fw={700}>Garantia Oficial</Text>
            <Text c="dimmed">Assistência técnica autorizada</Text>
          </Grid.Col>
          <Grid.Col span={4}>
            <Text fw={700}>Devoluções Fáceis</Text>
            <Text c="dimmed">Até 14 dias</Text>
          </Grid.Col>
        </Grid>
      </Container>
    </AppShell>
  );
}
