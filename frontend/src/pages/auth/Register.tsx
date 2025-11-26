// src/pages/auth/Register.tsx

import React, { useState } from "react";
import {
  AppShell,
  Container,
  Paper,
  TextInput,
  PasswordInput,
  Button,
  Title,
  Text,
  Stack,
  Anchor,
} from "@mantine/core";
import { useNavigate, Link } from "react-router-dom";
import { register } from "@services/auth";
import { notifications } from "@mantine/notifications";
import AppHeader from "@components/header";

export const RegisterPage = () => {
  const [formData, setFormData] = useState({
    firstName: "",
    lastName: "",
    username: "",
    email: "",
    password: "",
  });
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);

    try {
      await register(formData);
      notifications.show({
        title: "Success",
        message: "Account created successfully",
        color: "green",
      });
      navigate("/login");
    } catch (error) {
      notifications.show({
        title: "Error",
        message: "Failed to create account",
        color: "red",
      });
    } finally {
      setLoading(false);
    }
  };

  return (
    <AppShell>
      <AppHeader />
      <Container size={420} my={40}>
        <Title ta="center">Create an account</Title>
        <Text c="dimmed" size="sm" ta="center" mt={5}>
          Already have an account?{" "}
          <Anchor component={Link} to="/login" size="sm">
            Sign in
          </Anchor>
        </Text>

        <Paper withBorder shadow="md" p={30} mt={30} radius="md">
          <form onSubmit={handleSubmit}>
            <Stack>
              <TextInput
                label="First Name"
                placeholder="John"
                value={formData.firstName}
                onChange={(e) =>
                  setFormData({ ...formData, firstName: e.target.value })
                }
                required
              />
              <TextInput
                label="Last Name"
                placeholder="Doe"
                value={formData.lastName}
                onChange={(e) =>
                  setFormData({ ...formData, lastName: e.target.value })
                }
                required
              />
              <TextInput
                label="Username"
                placeholder="johndoe"
                value={formData.username}
                onChange={(e) =>
                  setFormData({ ...formData, username: e.target.value })
                }
                required
              />
              <TextInput
                label="Email"
                placeholder="your@email.com"
                value={formData.email}
                onChange={(e) =>
                  setFormData({ ...formData, email: e.target.value })
                }
                required
              />
              <PasswordInput
                label="Password"
                placeholder="Your password"
                value={formData.password}
                onChange={(e) =>
                  setFormData({ ...formData, password: e.target.value })
                }
                required
              />
              <Button type="submit" fullWidth loading={loading}>
                Create account
              </Button>
            </Stack>
          </form>
        </Paper>
      </Container>
    </AppShell>
  );
};

export default RegisterPage;
