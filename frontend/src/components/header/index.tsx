import CartDrawer from "@components/cart";
import {
  Container,
  Group,
  Text,
  ActionIcon,
  Drawer,
  Stack,
  AppShell,
  Indicator,
} from "@mantine/core";
import { useCart } from "@services/cart";
import { IconShoppingCart, IconUser } from "@tabler/icons-react";
import { useState } from "react";
import { useNavigate } from "react-router-dom";

export default function AppHeader() {
  const nav = useNavigate();
  const [drawerOpened, setDrawerOpened] = useState(false);
  const { items, removeFromCart } = useCart();
  const [cartOpened, setCartOpened] = useState(false);

  const navItems = [
    { label: "Início", path: "/" },
    { label: "Produtos", path: "/produtos" },
    { label: "Contactos", path: "/contactos" },
  ];

  return (
    <>
      <AppShell.Header
        px="md"
        style={{
          borderBottom: "1px solid #eee",
          display: "flex",
          alignItems: "center",
          height: "70px",
        }}
      >
        <Container
          size="lg"
          style={{
            display: "flex",
            alignItems: "center",
            width: "100%",
            justifyContent: "space-between",
          }}
        >
          {/* LOGO CENTRAL NO DESKTOP / ESQUERDA NO MOBILE */}
          <Text
            fw={800}
            size="xl"
            style={{ cursor: "pointer", flex: 1, textAlign: "center" }}
            className="desktop-logo"
            onClick={() => nav("/")}
          >
            CAPITEK
          </Text>
          {/* MOBILE LOGO ESQUERDO */}
          <Text
            fw={800}
            size="xl"
            style={{ cursor: "pointer" }}
            className="mobile-logo"
            onClick={() => nav("/")}
          >
            CAPITEK
          </Text>
          {/* NAV CENTRAL - DESKTOP */}
          <Group
            className="desktop-nav"
            style={{
              gap: 24,
              justifyContent: "center",
              flex: 2,
            }}
          >
            {navItems.map((item) => (
              <Text
                key={item.path}
                style={{ cursor: "pointer" }}
                onClick={() => nav(item.path)}
              >
                {item.label}
              </Text>
            ))}
          </Group>
          {/* BOTÕES DIREITA NO DESKTOP */}
          <Group className="desktop-actions" style={{ gap: 8 }}>
            <ActionIcon variant="light" size="lg">
              <IconUser size={20} />
            </ActionIcon>
            <Indicator
              label={items.reduce((acc, item) => acc + item.quantity, 0)}
              size={16}
              offset={4}
              disabled={items.length === 0}
              color="red"
            >
              <ActionIcon
                variant="light"
                size="lg"
                onClick={() => setCartOpened(true)}
              >
                <IconShoppingCart size={20} />
              </ActionIcon>
            </Indicator>
          </Group>
        </Container>

        {/* CSS RESPONSIVO */}
        <style>
          {`
      /* Desktop */
      @media (min-width: 768px) {
        .mobile-logo { display: none; }
        .mobile-menu { display: none; }
      }

      /* Mobile */
      @media (max-width: 767px) {
        .desktop-logo { display: none; }
        .desktop-nav { display: none; }
        .desktop-actions { display: none; }

        .mobile-logo {
          flex: 1;
          text-align: left;
        }
      }
    `}
        </style>
      </AppShell.Header>

      {/* DRAWER MOBILE */}
      <Drawer
        opened={drawerOpened}
        onClose={() => setDrawerOpened(false)}
        title="Menu"
        padding="md"
        size="xs"
      >
        <Stack style={{ gap: 16 }}>
          {navItems.map((item) => (
            <Text
              key={item.path}
              size="lg"
              style={{ cursor: "pointer" }}
              onClick={() => {
                nav(item.path);
                setDrawerOpened(false);
              }}
            >
              {item.label}
            </Text>
          ))}
        </Stack>
      </Drawer>

      <style>{`
        @media (max-width: 768px) {
          .desktop-nav {
            display: none;
          }
        }
      `}</style>

      <CartDrawer
        opened={cartOpened}
        onClose={() => setCartOpened(false)}
        items={items}
        onRemove={(id) => removeFromCart(id)}
        onCheckout={() => {
          nav("/checkout");
          setCartOpened(false);
        }}
      />
    </>
  );
}
