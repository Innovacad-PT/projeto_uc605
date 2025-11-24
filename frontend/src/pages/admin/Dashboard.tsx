// src/pages/admin/Dashboard.tsx

import { AppShell, Stack, NavLink, Text } from "@mantine/core";
import { Link, Outlet, useLocation } from "react-router-dom";
import { useAuth } from "@contexts/AuthContext";

/**
 * Admin dashboard layout. Uses Mantine v8 AppShell with sidebar navigation.
 * The <Outlet /> will render nested admin routes (e.g., Products, Brands, etc.).
 */
export const AdminDashboard = () => {
  const { role } = useAuth();
  const location = useLocation();

  const navLinks = [
    { to: "/admin/products", label: "Products" },
    { to: "/admin/brands", label: "Brands" },
    { to: "/admin/categories", label: "Categories" },
    { to: "/admin/discounts", label: "Discounts" },
  ];

  return (
    <AppShell
      header={{ height: 60 }}
      navbar={{ width: 250, breakpoint: "sm" }}
      padding="md"
    >
      <AppShell.Header p="md">
        <Text size="xl" fw={700}>
          Admin Dashboard {role === "admin" ? "(Admin)" : ""}
        </Text>
      </AppShell.Header>

      <AppShell.Navbar p="md">
        <Stack gap="xs">
          {navLinks.map((link) => (
            <NavLink
              key={link.to}
              component={Link}
              to={link.to}
              label={link.label}
              active={location.pathname === link.to}
            />
          ))}
        </Stack>
      </AppShell.Navbar>

      <AppShell.Main>
        <Outlet />
      </AppShell.Main>
    </AppShell>
  );
};

export default AdminDashboard;
