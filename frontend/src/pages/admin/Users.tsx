import { useEffect, useState } from "react";
import {
  Table,
  Button,
  Group,
  Text,
  Modal,
  TextInput,
  Stack,
  ActionIcon,
  Select,
  PasswordInput,
  Card,
  Badge,
} from "@mantine/core";
import { IconEdit, IconTrash, IconPlus } from "@tabler/icons-react";
import { getUsers, createUser, updateUser, deleteUser } from "@services/user";
import type UserType from "@_types/user";
import { notifications } from "@mantine/notifications";

export const AdminUsers = () => {
  const [users, setUsers] = useState<UserType[]>([]);
  const [loading, setLoading] = useState(true);
  const [modalOpen, setModalOpen] = useState(false);
  const [editingUser, setEditingUser] = useState<UserType | null>(null);

  const [formData, setFormData] = useState({
    firstName: "",
    lastName: "",
    username: "",
    email: "",
    role: "guest",
    password: "",
  });

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    try {
      const data = await getUsers();
      setUsers(data);
    } catch (error) {
      notifications.show({
        title: "Erro",
        message: "Erro ao carregar os utilizadores",
        color: "red",
      });
    } finally {
      setLoading(false);
    }
  };

  const handleCreate = () => {
    setEditingUser(null);
    setFormData({
      firstName: "",
      lastName: "",
      username: "",
      email: "",
      role: "guest",
      password: "",
    });
    setModalOpen(true);
  };

  const handleEdit = (user: UserType) => {
    setEditingUser(user);
    setFormData({
      firstName: user.firstName,
      lastName: user.lastName,
      username: user.username,
      email: user.email,
      role: user.role,
      password: "",
    });
    setModalOpen(true);
  };

  const handleSubmit = async () => {
    try {
      if (editingUser) {
        const payload: any = {
          firstName: formData.firstName,
          lastName: formData.lastName,
          username: formData.username,
          email: formData.email,
          role: formData.role,
        };

        await updateUser(editingUser.id, payload);
        notifications.show({
          title: "Sucesso",
          message: "Utilizador atualizado com sucesso",
          color: "green",
        });
      } else {
        await createUser({
          id: crypto.randomUUID(),
          firstName: formData.firstName,
          lastName: formData.lastName,
          username: formData.username,
          email: formData.email,
          role: formData.role,
          passwordHash: formData.password,
        });
        notifications.show({
          title: "Sucesso",
          message: "Utilizador criado com sucesso",
          color: "green",
        });
      }
      setModalOpen(false);
      loadData();
    } catch (error) {
      notifications.show({
        title: "Erro",
        message: "Erro ao guardar o utilizador",
        color: "red",
      });
    }
  };

  const handleDelete = async (id: string) => {
    if (!confirm("Tens a certeza que queres eliminar este utilizador?")) return;

    try {
      await deleteUser(id);
      notifications.show({
        title: "Sucesso",
        message: "Utilizador eliminado com sucesso",
        color: "green",
      });
      loadData();
    } catch (error) {
      notifications.show({
        title: "Erro",
        message: "Erro ao eliminar o utilizador",
        color: "red",
      });
    }
  };

  if (loading) return <Text>A Carregar...</Text>;

  return (
    <Stack gap="lg">
      <Group justify="space-between" align="center">
        <div>
          <Text size="xl" fw={700}>
            Utilizadores
          </Text>
          <Text size="sm" c="dimmed">
            Gerir o acesso ao sistema e os papéis
          </Text>
        </div>
        <Button
          leftSection={<IconPlus size={16} />}
          onClick={handleCreate}
          variant="filled"
          color="blue"
        >
          Adicionar Utilizador
        </Button>
      </Group>

      <Card withBorder shadow="sm" radius="md" p="md">
        <Table.ScrollContainer minWidth={800}>
          <Table striped highlightOnHover verticalSpacing="sm">
            <Table.Thead>
              <Table.Tr>
                <Table.Th>Nome</Table.Th>
                <Table.Th>Email</Table.Th>
                <Table.Th>Username</Table.Th>
                <Table.Th>Role</Table.Th>
                <Table.Th>Ações</Table.Th>
              </Table.Tr>
            </Table.Thead>
            <Table.Tbody>
              {users.length === 0 ? (
                <Table.Tr>
                  <Table.Td colSpan={5} align="center">
                    <Text c="dimmed" py="xl">
                      Nenhum utilizador encontrado
                    </Text>
                  </Table.Td>
                </Table.Tr>
              ) : (
                users.map((user) => (
                  <Table.Tr key={user.id}>
                    <Table.Td>
                      <Text fw={500} size="sm">
                        {user.firstName} {user.lastName}
                      </Text>
                    </Table.Td>
                    <Table.Td>{user.email}</Table.Td>
                    <Table.Td>
                      <Badge variant="dot" color="gray">
                        {user.username}
                      </Badge>
                    </Table.Td>
                    <Table.Td>
                      <Badge
                        color={user.role === "admin" ? "blue" : "gray"}
                        variant="light"
                      >
                        {user.role}
                      </Badge>
                    </Table.Td>
                    <Table.Td>
                      <Group gap="xs">
                        <ActionIcon
                          variant="subtle"
                          color="blue"
                          onClick={() => handleEdit(user)}
                        >
                          <IconEdit size={16} />
                        </ActionIcon>
                        <ActionIcon
                          variant="subtle"
                          color="red"
                          onClick={() => handleDelete(user.id)}
                        >
                          <IconTrash size={16} />
                        </ActionIcon>
                      </Group>
                    </Table.Td>
                  </Table.Tr>
                ))
              )}
            </Table.Tbody>
          </Table>
        </Table.ScrollContainer>
      </Card>

      <Modal
        opened={modalOpen}
        onClose={() => setModalOpen(false)}
        title={editingUser ? "Editar Utilizador" : "Criar Utilizador"}
      >
        <Stack>
          <Group grow>
            <TextInput
              label="Nome"
              value={formData.firstName}
              onChange={(e) =>
                setFormData({ ...formData, firstName: e.target.value })
              }
              required
            />
            <TextInput
              label="Apelido"
              value={formData.lastName}
              onChange={(e) =>
                setFormData({ ...formData, lastName: e.target.value })
              }
              required
            />
          </Group>
          <TextInput
            label="Username"
            value={formData.username}
            onChange={(e) =>
              setFormData({ ...formData, username: e.target.value })
            }
            required
          />
          <TextInput
            label="Email"
            value={formData.email}
            onChange={(e) =>
              setFormData({ ...formData, email: e.target.value })
            }
            required
          />
          <Select
            label="Role"
            value={formData.role}
            onChange={(val) =>
              setFormData({ ...formData, role: val || "guest" })
            }
            data={[
              { value: "guest", label: "Guest" },
              { value: "admin", label: "Admin" },
            ]}
          />
          {!editingUser && (
            <PasswordInput
              label="Password"
              value={formData.password}
              onChange={(e) =>
                setFormData({ ...formData, password: e.target.value })
              }
              required
            />
          )}

          <Button onClick={handleSubmit}>
            {editingUser ? "Atualizar" : "Criar"}
          </Button>
        </Stack>
      </Modal>
    </Stack>
  );
};

export default AdminUsers;
