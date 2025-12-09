import {
  Container,
  Paper,
  Text,
  Stack,
  Avatar,
  Title,
  Button,
  Loader,
  Group,
} from "@mantine/core";
import { getUserId, logout } from "@services/auth";
import { useNavigate } from "react-router-dom";
import { IconLogout, IconMail, IconId } from "@tabler/icons-react";
import AppHeader from "@components/header";
import { useEffect, useState } from "react";
import { apiClient } from "@utils/api";
import type UserType from "@_types/user";

export default function ProfilePage() {
  const userId = getUserId();
  const navigate = useNavigate();
  const [user, setUser] = useState<UserType | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchUser = async () => {
      if (userId) {
        try {
          const userData = await apiClient.get<UserType>(`/users/${userId}`);
          if (userData) {
            setUser(userData);
          }
        } catch (error) {
          console.error("Failed to fetch user profile", error);
        } finally {
          setLoading(false);
        }
      } else {
        setLoading(false);
      }
    };
    fetchUser();
  }, [userId]);

  const handleLogout = () => {
    logout();
    navigate("/");
  };

  if (!userId) {
    return null;
  }

  return (
    <>
      <AppHeader />
      <Container size="sm" mt="xl">
        <Paper radius="md" p="xl" withBorder>
          <Stack align="center" gap="md">
            <Avatar size={120} radius={120} color="blue" variant="light">
              {user?.firstName?.charAt(0) || "U"}
            </Avatar>

            <Title order={2}>
              {user ? `${user.firstName} ${user.lastName}` : "O meu perfil"}
            </Title>
            {user?.username && <Text c="dimmed">@{user.username}</Text>}

            {loading ? (
              <Loader />
            ) : (
              <Stack w="100%" gap="md" mt="md">
                <Paper withBorder p="md" bg="gray.0">
                  <Group>
                    <IconId size={20} color="gray" />
                    <Stack gap={0}>
                      <Text size="xs" c="dimmed">
                        ID de Utilizador
                      </Text>
                      <Text size="sm" fw={500}>
                        {userId}
                      </Text>
                    </Stack>
                  </Group>
                </Paper>

                <Paper withBorder p="md" bg="gray.0">
                  <Group>
                    <IconMail size={20} color="gray" />
                    <Stack gap={0}>
                      <Text size="xs" c="dimmed">
                        Email
                      </Text>
                      <Text size="sm" fw={500}>
                        {user?.email || "N/A"}
                      </Text>
                    </Stack>
                  </Group>
                </Paper>

                {user?.createdAt && (
                  <Text size="xs" c="dimmed" ta="center">
                    Membro desde {new Date(user.createdAt).toLocaleDateString()}
                  </Text>
                )}
              </Stack>
            )}

            <Button
              color="red"
              variant="subtle"
              leftSection={<IconLogout size={18} />}
              onClick={handleLogout}
              mt="lg"
            >
              Terminar Sessão
            </Button>
          </Stack>
        </Paper>
      </Container>
    </>
  );
}
