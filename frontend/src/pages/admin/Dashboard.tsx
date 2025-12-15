import {
  AppShell,
  Stack,
  NavLink,
  Text,
  Group,
  ThemeIcon,
  Breadcrumbs,
  Anchor,
} from "@mantine/core";
import { Link, Navigate, Outlet, useLocation } from "react-router-dom";
import { useAuth } from "@contexts/AuthContext";
import {
  IconBox,
  IconTag,
  IconCategory,
  IconPercentage,
  IconUsers,
  IconListDetails,
  IconDashboard,
} from "@tabler/icons-react";

export const AdminDashboard = () => {
  const { role } = useAuth();
  const location = useLocation();

  const navLinks = [
    { to: "/admin", label: "Dashboard", icon: IconDashboard },
    { to: "/admin/products", label: "Produtos", icon: IconBox },
    { to: "/admin/brands", label: "Marcas", icon: IconTag },
    { to: "/admin/categories", label: "Categorias", icon: IconCategory },
    { to: "/admin/discounts", label: "Descontos", icon: IconPercentage },
    { to: "/admin/users", label: "Utilizadores", icon: IconUsers },
    {
      to: "/admin/tech-specs",
      label: "Especificações Técnicas",
      icon: IconListDetails,
    },
  ];

  return (
    <AppShell
      header={{ height: 60 }}
      navbar={{ width: 260, breakpoint: "sm" }}
      padding="md"
      layout="alt"
    >
      <AppShell.Header p="md" c="white">
        <Group align="center" h="100%">
          <Text size="xl" fw={700}>
            <Breadcrumbs>
              {navLinks
                .map((link) => {
                  return {
                    title: link.label,
                    href: link.to,
                  };
                })
                .map((link) => {
                  const isActive = location.pathname === link.href;

                  if (!isActive && !location.pathname.includes(link.href))
                    return null;

                  return (
                    <Anchor
                      href={link.href}
                      key={link.href}
                      c={isActive ? "blue" : "white"}
                    >
                      {link.title}
                    </Anchor>
                  );
                })}
            </Breadcrumbs>
          </Text>
        </Group>
      </AppShell.Header>

      <AppShell.Navbar p="md">
        <Text c="dimmed" size="xs" fw={700} mb="sm" tt="uppercase">
          Painel de Gestão
        </Text>
        <Stack gap={4}>
          {navLinks.map((link) => (
            <NavLink
              key={link.to}
              component={Link}
              to={link.to}
              label={
                <Text size="sm" fw={500}>
                  {link.label}
                </Text>
              }
              leftSection={
                <ThemeIcon variant="subtle" size="sm" color="blue">
                  <link.icon size={14} />
                </ThemeIcon>
              }
              active={location.pathname === link.to}
              variant="subtle"
              color="blue"
              style={{ borderRadius: 6 }}
            />
          ))}
        </Stack>
      </AppShell.Navbar>

      <AppShell.Main bg="gray.9">
        <Outlet />
      </AppShell.Main>
    </AppShell>
  );
};

export default AdminDashboard;
