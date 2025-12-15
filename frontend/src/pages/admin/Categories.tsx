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
  Card,
} from "@mantine/core";
import { IconPlus, IconEdit, IconTrash } from "@tabler/icons-react";
import { categoryService } from "@services/categories";
import type { Category } from "@_types/category";
import { notifications } from "@mantine/notifications";

export const AdminCategories = () => {
  const [categories, setCategories] = useState<Category[]>([]);
  const [loading, setLoading] = useState(true);
  const [modalOpen, setModalOpen] = useState(false);
  const [editingCategory, setEditingCategory] = useState<Category | null>(null);
  const [formData, setFormData] = useState({ name: "" });

  useEffect(() => {
    loadCategories();
  }, []);

  const loadCategories = async () => {
    try {
      const data = await categoryService.getAll();
      setCategories(data);
    } catch (error) {
      notifications.show({
        title: "Error",
        message: "Failed to load categories",
        color: "red",
      });
    } finally {
      setLoading(false);
    }
  };

  const handleCreate = () => {
    setEditingCategory(null);
    setFormData({ name: "" });
    setModalOpen(true);
  };

  const handleEdit = (category: Category) => {
    setEditingCategory(category);
    setFormData({ name: category.name });
    setModalOpen(true);
  };

  const handleSubmit = async () => {
    try {
      if (editingCategory) {
        await categoryService.update(editingCategory.id, {
          name: formData.name,
        });
        notifications.show({
          title: "Sucesso",
          message: "Categoria atualizada com sucesso",
          color: "green",
        });
      } else {
        await categoryService.create({
          id: crypto.randomUUID(),
          name: formData.name,
        });
        notifications.show({
          title: "Sucesso",
          message: "Categoria criada com sucesso",
          color: "green",
        });
      }
      setModalOpen(false);
      loadCategories();
    } catch (error) {
      notifications.show({
        title: "Erro",
        message: "Falha ao guardar categoria",
        color: "red",
      });
    }
  };

  const handleDelete = async (id: string) => {
    if (!confirm("Tens a certeza que queres eliminar esta categoria?")) return;

    try {
      await categoryService.delete(id);
      notifications.show({
        title: "Sucesso",
        message: "Categoria eliminada com sucesso",
        color: "green",
      });
      loadCategories();
    } catch (error) {
      notifications.show({
        title: "Erro",
        message: "Falha ao eliminar categoria",
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
            Categorias
          </Text>
          <Text size="sm" c="dimmed">
            Gerir categorias de produtos
          </Text>
        </div>
        <Button
          leftSection={<IconPlus size={16} />}
          onClick={handleCreate}
          variant="filled"
          color="blue"
        >
          Adicionar Categoria
        </Button>
      </Group>

      <Card withBorder shadow="sm" radius="md" p="md">
        <Table.ScrollContainer minWidth={600}>
          <Table striped highlightOnHover verticalSpacing="sm">
            <Table.Thead>
              <Table.Tr>
                <Table.Th>Nome</Table.Th>
                <Table.Th style={{ width: 100 }}>Ações</Table.Th>
              </Table.Tr>
            </Table.Thead>
            <Table.Tbody>
              {categories.length === 0 ? (
                <Table.Tr>
                  <Table.Td colSpan={2} align="center">
                    <Text c="dimmed" py="xl">
                      Nenhuma categoria encontrada
                    </Text>
                  </Table.Td>
                </Table.Tr>
              ) : (
                categories.length > 0 &&
                categories.map((category) => (
                  <Table.Tr key={category.id}>
                    <Table.Td>
                      <Text fw={500} size="sm">
                        {category.name}
                      </Text>
                    </Table.Td>
                    <Table.Td>
                      <Group gap="xs">
                        <ActionIcon
                          variant="subtle"
                          color="blue"
                          onClick={() => handleEdit(category)}
                        >
                          <IconEdit size={16} />
                        </ActionIcon>
                        <ActionIcon
                          variant="subtle"
                          color="red"
                          onClick={() => handleDelete(category.id)}
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
        title={editingCategory ? "Editar Categoria" : "Criar Categoria"}
      >
        <Stack>
          <TextInput
            label="Nome"
            value={formData.name}
            onChange={(e) => setFormData({ name: e.target.value })}
            required
          />
          <Button onClick={handleSubmit}>
            {editingCategory ? "Atualizar" : "Criar"}
          </Button>
        </Stack>
      </Modal>
    </Stack>
  );
};

export default AdminCategories;
